using FreelanceHub.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelanceHub.Models
{
    public class Vacancy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(8)]
        public string? Title { get; set; }

        [Required]
        [MinLength(20)]
        [MaxLength(4000)]
        public string? Description { get; set; }

        public string? TegLanguage { get; set; }

        public string? UserName { get; set; }
    }
}
