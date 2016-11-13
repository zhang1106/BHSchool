using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using data;

namespace web.Models
{
    public class TimeSlotModel
    {
       
        public TimeSlotModel( )
        {
            
        }

        public int id
        { get; set; }

        public string Description
        {
            get; set; }
        }
    }
