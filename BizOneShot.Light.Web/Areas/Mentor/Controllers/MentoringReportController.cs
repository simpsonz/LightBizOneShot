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

namespace BizOneShot.Light.Web.Areas.Mentor.Controllers
{
   

    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
    public class MentoringReportController : BaseController
    {
        private readonly IScCompMappingService _scCompMappingService;
        private readonly IScMentorMappingService _scMentorMappingService;
        private readonly IScMentoringTotalReportService _scMentoringTotalReportService;
        private readonly IScMentoringTrFileInfoService _scMentoringTrFileInfoService;

        public MentoringReportController(IScCompMappingService scCompMappingService
            , IScMentorMappingService scMentorMappingService 
            , IScMentoringTotalReportService scMentoringTotalReportService
            , IScMentoringTrFileInfoService scMentoringTrFileInfoService)
        {
            this._scCompMappingService = scCompMappingService;
            this._scMentorMappingService = scMentorMappingService;
            this._scMentoringTotalReportService = scMentoringTotalReportService;
            this._scMentoringTrFileInfoService = scMentoringTrFileInfoService;
        }


        public async Task<ActionResult> RegMentoringTotalReport()
        {
            ViewBag.LeftMenu = Global.MentoringReport;

            var mentorId = Session[Global.LoginID].ToString();

            //사업 DropDown List Data
            var listScMentorMapping = await _scMentorMappingService.GetMentorMappingListByMentorId(mentorId);
            var listScBizWork = listScMentorMapping.Select(mmp => mmp.ScBizWork);

            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);

            //사업드롭다운 타이틀 추가
            BizWorkDropDownModel titleBizWork = new BizWorkDropDownModel
            {
                BizWorkSn = 0,
                BizWorkNm = "사업명 선택"
            };

            bizWorkDropDown.Insert(0, titleBizWork);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;


            //기업 DropDwon List Data
            var listScCompMapping = await _scCompMappingService.GetCompMappingListByMentorId(mentorId, "A");
            var listScCompInfo = listScCompMapping.Select(cmp => cmp.ScCompInfo);//.ToList();

            var compInfoDropDown =
                Mapper.Map<List<CompInfoDropDownModel>>(listScCompInfo);

            //기업 드롭다운 타이틀 추가
            CompInfoDropDownModel titleCompInfo = new CompInfoDropDownModel
            {
                CompSn = 0,
                CompNm = "기업명 선택"
            };

            compInfoDropDown.Insert(0, titleCompInfo);

            SelectList compInfoList = new SelectList(compInfoDropDown, "CompSn", "CompNm");

            ViewBag.SelectCompInfoList = compInfoList;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegMentoringTotalReport(MentoringTotalReportViewModel dataRequestViewModel, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.LeftMenu = Global.MentoringReport;

            var mentorId = Session[Global.LoginID].ToString();

            if (ModelState.IsValid)
            {
                var scMentoringTotalReport = Mapper.Map<ScMentoringTotalReport>(dataRequestViewModel);

                scMentoringTotalReport.MentorId = mentorId;
                scMentoringTotalReport.RegId = mentorId;
                scMentoringTotalReport.RegDt = DateTime.Now;
                scMentoringTotalReport.Status = "N";

                //신규파일정보저장 및 파일업로드
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file != null)
                        {
                            var fileHelper = new FileHelper();

                            var savedFileName = fileHelper.GetUploadFileName(file);

                            var subDirectoryPath = Path.Combine(FileType.Mentoring_Total.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                            var savedFilePath = Path.Combine(subDirectoryPath, savedFileName);

                            var scFileInfo = new ScFileInfo { FileNm = Path.GetFileName(file.FileName), FilePath = savedFilePath, Status = "N", RegId = Session[Global.LoginID].ToString(), RegDt = DateTime.Now };

                            var scMentoringTrFileInfo = new ScMentoringTrFileInfo { ScFileInfo = scFileInfo };
                            scMentoringTrFileInfo.Classify = "A";

                            scMentoringTotalReport.ScMentoringTrFileInfoes.Add(scMentoringTrFileInfo);

                            await fileHelper.UploadFile(file, subDirectoryPath, savedFileName);
                        }
                    }
                }

                //저장
                int result = await _scMentoringTotalReportService.AddScMentoringTotalReportAsync(scMentoringTotalReport);

                if (result != -1)
                    return RedirectToAction("MentoringTotalReportList", "MentoringReport");
                else
                {
                    ModelState.AddModelError("", "자료요청 등록 실패.");
                    return View(dataRequestViewModel);
                }
            }
            ModelState.AddModelError("", "입력값 검증 실패.");
            return View(dataRequestViewModel);
        }


        [HttpPost]
        public async Task<JsonResult> DeleteMentoringTotalReport(string [] totalReportSns)
        {
            ViewBag.LeftMenu = Global.MentoringReport;

            await _scMentoringTotalReportService.DeleteMentoringTotalReport(totalReportSns);

            return Json(new { result = true });
        }

        public async Task<ActionResult> MentoringTotalReportDetail(int totalReportSn, SelectedMentorTotalReportParmModel selectParam)
        {
            ViewBag.LeftMenu = Global.MentoringReport;

            var scMentoringTotalReport = await _scMentoringTotalReportService.GetMentoringTotalReportById(totalReportSn);

            var listscFileInfo = scMentoringTotalReport.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo).Where(fi => fi.Status == "N");

            var listFileContent =
               Mapper.Map<List<FileContent>>(listscFileInfo);


            var totalReportViewModel =
               Mapper.Map<MentoringTotalReportViewModel>(scMentoringTotalReport);

            totalReportViewModel.FileContents = listFileContent;

            //검색조건 유지를 위해
            ViewBag.SelectParam = selectParam;

            return View(totalReportViewModel);
        }

        public async Task<ActionResult> MentoringTotalReportList(SelectedMentorTotalReportParmModel param, string curPage = "1")
        {
            ViewBag.LeftMenu = Global.MentoringReport;

            var mentorId = Session[Global.LoginID].ToString();

            //사업 DropDown List Data
            var listScMentorMapping = await _scMentorMappingService.GetMentorMappingListByMentorId(mentorId);
            var listScBizWork = listScMentorMapping.Select(mmp => mmp.ScBizWork);//.ToList();

            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);

            //사업드롭다운 타이틀 추가
            BizWorkDropDownModel titleBizWork = new BizWorkDropDownModel
            {
                BizWorkSn = 0,
                BizWorkNm = "사업명 선택"
            };
            
            bizWorkDropDown.Insert(0, titleBizWork);
          
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm", param.BizWorkSn);

            ViewBag.SelectBizWorkList = bizList;


            //기업 DropDwon List Data
            var listScCompMapping = await _scCompMappingService.GetCompMappingListByMentorId(mentorId, "A");
            var listScCompInfo = listScCompMapping.Select(cmp => cmp.ScCompInfo);//.ToList();

            var compInfoDropDown =
                Mapper.Map<List<CompInfoDropDownModel>>(listScCompInfo);

            //기업 드롭다운 타이틀 추가
            CompInfoDropDownModel titleCompInfo = new CompInfoDropDownModel
            {
                CompSn = 0,
                CompNm="기업명 선택"
            };

            compInfoDropDown.Insert(0, titleCompInfo);

            SelectList compInfoList = new SelectList(compInfoDropDown, "CompSn", "CompNm", param.CompSn);

            ViewBag.SelectCompInfoList = compInfoList;


            //제출년도 DownDown List Data
            var listSubmitDt = await _scMentoringTotalReportService.GetMentoringTotalReportSubmitDt(mentorId);

            var submitDtDropDown = new List<SubmitDtDropDownModel>();

            foreach (var submitDt in listSubmitDt.AsEnumerable())
            {
                //SubmitDtDropDownModel submitModel = new SubmitDtDropDownModel
                //{
                //    SubmitDt = submitDt,
                //    SubmitYear = submitDt.ToString()
                //};

                submitDtDropDown.Add(
                    new SubmitDtDropDownModel
                    {
                        SubmitDt = submitDt,
                        SubmitYear = submitDt.ToString()
                    });
            }

            SubmitDtDropDownModel titleSubmitDt = new SubmitDtDropDownModel
            {
                SubmitDt = 0,
                SubmitYear = "제출일 선택"
            };

            submitDtDropDown.Insert(0, titleSubmitDt);

            SelectList submitDtList = new SelectList(submitDtDropDown, "SubmitDt", "SubmitYear", param.SubmitDt);

            ViewBag.SelectSubmitList = submitDtList;

     
            //검색조건을 유지하기 위한
            ViewBag.SelectParam = param;

            //실제 쿼리
            var listscMentoringTotalReport = await _scMentoringTotalReportService.GetMentoringTotalReportAsync(mentorId, param.SubmitDt, param.BizWorkSn, param.CompSn);


            //맨토링 종합 레포트 정보 매핑
            var listTotalReportView =
               Mapper.Map<List<MentoringTotalReportViewModel >>(listscMentoringTotalReport);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            return View(new StaticPagedList<MentoringTotalReportViewModel>(listTotalReportView.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, listTotalReportView.Count));
        }


        public void DownloadTRReportFile()
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


    }
}