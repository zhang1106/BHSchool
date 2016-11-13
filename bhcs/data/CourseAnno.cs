using System.ComponentModel.DataAnnotations;

namespace data
{
    [MetadataType(typeof(CourseAnno))]
    public partial class course
    {
    }

    public sealed class CourseAnno
    {
        [Display(Name="课程")]
        public string name { get; set; }
    }
}
