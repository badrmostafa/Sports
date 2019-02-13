using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sports.Models.Classes
{
    public class Video
    {
        public int VideoID { get; set; }
        
       
        public string Video_url { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}