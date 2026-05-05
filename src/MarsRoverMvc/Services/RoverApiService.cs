namespace MarsRoverMvc.Services
{
  using MarsRoverMvc.Models;
  using System.Text.Json;

  /// Implementation of the Rover API Service
  /// Makes HTTP calls to the Web API backend
  public class RoverApiService: IRoverApiService
  {
    private readonly HttpClient _httpClient;
    private readonly ILogger<RoverApiService> _logger;

    public RoverApiService(HttpClient httpClient, ILogger<RoverApiService> logger)
    {
      _httpClient = httpClient;
      _logger = logger;
    }

    /// Calls the Web API endpoint to simulate rovers
    /// POST /api/rover/simulate
    public async Task<SimulationResponse?> SimulateAsync(SimulationRequest request)
    {
      try
      {
        // Serialize the request to JSON
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        // Make the POST request to the API
        var response = await _httpClient.PostAsync("/api/rover/simulate", content);

        if (response.IsSuccessStatusCode)
        {
          // Deserialize the response
          var responseContent = await response.Content.ReadAsStringAsync();
          var result = JsonSerializer.Deserialize<SimulationResponse>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
          return result;
        }
        else
        {
          _logger.LogError($"API error: {response.StatusCode} - {response.ReasonPhrase}");
          return null;
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error calling simulation API: {ex.Message}");
        return null;
      }
    }

    /// Calls the Web API endpoint to get simulation history
    /// GET /api/rover/history
    /// IMPORTANT: We parse the JSON immediately and convert to objects to avoid JsonDocument disposal issues
    public async Task<List<SimulationHistoryItem>> GetHistoryAsync()
    {
      try
      {
        // Make the GET request to retrieve history
        var response = await _httpClient.GetAsync("/api/rover/history");

        if (response.IsSuccessStatusCode)
        {
          var responseContent = await response.Content.ReadAsStringAsync();
          var items = new List<SimulationHistoryItem>();

          // Use a using statement to ensure the JsonDocument is properly disposed
          // This is critical - we must extract all data before the document is disposed
          using (var document = JsonDocument.Parse(responseContent))
          {
            // Each item in the response is a simulation record
            foreach (var historyEntry in document.RootElement.EnumerateArray())
            {
              try
              {
                var element = historyEntry.GetProperty("item2");
                var screenshotDataUri = historyEntry.GetProperty("item1").GetString() ?? string.Empty;
                // Extract properties from each simulation record
                // IMPORTANT: We call .GetString() immediately to get the actual string value
                // NOT storing the JsonElement itself which would cause disposal issues
                var item = new SimulationHistoryItem
                {
                  // Get string values immediately while JsonDocument is still alive
                  SimulationId = element.GetProperty("SimulationId").GetString() ?? string.Empty,
                  PlateauSize = element.GetProperty("PlateauSize").GetString() ?? string.Empty,
                  RoverCount = element.GetProperty("RoverCount").GetInt32(),
                  // Parse the date string immediately
                  ExecutedAt =
                    DateTime.Parse(element.GetProperty("ExecutedAt").GetString() ?? DateTime.UtcNow.ToString()),
                  ScreenshotDataUri = screenshotDataUri
                };

                // Parse rover results if available
                // Again, we extract ALL data while the document is still available
                if (element.TryGetProperty("Results", out var resultsElement))
                {
                  foreach (var result in resultsElement.EnumerateArray())
                  {
                    var roverResult = new RoverResultData
                    {
                      RoverId = result.GetProperty("RoverId").GetInt32(),
                      FinalX = result.GetProperty("FinalX").GetInt32(),
                      FinalY = result.GetProperty("FinalY").GetInt32(),
                      // Get string values immediately
                      FinalDirection = result.GetProperty("FinalDirection").GetString() ?? "N",
                      Commands = result.GetProperty("Commands").GetString() ?? string.Empty,
                    };

                    // Parse path if available - extract all strings immediately
                    if (result.TryGetProperty("Path", out var pathElement))
                    {
                      foreach (var pathPoint in pathElement.EnumerateArray())
                      {
                        // Get the string value immediately
                        var pathString = pathPoint.GetString() ?? string.Empty;
                        roverResult.Path.Add(pathString);
                      }
                    }

                    item.Results.Add(roverResult);
                  }
                }

                items.Add(item);
              }
              catch (Exception ex)
              {
                _logger.LogWarning($"Error parsing history item: {ex.Message}");
                continue;
              }
            }
          } // JsonDocument is disposed here after all data is extracted

          return items;
        }
        else
        {
          _logger.LogError($"API error: {response.StatusCode}");
          return new List<SimulationHistoryItem>();
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error calling history API: {ex.Message}");
        return new List<SimulationHistoryItem>();
      }
    }

    /// Calls the Web API endpoint to save a screenshot
    /// POST /api/rover/save-screenshot
    public async Task<bool> SaveScreenshotAsync(string simulationId, string screenshotBase64)
    {
      try
      {
        // Create request body with the screenshot data
        var requestBody = new { imageBase64 = screenshotBase64 };
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        // Make the POST request with the simulation ID as a query parameter
        var response = await _httpClient.PostAsync(
          $"/api/rover/save-screenshot?simulationId={simulationId}",
          content);

        return response.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error saving screenshot: {ex.Message}");
        return false;
      }
    }
  }
}