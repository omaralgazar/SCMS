using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMS.Models;
using SCMS.ViewModels;

namespace SCMS.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        private int CurrentUserId()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            return int.TryParse(userIdStr, out var id) ? id : 0;
        }

        private UserType CurrentUserType()
        {
            var t = HttpContext.Session.GetInt32("UserType");
            return t.HasValue ? (UserType)t.Value : UserType.User;
        }

        public async Task<IActionResult> Index()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Patient)
                .Include(f => f.Doctor)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            var vm = new FeedbackListVm
            {
                Items = feedbacks.Select(f => new FeedbackItemVm
                {
                    FeedbackId = f.FeedbackId,
                    PatientName = f.Patient.FullName,
                    Rate = f.Rate,
                    FeedbackText = f.FeedbackText ?? "",
                    CreatedAt = f.CreatedAt
                }).ToList()
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create(int doctorId)
        {
            return View(new FeedbackFormVm { DoctorId = doctorId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackFormVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = CurrentUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Account");

            if (CurrentUserType() != UserType.Patient)
                return Forbid();

            var patientExists = await _context.Set<Patient>().AnyAsync(p => p.UserId == userId);
            if (!patientExists)
            {
                ModelState.AddModelError("", "هذا المستخدم ليس Patient.");
                return View(vm);
            }

            if (vm.DoctorId <= 0)
            {
                ModelState.AddModelError("", "DoctorId غير صحيح.");
                return View(vm);
            }

            var doctorExists = await _context.Set<Doctor>().AnyAsync(d => d.UserId == vm.DoctorId);
            if (!doctorExists)
            {
                ModelState.AddModelError("", "Doctor غير موجود.");
                return View(vm);
            }

            var feedback = new Feedback
            {
                PatientId = userId,
                DoctorId = vm.DoctorId,
                Rate = vm.Rate,
                FeedbackText = vm.FeedbackText,
                CreatedAt = DateTime.UtcNow
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
