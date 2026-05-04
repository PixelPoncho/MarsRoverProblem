namespace MarsRoverWebApi.Models
{
  /// Represents the input data for a rover simulation
  /// Contains plateau dimensions and all rovers to be simulated
  public class SimulationRequest
  {
    /// Maximum X coordinate of the plateau
    public int PlateauMaxX { get; set; }

    /// Maximum Y coordinate of the plateau
    public int PlateauMaxY { get; set; }

    /// Collection of rovers to simulate
    /// Each rover contains starting position and command sequence
    public List<RoverInputData> Rovers { get; set; } = new List<RoverInputData>();
  }

  /// Input data for a single rover
  public class RoverInputData
  {
    /// Starting X coordinate
    public int StartX { get; set; }

    /// Starting Y coordinate
    public int StartY { get; set; }

    /// Starting direction (N, E, S, or W)
    public string StartDirection { get; set; } = "N";

    /// Command sequence: combination of L (left), R (right), M (move)
    public string Commands { get; set; } = string.Empty;
  }
}