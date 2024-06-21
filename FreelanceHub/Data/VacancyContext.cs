using FreelanceHub.Models;
using Microsoft.EntityFrameworkCore;

public class VacancyContext : DbContext
{
    public VacancyContext(DbContextOptions<VacancyContext> options) : base(options)
    {
    }

    public DbSet<Vacancy> Vacancy { get; set; }
    public DbSet<ApplyViewModel> Applications { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vacancy>().HasKey(v => v.Id);
    }
}
