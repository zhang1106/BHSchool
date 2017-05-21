using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using data;
using web.Models;

namespace web.Controllers
{
     public class bhclassesController : Controller
    {
        private bhcsEntities db = new bhcsEntities();

        // GET: bhclasses
        public ActionResult Index()
        {
            var classes = from c in db.bhclasses
                          join cs in db.courses on c.courseId equals cs.id
                          join cr in db.classrooms on c.classroomId equals cr.id
                          join tl in db.timeslots on c.timeslotId equals tl.id
                          join t in db.members on c.teacherId equals t.id
                          where c.deleted == false
                          select new ClassModel() { id = c.id, Deleted=c.deleted.Value, Classroom = cr.description, Semester=c.semester, Fee=c.fee,  Course = cs.name, Time = tl.start + "-" + tl.end, Teacher=t.firstname + " " + t.lastname };

            return View(classes.OrderBy(c=>c.Course).ToList());
        }

        // GET: bhclasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bhclass bhclass = db.bhclasses.Find(id);
            if (bhclass == null)
            {
                return HttpNotFound();
            }
            return View(bhclass);
        }

        [Authorize(Roles = "Admin")]
        // GET: bhclasses/Create
        public ActionResult Create()
        {
            var model = GetModel();
            return View(model);
        }

        // POST: bhclasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "id,timeslotId,teacherId,classroomId,courseId,fee,semester")] BHClassModel bhclass)
        {
            if (ModelState.IsValid)
            {
                var newClass = new bhclass() { timeslotId = bhclass.timeslotId, classroomId = bhclass.classroomId, courseId = bhclass.courseId, fee = bhclass.fee, teacherId = bhclass.teacherId, semester = bhclass.semester };
                newClass.deleted = false;
                db.bhclasses.Add(newClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bhclass);
        }

        // GET: bhclasses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bhclass bhclass = db.bhclasses.Find(id);
            if (bhclass == null)
            {
                return HttpNotFound();
            }

            var model = GetModel();
            model.BHClass = bhclass;
            
            return View(model);
        }

        private BHClassModel GetModel()
        {
            var model = new BHClassModel()
            {
                Classrooms = db.classrooms.ToList(),
                Teachers = db.members.Where(m => m.type == "teacher").Select(m => new BHMember() { id = m.id, fullname = m.firstname + " " + m.lastname }).ToList(),
                TimeSlots = db.timeslots.Select(t=>new TimeSlotModel() {id=t.id, Description=t.start + " - " + t.end }).ToList(),
                Courses = db.courses.ToList()
            };

            return model;
        }

        // POST: bhclasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "id,timeslotId,teacherId,classroomId,courseId,fee,semester,deleted,createdAt,modifiedAt,modifiedBy")] bhclass bhclass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bhclass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bhclass);
        }

        // GET: bhclasses/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bhclass bhclass = db.bhclasses.Find(id);
            if (bhclass == null)
            {
                return HttpNotFound();
            }
            return View(bhclass);
        }

        // POST: bhclasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            bhclass bhclass = db.bhclasses.Find(id);
            bhclass.deleted = true;
            db.Entry(bhclass).State = EntityState.Modified;
            db.SaveChanges();
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
