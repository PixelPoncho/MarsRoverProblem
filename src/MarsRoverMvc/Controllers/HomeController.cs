using Microsoft.AspNetCore.Mvc;

namespace MarsRoverMvc.Controllers
{
  /// Controller for the home page
  /// Provides navigation to main features
  public class HomeController: Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    /// GET /Home/Index
    /// Displays the home page with navigation links
    public IActionResult Index()
    {
      return View();
    }
  }
}