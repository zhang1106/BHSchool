using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using data;

namespace web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class creditsController : Controller
    {
        private DataRepository<Credit> db = DataRepository<Credit>.Create();

        // GET: balances
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.FindAll());
        }

        // GET: balances/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credit credit = db.Find(c=>c.id==id);
            if (credit == null)
            {
                return HttpNotFound();
            }
            return View(credit);
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
        public ActionResult Create([Bind(Include = "id,email,amount,type,description")] Credit credit)
        {
            if (ModelState.IsValid)
            {
                credit.updatedBy = web.Service.SiteHelper.UserName;
                credit.updatedAt = DateTime.Now;
             
                return RedirectToAction("Index");
            }
            return View(credit);
        }

        // GET: balances/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credit credit = db.Find(c=>c.id==id);
            if (credit == null)
            {
                return HttpNotFound();
            }
            return View(credit);
        }

        // POST: balances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "id,email,amount,type,description")] Credit credit)
        {
            if (ModelState.IsValid)
            {
                credit.updatedAt = DateTime.Now;
                credit.updatedBy = web.Service.SiteHelper.UserName;
                db.Update(credit);
                return RedirectToAction("Index");
            }
            return View(credit);
        }

        // GET: balances/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credit credit = db.Find(c=>c.id==id);
            if (credit == null)
            {
                return HttpNotFound();
            }
            return View(credit);
        }

        // POST: balances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Credit credit = db.Find(c=>c.id==id);
            credit.deleted = true;
            credit.updatedBy = web.Service.SiteHelper.UserName;
            credit.updatedAt = DateTime.Now;
            db.Delete(credit);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Connection.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
