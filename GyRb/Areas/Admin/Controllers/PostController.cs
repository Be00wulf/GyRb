using Microsoft.AspNetCore.Mvc;

namespace GyRb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
