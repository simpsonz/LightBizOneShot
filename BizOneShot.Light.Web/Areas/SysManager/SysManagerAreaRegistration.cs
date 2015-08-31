using System.Web.Mvc;

namespace BizOneShot.Light.Web.Areas.SysManager
{
    public class SysManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SysManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SysManager_default",
                "SysManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BizOneShot.Light.Web.Areas.SysManager.Controllers" }
            );
        }
    }
}