using Microsoft.AspNetCore.Mvc;
using MarsRoverMvc.Models;

namespace MarsRoverMvc.Controllers
{
    /// <summary>
    /// Controller for the home/dashboard page
    /// Provides navigation to main features
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET /Home/Index
        /// Displays the home/dashboard page with navigation links
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET /Home/About
        /// Displays information about the application
        /// </summary>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Handles errors
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    /// <summary>
    /// ViewModel for error page
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
