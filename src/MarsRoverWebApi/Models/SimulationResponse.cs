namespace MarsRoverWebApi.Models
{
    /// <summary>
    /// Represents the output data from a rover simulation
    /// Contains final positions and paths for all rovers
    /// </summary>
    public class SimulationResponse
    {
        /// <summary>
        /// Unique simulation ID for tracking in history
        /// </summary>
        public string SimulationId { get; set; }

        /// <summary>
        /// Results for each rover after simulation
        /// </summary>
        public List<RoverOutputData> Rovers { get; set; } = new List<RoverOutputData>();

        /// <summary>
        /// Timestamp when simulation was executed
        /// </summary>
        public DateTime ExecutedAt { get; set; }

        public SimulationResponse()
        {
            SimulationId = Guid.NewGuid().ToString();
            ExecutedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Output data for a single rover after simulation
    /// </summary>
    public class RoverOutputData
    {
        /// <summary>
        /// Rover identifier
        /// </summary>
        public int RoverId { get; set; }

        /// <summary>
        /// Final X coordinate
        /// </summary>
        public int FinalX { get; set; }

        /// <summary>
        /// Final Y coordinate
        /// </summary>
        public int FinalY { get; set; }

        /// <summary>
        /// Final direction heading
        /// </summary>
        public string FinalDirection { get; set; } = string.Empty;

        /// <summary>
        /// Complete path taken by rover (array of coordinates)
        /// Used for visualization on the plateau
        /// Example: ["1 2 N", "1 3 N", "2 3 N"]
        /// </summary>
        public List<string> Path { get; set; } = new List<string>();

        /// <summary>
        /// Commands that were executed
        /// </summary>
        public string Commands { get; set; } = string.Empty;
    }
}
