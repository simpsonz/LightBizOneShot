using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;
using PagedList;

namespace BizOneShot.Light.Web.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Company | UserType.Expert | UserType.SysManager | UserType.BizManager, Order = 2)]
    public class FinanceReportController : BaseController
    {
        
        private readonly IScCompMappingService _scCompMappingService;
        private readonly IScBizWorkService _scBizWorkService;
        private readonly IFinenceReportService _finenceReportService;
        private readonly IScCompInfoService _scCompInfoService;
        private readonly IRptFinanceCommentService _rptFinanceCommentService;

        public FinanceReportController(IScCompMappingService _scCompMappingService, IFinenceReportService _finenceReportService, IScBizWorkService _scBizWorkService, IScCompInfoService _scCompInfoService, IRptFinanceCommentService _rptFinanceCommentService)
        {
            this._scCompMappingService = _scCompMappingService;
            this._finenceReportService = _finenceReportService;
            this._scBizWorkService = _scBizWorkService;
            this._scCompInfoService = _scCompInfoService;
            this._rptFinanceCommentService = _rptFinanceCommentService;
        }


        // GET: FinanceReopert
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> FinanceMng(int CompSn = 0, int BizWorkSn = 0)
        {
            ViewBag.LeftMenu = Global.Report;

            if(CompSn == 0)
            {
                CompSn = int.Parse(Session[Global.CompSN].ToString());
            }

            //기업회원 승인된 사업이 없으면 리다이렉트 함
            if(Session[Global.UserType].ToString() == Global.Company)
            { 
                var scCompMappings = await _scCompMappingService.GetCompMappingsForCompanyAsync(CompSn);
                if (scCompMappings.Count == 0)
                {
                    TempData["alert"] = "승인된 사업이 없습니다.";

                    return RedirectToAction("index", "Company/Main");
                }
            }

            // 로그인 기업의 승인된 사업정보를 가져옮
            ViewBag.SelectYearList = ReportHelper.MakeYear(2014);
            ViewBag.SelectMonthList = ReportHelper.MakeMonth();

            FinanceMngViewModel financeMngViewModel = new FinanceMngViewModel();
            financeMngViewModel.Display = "N";
            financeMngViewModel.CompSn = CompSn;
            financeMngViewModel.BizWorkSn = BizWorkSn;

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
            var scCompInfo = await _scCompInfoService.GetScCompInfoByCompSn(financeMngViewModel.CompSn);
            ViewBag.SelectYearList = ReportHelper.MakeYear(2014);
            ViewBag.SelectMonthList = ReportHelper.MakeMonth(int.Parse(financeMngViewModel.Year));

            financeMngViewModel.Display = "Y";
            financeMngViewModel.CompNm = scCompInfo.CompNm;

            // 현금시제
            var cashResultList = await _finenceReportService.GetMonthlyCashListAsync(ReportHelper.MakeProcedureParams(scCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));

            financeMngViewModel.cashViewModel = ReportHelper.MakeCashViewModel(cashResultList);

            // 매출
            var salesResult = await _finenceReportService.GetMonthlySalesAsync(ReportHelper.MakeProcedureParams(scCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));

            var yearTotalResult = await _finenceReportService.GetYearTotalSalesAsync(ReportHelper.MakeProcedureParams(scCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));

            financeMngViewModel.salesViewModel = ReportHelper.MakeSalesViewModel(salesResult, yearTotalResult);

            // 이익분석
            var costAnalysisListResult = await _finenceReportService.GetCostAnalysisAsync(ReportHelper.MakeProcedureParams(scCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));

            financeMngViewModel.curMenthTotalCostViewModel = ReportHelper.MakeCostAnalysisViewModel(costAnalysisListResult[0]);
            //financeMngViewModel.lastMenthTotalCostViewModel = ReportHelper.MakeCostAnalysisViewModel(costAnalysisListResult[1]);

            // 비용분석
            var expenseCostResult = await _finenceReportService.GetExpenseCostAsync(ReportHelper.MakeProcedureParams(scCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));
            financeMngViewModel.expenseCostViewModel = ReportHelper.MakeExpenseCostViewModel(expenseCostResult[0]);

            // 주요매출
            var taxSalesResult = await _finenceReportService.GetTaxSalesAsync(ReportHelper.MakeProcedureParams(scCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString()));
            financeMngViewModel.taxSalesListViewModel = ReportHelper.MakeTaxSalseListViewModel(taxSalesResult, salesResult);

            // 주요지출
            var bankOutResult = await _finenceReportService.GetBankOutAsync(ReportHelper.MakeProcedureParams(scCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], financeMngViewModel.Year.ToString(), financeMngViewModel.Month.ToString())); 
            financeMngViewModel.bankOutListViewModel = ReportHelper.MakeBnakOutListViewModel(bankOutResult);

            return View(financeMngViewModel);
        }


        [HttpPost]
        public async Task<JsonResult> GetMonth(string Year, int CompSn, int BizWorkSn)
        {

            if (Session[Global.UserType].Equals(Global.Company))
            {// 기업회원
                var month = ReportHelper.MakeMonth(int.Parse(Year));
                return Json(month);
            }
            else
            {
                var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);
                var month = ReportHelper.MakeBizMonth(scBizWork, int.Parse(Year));
                return Json(month);
            }
        }


        [HttpPost]
        public async Task<ActionResult> SaveComment(RegCommentViewModel paramModel)
        {
            var rptFinanceComment =
                Mapper.Map<RptFinanceComment>(paramModel);

            rptFinanceComment.ExpertId = Session[Global.LoginID].ToString();
            rptFinanceComment.WriteDt = DateTime.Now;

            var saveResult = _rptFinanceCommentService.Insert(rptFinanceComment);

            if (saveResult != null)
            {
                await _rptFinanceCommentService.SaveDbContextAsync();
                return Json(new { result = true });
            }
            else
            {
                return Json(new { result = false });
            }
        }
        

        public async Task<ActionResult> RegCommentPopup(RegCommentViewModel paramModel)
        {
            var rptFinanceComment = await _rptFinanceCommentService.GetRptFinanceCommentAsync(paramModel.BizWorkSn, paramModel.CompSn, paramModel.BasicYear, paramModel.BasicMonth);

            if(rptFinanceComment == null)
            {
                paramModel.WriteYN = "N";
            }
            else
            {
                paramModel.WriteYN = "Y";
                paramModel.Comment = rptFinanceComment.Comment;
            }
            return View(paramModel);

        }


        public async Task<ActionResult> SearchCommentPopup(RegCommentViewModel paramModel)
        {
            var rptFinanceComment = await _rptFinanceCommentService.GetRptFinanceCommentsAsync(paramModel.CompSn, paramModel.BasicYear, paramModel.BasicMonth);

            var regCommentViewModels =
                Mapper.Map<List<RegCommentViewModel>>(rptFinanceComment);

            return View(regCommentViewModels);

        }
    }
}