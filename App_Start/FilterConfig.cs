using System.Web;
using System.Web.Mvc;

namespace DemoWebApiWithSwagger
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
