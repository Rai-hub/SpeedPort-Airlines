using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpeedPort_Airlines.Models
{
    public class Promo //making table structure of Promo
    {
        //defining column needed and to generate crud table

        public int ID {get; set;}

        public string DestinationCountry { get; set; }
        [Display(Name = "Destination Country")]

        public DateTime PromoValidity { get; set; }
        [Display(Name = "Promo Validity")]

        public string TravelAgency { get; set; }
        [Display(Name = "Travel Agency")]

        public decimal PromoPrice { get; set; }
        [Display(Name = "Promo Price")]
    }
}
