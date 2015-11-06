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

namespace BizOneShot.Light.Web.Areas.Expert.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Expert, Order = 2)]
    public class ReportController : BaseController
    {
        private readonly IScBizWorkService _scBizWorkService;
        private readonly IRptMasterService rptMasterService;
        private readonly IScUsrService scUsrService;
        private readonly IScExpertMappingService scExpertMappingService;


        public ReportController(IScBizWorkService scBizWorkService
            , IRptMasterService rptMasterService
            , IScUsrService scUsrService
            , IScExpertMappingService scExpertMappingService
            )
        {
            this._scBizWorkService = scBizWorkService;
            this.rptMasterService = rptMasterService;
            this.scUsrService = scUsrService;
            this.scExpertMappingService = scExpertMappingService;
        }

        // GET: SysManager/Report
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> BasicSurveyReport(string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //var bizMngList = await scUsrService.GetBizManagerAsync();

            var loginId = Session[Global.LoginID].ToString();
            var expertMappings = await scExpertMappingService.GetExpertsAsync(loginId);
            var bizMngList = expertMappings.Select(s => s.ScBizWork.ScCompInfo).GroupBy(s => new { s.CompSn, s.CompNm });


            var bizWorkMngr = new List<SelectListItem>();
            bizWorkMngr.Add(new SelectListItem { Value = "0", Text = "사업관리기관 선택", Selected = true });

            if (bizMngList != null)
            {
                foreach (var item in bizMngList)
                {
                    bizWorkMngr.Add(new SelectListItem { Value = item.Key.CompSn.ToString(), Text = item.Key.CompNm.ToString() });
                }
            }

            SelectList list = new SelectList(bizWorkMngr, "Value", "Text");


            //사업관리기관 DownDown List Data
            ViewBag.SelectBizWorkMngrList = list;
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BasicSurveyReport(BasicSurveyReportViewModel paramModel, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업관리기관 DownDown List Data
            var loginId = Session[Global.LoginID].ToString();
            var expertMappings = await scExpertMappingService.GetExpertsAsync(loginId);
            var bizMngList = expertMappings.Select(s => s.ScBizWork.ScCompInfo).GroupBy(s => new { s.CompSn, s.CompNm });


            var bizWorkMngr = new List<SelectListItem>();
            bizWorkMngr.Add(new SelectListItem { Value = "0", Text = "사업관리기관 선택", Selected = true });

            if (bizMngList != null)
            {
                foreach (var item in bizMngList)
                {
                    bizWorkMngr.Add(new SelectListItem { Value = item.Key.CompSn.ToString(), Text = item.Key.CompNm.ToString() });
                }
            }

            SelectList list = new SelectList(bizWorkMngr, "Value", "Text");
            ViewBag.SelectBizWorkMngrList = list;

            //사업 DropDown List Data
            if (paramModel.BizWorkMngr == 0)
                ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            else
            {
                var bizWorkList = expertMappings.Where(i => i.ScBizWork.MngCompSn == paramModel.BizWorkMngr).Select(s => s.ScBizWork).ToList();

                ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(bizWorkList);
            }


            //기초역량 보고서 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var rptMsters = await rptMasterService.GetRptMasterListForExpert(int.Parse(curPage ?? "1"), pagingSize, loginId, paramModel.BizWorkSn, paramModel.BizWorkMngr, "C");

            //뷰모델 맵핑
            var rptMasterListView = Mapper.Map<List<BasicSurveyReportViewModel>>(rptMsters);

            return View(new StaticPagedList<BasicSurveyReportViewModel>(rptMasterListView, int.Parse(curPage ?? "1"), pagingSize, rptMsters.TotalItemCount));

        }


        public async Task<ActionResult> FinanceReport(string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //var bizMngList = await scUsrService.GetBizManagerAsync();

            var loginId = Session[Global.LoginID].ToString();
            var expertMappings = await scExpertMappingService.GetExpertsAsync(loginId);
            var bizMngList = expertMappings.Select(s => s.ScBizWork.ScCompInfo).GroupBy(s => new { s.CompSn, s.CompNm });


            var bizWorkMngr = new List<SelectListItem>();
            bizWorkMngr.Add(new SelectListItem { Value = "0", Text = "사업관리기관 선택", Selected = true });

            if (bizMngList != null)
            {
                foreach (var item in bizMngList)
                {
                    bizWorkMngr.Add(new SelectListItem { Value = item.Key.CompSn.ToString(), Text = item.Key.CompNm.ToString() });
                }
            }

            SelectList list = new SelectList(bizWorkMngr, "Value", "Text");


            //사업관리기관 DownDown List Data
            ViewBag.SelectBizWorkMngrList = list;
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> FinanceReport(BasicSurveyReportViewModel paramModel, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업관리기관 DownDown List Data
            var loginId = Session[Global.LoginID].ToString();
            var expertMappings = await scExpertMappingService.GetExpertsAsync(loginId);
            var bizMngList = expertMappings.Select(s => s.ScBizWork.ScCompInfo).GroupBy(s => new { s.CompSn, s.CompNm });


            var bizWorkMngr = new List<SelectListItem>();
            bizWorkMngr.Add(new SelectListItem { Value = "0", Text = "사업관리기관 선택", Selected = true });

            if (bizMngList != null)
            {
                foreach (var item in bizMngList)
                {
                    bizWorkMngr.Add(new SelectListItem { Value = item.Key.CompSn.ToString(), Text = item.Key.CompNm.ToString() });
                }
            }

            SelectList list = new SelectList(bizWorkMngr, "Value", "Text");
            ViewBag.SelectBizWorkMngrList = list;

            //사업 DropDown List Data
            if (paramModel.BizWorkMngr == 0)
                ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            else
            {
                var bizWorkList = expertMappings.Where(i => i.ScBizWork.MngCompSn == paramModel.BizWorkMngr).Select(s => s.ScBizWork).ToList();

                ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(bizWorkList);
            }


            //사업별 기업 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var scCompMappings = scExpertMappingService.GetScCompMappings(int.Parse(curPage ?? "1"), pagingSize, loginId, paramModel.BizWorkSn, paramModel.BizWorkMngr, "A");

            //뷰모델 맵핑
            var rptMasterListView = Mapper.Map<List<BasicSurveyReportViewModel>>(scCompMappings.ToList());

            return View(new StaticPagedList<BasicSurveyReportViewModel>(rptMasterListView, int.Parse(curPage ?? "1"), pagingSize, scCompMappings.TotalItemCount));

        }

        [HttpPost]
        public async Task<JsonResult> GetBizWorkNm(int MngCompSn)
        {
            //사업 DropDown List Data
            var loginId = Session[Global.LoginID].ToString();
            var expertMappings = await scExpertMappingService.GetExpertsAsync(loginId);
            var bizMngList = expertMappings.Where(i => i.ScBizWork.MngCompSn == MngCompSn).Select(s => s.ScBizWork).ToList();

            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(bizMngList);

            var bizList = ReportHelper.MakeBizWorkList(bizMngList);

            return Json(bizList);
        }
    }
}