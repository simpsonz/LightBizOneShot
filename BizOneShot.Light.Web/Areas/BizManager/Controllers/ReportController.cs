using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
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
namespace BizOneShot.Light.Web.Areas.BizManager.Controllers
{

    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.BizManager, Order = 2)]
    public class ReportController : BaseController
    {
        private readonly IScBizWorkService _scBizWorkService;
        private readonly IScCompMappingService _scCompMappingService;
        private readonly IScMentorMappingService _scMentorMappingService;
        private readonly IScMentoringTotalReportService _scMentoringTotalReportService;
        private readonly IScMentoringTrFileInfoService _scMentoringTrFileInfoService;
        private readonly IRptMasterService rptMasterService;
        private readonly IScMentoringReportService _scMentoringReportService;
        private readonly IScMentoringFileInfoService _scMentoringFileInfoService;
        private readonly IFinenceReportService _finenceReportService;


        public ReportController(IScBizWorkService scBizWorkService
            , IScCompMappingService scCompMappingService
            , IScMentorMappingService scMentorMappingService
            , IScMentoringTotalReportService scMentoringTotalReportService
            , IScMentoringTrFileInfoService scMentoringTrFileInfoService
            , IScMentoringReportService scMentoringReportService
            , IScMentoringFileInfoService scMentoringFileInfoService
            , IRptMasterService rptMasterService
            , IFinenceReportService finenceReportService
            )
        {
            this._scBizWorkService = scBizWorkService;
            this._scCompMappingService = scCompMappingService;
            this._scMentorMappingService = scMentorMappingService;
            this._scMentoringTotalReportService = scMentoringTotalReportService;
            this._scMentoringTrFileInfoService = scMentoringTrFileInfoService;
            this._scMentoringReportService = scMentoringReportService;
            this._scMentoringFileInfoService = scMentoringFileInfoService;
            this.rptMasterService = rptMasterService;
            this._finenceReportService = finenceReportService;
        }


        #region 멘토링 종합보고서
        public async Task<ActionResult> MentoringTotalReportListByComp(SelectedMentorTotalReportParmModel param, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            //사업년도 DownDown List Data
            var bizWorkYearDropDown = MakeBizYear(2015);
            SelectList bizWorkYear = new SelectList(bizWorkYearDropDown, "Value", "Text");
            ViewBag.SelectBizWorkYearList = bizWorkYear;

            //사업 DropDown List Data
            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, param.BizWorkYear);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;

            //기업 DropDwon List Data
            var compInfoDropDown = await MakeBizComp(mngCompSn, excutorId, param.BizWorkSn, param.BizWorkYear);
            SelectList compInfoList = new SelectList(compInfoDropDown, "CompSn", "CompNm");
            ViewBag.SelectCompInfoList = compInfoList;

            //검색조건을 유지하기 위한
            ViewBag.SelectParam = param;

            ////종합보고서 조회
            //var listscMentoringTotalReport = await _scMentoringTotalReportService.GetMentoringTotalReportAsync(mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.CompSn);

            ////맨토링 종합 레포트 정보 조회
            //var listTotalReportView =
            //   Mapper.Map<List<MentoringTotalReportViewModel>>(listscMentoringTotalReport);

            //int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            //return View(new StaticPagedList<MentoringTotalReportViewModel>(listTotalReportView.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, listTotalReportView.Count));


            //종합보고서 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            var pagedListMentoringTotalReport = await _scMentoringTotalReportService.GetPagedListMentoringTotalReportByMngComp(int.Parse(curPage ?? "1"), pagingSize, mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.CompSn, null);

            //맨토링 종합 레포트 정보 조회
            var listTotalReportView =
               Mapper.Map<List<MentoringTotalReportViewModel>>(pagedListMentoringTotalReport);


            return View(new StaticPagedList<MentoringTotalReportViewModel>(listTotalReportView, int.Parse(curPage ?? "1"), pagingSize, pagedListMentoringTotalReport.TotalItemCount));
        }

        public async Task<ActionResult> MentoringTotalReportListByMentor(SelectedMentorTotalReportParmModel param, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            //사업년도 DownDown List Data
            var bizWorkYearDropDown = MakeBizYear(2015);
            SelectList bizWorkYear = new SelectList(bizWorkYearDropDown, "Value", "Text");
            ViewBag.SelectBizWorkYearList = bizWorkYear;

            //사업 DropDown List Data
            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, param.BizWorkYear);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;

            //멘토 DropDwon List Data
            var mentorDropDown = await MakeBizMentor(mngCompSn, excutorId, param.BizWorkSn, param.BizWorkYear);
            SelectList mentorList = new SelectList(mentorDropDown, "LoginId", "Name");
            ViewBag.SelectMentorList = mentorList;

            //검색조건을 유지하기 위한
            ViewBag.SelectParam = param;

            //종합보고서 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            var pagedListMentoringTotalReport = await _scMentoringTotalReportService.GetPagedListMentoringTotalReportByMngComp(int.Parse(curPage ?? "1"), pagingSize, mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, 0, param.MentorId);

            //맨토링 종합 레포트 정보 조회
            var listTotalReportView =
               Mapper.Map<List<MentoringTotalReportViewModel>>(pagedListMentoringTotalReport);

            return View(new StaticPagedList<MentoringTotalReportViewModel>(listTotalReportView, int.Parse(curPage ?? "1"), pagingSize, pagedListMentoringTotalReport.TotalItemCount));

            //var listscMentoringTotalReport = await _scMentoringTotalReportService.GetMentoringTotalReportAsync(mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.MentorId);

            ////맨토링 종합 레포트 정보 조회
            //var listTotalReportView =
            //   Mapper.Map<List<MentoringTotalReportViewModel>>(listscMentoringTotalReport);

            //int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            //return View(new StaticPagedList<MentoringTotalReportViewModel>(listTotalReportView.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, listTotalReportView.Count));
        }

        public async Task<ActionResult> MentoringTotalReportDetail(int totalReportSn, SelectedMentorTotalReportParmModel selectParam, string searchType)
        {
            ViewBag.LeftMenu = Global.Report;

            var scMentoringTotalReport = await _scMentoringTotalReportService.GetMentoringTotalReportById(totalReportSn);

            var listscFileInfo = scMentoringTotalReport.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo).Where(fi => fi.Status == "N");

            var listFileContent =
               Mapper.Map<List<FileContent>>(listscFileInfo);

            var totalReportViewModel =
               Mapper.Map<MentoringTotalReportViewModel>(scMentoringTotalReport);

            totalReportViewModel.FileContents = listFileContent;

            //검색조건 유지를 위해
            ViewBag.SelectParam = selectParam;
            //호출한 탭으로 돌아가기 위해
            ViewBag.SearchType = searchType;

            return View(totalReportViewModel);
        }
        #endregion


        #region 멘토 일지
        public async Task<ActionResult> MentoringReportListByComp(SelectedMentorReportParmModel param, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            //사업년도 DownDown List Data
            var bizWorkYearDropDown = MakeBizYear(2015);
            SelectList bizWorkYear = new SelectList(bizWorkYearDropDown, "Value", "Text");
            ViewBag.SelectBizWorkYearList = bizWorkYear;

            //사업 DropDown List Data
            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, param.BizWorkYear);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;

            //기업 DropDwon List Data
            var compInfoDropDown = await MakeBizComp(mngCompSn,excutorId, param.BizWorkSn, param.BizWorkYear);
            SelectList compInfoList = new SelectList(compInfoDropDown, "CompSn", "CompNm");
            ViewBag.SelectCompInfoList = compInfoList;

            //검색조건을 유지하기 위한
            ViewBag.SelectParam = param;


            //맨토링 일지 정보 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pagedListscMentoringReport = await _scMentoringReportService.GetPagedListMentoringReportByMngComp(int.Parse(curPage ?? "1"), pagingSize, mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.CompSn, null);

            //맨토링 일지 정보 to 뷰모델 매핑
            var listTotalReportView =
               Mapper.Map<List<MentoringReportViewModel>>(pagedListscMentoringReport);

           
            return View(new StaticPagedList<MentoringReportViewModel>(listTotalReportView, int.Parse(curPage ?? "1"), pagingSize, pagedListscMentoringReport.TotalItemCount));


            //var listscMentoringReport = await _scMentoringReportService.GetMentoringReportAsync(mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.CompSn);

            ////맨토링 일지 정보 to 뷰모델 매핑
            //var listTotalReportView =
            //   Mapper.Map<List<MentoringReportViewModel>>(listscMentoringReport);

            //int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            //return View(new StaticPagedList<MentoringReportViewModel>(listTotalReportView.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, listTotalReportView.Count));

        }

        public async Task<ActionResult> MentoringReportListByMentor(SelectedMentorReportParmModel param, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            //사업년도 DownDown List Data
            var bizWorkYearDropDown = MakeBizYear(2015);
            SelectList bizWorkYear = new SelectList(bizWorkYearDropDown, "Value", "Text");
            ViewBag.SelectBizWorkYearList = bizWorkYear;

            //사업 DropDown List Data
            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, param.BizWorkYear);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;

            //멘토 DropDwon List Data
            var mentorDropDown = await MakeBizMentor(mngCompSn, excutorId, param.BizWorkSn, param.BizWorkYear);
            SelectList mentorList = new SelectList(mentorDropDown, "LoginId", "Name");
            ViewBag.SelectMentorList = mentorList;


            //검색조건을 유지하기 위한
            ViewBag.SelectParam = param;

            //맨토링 일지 정보 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
   
            var pagedListscMentoringReport = await _scMentoringReportService.GetPagedListMentoringReportByMngComp(int.Parse(curPage ?? "1"), pagingSize, mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn,0, param.MentorId);

            //맨토링 일지 정보 to 뷰모델 매핑
            var listTotalReportView =
               Mapper.Map<List<MentoringReportViewModel>>(pagedListscMentoringReport);

            
            return View(new StaticPagedList<MentoringReportViewModel>(listTotalReportView, int.Parse(curPage ?? "1"), pagingSize, pagedListscMentoringReport.TotalItemCount));


            //var listscMentoringReport = await _scMentoringReportService.GetMentoringReportAsync(mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.MentorId);

            ////맨토링 일지 정보 to 뷰모델 매핑
            //var listTotalReportView =
            //   Mapper.Map<List<MentoringReportViewModel>>(listscMentoringReport);

            //int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            //return View(new StaticPagedList<MentoringReportViewModel>(listTotalReportView.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, listTotalReportView.Count));

        }

        public async Task<ActionResult> MentoringReportDetail(int reportSn, SelectedMentorReportParmModel selectParam, string searchType)
        {
            ViewBag.LeftMenu = Global.Report;

            var scMentoringReport = await _scMentoringReportService.GetMentoringReportById(reportSn);

            //멘토링 사진
            var listscMentoringImageInfo = scMentoringReport.ScMentoringFileInfoes.Where(mtfi => mtfi.Classify == "P").Select(mtfi => mtfi.ScFileInfo).Where(fi => fi.Status == "N");

            //사진추가
            var listMentoringPhotoView =
              Mapper.Map<List<FileContent>>(listscMentoringImageInfo);

            FileHelper fileHelper = new FileHelper();
            foreach (var mentoringPhoto in listMentoringPhotoView)
            {
                mentoringPhoto.FileBase64String = await fileHelper.GetPhotoString(mentoringPhoto.FilePath);
            }

            //첨부파일
            var listscFileInfo = scMentoringReport.ScMentoringFileInfoes.Where(mtfi => mtfi.Classify == "A").Select(mtfi => mtfi.ScFileInfo).Where(fi => fi.Status == "N");

            var listFileContentView =
               Mapper.Map<List<FileContent>>(listscFileInfo);

            //멘토링 상세 매핑
            var reportViewModel =
               Mapper.Map<MentoringReportViewModel>(scMentoringReport);

            //멘토링상세뷰에 파일정보 추가
            reportViewModel.FileContents = listFileContentView;
            reportViewModel.MentoringPhoto = listMentoringPhotoView;

            //검색조건 유지를 위해
            ViewBag.SelectParam = selectParam;
            //호출한 탭으로 돌아가기 위해
            ViewBag.SearchType = searchType;

            return View(reportViewModel);
        }

        #endregion

        #region 드롭다운박스 처리 controller
        [HttpPost]
        public async Task<JsonResult> getBizWork(int bizWorkYear)
        {
            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizList = await MakeBizWork(mngCompSn, excutorId, bizWorkYear);

            return Json(bizList);
        }

        [HttpPost]
        public async Task<JsonResult> getBizComp(int bizWorkSn, int bizWorkYear)
        {
            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var compInfoList = await MakeBizComp(mngCompSn, excutorId, bizWorkSn, bizWorkYear);

            return Json(compInfoList);
        }

        [HttpPost]
        public async Task<JsonResult> getBizMentor(int bizWorkSn, int bizWorkYear)
        {
            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var mentorList = await MakeBizMentor(mngCompSn, excutorId, bizWorkSn, bizWorkYear);

            return Json(mentorList);
        }
        #endregion


        #region 멘토링 관련 드롭다운리스트
        public IList<SelectListItem> MakeBizYear(int startYear)
        {
            //사업년도
            var bizWorkYearDropDown = new List<SelectListItem>();

            bizWorkYearDropDown.Add(new SelectListItem { Value = "0", Text = "사업년도 선택", Selected = true });

            for (int year = startYear; year <= DateTime.Now.Year; year++)
            {
                bizWorkYearDropDown.Add(
                    new SelectListItem
                    {
                        Value = year.ToString(),
                        Text = year.ToString()
                    });
            }

            return bizWorkYearDropDown;
        }

        public async Task<IList<BizWorkDropDownModel>> MakeBizWork(int mngCompSn, string excutorId, int bizWorkYear)
        {
            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(mngCompSn, excutorId, bizWorkYear);

            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);

            //사업드롭다운 타이틀 추가
            BizWorkDropDownModel titleBizWork = new BizWorkDropDownModel
            {
                BizWorkSn = 0,
                BizWorkNm = "사업명 선택"
            };

            bizWorkDropDown.Insert(0, titleBizWork);

            return bizWorkDropDown;
        }

        public async Task<IList<CompInfoDropDownModel>> MakeBizComp(int mngCompSn, string excutorId, int bizWorkSn, int bizWorkYear)
        {
            //기업 DropDwon List Data
            var listScCompInfo = await _scCompMappingService.GetBizWorkComList(mngCompSn, excutorId , bizWorkSn, bizWorkYear);

            var compInfoDropDown =
                Mapper.Map<List<CompInfoDropDownModel>>(listScCompInfo);

            //기업 드롭다운 타이틀 추가
            CompInfoDropDownModel titleCompInfo = new CompInfoDropDownModel
            {
                CompSn = 0,
                CompNm = "기업명 선택"
            };

            compInfoDropDown.Insert(0, titleCompInfo);

            return compInfoDropDown;
        }

        public async Task<IList<MentorDropDownModel>> MakeBizMentor(int mngCompSn, string excutorId, int bizWorkSn, int bizWorkYear)
        {
            //기업 DropDwon List Data
            var listScUsr = await _scMentorMappingService.GetMentorListByBizMng(mngCompSn, excutorId, bizWorkSn, bizWorkYear);

            var mentorDropDown =
                Mapper.Map<List<MentorDropDownModel>>(listScUsr);

            //기업 드롭다운 타이틀 추가
            MentorDropDownModel titlementor = new MentorDropDownModel
            {
                LoginId = "",
                Name = "멘토 선택"
            };

            mentorDropDown.Insert(0, titlementor);

            return mentorDropDown;
        }


        [HttpPost]
        public async Task<JsonResult> GetMonth(int BizWorkSn, int Year)
        {
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);
            var month = ReportHelper.MakeBizMonth(scBizWork, Year);
            return Json(month);
        }

        [HttpPost]
        public async Task<JsonResult> GetYear(int BizWorkSn)
        {
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);
            var year = ReportHelper.MakeBizYear(scBizWork);
            return Json(year);
        }

        [HttpPost]
        public async Task<JsonResult> GetQuarter(int BizWorkSn, int Year)
        {
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);
            var quarter = ReportHelper.MakeBizQuarter(scBizWork, Year);
            return Json(quarter);
        }
        #endregion


        #region 파일 다운로드
        public void DownloadReportFile()
        {
            //System.Collections.Specialized.NameValueCollection col = Request.QueryString;
            string fileNm = Request.QueryString["FileNm"];
            string filePath = Request.QueryString["FilePath"];

            string archiveName = fileNm;

            var files = new List<FileContent>();

            var file = new FileContent
            {
                FileNm = fileNm,
                FilePath = filePath
            };
            files.Add(file);

            new FileHelper().DownloadFile(files, archiveName);
        }


        public async Task DownloadTRReportFileMulti()
        {
            string totalReportSn = Request.QueryString["TotalReportSn"];

            string archiveName = "download.zip";

            //Eager Loading 방식
            var listscMentoringTrFileInfo = await _scMentoringTrFileInfoService.GetMentoringTrFileInfo(int.Parse(totalReportSn));

            var listScFileInfo = new List<ScFileInfo>();
            foreach (var scMentoringTrFileInfo in listscMentoringTrFileInfo)
            {
                listScFileInfo.Add(scMentoringTrFileInfo.ScFileInfo);
            }

            var files = Mapper.Map<IList<FileContent>>(listScFileInfo);

            new FileHelper().DownloadFile(files, archiveName);

        }

        public async Task DownloadReportFileMulti()
        {
            string reportSn = Request.QueryString["ReportSn"];

            string archiveName = "download.zip";

            //Eager Loading 방식
            var listscMentoringFileInfo = await _scMentoringFileInfoService.GetMentoringFileInfo(int.Parse(reportSn));

            var listScFileInfo = new List<ScFileInfo>();
            foreach (var scMentoringFileInfo in listscMentoringFileInfo)
            {
                listScFileInfo.Add(scMentoringFileInfo.ScFileInfo);
            }

            var files = Mapper.Map<IList<FileContent>>(listScFileInfo);

            new FileHelper().DownloadFile(files, archiveName);

        }
        #endregion

        #region 기업지원 통계
        public async Task<ActionResult> MentoringCompanyStats()
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(null);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(null);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MentoringCompanyStats(int BizWorkSn, int StartYear, int StartMonth, int EndYear, int EndMonth)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(scBizWork, StartYear);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(scBizWork, EndYear);

            var mentoringCompanyStatsViewModel = Mapper.Map<MentoringCompanyStatsViewModel>(scBizWork);

            mentoringCompanyStatsViewModel.Display = "Y";

            mentoringCompanyStatsViewModel.StartYear = StartYear.ToString();
            mentoringCompanyStatsViewModel.StartMonth = StartMonth.ToString();
            mentoringCompanyStatsViewModel.EndYear = EndYear.ToString();
            mentoringCompanyStatsViewModel.EndMonth = EndMonth.ToString();

            var listScCompMapping = await _scCompMappingService.GetCompMappingAsync(BizWorkSn);

            var listMentoringStatByCompanyViewModel = Mapper.Map<List<MentoringStatByCompanyViewModel>>(listScCompMapping);


            //실제 카운트 가져옴
            var listMentoringStatsByCompanyGroupModel = await _scMentoringReportService.GetMentoringReportGroupBy(BizWorkSn, StartYear, StartMonth, EndYear, EndMonth);

            var diffMonth = 12 * (EndYear - StartYear) + (EndMonth - StartMonth) + 1;
            int totalCount = 0, totalCount_D = 0, totalCount_F = 0, totalCount_L = 0, totalCount_M = 0, totalCount_P = 0, totalCount_T = 0, totalCount_W = 0, totalCount_E = 0;
            foreach (var mentoringStatByCompanyViewModel in listMentoringStatByCompanyViewModel.AsEnumerable())
            {
                var comSn = mentoringStatByCompanyViewModel.ComSn;


                int Count_D = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "D").FirstOrDefault() == null ? Count_D = 0 : Count_D = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "D").FirstOrDefault().Count;
                int Count_F = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "F").FirstOrDefault() == null ? Count_F = 0 : Count_F = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "F").FirstOrDefault().Count;
                int Count_L = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "L").FirstOrDefault() == null ? Count_L = 0 : Count_L = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "L").FirstOrDefault().Count;
                int Count_M = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "M").FirstOrDefault() == null ? Count_M = 0 : Count_M = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "M").FirstOrDefault().Count;
                int Count_P = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "P").FirstOrDefault() == null ? Count_P = 0 : Count_P = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "P").FirstOrDefault().Count;
                int Count_T = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "T").FirstOrDefault() == null ? Count_T = 0 : Count_T = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "T").FirstOrDefault().Count;
                int Count_W = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "W").FirstOrDefault() == null ? Count_W = 0 : Count_W = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "W").FirstOrDefault().Count;
                int Count_E = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "E").FirstOrDefault() == null ? Count_E = 0 : Count_E = listMentoringStatsByCompanyGroupModel.Where(mtcg => mtcg.CompSn == comSn && mtcg.MentoringAreaCd == "E").FirstOrDefault().Count;

                mentoringStatByCompanyViewModel.SumMentoring_D = Count_D;
                mentoringStatByCompanyViewModel.SumMentoring_F = Count_F;
                mentoringStatByCompanyViewModel.SumMentoring_L = Count_L;
                mentoringStatByCompanyViewModel.SumMentoring_M = Count_M;
                mentoringStatByCompanyViewModel.SumMentoring_P = Count_P;
                mentoringStatByCompanyViewModel.SumMentoring_T = Count_T;
                mentoringStatByCompanyViewModel.SumMentoring_W = Count_W;
                mentoringStatByCompanyViewModel.SumMentoring_E = Count_E;

                var totalCountByCompany = Count_D + Count_F + Count_L + Count_M + Count_P + Count_T + Count_W + Count_E;
                mentoringStatByCompanyViewModel.SumMentoringDays = totalCountByCompany;
                mentoringStatByCompanyViewModel.AvgMentoringDays = totalCountByCompany == 0 ? 0 : Math.Round((double)totalCountByCompany / (double)diffMonth, 2);

                totalCount += totalCountByCompany;
                totalCount_D += Count_D;
                totalCount_F += Count_F;
                totalCount_L += Count_L;
                totalCount_M += Count_M;
                totalCount_P += Count_P;
                totalCount_T += Count_T;
                totalCount_W += Count_W;
                totalCount_E += Count_E;
            }


            mentoringCompanyStatsViewModel.SumMentoringDays = totalCount;
            mentoringCompanyStatsViewModel.AvgMentoringDays = totalCount == 0 ? 0 : Math.Round((double)totalCount / (double)listMentoringStatByCompanyViewModel.Count, 2);
            mentoringCompanyStatsViewModel.SumMentoring_D = totalCount_D;
            mentoringCompanyStatsViewModel.SumMentoring_F = totalCount_F;
            mentoringCompanyStatsViewModel.SumMentoring_L = totalCount_L;
            mentoringCompanyStatsViewModel.SumMentoring_M = totalCount_M;
            mentoringCompanyStatsViewModel.SumMentoring_P = totalCount_P;
            mentoringCompanyStatsViewModel.SumMentoring_T = totalCount_T;
            mentoringCompanyStatsViewModel.SumMentoring_W = totalCount_W;
            mentoringCompanyStatsViewModel.SumMentoring_E = totalCount_E;


            mentoringCompanyStatsViewModel.MentoringStatByCompanyViewModel = listMentoringStatByCompanyViewModel;

            return View(mentoringCompanyStatsViewModel);

        }


        public async Task<ActionResult> MentoringMentorStats()
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(null);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(null);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MentoringMentorStats(int BizWorkSn, int StartYear, int StartMonth, int EndYear, int EndMonth)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(scBizWork, StartYear);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(scBizWork, EndYear);

            var mentoringMentorStatsViewModel = Mapper.Map<MentoringMentorStatsViewModel>(scBizWork);

            mentoringMentorStatsViewModel.Display = "Y";

            mentoringMentorStatsViewModel.StartYear = StartYear.ToString();
            mentoringMentorStatsViewModel.StartMonth = StartMonth.ToString();
            mentoringMentorStatsViewModel.EndYear = EndYear.ToString();
            mentoringMentorStatsViewModel.EndMonth = EndMonth.ToString();

            var listScUsr = await _scMentorMappingService.GetMentorListByBizWork(BizWorkSn);

            var listMentoringStatByMentorViewModel = Mapper.Map<List<MentoringStatByMentorViewModel>>(listScUsr);


            //실제 카운트 가져옴
            var listMentoringStatsByMentorGroupModel = await _scMentoringReportService.GetMentoringReportGroupByMentor(BizWorkSn, StartYear, StartMonth, EndYear, EndMonth);
            var listMentorCompGroupModel = await _scMentoringReportService.GetMentoringReportGroupByMentorComp(BizWorkSn, StartYear, StartMonth, EndYear, EndMonth);

            var diffMonth = 12 * (EndYear - StartYear) + (EndMonth - StartMonth) + 1;
            int totalMentoringDays = 0, totalMentoringCount = 0, totalMentoringHours = 0;

            foreach (var mentoringStatByMentorViewModel in listMentoringStatByMentorViewModel.AsEnumerable())
            {
                var mentorId = mentoringStatByMentorViewModel.LoginId;

                int mentoringDays = listMentoringStatsByMentorGroupModel.Where(mtcg => mtcg.LoginId == mentorId).Count();
                int mentoringCount = listMentoringStatsByMentorGroupModel.Where(mtcg => mtcg.LoginId == mentorId).FirstOrDefault() == null ? mentoringCount = 0 : mentoringCount = listMentoringStatsByMentorGroupModel.Where(mtcg => mtcg.LoginId == mentorId).Select(mtcg => mtcg.Count).Sum();
                int mentoringHours = listMentoringStatsByMentorGroupModel.Where(mtcg => mtcg.LoginId == mentorId).FirstOrDefault() == null ? mentoringHours = 0 : mentoringHours = listMentoringStatsByMentorGroupModel.Where(mtcg => mtcg.LoginId == mentorId).Select(mtcg => mtcg.SumMentoringHours).Sum();


                mentoringStatByMentorViewModel.CountMentoringComp = listMentorCompGroupModel.Where(mtcg => mtcg.LoginId == mentorId).Count();
                mentoringStatByMentorViewModel.SumMentoringDays = listMentoringStatsByMentorGroupModel.Where(mtcg => mtcg.LoginId == mentorId).Count();
                mentoringStatByMentorViewModel.SumMentoringCount = mentoringCount;
                mentoringStatByMentorViewModel.SumMentoringHours = mentoringHours;


                totalMentoringDays += mentoringDays;
                totalMentoringCount += mentoringCount;
                totalMentoringHours += mentoringHours;
            }

            var mentorCount = listMentoringStatByMentorViewModel.Count;

            mentoringMentorStatsViewModel.SumMentoringDays = totalMentoringDays;
            mentoringMentorStatsViewModel.SumMentoringCount = totalMentoringCount;
            mentoringMentorStatsViewModel.SumMentoringHours = totalMentoringHours;
            mentoringMentorStatsViewModel.AvgMentoringDays = totalMentoringDays == 0 ? 0 : Math.Round((double)totalMentoringDays / (double)mentorCount, 2);
            mentoringMentorStatsViewModel.AvgMentoringCount = totalMentoringCount == 0 ? 0 : Math.Round((double)totalMentoringCount / (double)mentorCount, 2);
            mentoringMentorStatsViewModel.AvgMentoringHours = totalMentoringHours == 0 ? 0 : Math.Round((double)totalMentoringHours / (double)mentorCount, 2);

            mentoringMentorStatsViewModel.ListMentoringStatByMentorViewModel = listMentoringStatByMentorViewModel;

            return View(mentoringMentorStatsViewModel);
        }


        public async Task<ActionResult> MentoringAreaStats()
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(null);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(null);

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> MentoringAreaStats(int BizWorkSn, int StartYear, int StartMonth, int EndYear, int EndMonth)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(scBizWork, StartYear);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(scBizWork, EndYear);

            var mentoringAreayStatsViewModel = Mapper.Map<MentoringAreaStatsViewModel>(scBizWork);

            mentoringAreayStatsViewModel.Display = "Y";

            mentoringAreayStatsViewModel.StartYear = StartYear.ToString();
            mentoringAreayStatsViewModel.StartMonth = StartMonth.ToString();
            mentoringAreayStatsViewModel.EndYear = EndYear.ToString();
            mentoringAreayStatsViewModel.EndMonth = EndMonth.ToString();


            //실제 카운트 가져옴
            var listMentoringStatsByAreaGroupModel = await _scMentoringReportService.GetMentoringReportGroupByArea(BizWorkSn, StartYear, StartMonth, EndYear, EndMonth);

            var diffMonth = 12 * (EndYear - StartYear) + (EndMonth - StartMonth) + 1;

            mentoringAreayStatsViewModel.SumMentoring_D = listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "D").FirstOrDefault() == null ? 0 : listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "D").FirstOrDefault().Count;
            mentoringAreayStatsViewModel.SumMentoring_F = listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "F").FirstOrDefault() == null ? 0 : listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "F").FirstOrDefault().Count;
            mentoringAreayStatsViewModel.SumMentoring_L = listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "L").FirstOrDefault() == null ? 0 : listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "L").FirstOrDefault().Count;
            mentoringAreayStatsViewModel.SumMentoring_M = listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "M").FirstOrDefault() == null ? 0 : listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "M").FirstOrDefault().Count;
            mentoringAreayStatsViewModel.SumMentoring_P = listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "P").FirstOrDefault() == null ? 0 : listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "P").FirstOrDefault().Count;
            mentoringAreayStatsViewModel.SumMentoring_T = listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "T").FirstOrDefault() == null ? 0 : listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "T").FirstOrDefault().Count;
            mentoringAreayStatsViewModel.SumMentoring_W = listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "W").FirstOrDefault() == null ? 0 : listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "W").FirstOrDefault().Count;
            mentoringAreayStatsViewModel.SumMentoring_E = listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "E").FirstOrDefault() == null ? 0 : listMentoringStatsByAreaGroupModel.Where(mtag => mtag.MentoringAreaCd == "E").FirstOrDefault().Count;

            mentoringAreayStatsViewModel.AvgMentoring_D = mentoringAreayStatsViewModel.SumMentoring_D == 0 ? 0 : Math.Round((double)mentoringAreayStatsViewModel.SumMentoring_D / (double)diffMonth, 2);
            mentoringAreayStatsViewModel.AvgMentoring_F = mentoringAreayStatsViewModel.SumMentoring_F == 0 ? 0 : Math.Round((double)mentoringAreayStatsViewModel.SumMentoring_F / (double)diffMonth, 2);
            mentoringAreayStatsViewModel.AvgMentoring_L = mentoringAreayStatsViewModel.SumMentoring_L == 0 ? 0 : Math.Round((double)mentoringAreayStatsViewModel.SumMentoring_L / (double)diffMonth, 2);
            mentoringAreayStatsViewModel.AvgMentoring_M = mentoringAreayStatsViewModel.SumMentoring_M == 0 ? 0 : Math.Round((double)mentoringAreayStatsViewModel.SumMentoring_M / (double)diffMonth, 2);
            mentoringAreayStatsViewModel.AvgMentoring_P = mentoringAreayStatsViewModel.SumMentoring_P == 0 ? 0 : Math.Round((double)mentoringAreayStatsViewModel.SumMentoring_P / (double)diffMonth, 2);
            mentoringAreayStatsViewModel.AvgMentoring_T = mentoringAreayStatsViewModel.SumMentoring_T == 0 ? 0 : Math.Round((double)mentoringAreayStatsViewModel.SumMentoring_T / (double)diffMonth, 2);
            mentoringAreayStatsViewModel.AvgMentoring_W = mentoringAreayStatsViewModel.SumMentoring_W == 0 ? 0 : Math.Round((double)mentoringAreayStatsViewModel.SumMentoring_W / (double)diffMonth, 2);
            mentoringAreayStatsViewModel.AvgMentoring_E = mentoringAreayStatsViewModel.SumMentoring_E == 0 ? 0 : Math.Round((double)mentoringAreayStatsViewModel.SumMentoring_E / (double)diffMonth, 2);


            return View(mentoringAreayStatsViewModel);

        }

        #endregion


        #region 참여기업 통계

        public async Task<ActionResult> BizInCompanyStats()
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(null);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(null);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BizInCompanyStats(int BizWorkSn, int StartYear, int StartMonth, int EndYear, int EndMonth)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(scBizWork, StartYear);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(scBizWork, EndYear);

            var bizInCompanyStatsView =  Mapper.Map<BizInCompanyStatsViewModel>(scBizWork);
            bizInCompanyStatsView.Display = "Y";
            var scCompMappingList = await _scCompMappingService.GetCompMappingAsync(BizWorkSn);
            bizInCompanyStatsView.compnayStatsListViewModel = new List<CompnayStatsViewModel>();

            bizInCompanyStatsView.StartYear = StartYear.ToString();
            bizInCompanyStatsView.StartMonth = StartMonth.ToString();
            bizInCompanyStatsView.EndYear = EndYear.ToString();
            bizInCompanyStatsView.EndMonth = EndMonth.ToString();

            Dictionary<int, decimal> sales = new Dictionary<int, decimal>();
            Dictionary<int, decimal> employ = new Dictionary<int, decimal>();

            foreach(var item in scCompMappingList)
            {
                var result = await _finenceReportService.GetCompanyMonthSalesAsync(ReportHelper.MakeSalesMonthProcedureParams(item.ScCompInfo.RegistrationNo, "1000", "1100", StartYear.ToString(), StartMonth.ToString(), EndYear.ToString(), EndMonth.ToString()));
                sales.Add(item.CompSn, result.TERM_SALE.Value);
                employ.Add(item.CompSn, result.QT_EMP.Value);

                bizInCompanyStatsView.compnayStatsListViewModel.Add(ReportHelper.MakeMonthCompnayStatsViewModel(item, result));
            }

            bizInCompanyStatsView.SumSales = Math.Truncate(sales.Values.Sum() / 1000).ToString();
            bizInCompanyStatsView.AvgSales = Math.Truncate(sales.Values.Average() / 1000).ToString();

            bizInCompanyStatsView.SumEmploy = Math.Truncate(employ.Values.Sum() / 1000).ToString();
            bizInCompanyStatsView.AvgEmploy = Math.Truncate(employ.Values.Average() / 1000).ToString();



            return View(bizInCompanyStatsView);
        }
        #endregion

        #region 기업별 통계

        public async Task<ActionResult> CompanyMonthlyStats()
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(null);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(null);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CompanyMonthlyStats(int BizWorkSn, int StartYear, int StartMonth, int EndYear, int EndMonth)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectStartMonthList = ReportHelper.MakeBizMonth(scBizWork, StartYear);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectEndMonthList = ReportHelper.MakeBizMonth(scBizWork, EndYear);

            var bizInCompanyStatsView = Mapper.Map<BizInCompanyStatsViewModel>(scBizWork);
            bizInCompanyStatsView.Display = "Y";

            var scCompMappingList = await _scCompMappingService.GetCompMappingAsync(BizWorkSn);

            bizInCompanyStatsView.compnayStatsListViewModel = new List<CompnayStatsViewModel>();
            bizInCompanyStatsView.StartYear = StartYear.ToString();
            bizInCompanyStatsView.StartMonth = StartMonth.ToString();
            bizInCompanyStatsView.EndYear = EndYear.ToString();
            bizInCompanyStatsView.EndMonth = EndMonth.ToString();

            foreach (var item in scCompMappingList)
            {
                var result = await _finenceReportService.GetCompanyMonthSalesAsync(ReportHelper.MakeSalesMonthProcedureParams(item.ScCompInfo.RegistrationNo, "1000", "1100", StartYear.ToString(), StartMonth.ToString(), EndYear.ToString(), EndMonth.ToString()));
                bizInCompanyStatsView.compnayStatsListViewModel.Add(ReportHelper.MakeMonthCompnayStatsViewModel(item, result));
            }

            return View(bizInCompanyStatsView);
        }


        public async Task<ActionResult> CompanyQuarterlyStats()
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectStartQuarterList = ReportHelper.MakeBizQuarter(null);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectEndQuarterList = ReportHelper.MakeBizQuarter(null);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CompanyQuarterlyStats(int BizWorkSn, int StartYear, int StartQuarter, int EndYear, int EndQuarter)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectStartQuarterList = ReportHelper.MakeBizQuarter(scBizWork, StartYear);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectEndQuarterList = ReportHelper.MakeBizQuarter(scBizWork, EndYear);

            var bizInCompanyStatsView = Mapper.Map<BizInCompanyStatsViewModel>(scBizWork);
            bizInCompanyStatsView.Display = "Y";

            var scCompMappingList = await _scCompMappingService.GetCompMappingAsync(BizWorkSn);

            bizInCompanyStatsView.compnayStatsListViewModel = new List<CompnayStatsViewModel>();
            bizInCompanyStatsView.StartYear = StartYear.ToString();
            bizInCompanyStatsView.StartQuarter = StartQuarter.ToString();
            bizInCompanyStatsView.EndYear = EndYear.ToString();
            bizInCompanyStatsView.EndQuarter = EndQuarter.ToString();

            foreach (var item in scCompMappingList)
            {
                var result = await _finenceReportService.GetCompanyQuarterSalesAsync(ReportHelper.MakeSalesQuarterProcedureParams(item.ScCompInfo.RegistrationNo, "1000", "1100", StartYear.ToString(), StartQuarter.ToString(), EndYear.ToString(), EndQuarter.ToString()));
                bizInCompanyStatsView.compnayStatsListViewModel.Add(ReportHelper.MakeQuarterCompnayStatsViewModel(item, result));
            }

            return View(bizInCompanyStatsView);
        }


        public async Task<ActionResult> CompanyYearlyStats()
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(null);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(null);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CompanyYearlyStats(int BizWorkSn, int StartYear, int EndYear)
        {
            ViewBag.LeftMenu = Global.Report;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            int mngCompSn = int.Parse(Session[Global.CompSN].ToString());

            var bizWorkDropDown = await MakeBizWork(mngCompSn, excutorId, 0);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(BizWorkSn);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");
            ViewBag.SelectBizWorkList = bizList;
            ViewBag.SelectStartYearList = ReportHelper.MakeBizYear(scBizWork);
            ViewBag.SelectEndYearList = ReportHelper.MakeBizYear(scBizWork);

            var bizInCompanyStatsView = Mapper.Map<BizInCompanyStatsViewModel>(scBizWork);
            bizInCompanyStatsView.Display = "Y";

            var scCompMappingList = await _scCompMappingService.GetCompMappingAsync(BizWorkSn);

            bizInCompanyStatsView.compnayStatsListViewModel = new List<CompnayStatsViewModel>(); 
            bizInCompanyStatsView.StartYear = StartYear.ToString();
            bizInCompanyStatsView.EndYear = EndYear.ToString();

            foreach (var item in scCompMappingList)
            {
                var result = await _finenceReportService.GetCompanyYearSalesAsync(ReportHelper.MakeSalesYearProcedureParams(item.ScCompInfo.RegistrationNo, "1000", "1100", StartYear.ToString(), EndYear.ToString()));
                bizInCompanyStatsView.compnayStatsListViewModel.Add(ReportHelper.MakeYearCompnayStatsViewModel(item, result));
            }

            return View(bizInCompanyStatsView);
        }
        #endregion

        public ActionResult BasicSurveyReport(string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업년도 DownDown List Data
            ViewBag.SelectBizWorkYearList = ReportHelper.MakeYear(2015);
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BasicSurveyReport(BasicSurveyReportViewModel paramModel, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업년도 DownDown List Data
            ViewBag.SelectBizWorkYearList = ReportHelper.MakeYear(2015);

            var compSn = Session[Global.CompSN].ToString();
            var executorId = Session[Global.LoginID].ToString();

            if (Session[Global.UserDetailType].ToString() == "A")
                executorId = null;

            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(compSn), executorId, paramModel.BizWorkYear);
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(listScBizWork);


            //기초역량 보고서 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var rptMsters = await rptMasterService.GetRptMasterListForBizManager(int.Parse(curPage ?? "1"), pagingSize, executorId, paramModel.BizWorkYear, paramModel.BizWorkSn, int.Parse(compSn), "C");

            //뷰모델 맵핑
            var rptMasterListView = Mapper.Map<List<BasicSurveyReportViewModel>>(rptMsters);

            return View(new StaticPagedList<BasicSurveyReportViewModel>(rptMasterListView, int.Parse(curPage ?? "1"), pagingSize, rptMsters.TotalItemCount));

        }


        [HttpPost]
        public async Task<JsonResult> GetBizWorkNm(int Year)
        {
            var compSn = Session[Global.CompSN].ToString();
            var executorId = Session[Global.LoginID].ToString();

            if (Session[Global.UserDetailType].ToString() == "A")
                executorId = null;

            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(compSn), executorId, Year);
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(listScBizWork);

            var bizList = ReportHelper.MakeBizWorkList(listScBizWork);

            return Json(bizList);
        }

        public async Task<ActionResult> FinanceReport(string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업년도 DownDown List Data
            ViewBag.SelectBizWorkYearList = ReportHelper.MakeYear(2015);
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> FinanceReport(BasicSurveyReportViewModel paramModel, string curPage)
        {
            ViewBag.LeftMenu = Global.Report;
            //사업관리기관 DownDown List Data
            ViewBag.SelectBizWorkYearList = ReportHelper.MakeYear(2015);

            var compSn = Session[Global.CompSN].ToString();
            var executorId = Session[Global.LoginID].ToString();

            if (Session[Global.UserDetailType].ToString() == "A")
                executorId = null;

            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(compSn), executorId, paramModel.BizWorkYear);
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(listScBizWork);


            //사업별 기업 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var scCompMappings = await _scCompMappingService.GetPagedListCompMappingsAsync(int.Parse(curPage ?? "1"), pagingSize, int.Parse(compSn), executorId, paramModel.BizWorkSn);

            //뷰모델 맵핑
            var rptMasterListView = Mapper.Map<List<BasicSurveyReportViewModel>>(scCompMappings.ToList());

            return View(new StaticPagedList<BasicSurveyReportViewModel>(rptMasterListView, int.Parse(curPage ?? "1"), pagingSize, scCompMappings.TotalItemCount));

        }


    }
}