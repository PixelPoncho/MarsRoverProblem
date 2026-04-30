namespace MarsRoverMvc.Models
{
    /// ViewModel for the simulation form
    /// Collects all input data from the user for rover simulation
    public class SimulationViewModel
    {
        /// Maximum X coordinate of the plateau (width)
        public int PlateauMaxX { get; set; } = 5;

        /// Maximum Y coordinate of the plateau (height)
        public int PlateauMaxY { get; set; } = 5;

        /// Collection of rovers to be simulated
        public List<RoverInputViewModel> Rovers { get; set; } = new List<RoverInputViewModel>();
    }

    /// ViewModel for individual rover input
    public class RoverInputViewModel
    {
        /// Starting X coordinate
        public int StartX { get; set; }

        /// Starting Y coordinate
        public int StartY { get; set; }

        /// Starting direction: N, E, S, or W
        public string StartDirection { get; set; } = "N";

        /// Command sequence for this rover
        public string Commands { get; set; } = string.Empty;
    }

    /// ViewModel for displaying simulation results
    public class SimulationResultViewModel
    {
        /// Unique ID of the simulation
        public string SimulationId { get; set; } = string.Empty;

        /// Plateau dimensions
        public int PlateauMaxX { get; set; }
        public int PlateauMaxY { get; set; }

        /// Original input rovers data
        public List<RoverInputViewModel> InputRovers { get; set; } = new List<RoverInputViewModel>();

        /// Results for each rover
        public List<RoverResultData> Results { get; set; } = new List<RoverResultData>();

        /// Timestamp of execution
        public DateTime ExecutedAt { get; set; }
    }

    /// Data for a single rover result
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
