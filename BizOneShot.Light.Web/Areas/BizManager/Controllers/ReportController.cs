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

        private readonly IScMentoringReportService _scMentoringReportService;
        private readonly IScMentoringFileInfoService _scMentoringFileInfoService;


        public ReportController(IScBizWorkService scBizWorkService
            , IScCompMappingService scCompMappingService
            , IScMentorMappingService scMentorMappingService
            , IScMentoringTotalReportService scMentoringTotalReportService
            , IScMentoringTrFileInfoService scMentoringTrFileInfoService
            , IScMentoringReportService scMentoringReportService
            , IScMentoringFileInfoService scMentoringFileInfoService
            )
        {
            this._scBizWorkService = scBizWorkService;
            this._scCompMappingService = scCompMappingService;
            this._scMentorMappingService = scMentorMappingService;
            this._scMentoringTotalReportService = scMentoringTotalReportService;
            this._scMentoringTrFileInfoService = scMentoringTrFileInfoService;
            this._scMentoringReportService = scMentoringReportService;
            this._scMentoringFileInfoService = scMentoringFileInfoService;
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
            var pagedListMentoringTotalReport = _scMentoringTotalReportService.GetMentoringTotalReportAsync(int.Parse(curPage ?? "1"), pagingSize, mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.CompSn);

            //맨토링 종합 레포트 정보 조회
            var listTotalReportView =
               Mapper.Map<List<MentoringTotalReportViewModel>>(pagedListMentoringTotalReport.ToList());


            return View(new StaticPagedList<MentoringTotalReportViewModel>(listTotalReportView, int.Parse(curPage ?? "1"), pagingSize, pagedListMentoringTotalReport.TotalCount));
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
            var listscMentoringTotalReport = await _scMentoringTotalReportService.GetMentoringTotalReportAsync(mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.MentorId);


            //맨토링 종합 레포트 정보 조회
            var listTotalReportView =
               Mapper.Map<List<MentoringTotalReportViewModel>>(listscMentoringTotalReport);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            return View(new StaticPagedList<MentoringTotalReportViewModel>(listTotalReportView.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, listTotalReportView.Count));
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
            var listscMentoringReport = await _scMentoringReportService.GetMentoringReportAsync(mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.CompSn);

            //맨토링 일지 정보 to 뷰모델 매핑
            var listTotalReportView =
               Mapper.Map<List<MentoringReportViewModel>>(listscMentoringReport);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            return View(new StaticPagedList<MentoringReportViewModel>(listTotalReportView.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, listTotalReportView.Count));

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
            var listscMentoringReport = await _scMentoringReportService.GetMentoringReportAsync(mngCompSn, excutorId, param.BizWorkYear, param.BizWorkSn, param.MentorId);

            //맨토링 일지 정보 to 뷰모델 매핑
            var listTotalReportView =
               Mapper.Map<List<MentoringReportViewModel>>(listscMentoringReport);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            return View(new StaticPagedList<MentoringReportViewModel>(listTotalReportView.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, listTotalReportView.Count));

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


    }
}