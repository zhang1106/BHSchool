using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class MyClass
    {
        public ClassModel ClassDetail { get; set; }
        public IList<ClassModel> MyClasses { get; set; }

        public IEnumerable<SelectListItem> ClassList
        {
            get
            {
                return new SelectList(MyClasses, "id", "Course");
            }
        }

        [Display(Name = "Class Name")]
        public int classId { get; set; }
    }
}