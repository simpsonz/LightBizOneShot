using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Company | UserType.Expert | UserType.SysManager, Order = 2)]
    public class FinanceReportController : Controller
    {
        
        private readonly IScCompMappingService _scCompMappingService;

        public FinanceReportController(IScCompMappingService _scCompMappingService)
        {
            this._scCompMappingService = _scCompMappingService;
        }


        // GET: FinanceReopert
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> FinanceMng()
        {
            ViewBag.LeftMenu = Global.Report;

            // 로그인 기업의 승인된 사업정보를 가져옮
            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(Session[Global.CompSN].ToString()), "A");
            ViewBag.SelectYearList = ReportHelper.MakeBizYear(scCompMapping.ScBizWork);
            ViewBag.SelectMonthList = ReportHelper.MakeBizMonth(null);

            FinanceMngViewModel financeMngViewModel = new FinanceMngViewModel();
            financeMngViewModel.Display = "N";

            return View(financeMngViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> FinanceMng(FinanceMngViewModel financeMngViewModel)
        {
            ViewBag.LeftMenu = Global.Report;

            // 로그인 기업의 승인된 사업정보를 가져옮
            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(Session[Global.CompSN].ToString()), "A");
            ViewBag.SelectYearList = ReportHelper.MakeBizYear(scCompMapping.ScBizWork);
            ViewBag.SelectMonthList = ReportHelper.MakeBizMonth(null);

            financeMngViewModel.Display = "Y";
            financeMngViewModel.CompNm = scCompMapping.ScCompInfo.CompNm;

            return View(financeMngViewModel);
        }


        [HttpPost]
        public async Task<JsonResult> GetMonth(string Year)
        {
            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(Session[Global.CompSN].ToString()), "A");

            var month = ReportHelper.MakeBizMonth(scCompMapping.ScBizWork, int.Parse(Year));

            return Json(month);
        }
    }
}