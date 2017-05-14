using System.Web;
using System.Web.Mvc;

namespace MVC5Course
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //加在全網站Action的ActionFilter
            filters.Add(new HandleErrorAttribute());
        }
    }
}
