
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedDataAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {s
            // Create roles
            string[] roles = { "Manager", "HR" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create default Manager
            string email = "manager@gmail.com";

            var manager = await userManager.FindByEmailAsync(email);

            if (manager == null)
            {
                manager = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    manager,
                    "Manager@123"
                );

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", result.Errors.Select(e => e.Description))
                    );
                }
            }

            // Assign Manager role
            if (!await userManager.IsInRoleAsync(manager, "Manager"))
            {
                await userManager.AddToRoleAsync(manager, "Manager");
            }
        }
    }
}
