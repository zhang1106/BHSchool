using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
#if DEBUG
#else
            Task.Factory.StartNew(() =>
            {
                while(true)
                {
                    var s = web.Service.RegistrationSvc.GetWebPage("http://www.huaxiabh.org");
                    Thread.Sleep(10000);
                }
            });
#endif

        }
    }
}
