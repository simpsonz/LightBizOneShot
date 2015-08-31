 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Areas.SysManager.Controllers
{
    public class BizManagerController : Controller
    {
        // GET: SysManager/BizManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BizManager()
        {
            ViewBag.LeftMenu = Global.BizMng;
            return View();
        }
    }
}