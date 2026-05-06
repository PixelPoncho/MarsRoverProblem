using MarsRoverMvc.Models.Simulations;
using MarsRoverMvc.Models.Api;

namespace MarsRoverMvc.Services
{
  /// Interface for communication with the Rover Web API
  /// Handles all HTTP calls to the backend service
  public interface IRoverApiService
  {
    /// Calls the Web API to run a rover simulation
    /// <param name="request">The simulation request data</param>
    /// <returns>The simulation response with results</returns>
    Task<SimulationResponse?> SimulateAsync(SimulationRequest request);

    /// Calls the Web API to retrieve all simulation history
    /// <returns>List of historical simulations</returns>
    Task<List<SimulationSummary>> GetHistoryAsync();

    /// Calls the Web API to save a screenshot
    /// <param name="simulationId">The simulation ID</param>
    /// <param name="screenshotBase64">The screenshot in base64 format</param>
    /// <returns>Success indicator</returns>
    Task<bool> SaveScreenshotAsync(string simulationId, string screenshotBase64);
  }
}