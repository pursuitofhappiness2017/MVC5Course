using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Unknow()
        {
            return View();
        }

        public ActionResult SomeAction()
        {
            return PartialView("SuccessRedirect", "/");
        }

        public ActionResult PartualAbout()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("About");
            }

            return View("About");
        }

        [SharedViewBag]
        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult GetFile()
        {
            return File(Server.MapPath("~/Content/32kNltm.jpg"), "image/png", "NewName");
        }

        public ActionResult GetJson()
        {
            db.Configuration.LazyLoadingEnabled = false;

            return Json(db.Product.Take(5), JsonRequestBehavior.AllowGet);
        }
    }
}