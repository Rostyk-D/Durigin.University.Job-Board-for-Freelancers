using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FreelanceHub.Data;
using FreelanceHub.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "1097392614913-7nhkdj0ipm4ikl7kjkh5lnc37ffvv6p1.apps.googleusercontent.com";
    options.ClientSecret = "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";
});
var connectionString = builder.Configuration.GetConnectionString("FreelanceHubContextConnection") ?? throw new InvalidOperationException("Connection string 'FreelanceHubContextConnection' not found.");

builder.Services.AddDbContext<FreelanceHubContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<FreelanceHubUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FreelanceHubContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
