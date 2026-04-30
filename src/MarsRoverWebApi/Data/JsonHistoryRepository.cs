using MarsRoverWebApi.Services;
using System.Text.Json;

namespace MarsRoverWebApi.Data
{
    /// JSON-based implementation of history persistence
    /// Stores simulation records and screenshots in JSON files
    /// This is a temporary solution until a database is implemented
    public class JsonHistoryRepository : IHistoryRepository
    {
        // Directory where all history files are stored
        private readonly string _historyDirectory;
        // File containing all simulation records
        private readonly string _historyFilePath;
        // Directory where screenshots are stored
        private readonly string _screenshotsDirectory;

        public JsonHistoryRepository(IWebHostEnvironment environment)
        {
            // Create necessary directories in the application root
            _historyDirectory = Path.Combine(environment.ContentRootPath, "Data", "History");
            _screenshotsDirectory = Path.Combine(environment.ContentRootPath, "Data", "Screenshots");
            _historyFilePath = Path.Combine(_historyDirectory, "simulations.json");

            // Ensure directories exist
            Directory.CreateDirectory(_historyDirectory);
            Directory.CreateDirectory(_screenshotsDirectory);
        }

        /// Saves a simulation result to the JSON history file
        /// If file doesn't exist, creates it with a new array
        /// If file exists, appends to the existing array
        public async Task SaveSimulationAsync(object simulation)
        {
            try
            {
                // Read existing simulations or create new list
                var simulations = new List<object>();

                if (File.Exists(_historyFilePath))
                {
                    var json = await File.ReadAllTextAsync(_historyFilePath);
                    // Parse JSON array
                    using var document = JsonDocument.Parse(json);
                    if (document.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var element in document.RootElement.EnumerateArray())
                        {
                            simulations.Add(element.GetRawText());
                        }
                    }
                }

                // Add new simulation
                simulations.Add(simulation);

                // Write updated list back to file
                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonContent = JsonSerializer.Serialize(simulations, options);
                await File.WriteAllTextAsync(_historyFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving simulation to history: {ex.Message}");
                throw;
            }
        }

        /// Retrieves all historical simulations from the JSON file
        public async Task<List<object>> GetAllSimulationsAsync()
        {
            try
            {
                var simulations = new List<object>();

                if (!File.Exists(_historyFilePath))
                {
                    return simulations; // Return empty list if no history file
                }

                var json = await File.ReadAllTextAsync(_historyFilePath);
                using var document = JsonDocument.Parse(json);

                // Convert JSON elements to objects
                if (document.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var element in document.RootElement.EnumerateArray())
                    {
                        // Store as raw JSON string for flexibility
                        simulations.Add(element.GetRawText());
                    }
                }

                return simulations;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving simulations from history: {ex.Message}");
                return new List<object>();
            }
        }

        /// Saves a screenshot (as base64) with its associated simulation ID
        /// Screenshots are stored as separate files for easy management
        public async Task SaveScreenshotAsync(string simulationId, string screenshotBase64)
        {
            try
            {
                // Create a filename based on the simulation ID
                var filename = $"{simulationId}.png.base64";
                var filepath = Path.Combine(_screenshotsDirectory, filename);

                // Write the base64-encoded image data to file
                await File.WriteAllTextAsync(filepath, screenshotBase64);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving screenshot: {ex.Message}");
                throw;
            }
        }
    }
}
