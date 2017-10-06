using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SisComprasWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //https://blogs.msdn.microsoft.com/simonince/2010/06/04/conditional-validation-in-mvc/
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(Modelos.CotizacionRequiredIfAttribute), typeof(Modelos.CotizacionRequiredIfValidator));
        }
    }
}
