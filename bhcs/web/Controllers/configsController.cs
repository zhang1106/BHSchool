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
        private DataRepository<Config> db = DataRepository<Config>.Create();

        // GET: configs
        public ActionResult Index()
        {
            return View(db.FindAll());
        }

        // GET: configs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var config = db.Find(c=>c.id==id);
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
        public ActionResult Create([Bind(Include = "type,key,value,description")] Config config)
        {
            if (ModelState.IsValid)
            {
                db.Update(config);
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
            var config = db.Find(c=>c.id==id);
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
        public ActionResult Edit([Bind(Include = "id,type,key,value,description")] Config config)
        {
            if (ModelState.IsValid)
            {
                db.Update(config);
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
            var config = db.Find(c=>c.id==id);
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
            var config = db.Find(c=>c.id==id);
            db.Delete(config);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Connection.Close();
            }
            base.Dispose(disposing);
        }
    }
}
