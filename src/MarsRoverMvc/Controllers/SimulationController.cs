using MarsRoverMvc.Models;
using MarsRoverMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarsRoverMvc.Controllers
{
  /// Controller for the main simulation page
  /// Handles user input for plateau and rover configuration
  /// Communicates with the Web API for simulation execution
  public class SimulationController: Controller
  {
    private readonly IRoverApiService _apiService;
    private readonly ILogger<SimulationController> _logger;

    public SimulationController(IRoverApiService apiService, ILogger<SimulationController> logger)
    {
      _apiService = apiService;
      _logger = logger;
    }

    /// GET /Simulation/Index
    /// Displays the simulation form where users can input plateau and rover data
    public IActionResult Index()
    {
      // Create a new simulation view model with default values
      var model = new SimulationResultViewModel
      {
        PlateauMaxX = 5,
        PlateauMaxY = 5,
        InputRovers = new List<RoverInputViewModel>
                {
                    // Initialize with one empty rover row
                    new RoverInputViewModel { StartX = 0, StartY = 0, StartDirection = "N", Commands = "" }
                }
      };

      return View(model);
    }

    /// POST /Simulation/Run
    /// Processes the simulation form submission
    /// Calls the Web API to run the simulation and displays results
    [HttpPost]
    public async Task<IActionResult> Run(SimulationViewModel model)
    {
      try
      {
        // Validate input
        if (model?.Rovers == null || model.Rovers.Count == 0)
        {
          ModelState.AddModelError("", "At least one rover is required");
          return View("Index", model);
        }

        // Create the API request from the view model
        var request = new SimulationRequest
        {
          PlateauMaxX = model.PlateauMaxX,
          PlateauMaxY = model.PlateauMaxY,
          Rovers = model.Rovers.Select(r => new RoverInputData
          {
            StartX = r.StartX,
            StartY = r.StartY,
            StartDirection = r.StartDirection,
            Commands = r.Commands
          }).ToList()
        };

        // Call the Web API to run the simulation
        var response = await _apiService.SimulateAsync(request);

        if (response == null)
        {
          ModelState.AddModelError("", "Error communicating with the simulation service");
          return View("Index", model);
        }

        // Convert the API response to an updated view model
        var resultModel = new SimulationResultViewModel
        {
          SimulationId = response.SimulationId,
          PlateauMaxX = model.PlateauMaxX,
          PlateauMaxY = model.PlateauMaxY,
          InputRovers = model.Rovers,
          ExecutedAt = response.ExecutedAt,
          Results = response.Rovers.Select(r => new RoverResultData
          {
            RoverId = r.RoverId,
            FinalX = r.FinalX,
            FinalY = r.FinalY,
            FinalDirection = r.FinalDirection,
            Path = r.Path,
            Commands = r.Commands
          }).ToList()
        };

        return View("Index", resultModel);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Simulation error: {ex.Message}");
        ModelState.AddModelError("", "An unexpected error occurred");
        return View("Index", model);
      }
    }
  }
}