using MarsRoverMvc.Models;
using MarsRoverMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarsRoverMvc.Controllers
{
  /// Controller for the simulation history page
  /// Displays all past simulations and their results
  public class HistoryController : Controller
  {
    private readonly IRoverApiService _apiService;
    private readonly ILogger<HistoryController> _logger;

    public HistoryController(IRoverApiService apiService, ILogger<HistoryController> logger)
    {
      _apiService = apiService;
      _logger = logger;
    }

    /// GET /History/Index
    /// Displays all historical simulations
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
        _logger.LogError(ex, "Error loading history page");
        // Return empty history on error
        return View(new HistoryViewModel());
      }
    }
  }
}