namespace MarsRoverMvc.Models
{
    /// <summary>
    /// ViewModel for displaying the history page
    /// Shows all past simulations with their results
    /// </summary>
    public class HistoryViewModel
    {
        /// <summary>
        /// Collection of all past simulations
        /// </summary>
        public List<SimulationHistoryItem> Simulations { get; set; } = new List<SimulationHistoryItem>();
    }

    /// <summary>
    /// Represents a single item in the simulation history
    /// </summary>
    public class SimulationHistoryItem
    {
        /// <summary>
        /// Unique simulation identifier
        /// </summary>
        public string SimulationId { get; set; } = string.Empty;

        /// <summary>
        /// Plateau dimensions (e.g., "5 x 5")
        /// </summary>
        public string PlateauSize { get; set; } = string.Empty;

        /// <summary>
        /// Number of rovers in the simulation
        /// </summary>
        public int RoverCount { get; set; }

        /// <summary>
        /// When the simulation was executed
        /// </summary>
        public DateTime ExecutedAt { get; set; }

        /// <summary>
        /// Final results for each rover
        /// </summary>
        public List<RoverResultData> Results { get; set; } = new List<RoverResultData>();
    }
}
