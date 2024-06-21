using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Додайте цей using
using Microsoft.EntityFrameworkCore;
using FreelanceHub.Areas.Identity.Data;
using FreelanceHub.Models;
using static System.Net.Mime.MediaTypeNames;

namespace FreelanceHub.Data
{
    public class FreelanceHubContext : IdentityDbContext<FreelanceHubUser>
    {
        public FreelanceHubContext(DbContextOptions<FreelanceHubContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Vacancy>().HasKey(v => v.Id);
            modelBuilder.Entity<ApplyViewModel>().HasKey(a => a.Id);
        }
    }
}
