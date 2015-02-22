using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace api_project
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private string _defaultIpAddress;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _defaultIpAddress = WebConfigurationManager.AppSettings["DefaultIpAddress"];
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg == "ipAddress")
            {
                return "IpAddress=" + (Request.IsLocal ? _defaultIpAddress : Request.UserHostAddress);
            }

            return base.GetVaryByCustomString(context, arg);
        }
    }
}
