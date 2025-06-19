using AspNetCoreHero.ToastNotification.Abstractions;
using GyRb.Data;
using GyRb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GyRb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PageController(ApplicationDbContext context, INotyfService notification, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
        }

        //about
        [HttpGet]
        public async Task<IActionResult> About()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "about");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> About(PageVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "about");
            if (page == null)
            {
                _notification.Error("Page not found");
                return View();
            }

            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("About page updated successfully.");
            return RedirectToAction("About", "Page", new { area = "Admin"});
        }

        

        //contact
        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "contact");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(PageVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "contact");
            if (page == null)
            {
                _notification.Error("Page not found");
                return View();
            }

            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Contact page updated successfully.");
            return RedirectToAction("Contact", "Page", new { area = "Admin" });
        }

        //privcy policy
        [HttpGet]
        public async Task<IActionResult> Privacy()
        {
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "privacy");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Privacy(PageVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var page = await _context.Pages!.FirstOrDefaultAsync(x => x.Slug == "privacy");
            if (page == null)
            {
                _notification.Error("Page not found");
                return View();
            }

            page.Title = vm.Title;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if (vm.Thumbnail != null)
            {
                page.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            _notification.Success("Privacy page updated successfully.");
            return RedirectToAction("Privacy", "Page", new { area = "Admin" });
        }


        //upload image
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filepath = Path.Combine(folderPath, uniqueFileName);

            using (FileStream fileStream = System.IO.File.Create(filepath))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }


    }
}
