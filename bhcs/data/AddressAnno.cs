using System.ComponentModel.DataAnnotations;

namespace data
{
    [MetadataType(typeof(AddressAnno))]
    public partial class address
    {
    }

    public sealed class AddressAnno
    {
        [Display(Name = "州")]
        public string state { get; set; }
    }
    
}
