using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TrainingAPI.Models;

public static class SeedData
{
    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var config = serviceProvider.GetRequiredService<IConfiguration>();
        var adminUserName = "admin";
        var adminEmail = config["AdminUser:Email"];
        var adminPassword = config["AdminUser:Password"];

        if (!await roleManager.RoleExistsAsync("ADMIN"))
        {
            await roleManager.CreateAsync(new IdentityRole("ADMIN"));
        }

        var existingUser = await userManager.FindByEmailAsync(adminEmail);
        if (existingUser == null)
        {
            var user = new User
            {
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "ADMIN");
            }
        }
    }
}
