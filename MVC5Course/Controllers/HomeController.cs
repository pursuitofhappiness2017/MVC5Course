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
        //[LocalOnly]
        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            throw new ArgumentException("Error Handled!");

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

        public ActionResult VT()
        {
            ViewBag.IsEnabled = true;

            return View();
        }

        public ActionResult RazorTest()
        {
            int[] data = new int[] { 1, 2, 3, 4, 5};

            return PartialView(data);
        }
    }
}