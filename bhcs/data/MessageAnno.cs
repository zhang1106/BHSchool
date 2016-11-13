using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
 
namespace data
{
    [MetadataType(typeof(MessageAnno))]
    public partial class message
    {
    }

    public sealed class MessageAnno
    {
        [DataType(DataType.MultilineText)]
   
        public string body { get; set; }
    }
}
