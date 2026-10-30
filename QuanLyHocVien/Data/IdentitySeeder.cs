using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using QuanLyHocVien.Models;

namespace QuanLyHocVien.Data;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles =
        {
            AppRoles.Admin,
            AppRoles.Staff,
            AppRoles.Student
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(role));

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        result.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(errors);
                }
            }
        }
    }
    public static async Task SeedAdminAsync(
    IServiceProvider services,
    IConfiguration configuration)
    {
        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        var userName = configuration["SeedAdmin:UserName"];
        var email = configuration["SeedAdmin:Email"];
        var fullName = configuration["SeedAdmin:FullName"];
        var password = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(userName)
            || string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(fullName)
            || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Thiếu cấu hình tài khoản quản trị viên trong User Secrets.");
        }

        var admin = await userManager.FindByNameAsync(userName);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(admin, password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    createResult.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Không tạo được tài khoản quản trị viên: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, AppRoles.Admin))
        {
            var roleResult =
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    roleResult.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Không gán được vai trò Admin: {errors}");
            }
        }
    }
}