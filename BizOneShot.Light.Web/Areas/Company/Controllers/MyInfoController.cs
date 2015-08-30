using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;

namespace BizOneShot.Light.Web.Areas.Company.Controllers
{
    [UserAuthorize(Order = 1)]
    public class MyInfoController : BaseController
    {
        // GET: Company/MyInfo
        public ActionResult Index()
        {
            return View();
        }

        // GET: Company/MyInfo
        [MenuAuthorize(Roles = UserType.Company, Order = 2)]
        public ActionResult MyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;
            return View();
        }
    }
}