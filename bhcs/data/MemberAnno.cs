using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace data
{
    [MetadataType(typeof(MemberAnno))]
    public partial class member
    {
    }

    public sealed class MemberAnno
    {
        [DisplayFormat(DataFormatString = "{0:M/d/yyyy}")]
        [Display(Name="Birthday")]

        public DateTime born { get; set; }
    }
}
