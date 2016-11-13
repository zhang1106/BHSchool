using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using System.Data.Entity;
using data;
using web.Models;

namespace web.Controllers
{
    [Authorize]
    public class FamilyAcctController : Controller
    {
        private bhcsEntities db = new bhcsEntities();

        // GET: FamilyAcct
        public ActionResult Index()
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
                var members = db.members.Where(m => m.id == u.id || m.householdId == u.id);
                return View(members);
            };
        }

        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var uid = User.Identity.GetUserName();
            var u = db.members.FirstOrDefault(m => m.email == uid);
            if(u!=null)
            {
                return RedirectToAction("Edit");
            }
            return View();
        }
        
        // POST: members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "firstname,lastname,phone,address1,address2,city")] FamilyAcctViewModel m)
        {
            if (ModelState.IsValid)
            {
                var household = new member() { firstname = m.firstname, lastname = m.lastname, phone = m.phone, email = User.Identity.GetUserName() };
                db.members.Add(household);
                db.SaveChanges();
                var familyAddr = new address { address1 = m.address1, address2 = m.address2, city = m.city, country = "US", state = "NJ", zip = m.zip, memberId = household.id };
                db.addresses.Add(familyAddr);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Student()
        {
            return View();
        }

        public ActionResult CreateStudent()
        {
            var uid = User.Identity.GetUserName();
            var u = db.members.FirstOrDefault(m => m.email == uid);
            if (u == null)
            {
                return RedirectToAction("Create", "FamilyAcct");
            }
            return View();
        }

        // POST: members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudent([Bind(Include = "firstname,lastname,gender,born")] member m)
        {
            if (ModelState.IsValid)
            {
                var student = new member() { firstname = m.firstname, lastname = m.lastname, gender = m.gender, born=m.born};
                var uid = User.Identity.GetUserName();
                var u = db.members.FirstOrDefault(h => h.email == uid);
                student.householdId = u.id;
                db.members.Add(student);

                db.SaveChanges();
            }
             
            return RedirectToAction("Index");
           
        }

        // GET: members/Edit/5
        public ActionResult EditStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent([Bind(Include = "id,householdId,firstname,lastname,email,gender,born")] member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: members/Edit/5
        public ActionResult Edit()
        {
            var uid = User.Identity.GetUserName();
            var u = db.members.FirstOrDefault(m => m.email == uid);
            if(u==null)
            {
                return Redirect("Create");
            }
            var address = db.addresses.FirstOrDefault(a => a.memberId == u.id);
            if(address == null)
            {
                address = new address();
            }
            var model = new FamilyAcctViewModel() { address1 = address.address1, address2 = address.address2, city = address.city,
                country = address.country,state = address.state, zip = address.zip,
                 firstname = u.firstname, lastname = u.lastname, phone = u.phone, id=address.id, memeberId=address.memberId };

            return View(model);
        }

        // POST: members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "firstname,lastname,phone,address1,address2,city,zip")] FamilyAcctViewModel familyAcct)
        {
            if (ModelState.IsValid)
            {
                var uid = User.Identity.GetUserName();
                var u = db.members.FirstOrDefault(m => m.email == uid);
                if (u.phone != familyAcct.phone)
                {
                    u.phone = familyAcct.phone;
                    db.Entry(u).State = EntityState.Modified;
                }
                              
                db.SaveChanges();

                var address = db.addresses.FirstOrDefault(a => a.memberId == u.id);
                if(address==null)
                {
                    address = new address() {memberId=u.id, address1 = familyAcct.address1, address2 = familyAcct.address2, city = familyAcct.city, zip = familyAcct.zip };
                    db.addresses.Add(address);
                    db.SaveChanges();
                }
                else
                {
                    address.address1 = familyAcct.address1;
                    address.address2 = familyAcct.address2;
                    address.city = familyAcct.city;
                    address.zip = familyAcct.zip;
                    db.Entry(u).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Details");
            }

            return View(familyAcct);
        }

        public ActionResult Details()
        {
            var uid = User.Identity.GetUserName();
            var u = db.members.FirstOrDefault(m => m.email == uid);
            if (u == null)
            {
                return Redirect("Create");
            }
            var address = db.addresses.FirstOrDefault(a => a.memberId == u.id);
            if (address == null)
            {
                address = new address();
            }
            var model = new FamilyAcctViewModel()
            {
                address1 = address.address1,
                address2 = address.address2,
                city = address.city,
                country = address.country,
                state = address.state,
                zip = address.zip,
                firstname = u.firstname,
                lastname = u.lastname,
                phone = u.phone,
                id = address.id,
                memeberId = address.memberId
            };

            return View(model);
        }
    }
}