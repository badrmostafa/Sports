using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sports.Models.Classes
{
    public class TypeOfSport
    {
        public int TypeOfSportID { get; set; }
        
        public string Head { get; set; }
        
        public string Description { get; set; }
        
        public string Image { get; set; }
        
        public string Image1 { get; set; }
       
        public int SportID { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //Navigation Property
        public Sport Sport { get; set; }
        public List<Hobby> Hobbies { get; set; }
    }
}