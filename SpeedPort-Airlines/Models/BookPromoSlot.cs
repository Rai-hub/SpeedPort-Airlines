using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedPort_Airlines.Models
{
    public class BookPromoSlot
    {
        public int ID { get; set; }

        [Display(Name = "Travel Agency")]
        public string TravelAgencyName { get; set; }

        [Display(Name = "Destination Country")]
        public string DestinationCountryName { get; set; }

        [Display(Name = "Special Flight Deal")]
        public string SpecialFlightDeal { get; set; }
    }
}
