using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class RegistrationReport
    {
        public string ClassName { get; set; }
        public int Capacity { get; set; }

        public int Registered { get; set; }

        public int Confirmed { get; set; }

        public int Id { get; set; }
    }
}