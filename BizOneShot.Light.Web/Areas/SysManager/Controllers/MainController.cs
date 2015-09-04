using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Areas.SysManager.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
    public class MainController : BaseController
    {
        // GET: SysManager/Main
        public ActionResult Index()
        {
            return View();
        }
    }
}