namespace MarsRoverWebApi.Models
{
    /// <summary>
    /// Represents the input data for a rover simulation
    /// Contains plateau dimensions and all rovers to be simulated
    /// </summary>
    public class SimulationRequest
    {
        /// <summary>
        /// Maximum X coordinate of the plateau
        /// </summary>
        public int PlateauMaxX { get; set; }

        /// <summary>
        /// Maximum Y coordinate of the plateau
        /// </summary>
        public int PlateauMaxY { get; set; }

        /// <summary>
        /// Collection of rovers to simulate
        /// Each rover contains starting position and command sequence
        /// </summary>
        public List<RoverInputData> Rovers { get; set; } = new List<RoverInputData>();
    }

    /// <summary>
    /// Input data for a single rover
    /// </summary>
    public class RoverInputData
    {
        /// <summary>
        /// Starting X coordinate
        /// </summary>
        public int StartX { get; set; }

        /// <summary>
        /// Starting Y coordinate
        /// </summary>
        public int StartY { get; set; }

        /// <summary>
        /// Starting direction (N, E, S, or W)
        /// </summary>
        public string StartDirection { get; set; } = "N";

        /// <summary>
        /// Command sequence: combination of L (left), R (right), M (move)
        /// </summary>
        public string Commands { get; set; } = string.Empty;
    }
}
