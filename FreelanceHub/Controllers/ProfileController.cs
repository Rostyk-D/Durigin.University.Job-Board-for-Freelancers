using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreelanceHub.Models;
using Microsoft.AspNetCore.Authorization;

namespace FreelanceHub.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly VacancyContext _context;

        public ProfileController(VacancyContext context)
        {
            _context = context;
        }

        // GET: Profile/MyApplications /////// Це до робити 
        public IActionResult MyApplications()
        {
            // Отримання ідентифікатора поточного користувача
            var userId = User.Identity.Name;

            // Зчитування заявок користувача з бази даних
            var applications = _context.Applications
                .Where(a => a.UserId == userId)
                .Include(a => a.VacancyId) // Завантаження пов'язаної вакансії
                .ToList();

            // Передача списку заявок на відповідну сторінку для відображення
            return View(applications);
        }
    }
}
