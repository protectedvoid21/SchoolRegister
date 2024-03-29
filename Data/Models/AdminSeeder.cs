﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Models {
    public class AdminSeeder : ISeeder {
        public async Task SeedAsync(SchoolContext dbContext, IServiceProvider serviceProvider) {
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            var isExisting = await userManager.Users.AnyAsync(u => u.UserName == GlobalConstants.AdministratorName);
            if (isExisting) {
                return;
            }

            AppUser adminUser = new() {
                UserName = GlobalConstants.AdministratorName,
                Name = "Admin",
                Surname = "Admin"
            };

            var result = await userManager.CreateAsync(adminUser, GlobalConstants.AdministratorPassword);
            if (result.Succeeded) {
                var isRoleExisting = await roleManager.RoleExistsAsync(GlobalConstants.AdministratorRoleName);
                if (!isRoleExisting) {
                    throw new Exception("Missing admin role");
                }

                await userManager.AddToRoleAsync(adminUser, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}