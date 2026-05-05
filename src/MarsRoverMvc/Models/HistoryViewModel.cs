namespace MarsRoverMvc.Models
{
  /// ViewModel for displaying the history page
  /// Shows all past simulations with their results
  public class HistoryViewModel
  {
    /// Collection of all past simulations
    public List<SimulationHistoryItem> Simulations { get; set; } = new List<SimulationHistoryItem>();
  }

  /// Represents a single item in the simulation history
  public class SimulationHistoryItem
  {
    /// Unique simulation identifier
    public string SimulationId { get; set; } = string.Empty;

    /// Plateau dimensions (e.g., "5 x 5")
    public string PlateauSize { get; set; } = string.Empty;

    /// When the simulation was executed
    public DateTime ExecutedAt { get; set; }

    /// Final results for each rover
    public List<RoverResultData> Results { get; set; } = new List<RoverResultData>();

    public string ScreenshotDataUri { get; set; } = string.Empty;
  }
}