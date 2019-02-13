using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sports.Models.Classes
{
    public class Client
    {
        public int ClientID { get; set; }
        
        public string Head { get; set; }
       
        public string Image { get; set; }
        
        public string Description { get; set; }
        
        public int PricingPlanID { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //Navigation Property
        public PricingPlan PricingPlan { get; set; }
        public List<Hobby> Hobbies { get; set; }
    }
}