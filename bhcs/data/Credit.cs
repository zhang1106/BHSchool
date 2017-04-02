using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MicroOrm.Dapper.Repositories.Attributes;

namespace data
{
    [Table("credit")]
    public class Credit
    {
        [Key]
        [Identity]
        public int id { get; set; }
        public bool deleted { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime validStart { get; set; }
        public DateTime validEnd { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
        public string email { get; set; }
    }
}
