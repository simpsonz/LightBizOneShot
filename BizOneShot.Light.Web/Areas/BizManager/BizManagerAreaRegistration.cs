using System.Web.Mvc;

namespace BizOneShot.Light.Web.Areas.BizManager
{
    public class BizManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BizManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BizManager_default",
                "BizManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BizOneShot.Light.Web.Areas.BizManager.Controllers" }
            );
        }
    }
}