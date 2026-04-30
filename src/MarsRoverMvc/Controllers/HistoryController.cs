using Microsoft.AspNetCore.Mvc;
using MarsRoverMvc.Models;
using MarsRoverMvc.Services;

namespace MarsRoverMvc.Controllers
{
    /// <summary>
    /// Controller for the simulation history page
    /// Displays all past simulations and their results
    /// </summary>
    public class HistoryController : Controller
    {
        private readonly IRoverApiService _apiService;
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(IRoverApiService apiService, ILogger<HistoryController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        /// <summary>
        /// GET /History/Index
        /// Displays all historical simulations
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                // Call the API to retrieve all past simulations
                var simulations = await _apiService.GetHistoryAsync();

                // Create a view model with the history data
                var model = new HistoryViewModel
                {
                    // Sort by most recent first
                    Simulations = simulations.OrderByDescending(s => s.ExecutedAt).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading history: {ex.Message}");
                // Return empty history on error
                return View(new HistoryViewModel());
            }
        }
    }
}
