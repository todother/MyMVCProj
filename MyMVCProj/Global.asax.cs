using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using QuartzToken;
using RobotQuartz;

namespace MyMVCProj
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            TokenTrigger.Start();
            RobotTrigger.Start();
        }
    }
}
