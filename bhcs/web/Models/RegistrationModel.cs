using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class RegistrationModel
    {
        public int id { get; set; }
        public string Household { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int NumberofStudents { get; set; }
        public int NumberofClasses { get; set; }
        public int NumberofConfirmedClasses { get; set; }
    }
}