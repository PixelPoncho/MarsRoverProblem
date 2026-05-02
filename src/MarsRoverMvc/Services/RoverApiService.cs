namespace MarsRoverMvc.Services
{
    using MarsRoverMvc.Models;
    using System.Text.Json;

    /// Implementation of the Rover API Service
    /// Makes HTTP calls to the Web API backend
    public class RoverApiService : IRoverApiService
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
        public async Task<List<SimulationHistoryItem>> GetHistoryAsync()
        {
            try
            {
                // Make the GET request to retrieve history
                var response = await _httpClient.GetAsync("/api/rover/history");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"History API Response: {responseContent}");

                    // Handle empty response
                    if (string.IsNullOrEmpty(responseContent))
                    {
                        _logger.LogWarning("History API returned empty content");
                        return new List<SimulationHistoryItem>();
                    }

                    // Parse the JSON response
                    using var document = JsonDocument.Parse(responseContent);
                    var items = new List<SimulationHistoryItem>();

                    _logger.LogInformation($"Parsed JSON, root element kind: {document.RootElement.ValueKind}");

                    // Each item in the response is a simulation record
                    foreach (var element in document.RootElement.EnumerateArray())
                    {
                        try
                        {
                            _logger.LogInformation($"Processing history item: {element.GetRawText().Substring(0, Math.Min(100, element.GetRawText().Length))}...");

                            // Extract properties from each simulation record
                            var item = new SimulationHistoryItem
                            {
                                SimulationId = GetJsonString(element, "simulationId"),
                                PlateauSize = GetJsonString(element, "plateauSize"),
                                RoverCount = GetJsonInt(element, "roverCount"),
                                ExecutedAt = GetJsonDateTime(element, "executedAt"),
                            };

                            // Parse rover results if available
                            if (element.TryGetProperty("results", out var resultsElement))
                            {
                                foreach (var result in resultsElement.EnumerateArray())
                                {
                                    var roverResult = new RoverResultData
                                    {
                                        RoverId = GetJsonInt(result, "roverId"),
                                        FinalX = GetJsonInt(result, "finalX"),
                                        FinalY = GetJsonInt(result, "finalY"),
                                        FinalDirection = GetJsonString(result, "finalDirection", "N"),
                                        Commands = GetJsonString(result, "commands"),
                                    };

                                    // Parse path if available
                                    if (result.TryGetProperty("path", out var pathElement))
                                    {
                                        foreach (var pathPoint in pathElement.EnumerateArray())
                                        {
                                            var pathStr = pathPoint.ValueKind == JsonValueKind.String 
                                                ? pathPoint.GetString() 
                                                : pathPoint.GetRawText();
                                            if (!string.IsNullOrEmpty(pathStr))
                                            {
                                                roverResult.Path.Add(pathStr);
                                            }
                                        }
                                    }

                                    item.Results.Add(roverResult);
                                }
                            }

                            items.Add(item);
                            _logger.LogInformation($"Successfully parsed history item: {item.SimulationId}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"Error parsing history item: {ex.Message}\n{element.GetRawText()}");
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

        // Helper methods for safe JSON property extraction
        private string GetJsonString(JsonElement element, string propertyName, string defaultValue = "")
        {
            if (element.TryGetProperty(propertyName, out var property))
            {
                return property.GetString() ?? defaultValue;
            }
            return defaultValue;
        }

        private int GetJsonInt(JsonElement element, string propertyName, int defaultValue = 0)
        {
            if (element.TryGetProperty(propertyName, out var property))
            {
                try
                {
                    return property.GetInt32();
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        private DateTime GetJsonDateTime(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var property))
            {
                var dateStr = property.GetString();
                if (DateTime.TryParse(dateStr, out var result))
                {
                    return result;
                }
            }
            return DateTime.UtcNow;
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
