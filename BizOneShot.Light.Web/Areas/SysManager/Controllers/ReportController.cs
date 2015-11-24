using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;
using PagedList;

namespace BizOneShot.Light.Web.Areas.SysManager.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
    public class ReportController : BaseController
    {
        private readonly IScBizWorkService scBizWorkService;
        private readonly IScCompMappingService scCompMappingService;
        private readonly IRptMasterService rptMasterService;
        private readonly IScUsrService scUsrService;


        public ReportController(IScBizWorkService scBizWorkService
            , IRptMasterService rptMasterService
            , IScUsrService scUsrService
            , IScCompMappingService scCompMappingService
            )
        {
            this.scBizWorkService = scBizWorkService;
            this.rptMasterService = rptMasterService;
            this.scUsrService = scUsrService;
            this.scCompMappingService = scCompMappingService;
        }

        // GET: SysManager/Report
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> BasicSurveyReport(string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            var bizMngList = await scUsrService.GetBizManagerAsync();
            var bizMngDropDown =
                Mapper.Map<List<BizMngDropDownModel>>(bizMngList);

            BizMngDropDownModel title = new BizMngDropDownModel();
            title.CompSn = 0;
            title.CompNm = "사업관리기관 선택";
            bizMngDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizMngDropDown, "CompSn", "CompNm");


            //사업관리기관 DownDown List Data
            ViewBag.SelectBizWorkMngrList = bizList;
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BasicSurveyReport(BasicSurveyReportViewModel paramModel, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업관리기관 DownDown List Data
            var bizMngList = await scUsrService.GetBizManagerAsync();
            var bizMngDropDown =
                Mapper.Map<List<BizMngDropDownModel>>(bizMngList);

            BizMngDropDownModel title = new BizMngDropDownModel();
            title.CompSn = 0;
            title.CompNm = "사업관리기관 선택";
            bizMngDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizMngDropDown, "CompSn", "CompNm");
            ViewBag.SelectBizWorkMngrList = bizList;

            //사업 DropDown List Data
            if (paramModel.BizWorkMngr == 0)
                ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            else
            {
                
                var listScBizWork = await scBizWorkService.GetBizWorkList(paramModel.BizWorkMngr, null, 0);
                ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(listScBizWork);
            }


            //기초역량 보고서 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            //var rptMsters = await rptMasterService.GetRptMasterListForSysManager(int.Parse(curPage ?? "1"), pagingSize, paramModel.BizWorkSn, paramModel.BizWorkMngr, "C");

            var compMappings = await scCompMappingService.GetPagedListCompMappingsForBasicReportAsync(int.Parse(curPage ?? "1"), pagingSize, paramModel.BizWorkMngr, null, paramModel.BizWorkSn);

            //뷰모델 맵핑
            var rptMasterListView = Mapper.Map<List<BasicSurveyReportViewModel>>(compMappings);

            return View(new StaticPagedList<BasicSurveyReportViewModel>(rptMasterListView, int.Parse(curPage ?? "1"), pagingSize, compMappings.TotalItemCount));

        }

        [HttpPost]
        public async Task<JsonResult> GetBizWorkNm(int MngCompSn)
        {
            //사업 DropDown List Data
            var listScBizWork = await scBizWorkService.GetBizWorkList(MngCompSn, null, 0);
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(listScBizWork);

            var bizList = ReportHelper.MakeBizWorkList(listScBizWork);

            return Json(bizList);
        }


        public async Task<ActionResult> FinanceReport(string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            var bizMngList = await scUsrService.GetBizManagerAsync();
            var bizMngDropDown =
                Mapper.Map<List<BizMngDropDownModel>>(bizMngList);

            BizMngDropDownModel title = new BizMngDropDownModel();
            title.CompSn = 0;
            title.CompNm = "사업관리기관 선택";
            bizMngDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizMngDropDown, "CompSn", "CompNm");


            //사업관리기관 DownDown List Data
            ViewBag.SelectBizWorkMngrList = bizList;
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> FinanceReport(BasicSurveyReportViewModel paramModel, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업관리기관 DownDown List Data
            var bizMngList = await scUsrService.GetBizManagerAsync();
            var bizMngDropDown =
                Mapper.Map<List<BizMngDropDownModel>>(bizMngList);

            BizMngDropDownModel title = new BizMngDropDownModel();
            title.CompSn = 0;
            title.CompNm = "사업관리기관 선택";
            bizMngDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizMngDropDown, "CompSn", "CompNm");
            ViewBag.SelectBizWorkMngrList = bizList;

            //사업 DropDown List Data
            if (paramModel.BizWorkMngr == 0)
                ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            else
            {

                var listScBizWork = await scBizWorkService.GetBizWorkList(paramModel.BizWorkMngr, null, 0);
                ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(listScBizWork);
            }


            //사업별 기업 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var scCompMappings = await scCompMappingService.GetPagedListCompMappingsAsync(int.Parse(curPage ?? "1"), pagingSize, paramModel.BizWorkMngr, null, paramModel.BizWorkSn);

            //뷰모델 맵핑
            var rptMasterListView = Mapper.Map<List<BasicSurveyReportViewModel>>(scCompMappings.ToList());

            return View(new StaticPagedList<BasicSurveyReportViewModel>(rptMasterListView, int.Parse(curPage ?? "1"), pagingSize, scCompMappings.TotalItemCount));

        }

        public async Task<ActionResult> BasicSurveyCover(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            if (paramModel.CompSn == 0 || paramModel.BizWorkSn == 0)
            {
                return View(paramModel);
            }

            var scCompMapping = await scCompMappingService.GetCompMappingAsync(paramModel.BizWorkSn, paramModel.CompSn);


            paramModel.CompNm = scCompMapping.ScCompInfo.CompNm;

            paramModel.BizWorkNm = scCompMapping.ScBizWork.BizWorkNm;
            paramModel.BizWorkYear = scCompMapping.ScCompInfo.RptMasters.Max(rm => rm.BasicYear);

            var rptMaster = scCompMapping.ScCompInfo.RptMasters.Where(rm => rm.BasicYear == paramModel.BizWorkYear && rm.BizWorkSn == paramModel.BizWorkSn && rm.CompSn == paramModel.CompSn).SingleOrDefault();

            paramModel.QuestionSn = rptMaster.QuestionSn;
            paramModel.Status = rptMaster.Status;
            paramModel.RegistrationNo = scCompMapping.ScCompInfo.RegistrationNo;

            var rptMaters = scCompMapping.ScCompInfo.RptMasters.Where(rm => rm.BizWorkSn == paramModel.BizWorkSn && rm.Status == "C").ToList();

            ViewBag.SelectReportYearList = ReportHelper.MakeBasicSurveyYear(rptMaters, paramModel.BizWorkYear);

            return View(paramModel);

        }

        [HttpPost]
        public async Task<ActionResult> BasicSurveyCover(BasicSurveyReportViewModel paramModel, int curPage = 0)
        {
            ViewBag.LeftMenu = Global.Report;

            if (paramModel.CompSn == 0 || paramModel.BizWorkSn == 0)
            {
                return View(paramModel);
            }

            var scCompMapping = await scCompMappingService.GetCompMappingAsync(paramModel.BizWorkSn, paramModel.CompSn);

            paramModel.CompNm = scCompMapping.ScCompInfo.CompNm;
            paramModel.BizWorkNm = scCompMapping.ScBizWork.BizWorkNm;

            var rptMaster = scCompMapping.ScCompInfo.RptMasters.Where(rm => rm.BasicYear == paramModel.BizWorkYear && rm.BizWorkSn == paramModel.BizWorkSn && rm.CompSn == paramModel.CompSn).SingleOrDefault();

            paramModel.QuestionSn = rptMaster.QuestionSn;
            paramModel.Status = rptMaster.Status;

            var rptMaters = scCompMapping.ScCompInfo.RptMasters.Where(rm => rm.BizWorkSn == paramModel.BizWorkSn && rm.Status == "C").ToList();
            ViewBag.SelectReportYearList = ReportHelper.MakeBasicSurveyYear(rptMaters, paramModel.BizWorkYear);

            return View(paramModel);

        }
    }
}