using Animals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Animals
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

    public class DbContext
    {
        private static AnimalsEntities instance;

        private DbContext() { }

        public static AnimalsEntities Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AnimalsEntities();
                }
                return instance;
            }
        }
    }
}
