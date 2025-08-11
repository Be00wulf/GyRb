using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using AspNetCoreHero.ToastNotification.Abstractions;
using GyRb.Data;
using GyRb.Models;
using GyRb.Utilites;
using GyRb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace GyRb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApplicationDbContext context,
            IWebHostEnvironment webHostEnviroment,
            INotyfService notyfService,
            UserManager<ApplicationUser> userManager )
        {
            _context = context;
            _notification = notyfService;
            _webHostEnviroment = webHostEnviroment;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var listOfPosts = new List<Post>();

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin)
            {
                listOfPosts = await _context.Posts!.Include(x => x.ApplicationUser).ToListAsync();
            }
            else
            {
                listOfPosts = await _context.Posts!.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser!.Id == loggedInUser!.Id).ToListAsync();
            }

			var listOfPostsVM = listOfPosts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ThumbnailUrl = x.ThumbnailUrl,
                AuthorName = x.ApplicationUser!.FirstName + " " + x.ApplicationUser.LastName
            }).ToList();

			int pageSize = 5;
			int pageNumber = (page ?? 1);

			var pagedList = listOfPostsVM
                .OrderByDescending(x => x.CreatedDate)
                .ToPagedList(pageNumber, pageSize);

			return View(pagedList);
        }

        //actions and functions
        [HttpPost]
        public async Task<IActionResult> GenerateTickets(int postId, int quantity)
        {
            if (quantity <= 0)
            {
                TempData["ErrorMessage"] = "La cantidad debe ser mayor a cero";
                return RedirectToAction("Edit", new { id = postId });
            }

            var tickets = new List<TicketCode>();

            for (int i = 0; i < quantity; i++)
            {
                string code;
                do
                {
                    code = GenerateUniqueCode();
                }
                while (await _context.TicketCodes.AnyAsync(t => t.Code == code));

                tickets.Add(new TicketCode { PostId = postId, Code = code });
            }

            _context.TicketCodes.AddRange(tickets);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"{quantity} tickets generados para el evento {postId}.";
            return RedirectToAction("Edit", new { id = postId });
        }

        private string GenerateUniqueCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }


        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreatePostVM
            {
                FechaExpiracionRegistro = DateTime.Today.AddDays(7)  
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name );
            var post = new Post();
            
            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.Description = vm.Description;
            post.ApplicationUserId = loggedInUser!.Id;
            post.FechaExpiracionRegistro = vm.FechaExpiracionRegistro;


            if (post.Title != null)
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid() ;
            }

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();
            _notification.Success("Post Creado Satisfactoriamente");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == id);

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUser?.Id ==  post?.ApplicationUserId)
            {
                _context.Posts!.Remove(post!);
                await _context.SaveChangesAsync();
                _notification.Success("Post Eliminado Satisfactoriamente");
                return RedirectToAction("Index", "Post", new { area = "Admin" });
            }
            return View();
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == id );
            if(post == null)
            {
                _notification.Error("El Post no ha sido hallado");
                return View();
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);
            if (loggedInUserRole[0] != WebsiteRoles.WebsiteAdmin && loggedInUser!.Id != post.ApplicationUserId )
            {
                _notification.Error("No tienes suficientes permisos para realizar esta accion");
                return RedirectToAction("Index"); 
            }


            var vm = new CreatePostVM()
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Description = post.Description,
                ThumbnailUrl = post.ThumbnailUrl,
                FechaExpiracionRegistro = post.FechaExpiracionRegistro
            };

            return View(vm);
        }

        [HttpPost]
         public async Task<IActionResult> Edit(CreatePostVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if(post == null)
            {
                _notification.Error("El Post no ha sido encontrado");
                return View();
            }

            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.Description = vm.Description;
            post.FechaExpiracionRegistro = vm.FechaExpiracionRegistro;

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);    
            }

            await _context.SaveChangesAsync();
            _notification.Success("Post actualizado exitosamente");
            return RedirectToAction("Index", "Post", new {area = "Admin"} );
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnviroment.WebRootPath, "thumbnails");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filepath = Path.Combine(folderPath, uniqueFileName);

            using(FileStream fileStream = System.IO.File.Create(filepath))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

    }
}
