using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Areas.Company.Controllers
{
    public class ReportController : Controller
    {
        // GET: Company/Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BasicSurvey()
        {
            ViewBag.LeftMenu = Global.Report;
            return View();
        }

        public ActionResult Summary01()
        {
            ViewBag.LeftMenu = Global.Report;
            return View();
        }

        public ActionResult FinanceMng()
        {
            ViewBag.LeftMenu = Global.Report;
            return View();
        }



        
    }
}