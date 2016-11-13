using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.Mvc;
using System.Net;
using data;
using web.Models;

namespace web.Controllers
{
    [OutputCache(Duration = 120, Location = OutputCacheLocation.Server, VaryByParam = "none")]
    public class HomeController : Controller
    {
        private bhcsEntities db = new bhcsEntities();

        public ActionResult Index()
        {
            var list = db.messages.Where(m=>m.category=="notification").ToList();
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
            var list = db.messages.Where(m => m.category == "notification").ToList();
            var main = list.Any() ? list.Find(m=>m.id==id) : new message() { title = "Welcome to BH Chinese School" };
            var model = new MessageModel() { Current = main, Messages = list };
            
            return View("Index", model);
        }


        public ActionResult Introduction()
        {
            var model = GetMsgModel("introduction");
            ViewBag.Message = "School Introduction";
            return View("Index", model);
        }

        public ActionResult Board()
        {
            var model = GetMsgModel("board");
            ViewBag.Message = "Board";
            return View("Index", model);
        }

        public ActionResult SchoolLeadership()
        {
            var model = GetMsgModel("schoolLeadership");
            ViewBag.Message = "School Leadership";
            return View("Index", model);
        }

        private MessageModel GetMsgModel(string name)
        {
            var list = db.messages.Where(m => m.category == "notification").ToList();
            var main = list?.Where(m => m.name == name).FirstOrDefault();
            if (main == null) main = new message() { title = "Welcome to BH Chinese School" };
            var model = new MessageModel() { Current = main, Messages = list };
            return model;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }
    }
}