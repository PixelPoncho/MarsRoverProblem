namespace MarsRoverMvc.Services
{
    using MarsRoverMvc.Models;

    /// <summary>
    /// Interface for communication with the Rover Web API
    /// Handles all HTTP calls to the backend service
    /// </summary>
    public interface IRoverApiService
    {
        /// <summary>
        /// Calls the Web API to run a rover simulation
        /// </summary>
        /// <param name="request">The simulation request data</param>
        /// <returns>The simulation response with results</returns>
        Task<SimulationResponse?> SimulateAsync(SimulationRequest request);

        /// <summary>
        /// Calls the Web API to retrieve all simulation history
        /// </summary>
        /// <returns>List of historical simulations</returns>
        Task<List<SimulationHistoryItem>> GetHistoryAsync();

        /// <summary>
        /// Calls the Web API to save a screenshot
        /// </summary>
        /// <param name="simulationId">The simulation ID</param>
        /// <param name="screenshotBase64">The screenshot in base64 format</param>
        /// <returns>Success indicator</returns>
        Task<bool> SaveScreenshotAsync(string simulationId, string screenshotBase64);
    }

    /// <summary>
    /// DTO for simulation request sent to the API
    /// </summary>
    public class SimulationRequest
    {
        public int PlateauMaxX { get; set; }
        public int PlateauMaxY { get; set; }
        public List<RoverInputData> Rovers { get; set; } = new List<RoverInputData>();
    }

    /// <summary>
    /// DTO for individual rover input
    /// </summary>
    public class RoverInputData
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public string StartDirection { get; set; } = "N";
        public string Commands { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for simulation response from the API
    /// </summary>
    public class SimulationResponse
    {
        public string SimulationId { get; set; } = string.Empty;
        public List<RoverOutput> Rovers { get; set; } = new List<RoverOutput>();
        public DateTime ExecutedAt { get; set; }
    }

    /// <summary>
    /// DTO for individual rover output
    /// </summary>
    public class RoverOutput
    {
        public int RoverId { get; set; }
        public int FinalX { get; set; }
        public int FinalY { get; set; }
        public string FinalDirection { get; set; } = string.Empty;
        public List<string> Path { get; set; } = new List<string>();
        public string Commands { get; set; } = string.Empty;
    }
}
