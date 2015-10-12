using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Controllers
{
    public class FinanceReportController : Controller
    {
        [UserAuthorize(Order = 1)]
        [MenuAuthorize(Roles = UserType.Company | UserType.Expert | UserType.SysManager, Order = 2)]
        // GET: FinanceReopert
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FinanceMng()
        {
            ViewBag.LeftMenu = Global.Report;

            return View();
        }
    }
}