using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using data;
using web.Service;

namespace web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class configsController : Controller
    {
        private bhcsEntities db = new bhcsEntities();

        // GET: configs
        public ActionResult Index()
        {
            return View(db.configs.ToList());
        }

        // GET: configs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            config config = db.configs.Find(id);
            if (config == null)
            {
                return HttpNotFound();
            }

            return View(config);
        }

        // GET: configs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: configs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "type,key,value,description")] config config)
        {
            if (ModelState.IsValid)
            {
                db.configs.Add(config);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(config);
        }

        // GET: configs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            config config = db.configs.Find(id);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }

        // POST: configs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,type,key,value,description")] config config)
        {
            if (ModelState.IsValid)
            {
                db.Entry(config).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(config);
        }

        // GET: configs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            config config = db.configs.Find(id);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }

        // POST: configs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            config config = db.configs.Find(id);
            db.configs.Remove(config);
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
