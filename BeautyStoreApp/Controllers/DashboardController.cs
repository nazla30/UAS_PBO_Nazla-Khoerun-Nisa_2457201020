using BeautyStoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeautyStoreApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardService _service;

        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View(_service.GetDashboard());
        }
    }
}