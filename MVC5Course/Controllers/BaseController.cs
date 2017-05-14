using MVC5Course.Models;
using System.Data.Entity.Validation;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
    public abstract class BaseController : Controller
    {
        protected FabricsEntities db = new FabricsEntities();

        [LocalOnly]
        public ActionResult Debug()
        {
            return Content("Hello");
        }

        //一般不建議
        //protected override void HandleUnknownAction(string actionName)
        //{
        //    this.RedirectToAction("Index", "Home").ExecuteResult(this.ControllerContext);
        //}
    }
}