using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDWORKS_STUDENT.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(IDWORKS_STUDENT.Pages.Role.Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(IDWORKS_STUDENT.Pages.Role.Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(IDWORKS_STUDENT.Pages.Role.Enums.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(IDWORKS_STUDENT.Pages.Role.Enums.Roles.Basic.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityUser
            {
                UserName = IDWORKS_STUDENT.AppConfig.IDWorksAppConfig.DefaultSuperAdmin,
                Email = IDWORKS_STUDENT.AppConfig.IDWorksAppConfig.DefaultSuperAdmin,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, IDWORKS_STUDENT.AppConfig.IDWorksAppConfig.DefaultSuperAdminPassword);
                    await userManager.AddToRoleAsync(defaultUser, IDWORKS_STUDENT.Pages.Role.Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, IDWORKS_STUDENT.Pages.Role.Enums.Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, IDWORKS_STUDENT.Pages.Role.Enums.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, IDWORKS_STUDENT.Pages.Role.Enums.Roles.SuperAdmin.ToString());
                }

            }
        }
    }
}
