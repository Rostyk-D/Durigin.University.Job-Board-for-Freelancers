using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreelanceHub.Models;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Identity;
using FreelanceHub.Areas.Identity.Data;

namespace FreelanceHub.Controllers
{
    [Authorize]
    public class VacancyController : Controller
    {
        private readonly VacancyContext _context;

        public VacancyController(VacancyContext context)
        {
            _context = context;
        }

        // GET: Vacancy
        public async Task<IActionResult> Index(string searchString, string tagFilter)
        {
            // Query vacancies from the database
            var vacancies = from v in _context.Vacancy select v;

            // Apply search filter if searchString is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                vacancies = vacancies.Where(v => v.Title.Contains(searchString));
            }

            // Apply tag filter if tagFilter is provided
            if (!string.IsNullOrEmpty(tagFilter))
            {
                vacancies = vacancies.Where(v => v.TegLanguage.Contains(tagFilter));
            }

            // Convert query result to a list and pass it to the view
            return View(await vacancies.ToListAsync());
        }

        // GET: Vacancy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Vacancy/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Description,TegLanguage")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                // Отримати ім'я поточного користувача
                string userName = User.Identity.Name;

                // Встановити ім'я користувача для вакансії
                vacancy.UserName = userName;

                // Зберегти вакансію
                _context.Add(vacancy);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(vacancy);
        }

        private bool VacancyExists(int id)
        {
            return _context.Vacancy.Any(e => e.Id == id);
        }

        // GET: Vacancy/Apply/5
        public async Task<IActionResult> Apply(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancy.FindAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }

            // Перевіряємо, чи користувач вже подавав заявку на цю вакансію
            var userId = User.Identity.Name;
            var existingApplication = await _context.Applications.FirstOrDefaultAsync(a => a.VacancyId == id && a.UserId == userId);

            if (existingApplication != null)
            {
                // Якщо користувач вже подавав заявку, перенаправляємо його на сторінку з повідомленням
                return RedirectToAction("AlreadyApplied", new { id = id });
            }

            var applyViewModel = new ApplyViewModel
            {
                VacancyId = vacancy.Id,
                UserId = userId
            };

            return View(applyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(ApplyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Маппінг моделі представлення на сутність Application
                var application = new ApplyViewModel
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Message = model.Message,
                    VacancyId = model.VacancyId,
                    UserId = model.UserId
                };

                // Перевіряємо, чи користувач вже подавав заявку на цю вакансію
                var existingApplication = await _context.Applications.FirstOrDefaultAsync(a => a.VacancyId == model.VacancyId && a.UserId == model.UserId);

                if (existingApplication != null)
                {
                    // Якщо користувач вже подавав заявку, перенаправляємо його на сторінку з повідомленням
                    return RedirectToAction("AlreadyApplied", new { id = model.VacancyId });
                }

                // Додаємо сутність Application до контексту бази даних і зберігаємо зміни
                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // Додатковий метод для сторінки з повідомленням, коли користувач вже подавав заявку
        public IActionResult AlreadyApplied(int id)
        {
            ViewData["VacancyId"] = id;
            return View();
        }
    }
}
