using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sports.Models.Classes
{
    public class Subscribe
    {
        public int SubscribeID { get; set; }
        
        public string Head { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}