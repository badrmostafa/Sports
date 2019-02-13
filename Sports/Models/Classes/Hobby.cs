using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sports.Models.Classes
{
    public class Hobby
    {
        public int HobbyID { get; set; }
        
      
        public int TypeOfSportID { get; set; }
        
        public int ClientID { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //Navigation Property
        public TypeOfSport TypeOfSport { get; set; }
        public Client Client { get; set; }
    }
}