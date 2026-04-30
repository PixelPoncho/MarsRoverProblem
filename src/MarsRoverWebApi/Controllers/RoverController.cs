using Microsoft.AspNetCore.Mvc;
using MarsRoverWebApi.Models;
using MarsRoverWebApi.Services;

namespace MarsRoverWebApi.Controllers
{
    /// <summary>
    /// REST API controller for rover simulation operations
    /// Provides endpoints for running simulations and managing history
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoverController : ControllerBase
    {
        // Service dependencies injected via constructor
        private readonly IRoverSimulationService _simulationService;
        private readonly IHistoryRepository _historyRepository;

        public RoverController(
            IRoverSimulationService simulationService,
            IHistoryRepository historyRepository)
        {
            _simulationService = simulationService;
            _historyRepository = historyRepository;
        }

        /// <summary>
        /// POST /api/rover/simulate
        /// Executes a rover simulation with the provided plateau and rover data
        /// </summary>
        /// <param name="request">The simulation request containing plateau dimensions and rover details</param>
        /// <returns>The simulation response with final positions and paths</returns>
        [HttpPost("simulate")]
        public async Task<ActionResult<SimulationResponse>> Simulate([FromBody] SimulationRequest request)
        {
            try
            {
                // Validate input
                if (request?.Rovers == null || request.Rovers.Count == 0)
                {
                    return BadRequest(new { error = "At least one rover is required" });
                }

                // Run the simulation
                // This is where the core rover movement logic is executed
                var response = _simulationService.Simulate(request);

                // Save the simulation to history for tracking
                await _historyRepository.SaveSimulationAsync(new
                {
                    response.SimulationId,
                    PlateauSize = $"{request.PlateauMaxX} x {request.PlateauMaxY}",
                    RoverCount = response.Rovers.Count,
                    response.ExecutedAt,
                    Results = response.Rovers
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception (in production, use proper logging)
                Console.WriteLine($"Simulation error: {ex.Message}");
                return StatusCode(500, new { error = "Simulation failed", details = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/rover/history
        /// Retrieves all historical simulations
        /// </summary>
        /// <returns>List of all past simulations</returns>
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<object>>> GetHistory()
        {
            try
            {
                var history = await _historyRepository.GetAllSimulationsAsync();
                return Ok(history);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving history: {ex.Message}");
                return StatusCode(500, new { error = "Failed to retrieve history" });
            }
        }

        /// <summary>
        /// POST /api/rover/save-screenshot
        /// Saves a screenshot of the plateau visualization
        /// </summary>
        /// <param name="simulationId">The ID of the simulation</param>
        /// <param name="screenshotData">The screenshot data in base64 format</param>
        /// <returns>Success confirmation</returns>
        [HttpPost("save-screenshot")]
        public async Task<ActionResult> SaveScreenshot([FromQuery] string simulationId, [FromBody] ScreenshotData screenshotData)
        {
            try
            {
                if (string.IsNullOrEmpty(simulationId) || string.IsNullOrEmpty(screenshotData?.ImageBase64))
                {
                    return BadRequest(new { error = "Simulation ID and screenshot data are required" });
                }

                // Save the screenshot with its simulation ID for future reference
                await _historyRepository.SaveScreenshotAsync(simulationId, screenshotData.ImageBase64);

                return Ok(new { message = "Screenshot saved successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving screenshot: {ex.Message}");
                return StatusCode(500, new { error = "Failed to save screenshot" });
            }
        }
    }

    /// <summary>
    /// Model for screenshot submission
    /// </summary>
    public class ScreenshotData
    {
        /// <summary>
        /// The image data encoded in base64 format
        /// This allows binary image data to be transmitted as JSON
        /// </summary>
        public string? ImageBase64 { get; set; }
    }
}
