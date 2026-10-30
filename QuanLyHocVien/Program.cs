using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyHocVien.Components;
using QuanLyHocVien.Data;
using QuanLyHocVien.Models;
using QuanLyHocVien.Components;
using Radzen;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();

//Xác định nơi lưu database
var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(dataDirectory);
var databasePath = Path.Combine(dataDirectory, "QuanLyHocVien.db");

// 3. Đăng ký Entity Framework với SQLite
builder.Services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlite($"Data Source={databasePath}"));

//4. Đăng ký tài khoản và vai trò
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);//Nhập sai 5 lần => Khóa 10p
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 5. Đường dẫn đăng nhập
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
});

// 6. Đăng ký dịch vụ phân quyền
builder.Services.AddAuthorization();
builder.Services.AddRazorPages();
builder.Services.AddCascadingAuthenticationState(); 
var app = builder.Build();

//Kiểm tra xem đã có 3 vai trò chưa , nếu chưa thì tạo, nếu có rồi thì không tạo lại
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
    await IdentitySeeder.SeedAdminAsync(scope.ServiceProvider, app.Configuration);

}

// 7. Cấu hình xử lý lỗi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Thứ tự: xác thực → phân quyền → chống giả mạo yêu cầu
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapRazorPages();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

