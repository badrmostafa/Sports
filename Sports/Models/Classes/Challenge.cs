using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sports.Models.Classes
{
    public class Challenge
    {
        public int ChallengeID { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Head { get; set; }
        
        public string Description1 { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}