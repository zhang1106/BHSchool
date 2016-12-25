using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using data;
using web.Models;

namespace web.Service
{
    public class RegistrationSvc  
    {
        public static ClassStudentSummary GetClassStudentSummary(int id)
        {
            using (bhcsEntities db = new bhcsEntities())
            {
                var user = db.members.FirstOrDefault(m => m.id == id);

                var address = db.addresses.FirstOrDefault(a => a.memberId == id);

                var students = db.members.Where(s => s.id == id || s.householdId == id);

                var classes = GetClasses();

                var classStudents = (from s in students
                                    join cs in db.class_students on s.id equals cs.studentId
                                    join cl in db.bhclasses on cs.classId equals cl.id
                                    join c in db.courses on cl.courseId equals c.id
                                    join t in db.timeslots on cl.timeslotId equals t.id
                                    select new 
                                    { Student = s, Class = cl, Course = c, id = cs.id, TimeSlot = t, Status=cs.status, Confirmed= 
                                      cs.status==RegistrationStatus.Confirmed.ToString(), ModifiedAt=cs.modifiedAt}).ToList();

                var fClasses = new List<ClassStudentModel>();
                foreach(var cs in classStudents)
                {
                    var registered = db.class_students.Where(rc => rc.modifiedAt < cs.ModifiedAt).ToList();
                    var c = classes.FirstOrDefault(cl => cl.id == cs.Class.id);
                    var csm = new ClassStudentModel()
                    {
                        Student = cs.Student,
                        Class = cs.Class,
                        Course = cs.Course,
                        id = cs.id,
                        TimeSlot = cs.TimeSlot,
                        Status = cs.Status
                    };
                    
                    if(c != null && registered.Count() >= c.Capacity)
                    {
                        csm.Warning = c.Warning;
                    }
                    fClasses.Add(csm);
                }

                using (var confgDB=DataRepository<Config>.Create())
                {
                    var fee = confgDB.FindAll(c => c.type == "fee" || (c.type == "discount" && c.key == user.email)).OrderBy(f => f.description).ToList();
                    if (classStudents.Any(c => c.Confirmed)) fee = new List<Config>();
                    var summary = new ClassStudentSummary() { Classes = fClasses, Fee = fee, Address = address, Household = user };
                    return summary;
                }
            }
        }

        public static string GetWebPage(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                // Add a user agent header in case the 
                // requested URI contains a query.
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                var data = client.OpenRead(url);
                var reader = new StreamReader(data);
                string s = reader.ReadToEnd();

                return s;
            }
        }

        public static IList<ClassModel> GetClasses()
        {
            using (bhcsEntities db = new bhcsEntities())
            {
                var classes = from c in db.bhclasses
                              join cs in db.courses on c.courseId equals cs.id
                              join cr in db.classrooms on c.classroomId equals cr.id
                              join tl in db.timeslots on c.timeslotId equals tl.id
                              where c.deleted.Value == false 
                              select new { id = c.id, Capacity = cr.capacity.Value, Classroom = cr.name, Course = cs.name,
                                  Time = tl.start + "-" + tl.end, Semester=c.semester, Fee=c.fee };
                var registered = from cs in db.class_students
                                 group cs by cs.classId into result
                                 select new { id = result.Key, numberOfRegistered = result.Count(), numberOfConfirmed = result.Where(r => r.status == RegistrationStatus.Confirmed.ToString()).Count() };

                var courses = (from c in classes
                              join r in registered on c.id equals r.id into rc
                              from subClass in rc.DefaultIfEmpty()
                              orderby c.Course
                              select new ClassModel()
                              {
                                  id = c.id,
                                  Classroom = c.Classroom,
                                  Course = c.Course,
                                  Time = c.Time,
                                  Semester = c.Semester,
                                  Fee = c.Fee,
                                  NumberofConfirmed = subClass == null? 0:subClass.numberOfConfirmed,
                                  NumberofRegistration = subClass == null ? 0 : subClass.numberOfRegistered,
                                  Capacity = c.Capacity
                              }).ToList();
                return courses;
            }
        }

        /// <summary>
        /// get details about a class registration
        /// </summary>
        public static ClassModel GetClassDetails(int classId)
        {
            var thisClass = GetClasses().FirstOrDefault(c=>c.id==classId);
            using (bhcsEntities db = new bhcsEntities())
            {
                var custodians = from m in db.members
                                select new { m.id, cid = m.householdId == null ? m.id : m.householdId };


                var confirmedStudents = from cs in db.class_students
                               join m in db.members on cs.studentId equals m.id
                               join c in custodians on m.id equals c.id
                               join mc in db.members on c.cid equals mc.id
                               where cs.classId == classId && cs.status == RegistrationStatus.Confirmed.ToString()
                               select new BHStudent() { id = cs.studentId, Born = m.born, Email = mc.email,
                                   Phone = mc.phone, Gender = m.gender, Name = m.firstname + " " + m.lastname
                                   , Custodian = mc.firstname + " " + mc.lastname
                               }                  
                               ;
                var unConfirmedStudents = from cs in db.class_students
                                        join m in db.members on cs.studentId equals m.id
                                        join c in custodians on m.id equals c.id
                                        join mc in db.members on c.cid equals mc.id
                                        where cs.classId == classId && cs.status != RegistrationStatus.Confirmed.ToString()
                                        select new BHStudent()
                                        {
                                            id = cs.studentId,
                                            Born = m.born,
                                            Email = mc.email,
                                            Phone = mc.phone,
                                            Gender = m.gender,
                                            Name = m.firstname + " " + m.lastname
                                            ,
                                            Custodian = mc.firstname + " " + mc.lastname
                                        }
                              ;

                thisClass.Confirmed = confirmedStudents.ToList();
                thisClass.UnConfirmed = unConfirmedStudents.ToList();

            }

            return thisClass;
        }

        public static ClassStudent GetStudentClass(int id)
        {
            member u;
            using (bhcsEntities db = new bhcsEntities())
            {
                u = db.members.FirstOrDefault(m => m.id == id);
            }
            return GetStudentClass(u);
        }

        public static ClassStudent GetStudentClass(member u)
        {
            using (bhcsEntities db = new bhcsEntities())
            {
                var students = db.members.Where(s => s.id == u.id || s.householdId == u.id).Select(s => new BHMember { id = s.id, fullname = s.firstname + " " + s.lastname });
                var classes = RegistrationSvc.GetClasses();
                var classStudent = new ClassStudent();

                classStudent.Courses = classes;
                classStudent.Students = students.ToList();


                return classStudent;
            }
        }

        public static void DeleteClass(int id)
        {
            using (bhcsEntities db = new bhcsEntities())
            {
                class_student class_student = db.class_students.Find(id);
                db.class_students.Remove(class_student);
                db.SaveChanges();
            }
        }

    }
}