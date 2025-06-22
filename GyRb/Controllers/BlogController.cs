using AspNetCoreHero.ToastNotification.Abstractions;
using GyRb.Data;
using GyRb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GyRb.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }

        public BlogController(ApplicationDbContext context, INotyfService notification)
        {
            _notification = notification;
            _context = context;
        }

        [HttpGet("[controller]/{slug}")]
        public IActionResult Post(string slug)
        {
            if(slug == "")
            {
                _notification.Error("El post no ha sido hallado");
                return RedirectToAction("Index", "Home");
            }
            var post = _context.Posts!.Include(x => x.ApplicationUser).FirstOrDefault(x => x.Slug == slug);
            if(post == null)
            {
                _notification.Error("El post no ha sido hallado");
                return View();
            }

            var vm = new BlogPostVM()
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.ApplicationUser!.FirstName + " " + post.ApplicationUser.LastName,
                CreatedDate = post.CreatedDate,
                ThumbnailUrl = post.ThumbnailUrl,
                Description = post.Description,
                ShortDescription = post.ShortDescription
            };
            return View(vm);
        }
    }
}
