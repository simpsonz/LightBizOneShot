using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;
using System.Threading.Tasks;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Helper;
using BizOneShot.Light.Util.Security;
using PagedList;
using AutoMapper;

namespace BizOneShot.Light.Web.Areas.Mentor.Controllers
{
    [UserAuthorize(Order = 1)]
    public class MentoringReportController : BaseController
    {
        [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
        public async Task<ActionResult> MentoringTotalReportList()
        {
            ViewBag.LeftMenu = Global.MentoringReport;

            return View();
        }
    }
}