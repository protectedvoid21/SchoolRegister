using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Models;

public class RolesSeeder : ISeeder {
    public async Task SeedAsync(SchoolContext dbContext, IServiceProvider serviceProvider) {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (string roleName in GlobalConstants.RequiredRoles) {
            bool exists = await roleManager.RoleExistsAsync(roleName);
            if (!exists) {
                await roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }
    }
}