using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MicroOrm.Dapper.Repositories.Attributes;


namespace data
{

    [Table("config")]
    public class Config
    {
        [Key]
        [Identity]
        public int id { get; set; }
        public string type { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public DateTime StartDt { get; set; }
        public DateTime EndDt { get; set; }
        public DateTime modifiedAt { get; set; }
    }
}