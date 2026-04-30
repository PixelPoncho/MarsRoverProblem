using MarsRoverWebApi.Models;

namespace MarsRoverWebApi.Services
{
    /// <summary>
    /// Interface for rover simulation logic
    /// Handles the core business logic of moving rovers on the plateau
    /// </summary>
    public interface IRoverSimulationService
    {
        /// <summary>
        /// Executes a complete simulation of rovers on a plateau
        /// </summary>
        /// <param name="request">The simulation request containing plateau and rover data</param>
        /// <returns>The simulation response with final positions and paths</returns>
        SimulationResponse Simulate(SimulationRequest request);
    }
}
