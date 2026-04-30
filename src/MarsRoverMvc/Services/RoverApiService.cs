namespace MarsRoverMvc.Services
{
    using MarsRoverMvc.Models;
    using System.Text.Json;

    /// <summary>
    /// Implementation of the Rover API Service
    /// Makes HTTP calls to the Web API backend
    /// </summary>
    public class RoverApiService : IRoverApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RoverApiService> _logger;

        public RoverApiService(HttpClient httpClient, ILogger<RoverApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Calls the Web API endpoint to simulate rovers
        /// POST /api/rover/simulate
        /// </summary>
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

        /// <summary>
        /// Calls the Web API endpoint to get simulation history
        /// GET /api/rover/history
        /// </summary>
        public async Task<List<SimulationHistoryItem>> GetHistoryAsync()
        {
            try
            {
                // Make the GET request to retrieve history
                var response = await _httpClient.GetAsync("/api/rover/history");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    // Parse the JSON response
                    using var document = JsonDocument.Parse(responseContent);
                    var items = new List<SimulationHistoryItem>();

                    // Each item in the response is a simulation record
                    foreach (var element in document.RootElement.EnumerateArray())
                    {
                        try
                        {
                            // Extract properties from each simulation record
                            var item = new SimulationHistoryItem
                            {
                                SimulationId = element.GetProperty("simulationId").GetString() ?? string.Empty,
                                PlateauSize = element.GetProperty("plateauSize").GetString() ?? string.Empty,
                                RoverCount = element.GetProperty("roverCount").GetInt32(),
                                ExecutedAt = DateTime.Parse(element.GetProperty("executedAt").GetString() ?? DateTime.UtcNow.ToString()),
                            };

                            // Parse rover results if available
                            if (element.TryGetProperty("results", out var resultsElement))
                            {
                                foreach (var result in resultsElement.EnumerateArray())
                                {
                                    var roverResult = new RoverResultData
                                    {
                                        RoverId = result.GetProperty("roverId").GetInt32(),
                                        FinalX = result.GetProperty("finalX").GetInt32(),
                                        FinalY = result.GetProperty("finalY").GetInt32(),
                                        FinalDirection = result.GetProperty("finalDirection").GetString() ?? "N",
                                        Commands = result.GetProperty("commands").GetString() ?? string.Empty,
                                    };

                                    // Parse path if available
                                    if (result.TryGetProperty("path", out var pathElement))
                                    {
                                        foreach (var pathPoint in pathElement.EnumerateArray())
                                        {
                                            roverResult.Path.Add(pathPoint.GetString() ?? string.Empty);
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

        /// <summary>
        /// Calls the Web API endpoint to save a screenshot
        /// POST /api/rover/save-screenshot
        /// </summary>
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
