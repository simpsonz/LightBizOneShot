using System.Web.Http;

namespace BizOneShot.Light.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("ActionApi", "api/{controller}/{action}/{id}", new {id = RouteParameter.Optional}
                );

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}",
                new {id = RouteParameter.Optional}
                );


            //config.Routes.MapHttpRoute(
            //    "FileUpload",
            //    "files/upload",
            //    new { controller = "FilesController", action = "FileUpload" },
            //    new { httpMethod = new HttpMethodConstraint("POST") }
            //);


            //config.Routes.MapHttpRoute(
            //    "FileDownload",
            //    "files/download/{blobId}",
            //    new { controller = "FilesController", action = "FileDownload" },
            //    new { httpMethod = new HttpMethodConstraint("GET") }
            //);
        }
    }
}