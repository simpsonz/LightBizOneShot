using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Controllers
{
    [UserAuthorize(Order = 1)]
    public class BasicSurveyReportController : Controller
    {
        // GET: BasicSurveyReport
        public ActionResult Index()
        {
            return View();
        }
    }
}