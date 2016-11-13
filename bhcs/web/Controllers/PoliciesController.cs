using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using web.Models;
using data;

namespace web.Controllers
{
    public class PoliciesController : Controller
    {
        private bhcsEntities db = new bhcsEntities();
        // GET: Policies
        public ActionResult Index()
        {
            var list = db.messages.Where(m => m.category == "policy").ToList();
            var main = list.Any() ? list[0] : new message() { title = "Welcome to BH Chinese School" };
            var model = new MessageModel() { Current = main, Messages = list };

            return View(model);
        }

        public ActionResult ViewMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.messages.Where(m => m.category == "policy").ToList();
            var main = list.Any() ? list.Find(m => m.id == id) : new message() { title = "Welcome to BH Chinese School" };
            var model = new MessageModel() { Current = main, Messages = list };

            return View("Index", model);
        }
    }
}