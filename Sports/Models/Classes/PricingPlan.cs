using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sports.Models.Classes
{
    public class PricingPlan
    {
        public int PricingPlanID { get; set; }
        public string Title { get; set; }

        public string Head { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        public string Head1 { get; set; }
        
        public string Head2 { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //Navigation Property
        public List<Client> Clients { get; set; }
    }
}