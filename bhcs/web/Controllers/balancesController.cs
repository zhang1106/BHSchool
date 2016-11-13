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
    [Authorize(Roles = "Admin")]
    public class balancesController : Controller
    {
        private bhcsEntities db = new bhcsEntities();

        // GET: balances
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.balances.ToList());
        }

        // GET: balances/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            balance balance = db.balances.Find(id);
            if (balance == null)
            {
                return HttpNotFound();
            }
            return View(balance);
        }

        // GET: balances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: balances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "id,email,amount,type,description")] balance balance)
        {
            if (ModelState.IsValid)
            {
                balance.updated_by = web.Service.SiteHelper.UserName;
                balance.updated_at = DateTime.Now;
                db.balances.Add(balance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(balance);
        }

        // GET: balances/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            balance balance = db.balances.Find(id);
            if (balance == null)
            {
                return HttpNotFound();
            }
            return View(balance);
        }

        // POST: balances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "id,email,amount,type,description")] balance balance)
        {
            if (ModelState.IsValid)
            {
                balance.updated_at = DateTime.Now;
                balance.updated_by = web.Service.SiteHelper.UserName;
                db.Entry(balance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(balance);
        }

        // GET: balances/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            balance balance = db.balances.Find(id);
            if (balance == null)
            {
                return HttpNotFound();
            }
            return View(balance);
        }

        // POST: balances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            balance balance = db.balances.Find(id);
            balance.deleted = true;
            balance.updated_by = web.Service.SiteHelper.UserName;
            balance.updated_at = DateTime.Now;
            db.Entry(balance).State = EntityState.Modified;
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
