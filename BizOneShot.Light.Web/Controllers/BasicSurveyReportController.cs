using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Controllers
{
    [UserAuthorize(Order = 1)]
    public class BasicSurveyReportController : BaseController
    {
        private readonly IScCompMappingService scCompMappingService;
        private readonly IQuesCompInfoService quesCompInfoService;
        public BasicSurveyReportController(
            IScCompMappingService scCompMappingService,
            IQuesCompInfoService quesCompInfoService)
        {
            this.scCompMappingService = scCompMappingService;
            this.quesCompInfoService = quesCompInfoService;
        }

        // GET: BasicSurveyReport
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Cover(int CompSn = 0, int BizWorkSn = 0)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;
            BasicSurveyReportViewModel coverViewModel = new BasicSurveyReportViewModel();

            CompSn = 97;
            BizWorkSn = 5;

            if (CompSn == 0 || BizWorkSn == 0)
            {
                return View(coverViewModel);
            }

            var scCompMapping = await scCompMappingService.GetCompMappingAsync(BizWorkSn, CompSn);

            coverViewModel.CompNm = scCompMapping.ScCompInfo.CompNm;
            coverViewModel.BizWorkNm = scCompMapping.ScBizWork.BizWorkNm;
            coverViewModel.Status = "T";

            return View(coverViewModel);

        }


        public async Task<ActionResult> CompanyInfo(int QustionSn = 0)
        {
            QustionSn = 16;
            var quesCompInfo = await quesCompInfoService.GetQuesCompInfoAsync(QustionSn);
            var quesCompInfoView = Mapper.Map<QuesCompanyInfoViewModel>(quesCompInfo);
            if (quesCompInfoView.PublishDt == "0001-01-01")
                quesCompInfoView.PublishDt = null;
            return View(quesCompInfoView);

        }


        public async Task<ActionResult> OverallSummaryCover(int QustionSn, int BizWorkSn, int CompSn, int Year)
        {
            QustionSn = 16;
            var quesCompInfo = await quesCompInfoService.GetQuesCompInfoAsync(QustionSn);
            var quesCompInfoView = Mapper.Map<QuesCompanyInfoViewModel>(quesCompInfo);
            if (quesCompInfoView.PublishDt == "0001-01-01")
                quesCompInfoView.PublishDt = null;
            return View(quesCompInfoView);

        }

        public async Task<ActionResult> BasicSurveyCompanyList(string curPage)
        {
            //사업년도 DownDown List Data
            ViewBag.SelectBizWorkYearList = ReportHelper.MakeYear(2015);
            
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            ViewBag.SelectCompInfoList = ReportHelper.MakeCompanyList(null);
            ViewBag.SelectStatusList = ReportHelper.MakeReportStatusList();
            return View();
        }
    }
}