using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using data;

namespace web.Models
{
    public class BHClassModel
    {
        public List<classroom> Classrooms { get; set; }
        public List<BHMember> Teachers { get; set; }
        public List<TimeSlotModel> TimeSlots { get; set; }

        public List<course> Courses { get; set; }

        public IEnumerable<SelectListItem> ClassroomList
        {
            get
            {
                return new SelectList(Classrooms, "id", "description");
            }
        }

        public IEnumerable<SelectListItem> TeacherList
        {
            get
            {
                return new SelectList(Teachers, "id", "fullname");
            }
        }

        public IEnumerable<SelectListItem> TimeslotList
        {
            get
            {
                return new SelectList(TimeSlots, "id", "Description");
            }
        }

        public IEnumerable<SelectListItem> CourseList
        {
            get
            {
                return new SelectList(Courses, "id", "name");
            }
        }
        [Display(Name ="Classromm")]
        public int? classroomId { get; set; }
        [Display(Name = "Teacher")]
        public int? teacherId { get; set; }
        [Display(Name = "Time")]
        public int? timeslotId { get; set; }
        [Display(Name = "Course")]
        public  int? courseId { get; set; }
        [Display(Name = "Semester")]
        public string semester { get; set; }
        [Display(Name = "Fee")]
        public decimal? fee { get; set; }
        public bool deleted { get; set; }
        public int id { get; set; }

        public bhclass BHClass { set
            {
                classroomId = value.classroomId;
                teacherId = value.teacherId;
                timeslotId = value.timeslotId;
                courseId = value.courseId;
                semester = value.semester;
                fee = value.fee;
                deleted = value.deleted.Value;
                id = value.id;
            } }
    }
}