using System.Diagnostics;
using GyRb.Data;
using GyRb.Models;
using GyRb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;
using X.PagedList;

namespace GyRb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

		public IActionResult Index(int? page)
		{
			var vm = new HomeVM();

			var setting = _context.Settings!.FirstOrDefault();
			if (setting != null)
			{
				vm.Title = setting.Title;
				vm.ShortDescription = setting.ShortDescription;
				vm.ThumbnailUrl = setting.ThumbnailUrl;
			}

			int pageSize = 4;
			int pageNumber = page ?? 1;

			vm.Posts = _context.Posts!
				.Include(x => x.ApplicationUser)
				//.OrderByDescending(p => p.Id) 
				.OrderByDescending(p => p.CreatedDate) 
				.ToPagedList(pageNumber, pageSize);

			return View(vm);
		}


		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
