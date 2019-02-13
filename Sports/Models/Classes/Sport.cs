using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sports.Models.Classes
{
    public class Sport
    {
        
        public int SportID { get; set; }
        
        public string Head1 { get; set; }
       
        public string Description { get; set; }
        
        public string Head2 { get; set; }
        
        public string Image { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //Navigation Property
        public List<TypeOfSport> TypesOfSports { get; set; }
    }
}