using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpeedPort_Airlines.Data;
using SpeedPort_Airlines.Models;
using SpeedPort_Airlines.Controllers;
using Microsoft.AspNetCore.Identity;
using SpeedPort_Airlines.Areas.Identity.Data;

namespace SpeedPort_Airlines
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //call the initialize functions
            ServicebusController.Initialize();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SpeedPort_AirlinesNewContext>();
                    var userManager = services.GetRequiredService<UserManager<SpeedPort_AirlinesUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await ContextRole.SeedSuperAdminAsync(userManager, roleManager);
                    await ContextRole.SeedRolesAsync(userManager, roleManager);
                    //check for migration
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
                host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
