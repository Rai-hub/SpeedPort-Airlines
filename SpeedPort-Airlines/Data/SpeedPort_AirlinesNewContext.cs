using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpeedPort_Airlines.Models;

namespace SpeedPort_Airlines.Data
{
    public class SpeedPort_AirlinesNewContext : DbContext
    {
        public SpeedPort_AirlinesNewContext (DbContextOptions<SpeedPort_AirlinesNewContext> options)
            : base(options)
        {
        }

        public DbSet<SpeedPort_Airlines.Models.Promo> Promo { get; set; }
    }
}
