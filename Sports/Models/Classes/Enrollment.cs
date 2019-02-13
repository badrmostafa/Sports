using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Sports.Models.Classes
{
    public enum Grade:byte
    {
        Students= 8,
        Professional= 19,
        Agency= 49,
        Enterprise= 79

    }
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
      
        public string FullName { get; set; }
       
        public int Age { get; set; }
      
        public string Email { get; set; }
        
        public Grade Grade  { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        

    }
}