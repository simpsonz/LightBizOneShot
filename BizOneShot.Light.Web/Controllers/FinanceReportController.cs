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
            ViewBag.SelectYearList = ReportHelper.MakeYear(2014);
            ViewBag.SelectMonthList = ReportHelper.MakeMonth();

            FinanceMngViewModel financeMngViewModel = new FinanceMngViewModel();
            financeMngViewModel.Display = "N";

            financeMngViewModel.curMenthTotalCostViewModel = new TotalCostViewModel();
            financeMngViewModel.curMenthTotalCostViewModel.AllOtherAmt = "0";
            financeMngViewModel.curMenthTotalCostViewModel.ManufacturingAmt = "0";
            financeMngViewModel.curMenthTotalCostViewModel.MaterialAmt = "0";
            financeMngViewModel.curMenthTotalCostViewModel.OperatingAmt = "0";
            financeMngViewModel.curMenthTotalCostViewModel.ProfitAmt = "0";
            financeMngViewModel.curMenthTotalCostViewModel.SalesAmt = "0";


            return View(financeMngViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> FinanceMng(FinanceMngViewModel financeMngViewModel)
        {
            ViewBag.LeftMenu = Global.Report;

            // 로그인 기업의 승인된 사업정보를 가져옮
            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(Session[Global.CompSN].ToString()), "A");
            ViewBag.SelectYearList = ReportHelper.MakeYear(2014);
            ViewBag.SelectMonthList = ReportHelper.MakeMonth();

            financeMngViewModel.Display = "Y";
            financeMngViewModel.CompNm = scCompMapping.ScCompInfo.CompNm;

            // 현금시제
            var cashResultList = await _finenceReportService.GetMonthlyCashListAsync(ReportHelper.MakeProcedureParams("8888888888", "1000", "0100", financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));

            financeMngViewModel.cashViewModel = ReportHelper.MakeCashViewModel(cashResultList);

            // 매출
            var salesResult = await _finenceReportService.GetMonthlySalesAsync(ReportHelper.MakeProcedureParams("8888888888", "1000", "0100", financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));

            var yearTotalResult = await _finenceReportService.GetYearTotalSalesAsync(ReportHelper.MakeProcedureParams("8888888888", "1000", "0100", financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));

            financeMngViewModel.salesViewModel = ReportHelper.MakeSalesViewModel(salesResult, yearTotalResult);

            // 이익분석
            var costAnalysisListResult = await _finenceReportService.GetCostAnalysisAsync(ReportHelper.MakeProcedureParams("8888888888", "1000", "0100", financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));

            financeMngViewModel.curMenthTotalCostViewModel = ReportHelper.MakeCostAnalysisViewModel(costAnalysisListResult[0]);
            //financeMngViewModel.lastMenthTotalCostViewModel = ReportHelper.MakeCostAnalysisViewModel(costAnalysisListResult[1]);

            // 비용분석
            var expenseCostResult = await _finenceReportService.GetExpenseCostAsync(ReportHelper.MakeProcedureParams("8888888888", "1000", "0100", financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));
            financeMngViewModel.expenseCostViewModel = ReportHelper.MakeExpenseCostViewModel(expenseCostResult[0]);

            // 주요매출
            var taxSalesResult = await _finenceReportService.GetTaxSalesAsync(ReportHelper.MakeProcedureParams("8888888888", "1000", "1000", financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));
            financeMngViewModel.taxSalesListViewModel = ReportHelper.MakeTaxSalseListViewModel(taxSalesResult, salesResult);

            // 주요지출
            var bankOutResult = await _finenceReportService.GetBankOutAsync(ReportHelper.MakeProcedureParams("1048196471", "1000", "1000", financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));
            financeMngViewModel.bankOutListViewModel = ReportHelper.MakeBnakOutListViewModel(bankOutResult);

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