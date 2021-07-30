using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedPort_Airlines.Areas.Identity.Data
{
    public enum Roles
    {
        Travel_Agency,
        Manager,
        Customer,
        Admin
    }
    public class ContextRole
    {
        public static async Task SeedRolesAsync(UserManager<SpeedPort_AirlinesUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Travel_Agency.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        }


        public static async Task SeedSuperAdminAsync(UserManager<SpeedPort_AirlinesUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new SpeedPort_AirlinesUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Momo-1234");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());

                }

            }
        }
    }
}
