using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly IFinenceReportService _finenceReportService;

        public FinanceReportController(IScCompMappingService _scCompMappingService, IFinenceReportService _finenceReportService)
        {
            this._scCompMappingService = _scCompMappingService;
            this._finenceReportService = _finenceReportService;
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

            // 현금시제
            //SqlParameter sidoParam = new SqlParameter("MEMB_BUSNPERS_NO", Session[Global.CompRegistrationNo].ToString());
            SqlParameter compRegNo = new SqlParameter("MEMB_BUSNPERS_NO", "8888888888");
            SqlParameter corpCode = new SqlParameter("CORP_CODE", "1000");
            SqlParameter bizCode = new SqlParameter("BIZ_CD", "0100");
            SqlParameter setYear = new SqlParameter("SET_YEAR", financeMngViewModel.Year.ToString());
            SqlParameter setMonth = new SqlParameter("SET_MONTH", financeMngViewModel.Month.ToString());

            object[] parameters = new object[] { compRegNo, corpCode, bizCode, setYear, setMonth };

            var resultList = await _finenceReportService.GetMonthlyCashListAsync(parameters);

            financeMngViewModel.cashViewModel = ReportHelper.MakeCashViewModel(resultList);


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