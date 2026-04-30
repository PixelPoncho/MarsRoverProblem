namespace MarsRoverWebApi.Models
{
    /// Represents the output data from a rover simulation
    /// Contains final positions and paths for all rovers
    public class SimulationResponse
    {
        /// Unique simulation ID for tracking in history
        public string SimulationId { get; set; }

        /// Results for each rover after simulation
        public List<RoverOutputData> Rovers { get; set; } = new List<RoverOutputData>();

        /// Timestamp when simulation was executed
        public DateTime ExecutedAt { get; set; }

        public SimulationResponse()
        {
            SimulationId = Guid.NewGuid().ToString();
            ExecutedAt = DateTime.UtcNow;
        }
    }

    /// Output data for a single rover after simulation
    public class RoverOutputData
    {
        /// Rover identifier
        public int RoverId { get; set; }

        /// Final X coordinate
        public int FinalX { get; set; }

        /// Final Y coordinate
        public int FinalY { get; set; }

        /// Final direction heading
        public string FinalDirection { get; set; }

        /// Complete path taken by rover (array of coordinates)
        /// Used for visualization on the plateau
        /// Example: ["1 2 N", "1 3 N", "2 3 N"]
        public List<string> Path { get; set; } = new List<string>();

        /// Commands that were executed
        public string Commands { get; set; }
    }
}
