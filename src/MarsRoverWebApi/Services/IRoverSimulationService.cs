using MarsRoverWebApi.Models;

namespace MarsRoverWebApi.Services
{
    /// Interface for rover simulation logic
    /// Handles the core business logic of moving rovers on the plateau
    public interface IRoverSimulationService
    {
        /// Executes a complete simulation of rovers on a plateau
        /// <param name="request">The simulation request containing plateau and rover data</param>
        /// <returns>The simulation response with final positions and paths</returns>
        SimulationResponse Simulate(SimulationRequest request);
    }
}
