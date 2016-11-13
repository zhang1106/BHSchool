using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using data;

namespace web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class classroomsController : Controller
    {
        private bhcsEntities db = new bhcsEntities();

        // GET: classrooms
        public ActionResult Index()
        {
            return View(db.classrooms.ToList());
        }

        // GET: classrooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classroom classroom = db.classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        // GET: classrooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: classrooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name,capacity,function,description")] classroom classroom)
        {
            if (ModelState.IsValid)
            {
                db.classrooms.Add(classroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(classroom);
        }

        // GET: classrooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classroom classroom = db.classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        // POST: classrooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,capacity,function,description")] classroom classroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classroom);
        }

        // GET: classrooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classroom classroom = db.classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        // POST: classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            classroom classroom = db.classrooms.Find(id);
            db.classrooms.Remove(classroom);
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
