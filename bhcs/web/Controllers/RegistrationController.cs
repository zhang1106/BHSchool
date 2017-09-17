using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using System.IO;
using Microsoft.AspNet.Identity;
using web.Models;
using data;
using web.Service;

namespace web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegistrationController : Controller
    {
        private bhcsEntities db = new bhcsEntities();

        // GET: Registration
        public ActionResult Index()
        {
            var registrations = GetRegistrations();
            return View(registrations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(FormCollection collection)
        {
            var searchfor = collection["searchfor"];
            var registrations = GetRegistrations().Where(r => r.Household.ToLower().Contains(searchfor) || r.Email.ToLower().Contains(searchfor));
            return View("Index",registrations);
        }

        protected IList<RegistrationModel> GetRegistrations()
        {
            var households = db.members.Select(m => new { id = m.id, householdId = m.householdId == null ? m.id : m.householdId });

            var students = from m in households
                           group m by m.householdId
                           into result
                           select new { id = result.Key.Value, NumberofStudents = result.Count() };

            var classes = from c in db.class_students
                          join bc in db.bhclasses.Where(c => c.deleted == false) on c.classId equals bc.id
                          join h in households on c.studentId equals h.id
                          group c by h.householdId
                          into result
                          select new { id = result.Key.Value, NumberofClasses = result.Count(), confirmed = result.Where(r => r.status == RegistrationStatus.Confirmed.ToString()).Count() };

            var registration = from m in db.members.Where(m => m.householdId == null)
                               join s in students on m.id equals s.id
                               join c in classes on m.id equals c.id
                               select new RegistrationModel() { id = m.id, Household = m.firstname + " " + m.lastname,
                                   Email = m.email, Phone = m.phone, NumberofClasses = c.NumberofClasses,
                                   NumberofStudents = s.NumberofStudents, NumberofConfirmedClasses = c.confirmed
                               };

            return registration.ToList();
        }

        public ActionResult MyClass(int? id)
        {
            var email = User.Identity.GetUserName();
            var user = db.members.FirstOrDefault(m => m.email == email);
            var userid = user.id;
            var myClass = RegistrationSvc.GetMyClasses(userid, id);
            return View(myClass);
        }

        [HttpPost]
        public ActionResult LoadMyClass(MyClass model)
        {
            var email = User.Identity.GetUserName();
            var user = db.members.FirstOrDefault(m => m.email == email);
            var userid = user.id;
            var myClass = RegistrationSvc.GetMyClasses(userid, model.classId);
            return View("MyClass", myClass);
        }

        public ActionResult Summary()
        {
            var classes = from cs in db.class_students
                          join c in db.bhclasses.Where(c=>c.deleted==false) on cs.classId equals c.id
                          join cr in db.courses on c.courseId equals cr.id 
                          join cl in db.classrooms on c.classroomId equals cl.id
                          group cs by new { cs.classId, cr.name, cl.capacity } into result
                          
                          select new RegistrationReport { Id = result.Key.classId, Capacity = result.Key.capacity.Value, ClassName = result.Key.name, Registered = result.Count(), Confirmed = result.Where(r => r.status == RegistrationStatus.Confirmed.ToString()).Count() };

            return View(classes);
        }

        public void Export()
        {
            var registrations = GetRegistrations();
            string parents = "Name, Parent, Phone, Email\r\n";
            foreach (var p in registrations)
            {
                if (p.NumberofConfirmedClasses == 0) continue;
                parents += ($"{p.Household},{p.Phone},{p.Email}\r\n");
            }

            ExportToClient(parents, "parents");
        }

        private void ExportToClient(string text, string filename)
        {
            Response.ContentType = "text/csv";
            //Get the physical path to the file.
            //Write the file directly to the HTTP content output stream.
            Response.AddHeader("Content-Disposition", $"attachment;filename={filename}.csv");
            Response.Write(text);
          
            Response.End();
        }

        public void ExportStudents(int id)
        {
            var detail = RegistrationSvc.GetClassDetails(id); 
            string course = "Name, Parent, Phone, Email\r\n";
            foreach (var s in detail.Confirmed)
            {
                course += $"{s.Name},{s.Custodian},{s.Phone},{s.Email}\r\n";
            }
            ExportToClient(course, detail.Course);
        }

        // GET: Registration/Details/5
        public ActionResult Details(int id)
        {
            var summary = RegistrationSvc.GetClassStudentSummary(id);
                    
            return View(summary);
        }

        // GET: Registration/Details/5
        public ActionResult ClassDetails(int id)
        {
            var classRegistrationDetail = RegistrationSvc.GetClassDetails(id);
            return View(classRegistrationDetail);
        }

        // GET: Registration/Details/5
        [AllowAnonymous]
        public ActionResult Email(int id)
        {
            var summary = RegistrationSvc.GetClassStudentSummary(id);
            return View(summary);
        }


        // GET: Registration/Create
        public ActionResult Create(int id)                                                              
        {  
            var classStudent = RegistrationSvc.GetStudentClass(id);

            return View("ClassRegistration",classStudent);
        }

        // POST: Registration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClassRegistration([Bind(Include = "ClassId,StudentId")] ClassStudent csModel)
        {
            if (ModelState.IsValid)
            {
                var cs = new class_student() { classId = csModel.classId, studentId = csModel.studentId, status = RegistrationStatus.Active.ToString() };
                //DBHelper.InsertStudentClass(csModel.studentId, csModel.classId, 0);
                db.class_students.Add(cs);
                db.SaveChanges();
            }

            var memeber = db.members.Find(csModel.studentId);
            var summary = RegistrationSvc.GetClassStudentSummary(memeber.householdId.HasValue ? memeber.householdId.Value : memeber.id);

            return View("Details", summary);
        }

        // GET: Registration/Edit/5
        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.members.FirstOrDefault(m => m.id == id.Value);
            var students = db.members.Where(s => s.id == id || s.householdId == id);
            var classes = from c in db.class_students
                          join s in students on c.studentId equals s.id
                          select c;
            
            foreach(var c in classes)
            {
                if (c.status != RegistrationStatus.Confirmed.ToString() && c.status != RegistrationStatus.CancelConfirmed.ToString())
                {
                    c.status = c.status == RegistrationStatus.Active.ToString() ?
                        RegistrationStatus.Confirmed.ToString() : RegistrationStatus.CancelConfirmed.ToString();
                    c.modifiedBy = User.Identity.GetUserName();
                    db.Entry(c).State = EntityState.Modified;
                }
            }

            //get confirm body
            var url = HttpContext.Request.Url.OriginalString; // ("details", "email");
            var emailUrl = url.Replace("Confirm", "Details");
            var body = web.Service.RegistrationSvc.GetWebPage(emailUrl);

            var summary = RegistrationSvc.GetClassStudentSummary(id.Value);
            //add transaction record
            var transaction = new transaction()
            {
                memberId = user.id,
                status = TransactionStatus.Confirmed.ToString(),
                amount = summary.Total,
                updatedAt = DateTime.Now,
                updatedBy = User.Identity.GetUserName()
            };
            db.transactions.Add(transaction);
            //save after get the body info
            db.SaveChanges();
            foreach(var cs in summary.Classes)
            {
                if (cs.Confirmed) continue;
                var detail = new transactiondetail()
                {
                    transactionId = transaction.id,
                    ItemId = cs.id,
                    transactionType = TransactionType.Registration.ToString(),
                    updatedAt = DateTime.Now,
                    updatedBy = User.Identity.GetUserName()
                };
                db.transactiondetails.Add(detail);
            }
           
            foreach(var fee in summary.Fee)
            {
                if (decimal.Parse(fee.value) != 0)
                {
                    var detail = new transactiondetail()
                    {
                        transactionId = transaction.id,
                        ItemId = fee.id,
                        transactionType = TransactionType.Fee.ToString(),
                        description = fee.description,
                        updatedAt = DateTime.Now,
                        updatedBy = User.Identity.GetUserName()
                    };
                    db.transactiondetails.Add(detail);
                }
            }

            db.SaveChanges();
            
            BHEmail email = new BHEmail() { Subject = "Your registration is confirmed", Body=body, From = "info@huaxiabh.org",
                To = user.email };

            email.Send();

            var updateSummary = RegistrationSvc.GetClassStudentSummary(id.Value);

            return View("Details", updateSummary);
        }

        // POST: Registration/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Registration/Delete/5
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cs = db.class_students.Find(id);
            if (cs == null)
            {
                return HttpNotFound();
            }
            return View(cs);
        }

        // POST: messages/Delete/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            var cs = db.class_students.Find(id);
            cs.status = RegistrationStatus.CancelActive.ToString();
            db.Entry(cs).State = EntityState.Modified;
            db.SaveChanges();

            var memeber = db.members.Find(cs.studentId);
            var summary = RegistrationSvc.GetClassStudentSummary(memeber.householdId.HasValue ? memeber.householdId.Value : memeber.id);

            return View("Details", summary); 
        }

        // GET: Registration/Delete/5
        public ActionResult Delete(int id)
        {
            var cs = db.class_students.Find(id);
            RegistrationSvc.DeleteClass(id);

            var memeber = db.members.Find(cs.studentId);
            var summary = RegistrationSvc.GetClassStudentSummary(memeber.householdId.HasValue?memeber.householdId.Value:memeber.id);

            return View("Details", summary);
        }

        // POST: Registration/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
