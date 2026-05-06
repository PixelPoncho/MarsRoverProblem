using MarsRoverMvc.Models.Simulations;

namespace MarsRoverMvc.Models
{
  /// ViewModel for displaying the history page
  /// Shows all past simulations with their results
  public class HistoryViewModel
  {
    /// Collection of all past simulations
    public List<SimulationSummary> Simulations { get; set; } = new();
  }
}