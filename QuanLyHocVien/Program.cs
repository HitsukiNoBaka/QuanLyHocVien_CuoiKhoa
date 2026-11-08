using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyHocVien.Components;
using QuanLyHocVien.Data;
using QuanLyHocVien.Models;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();

// Cấu hình database SQLite
var dataDirectory = Path.Combine(
    builder.Environment.ContentRootPath,
    "App_Data");

Directory.CreateDirectory(dataDirectory);

var databasePath = Path.Combine(dataDirectory, "QuanLyHocVien.db");

builder.Services.AddDbContextFactory<ApplicationDbContext>(
    options => options.UseSqlite($"Data Source={databasePath}"));

// Cấu hình Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
});

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Chạy migration trước khi seed roles và tài khoản
using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider
        .GetRequiredService<IDbContextFactory<ApplicationDbContext>>();

    await using var db = await dbFactory.CreateDbContextAsync();
    await db.Database.MigrateAsync();

    await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);

    if (app.Environment.IsDevelopment())
    {
        await IdentitySeeder.SeedAdminAsync(scope.ServiceProvider);
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapRazorPages();

app.Run();