using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace SpeedPort_Airlines.Models
{
    public class DestinationCountryEntity: TableEntity //partitionkey, rowkey
    {
        public DestinationCountryEntity(string Country, string City)
        {
            this.PartitionKey = Country;
            this.RowKey = City;
        }
        public DestinationCountryEntity() { }

        public String Season { get; set; }
        
        public DateTime  PromoExpiry { get; set; }

        public String PromoCode { get; set; }
    }
}

