using MarsRoverMvc.Models.Rovers;

namespace MarsRoverMvc.Models.Simulations
{
    /// ViewModel for the simulation form
    /// Collects all input data from the user for rover simulation
    public class SimulationResultViewModel : SimulationSummary
    {
        public List<RoverInputViewModel> InputRovers { get; set; } = new();
    }
}