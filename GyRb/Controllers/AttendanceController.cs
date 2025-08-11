using GyRb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GyRb.ViewModels;

namespace GyRb.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EnterCode(int id)
        {
            var model = new TicketCodeVM { PostId = id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EnterCode(TicketCodeVM model)
        {
            Console.WriteLine($"PostId recibido: {model.PostId}, Code: {model.Code}");

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Error: Corrige los errores e intenta de nuevo";
                return View(model);
            }

            var ticket = await _context.TicketCodes
                .FirstOrDefaultAsync(t => t.Code == model.Code && t.PostId == model.PostId);

            if (ticket != null)
            {
                ViewBag.SuccessMessage= "EL CODIGO ES VALIDO, SE LE REDIRECCIONARA A LA PANTALLA DE REGISTRO";
                // redireccion -> pendiente
            }
            else
            {
                ViewBag.ErrorMessage = "El código ingresado no es válido para este evento";
            }

            return View(model);
        }



        public IActionResult RegisterAttendee()
        {
            if (TempData["ValidCode"] is not string code || string.IsNullOrWhiteSpace(code))
            {
                return RedirectToAction("EnterCode");
            }

            ViewBag.Code = code;
            return View();
        }




    }
}
