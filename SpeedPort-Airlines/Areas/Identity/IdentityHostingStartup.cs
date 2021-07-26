using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpeedPort_Airlines.Areas.Identity.Data;
using SpeedPort_Airlines.Data;

[assembly: HostingStartup(typeof(SpeedPort_Airlines.Areas.Identity.IdentityHostingStartup))]
namespace SpeedPort_Airlines.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SpeedPort_AirlinesContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SpeedPort_AirlinesContextConnection")));

                services.AddDefaultIdentity<SpeedPort_AirlinesUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<SpeedPort_AirlinesContext>();
            });
        }
    }
}