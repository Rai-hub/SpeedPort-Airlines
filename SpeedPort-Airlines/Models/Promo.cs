using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedPort_Airlines.Models
{
    public class Promo //making table structure of Promo
    {
        //defining column needed and to generate crud table

        public int ID {get; set;}

        public string DestinationCountry { get; set; }

        public DateTime PromoValidity { get; set; }

        public string TravelAgency { get; set; }

        public decimal PromoPrice { get; set; }
    }
}
