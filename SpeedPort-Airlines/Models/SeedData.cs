using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpeedPort_Airlines.Data; //to find the context class to check the table connection

namespace SpeedPort_Airlines.Models
{
    public class SeedData // To hard code some data direct to the db
    {
        //run the seed data action each time the program loads
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SpeedPort_AirlinesNewContext(
                serviceProvider.GetRequiredService
                <DbContextOptions<SpeedPort_AirlinesNewContext>>()))
            {
                if (context.Promo.Any()) //To check if table is empty

                {
                    return;
                }

                context.Promo.AddRange( //hardcoded data to the promo table
                    
                    new Promo
                    {
                        DestinationCountry = "France",
                        PromoValidity = DateTime.Parse("2021-08-21"),
                        TravelAgency = "Star Travel Agency",
                        PromoPrice = 17000M,
                    },
                    new Promo
                    {
                        DestinationCountry = "Mauritius",
                        PromoValidity = DateTime.Parse("2021-09-15"),
                        TravelAgency = "Tropical Travel Agency",
                        PromoPrice = 18000M,
                    }

                    );
                context.SaveChanges();
            }
        }
    }
}
