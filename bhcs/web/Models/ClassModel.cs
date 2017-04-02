using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using data;

namespace web.Models
{
    public class ClassModel
    {
        public string Semester { get; set; }
        public string Course { get; set; }
        public string Classroom { get; set; }
        public string Teacher { get; set; }
        public int? TeacherId { get; set; }
        public string Time {get;set;}

        public bool Deleted { get; set; }

        public int Capacity { get; set; }
        public int NumberofRegistration { get; set; }
        public int NumberofConfirmed { get; set; }
        
        public decimal? Fee { get; set; }
        public int id { get; set; }
        public string Warning
        {
            get
            {
                if (NumberofRegistration >= Capacity)
                    return " warning:Too many registrations already";
                if (NumberofConfirmed >= Capacity)
                    return " warning:Registration closed";
                return string.Empty;
            }
        }

        public IList<BHStudent> Confirmed { get; set; }
        public IList<BHStudent> UnConfirmed { get; set; }
    
    }
}