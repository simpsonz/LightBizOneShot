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

namespace BizOneShot.Light.Web.Areas.Mentor.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
    public class ReportController : BaseController
    {

        private readonly IScCompMappingService scCompMappingService;
        private readonly IScMentorMappingService scMentorMappingService;
        private readonly IRptMasterService rptMasterService;

        public ReportController(
            IScCompMappingService scCompMappingService,
            IScMentorMappingService scMentorMappingService,
            IRptMasterService rptMasterService)
        {
            this.scCompMappingService = scCompMappingService;
            this.scMentorMappingService = scMentorMappingService;
            this.rptMasterService = rptMasterService;
        }


        // GET: Mentor/Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BasicSurveyReport(string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업년도 DownDown List Data
            ViewBag.SelectBizWorkYearList = ReportHelper.MakeYear(2015);

            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            ViewBag.SelectCompInfoList = ReportHelper.MakeCompanyList(null);
            ViewBag.SelectStatusList = ReportHelper.MakeReportStatusList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BasicSurveyReport(BasicSurveyReportViewModel paramModel, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업년도 DownDown List Data
            ViewBag.SelectBizWorkYearList = ReportHelper.MakeYear(2015);

            var mentorId = Session[Global.LoginID].ToString();
            if (string.IsNullOrEmpty(paramModel.Status))
                paramModel.Status = "";


            //사업 DropDown List Data
            var listScMentorMapping = await scMentorMappingService.GetMentorMappingListByMentorId(mentorId, paramModel.BizWorkYear);
            var listScBizWork = listScMentorMapping.Select(mmp => mmp.ScBizWork).ToList();
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(listScBizWork);

            var listScCompMapping = await scCompMappingService.GetCompMappingListByMentorId(mentorId, "A", paramModel.BizWorkSn, paramModel.BizWorkYear);
            var listScCompInfo = listScCompMapping.Select(cmp => cmp.ScCompInfo).ToList();
            ViewBag.SelectCompInfoList = ReportHelper.MakeCompanyList(listScCompInfo);
            ViewBag.SelectStatusList = ReportHelper.MakeReportStatusList();



            //기초역량 보고서 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var rptMsters = await rptMasterService.GetRptMasterList(int.Parse(curPage ?? "1"), pagingSize, mentorId, paramModel.BizWorkYear, paramModel.BizWorkSn, paramModel.CompSn, paramModel.Status);

            //뷰모델 맵핑
            var rptMasterListView = Mapper.Map<List<BasicSurveyReportViewModel>>(rptMsters);

            return View(new StaticPagedList<BasicSurveyReportViewModel>(rptMasterListView, int.Parse(curPage ?? "1"), pagingSize, rptMsters.TotalItemCount));

        }

        #region 드롭다운박스 처리 controller
        [HttpPost]
        public async Task<JsonResult> GetBizWorkNm(int Year)
        {
            var mentorId = Session[Global.LoginID].ToString();

            //사업 DropDown List Data
            var listScMentorMapping = await scMentorMappingService.GetMentorMappingListByMentorId(mentorId, Year);
            var listScBizWork = listScMentorMapping.Select(mmp => mmp.ScBizWork).ToList();

            var bizList = ReportHelper.MakeBizWorkList(listScBizWork);

            return Json(bizList);
        }

        [HttpPost]
        public async Task<JsonResult> GetCompanyNm(int BizWorkSn, int Year)
        {
            var mentorId = Session[Global.LoginID].ToString();

            var listScCompMapping = await scCompMappingService.GetCompMappingListByMentorId(mentorId, "A", BizWorkSn, Year);
            var listScCompInfo = listScCompMapping.Select(cmp => cmp.ScCompInfo).ToList();

            var bizList = ReportHelper.MakeCompanyList(listScCompInfo);

            return Json(bizList);
        }
        #endregion
    }
}