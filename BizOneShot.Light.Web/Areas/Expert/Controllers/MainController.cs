using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Helper;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Areas.Expert.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Expert, Order = 2)]
    public class MainController : BaseController
    {
        private readonly IScExpertMappingService _scExpertMappingService;
        private readonly IScUsrService _scUsrService;
        private readonly IScNtcService _scNtcService;
        private readonly IScBizTypeService _scBizTypeService;

        public MainController(IScNtcService scNtcServcie, IScExpertMappingService scExpertMappingService, IScUsrService scUsrService, IScBizTypeService scBizTypeService)
        {
            _scNtcService = scNtcServcie;
            _scExpertMappingService = scExpertMappingService;
            _scUsrService = scUsrService;
            _scBizTypeService = scBizTypeService;
        }
        // GET: Expert/Main
        public async Task<ActionResult> Index()
        {
            string agreeYn = Session[Global.AgreeYn].ToString();

            //개인정보 수집 및 이용에 대한 동의가 안되어 있으면 리다이렉트 함
            if (agreeYn != "Y")
            {
                TempData["alert"] = "개인정보 수집 및 이용을 동의하셔야 사용이 가능합니다";

                return RedirectToAction("ExpertAgreement", "Main");

            }
            //var listScNtc = _scNtcService.GetNotices();
            var listScNtc = await _scNtcService.GetNoticesForMainAsync();

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(listScNtc);
            return View(noticeViews);
        }

        public ActionResult ExpertAgreement()
        {

            return View();
        }

        public async Task<ActionResult> AgreeExpertAgreement()
        {
            ScUsr scUsr = await _scUsrService.SelectMentorInfo(Session[Global.LoginID].ToString());

            scUsr.AgreeYn = "Y";

            _scUsrService.ModifyScUsr(scUsr);

            await _scUsrService.SaveDbContextAsync();

            Session[Global.AgreeYn] = "Y";

            return RedirectToAction("Index", "Main");
        }

        public async Task<ActionResult> MyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;
            var scExpertMapping = await _scExpertMappingService.GetExpertAsync(Session[Global.LoginID].ToString());

            var usrView =
               Mapper.Map<JoinExpertViewModel>(scExpertMapping);

            //업종, 종목
            var listScBizType = await _scBizTypeService.GetScBizTypeByCompSn(int.Parse(Session[Global.CompSN].ToString()));
            var bizTypeViewModel =
               Mapper.Map<List<BizTypeViewModel>>(listScBizType);

            usrView.BizTypes = bizTypeViewModel;

            return View(usrView);
        }


        public async Task<ActionResult> ModifyMyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;
            var scExpertMapping = await _scExpertMappingService.GetExpertAsync(Session[Global.LoginID].ToString());

            var usrView =
               Mapper.Map<JoinExpertViewModel>(scExpertMapping);

            //업종, 종목
            var listScBizType = await _scBizTypeService.GetScBizTypeByCompSn(int.Parse(Session[Global.CompSN].ToString()));
            var bizTypeViewModel =
               Mapper.Map<List<BizTypeViewModel>>(listScBizType);

            usrView.BizTypes = bizTypeViewModel;

            return View(usrView);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyMyInfo(JoinExpertViewModel joinExpertViewModel, string DeleteFileSn, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.LeftMenu = Global.MyInfo;
            var scExpertMapping = await _scExpertMappingService.GetExpertAsync(Session[Global.LoginID].ToString());

            scExpertMapping.ScUsr.UsrTypeDetail = joinExpertViewModel.UsrTypeDetail;
            scExpertMapping.ScUsr.Name = joinExpertViewModel.Name;
            scExpertMapping.ScUsr.TelNo = joinExpertViewModel.TelNo1+"-"+ joinExpertViewModel.TelNo2 + "-" + joinExpertViewModel.TelNo3;
            scExpertMapping.ScUsr.MbNo = joinExpertViewModel.MbNo1 + "-" + joinExpertViewModel.MbNo2 + "-" + joinExpertViewModel.MbNo3;
            scExpertMapping.ScUsr.Email = joinExpertViewModel.Email1 + "@" + joinExpertViewModel.Email2;

            scExpertMapping.ScUsr.ScCompInfo.CompNm = joinExpertViewModel.CompNm;
            scExpertMapping.ScUsr.ScCompInfo.OwnNm = joinExpertViewModel.ComOwnNm;
            scExpertMapping.ScUsr.ScCompInfo.RegistrationNo = joinExpertViewModel.ComRegistrationNo;
            scExpertMapping.ScUsr.ScCompInfo.TelNo = joinExpertViewModel.ComTelNo1 + "-" + joinExpertViewModel.ComTelNo2 + "-" + joinExpertViewModel.ComTelNo3;
            scExpertMapping.ScUsr.ScCompInfo.PostNo = joinExpertViewModel.ComPostNo;
            scExpertMapping.ScUsr.ScCompInfo.Addr1 = joinExpertViewModel.ComAddr1;
            scExpertMapping.ScUsr.ScCompInfo.Addr2 = joinExpertViewModel.ComAddr2;

            //업종 , 종목
            int compSn = int.Parse(Session[Global.CompSN].ToString());
            if (joinExpertViewModel.BizTypes.Count > 0)
            {
                _scBizTypeService.DeleteScBizTypeByCompSn(compSn);

                foreach (var item in joinExpertViewModel.BizTypes)
                {
                    var scBizType = new ScBizType
                    {
                        CompSn = compSn,
                        BizTypeCd = item.BizTypeCd,
                        BizTypeNm = item.BizTypeNm
                    };

                    _scBizTypeService.AddScBizType(scBizType);
                }
            }

            //파일정보 업데이트
            if (!string.IsNullOrEmpty(DeleteFileSn))
            {
                scExpertMapping.ScUsr.ScUsrResume.ScFileInfo.Status = "D";
            }

            //신규파일정보저장 및 파일업로드
            foreach (var file in files)
            {
                if (file != null)
                {
                    var fileHelper = new FileHelper();

                    var savedFileName = fileHelper.GetUploadFileName(file);

                    var subDirectoryPath = Path.Combine(FileType.Resume.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                    var savedFilePath = Path.Combine(subDirectoryPath, savedFileName);

                    var scFileInfo = new ScFileInfo { FileNm = Path.GetFileName(file.FileName), FilePath = savedFilePath, Status = "N", RegId = Session[Global.LoginID].ToString(), RegDt = DateTime.Now };
                    var scUsrResume = new ScUsrResume { ScFileInfo = scFileInfo };
                    scExpertMapping.ScUsr.ScUsrResume = scUsrResume;

                    await fileHelper.UploadFile(file, subDirectoryPath, savedFileName);
                }
            }

            int result = await _scExpertMappingService.SaveDbContextAsync();

            if (result != -1)
                return RedirectToAction("MyInfo", "Main");
            else
            {
                ModelState.AddModelError("", "내정보 수정 실패.");

                var usrView =
                   Mapper.Map<JoinExpertViewModel>(scExpertMapping);

                return View(usrView);
            }
        }

        public void DownloadResumeFile()
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
    }
}