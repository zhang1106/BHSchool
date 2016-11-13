using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using data;

namespace web.Models
{
    public class FamilyAcctViewModel
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public int id { get; set; }
        public int memeberId { get; set; }
    }
}