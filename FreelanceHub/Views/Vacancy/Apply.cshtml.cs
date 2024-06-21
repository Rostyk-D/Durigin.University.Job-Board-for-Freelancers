using FreelanceHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreelanceHub.Views.Vacancy
{
    public class ApplyModel : PageModel
    {
        public ApplyViewModel? ApplyViewModel { get; set; }

        public void OnGet(int vacancyId)
        {
            ApplyViewModel = new ApplyViewModel
            {
                VacancyId = vacancyId
            };
        }
    }
}
