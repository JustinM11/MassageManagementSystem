using MassageManagementSystem.Models;
using MassageManagementSystem.Models.Data;
using MassageManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Use AddControllersWithViews() if you serve Razor Views.
builder.Services.AddControllersWithViews();

// Configure the DbContext with SQL Server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ASP.NET Core Identity for ApplicationUser.
// This makes UserManager<ApplicationUser> and SignInManager<ApplicationUser> available.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Optionally configure Cookie Authentication (if you want login/logout using cookies).
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // If your AuthController is serving views, consider setting these to /Auth/Login, /Auth/AccessDenied
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

// Register custom services for payment and email processing.
builder.Services.AddScoped<IPaymentService, PayPalService>();
builder.Services.AddScoped<IEmailService, GmailEmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/api/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map API controllers (if you have any).
app.MapControllers();

// Map the default MVC route (for Razor views).
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
