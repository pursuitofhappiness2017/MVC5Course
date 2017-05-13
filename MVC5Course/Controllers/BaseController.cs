using MVC5Course.Models;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public abstract class BaseController : Controller
    {
        protected FabricsEntities db = new FabricsEntities();

        public ActionResult Debug()
        {
            return Content("Hello");
        }
    }
}