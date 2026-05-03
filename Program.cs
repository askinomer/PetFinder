using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PetFinder.DataAccessLayer;
using PetFinder.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) MVC
builder.Services.AddControllersWithViews();

// 2) DbContext (SQL Server / Azure SQL Edge in Docker)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3) Dependency Injection - Service Layer
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPetAdService, PetAdService>();

// 4) Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.Name = "PetFinder.Auth";
        options.Cookie.HttpOnly = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 5) Auto-create database on startup (development convenience)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANT: Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PetAd}/{action=Index}/{id?}");

app.Run();
