using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FreelanceHub.Data;
using FreelanceHub.Areas.Identity.Data;
using FreelanceHub.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthentication().AddGoogle(options =>
        {
            options.ClientId = "1097392614913-7nhkdj0ipm4ikl7kjkh5lnc37ffvv6p1.apps.googleusercontent.com";
            options.ClientSecret = "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";
        });

        var connectionString = builder.Configuration.GetConnectionString("FreelanceHubContextConnection") ?? throw new InvalidOperationException("Connection string 'FreelanceHubContextConnection' not found.");

        builder.Services.AddDbContext<FreelanceHubContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddDbContext<VacancyContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<FreelanceHubUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FreelanceHubContext>();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "applyVacancy",
            pattern: "Vacancy/Apply/{id?}",
            defaults: new { controller = "Vacancy", action = "Apply" }
        );

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Manager" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<FreelanceHubUser>>();

            string email = "admin@admin.com";
            string password = "%iT?QWV5N64SAv6";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new FreelanceHubUser();
                user.UserName = email;
                user.Email = email;

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
        await app.RunAsync();
    }
}
