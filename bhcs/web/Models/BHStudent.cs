using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class BHStudent
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime Born { get; set; }
        public string Custodian { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}