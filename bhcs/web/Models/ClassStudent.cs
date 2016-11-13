using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using data;
using web.Service;

namespace web.Models
{
    public class ClassStudent : ValidationAttribute
    {
        public IList<BHMember>  Students { get; set; }
        
        public IList<ClassModel> Courses { get; set; }

        public IEnumerable<SelectListItem> StudentList
        {
            get
            {
                return new SelectList(Students, "id", "fullname");
            }
        }

        public IEnumerable<SelectListItem> ClassList
        {
            get
            {
                return new SelectList(Courses, "id", "Course");
            }
        }

        public class_student Class_Student
        {
            set
            {
                id = value.id;
                studentId = value.studentId;
                classId = value.classId;
            }
        }
        public int studentId { get; set; }

        [Display(Name = "Course")]
        public int classId { get; set; }
               
        public int id { get; set; }

        protected override ValidationResult IsValid(object value,
                       ValidationContext validationContext)
        {
            var model = (ClassStudent)validationContext.ObjectInstance;
            var cs = RegistrationSvc.GetClasses().FirstOrDefault(c => c.id == model.classId);
            if(string.IsNullOrWhiteSpace(cs.Warning))
                return new ValidationResult(cs.Warning);

            return ValidationResult.Success;
        }
    }
}