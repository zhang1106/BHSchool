using System;
using System.Collections.Generic;
using System.Linq;
using web.Models;
using data;

namespace web.Service
{
    public class ClassSvc
    {
        public static IList<ClassModel> GetClasses(bool active)
        {
            using (bhcsEntities db = new bhcsEntities())
            {
                var classes = from c in db.bhclasses
                              join cs in db.courses on c.courseId equals cs.id
                              join cr in db.classrooms on c.classroomId equals cr.id
                              join tl in db.timeslots on c.timeslotId equals tl.id
                              join t in db.members on c.teacherId equals t.id
                              where c.deleted.Value != active
                              select new ClassModel() { id = c.id, Deleted = c.deleted.Value, Classroom = cr.description,
                                  Semester = c.semester, Fee = c.fee, Course = cs.name,
                                  Time = tl.start + "-" + tl.end, Teacher = t.firstname + " " + t.lastname };

                return classes.ToList<ClassModel>();
            }
        }
    }
}