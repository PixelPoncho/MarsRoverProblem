namespace MarsRoverMvc.Models
{
    /// <summary>
    /// ViewModel for the simulation form
    /// Collects all input data from the user for rover simulation
    /// </summary>
    public class SimulationViewModel
    {
        /// <summary>
        /// Maximum X coordinate of the plateau (width)
        /// </summary>
        public int PlateauMaxX { get; set; } = 5;

        /// <summary>
        /// Maximum Y coordinate of the plateau (height)
        /// </summary>
        public int PlateauMaxY { get; set; } = 5;

        /// <summary>
        /// Collection of rovers to be simulated
        /// </summary>
        public List<RoverInputViewModel> Rovers { get; set; } = new List<RoverInputViewModel>();
    }

    /// <summary>
    /// ViewModel for individual rover input
    /// </summary>
    public class RoverInputViewModel
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
        /// Starting direction: N, E, S, or W
        /// </summary>
        public string StartDirection { get; set; } = "N";

        /// <summary>
        /// Command sequence for this rover
        /// </summary>
        public string Commands { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel for displaying simulation results
    /// </summary>
    public class SimulationResultViewModel
    {
        /// <summary>
        /// Unique ID of the simulation
        /// </summary>
        public string SimulationId { get; set; } = string.Empty;

        /// <summary>
        /// Plateau dimensions
        /// </summary>
        public int PlateauMaxX { get; set; }
        public int PlateauMaxY { get; set; }

        /// <summary>
        /// Original input rovers data
        /// </summary>
        public List<RoverInputViewModel> InputRovers { get; set; } = new List<RoverInputViewModel>();

        /// <summary>
        /// Results for each rover
        /// </summary>
        public List<RoverResultData> Results { get; set; } = new List<RoverResultData>();

        /// <summary>
        /// Timestamp of execution
        /// </summary>
        public DateTime ExecutedAt { get; set; }
    }

    /// <summary>
    /// Data for a single rover result
    /// </summary>
    public class RoverResultData
    {
        public int RoverId { get; set; }
        public int FinalX { get; set; }
        public int FinalY { get; set; }
        public string FinalDirection { get; set; } = string.Empty;
        public List<string> Path { get; set; } = new List<string>();
        public string Commands { get; set; } = string.Empty;
    }
}
