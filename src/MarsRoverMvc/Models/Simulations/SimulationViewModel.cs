using MarsRoverMvc.Models.Rovers;

namespace MarsRoverMvc.Models.Simulations
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
}

