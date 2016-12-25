using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Routing;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using data;
using web.Models;
using web.Service;

namespace web.Controllers
{
    public class class_studentController : Controller
    {
        private bhcsEntities db = new bhcsEntities();

        // GET: class_student
        public ActionResult Index()
        {
            Session["ReturnUrl"] = "/class_student";
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else 
            {
                var uid = User.Identity.GetUserName();
                var u = db.members.FirstOrDefault(m => m.email == uid ); 
                if(u == null)
                {
                    return RedirectToAction("Create", "FamilyAcct");
                }

               
                var summary =  GetClassStudentSummary(u.id);

                return View(summary);
            };
        }

        protected ClassStudentSummary GetClassStudentSummary(int id)
        {
           return RegistrationSvc.GetClassStudentSummary(id);
        }
        
        public ActionResult GetClass(int id)
        {
            var classInfo = RegistrationSvc.GetClasses().FirstOrDefault(c => c.id == id);
            return new JsonResult() { Data = classInfo };
        }

        //public ActionResult Balance()
        //{
        //    //var balances = db.balances.Where(b => b.email == SiteHelper.UserName).Select(b=>b);
        //    return View(balances);
        //}

        public ActionResult Print()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var uid = User.Identity.GetUserName();
                var u = db.members.FirstOrDefault(m => m.email == uid);
                if (u == null)
                {
                    return RedirectToAction("Create", "FamilyAcct");
                }

                var summary = GetClassStudentSummary(u.id);

                return View(summary);
            };
        }
               
        public ActionResult CreateStudent()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Session["ReturnUrl"] = "/class_student/Create";
            return RedirectToAction("CreateStudent", "FamilyAcct");
        }
        // GET: class_student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            class_student class_student = db.class_students.Find(id);
            if (class_student == null)
            {
                return HttpNotFound();
            }
            return View(class_student);
        }

        // GET: class_student/Create
        public ActionResult Create()
        {
            if (SiteHelper.RegsitrationClosed) return RedirectToAction("Index"); 
            if(!Request.IsAuthenticated) return RedirectToAction("Login", "Account");

            var uid = User.Identity.GetUserName();
            var u = db.members.FirstOrDefault(m => m.email == uid);
            if (u == null)
            {
                return RedirectToAction("Create", "FamilyAcct");
            }

            var classStudent = RegistrationSvc.GetStudentClass(u);
            return View(classStudent);
        }
        
        // POST: class_student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassId,StudentId")] ClassStudent csModel)
        {
            if (ModelState.IsValid)
            {
                var cs = new class_student() { classId = csModel.classId, studentId = csModel.studentId, status = RegistrationStatus.Active.ToString() };
                //DBHelper.InsertStudentClass(csModel.studentId, csModel.classId, 0);
                db.class_students.Add(cs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(csModel);
        }

        // GET: class_student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (SiteHelper.RegsitrationClosed) return RedirectToAction("Index");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var uid = User.Identity.GetUserName();
            var u = db.members.FirstOrDefault(m => m.email == uid);
            if (u == null)
            {
                return RedirectToAction("Create", "FamilyAcct");
            }

            var classStudent = RegistrationSvc.GetStudentClass(u);
            class_student class_student = db.class_students.Find(id);
            
            if (class_student == null)
            {
                return HttpNotFound();
            }

            classStudent.Class_Student = class_student;

            return View(classStudent);
        }

        // POST: class_student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,classId,studentId")] class_student class_student)
        {
            if (ModelState.IsValid)
            {
                class_student.status = RegistrationStatus.Active.ToString();
                db.Entry(class_student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(class_student);
        }

        // GET: class_student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (SiteHelper.RegsitrationClosed) return RedirectToAction("Index");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            class_student class_student = db.class_students.Find(id);
            if (class_student == null)
            {
                return HttpNotFound();
            }
            return View(class_student);
        }

        // POST: class_student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //check to make sure the user is not trying to delete other's stuff
            var uid = User.Identity.GetUserName();
            var u = db.members.FirstOrDefault(m => m.email == uid);
            if (u == null)
            {
                return new HttpUnauthorizedResult();
            }
            var students = GetClassStudentSummary(u.id);
            if(!students.Classes.Any(c => c.id == id))
            {
                return new HttpUnauthorizedResult();
            }

            RegistrationSvc.DeleteClass(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
