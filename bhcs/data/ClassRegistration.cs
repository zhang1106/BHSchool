using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace data
{
    public class ClassRegistration
    {
        [Key]
        public int id { get; set; }
        public member Student { get; set; }
        public IEnumerable<bhclass> Classes {get;set;}
    }
}
