using System.ComponentModel.DataAnnotations;

namespace FreelanceHub.Models
{
    public class ApplyViewModel
    {
        [Key]
        public int Id { get; set; } // Додано поле для ідентифікатора заявки

        [Required(ErrorMessage = "Please enter your full name")]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter your message")]
        [Display(Name = "Message")]
        public string? Message { get; set; }
        public int? VacancyId { get; set; }
        public string? UserId { get; set; } // це UserName приклад admin@admin.com
    }
}
