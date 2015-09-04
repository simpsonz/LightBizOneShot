using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Areas.BizManager.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.BizManager, Order = 2)]
    public class BizWorkMngController : BaseController
    {
        // GET: BizManager/BizWorkMng
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BizWorkMng()
        {
            ViewBag.LeftMenu = Global.BizWorkMng;
            return View();
        }
    }
}