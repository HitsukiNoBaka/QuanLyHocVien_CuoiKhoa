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
    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        const string userName = "basic-admin";
        const string email = "admin@example.com";
        const string fullName = "Quản trị viên";
        const string password = "Admin@1234";

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

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
                    $"Không tạo được tài khoản Admin mẫu: {errors}");
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