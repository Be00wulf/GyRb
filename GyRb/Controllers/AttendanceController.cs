using GyRb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult EnterCode()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnterCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "Debe ingresar un código.");
                return View();
            }

            var ticket = await _context.TicketCodes
                .FirstOrDefaultAsync(t => t.Code == code);

            if (ticket == null || ticket.UsedAt != null)
            {
                ModelState.AddModelError("", "El código es inválido o ya fue usado.");
                return View();
            }

            

            TempData["ValidCode"] = ticket.Code;
            return RedirectToAction("RegisterAttendee");
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
