using Microsoft.AspNetCore.Mvc;
using MarsRoverMvc.Models;

namespace MarsRoverMvc.Controllers
{
    /// Controller for the home/dashboard page
    /// Provides navigation to main features
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// GET /Home/Index
        /// Displays the home/dashboard page with navigation links
        public IActionResult Index()
        {
            return View();
        }

        /// Handles errors
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    /// ViewModel for error page
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
