using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class MainController : BaseController
    {
        private readonly IScNtcService _scNtcService;
        private readonly IScUsrService _scUsrService;
        private readonly IScFileInfoService _scFileInfoService;

        public MainController(IScNtcService scNtcServcie, IScUsrService scUsrService, IScFileInfoService scFileInfoService)
        {
            this._scNtcService = scNtcServcie;
            this._scUsrService = scUsrService;
            this._scFileInfoService = scFileInfoService;

        }

        public async Task<ActionResult> Index()
        {
            string agreeYn = Session[Global.AgreeYn].ToString();

            //개인정보 수집 및 이용에 대한 동의가 안되어 있으면 리다이렉트 함
            if (agreeYn != "Y")
            {
                TempData["alert"] = "개인정보 수집 및 이용을 동의하셔야 사용이 가능합니다";

                return RedirectToAction("MentorAgreement", "Main");
            }


            //var listScNtc = _scNtcService.GetNotices();
            var listScNtc = await _scNtcService.GetNoticesForMainAsync();

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(listScNtc);
            return View(noticeViews);
        }

        public ActionResult MentorAgreement()
        {
            
            return View();
        }

        public async Task<ActionResult> AgreeMentorAgreement()
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

            ScUsr scUsr = await _scUsrService.SelectMentorInfo(Session[Global.LoginID].ToString());

            var myInfo =
               Mapper.Map<MentorMyInfoViewModel>(scUsr);

            switch (myInfo.UsrTypeDetail)
            {
                case "T":
                    myInfo.UsrTypeDetailName = "세무/회계사";
                    break;
                case "W":
                    myInfo.UsrTypeDetailName = "노무";
                    break;
                case "L":
                    myInfo.UsrTypeDetailName = "법무";
                    break;
                case "P":
                    myInfo.UsrTypeDetailName = "특허";
                    break;
                case "M":
                    myInfo.UsrTypeDetailName = "마케팅";
                    break;
                case "F":
                    myInfo.UsrTypeDetailName = "자금";
                    break;
                case "D":
                    myInfo.UsrTypeDetailName = "기술개발";
                    break;
                case "Z":
                    myInfo.UsrTypeDetailName = "기타";
                    break;
            }

            return View(myInfo);
        }

        public ActionResult ModifyMyInfo(MentorMyInfoViewModel myInfo)
        {
            ViewBag.LeftMenu = Global.MyInfo;

            return View(myInfo);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyMyInfo(MentorMyInfoViewModel mentorViewModel, ModifyMentorParamModel param, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.LeftMenu = Global.MyInfo;

            ScUsr scUsr = await _scUsrService.SelectMentorInfo(Session[Global.LoginID].ToString());

            if (Session[Global.LoginID].ToString() != param.LoginIdChk)
            {
                mentorViewModel =
                   Mapper.Map<MentorMyInfoViewModel>(scUsr);
                ModelState.AddModelError("", "로그인된 아이디가 아닙니다.");
                return View(mentorViewModel);
            }

            //실제패스워드와 입력패스워드 비교
            SHACryptography sha2 = new SHACryptography();
            if (param.LoginPwChk != sha2.EncryptString(mentorViewModel.LoginPw))
            {
                mentorViewModel =
                   Mapper.Map<MentorMyInfoViewModel>(scUsr);

                ModelState.AddModelError("", "비밀번호가 일치하지 않습니다.");
                return View(mentorViewModel);
            }

            scUsr.AccountNo = mentorViewModel.AccountNo;
            scUsr.Addr1 = mentorViewModel.Addr1;
            scUsr.Addr2 = mentorViewModel.Addr2;
            scUsr.BankNm = mentorViewModel.BankNm;
            scUsr.Email = mentorViewModel.Email1 + "@" + mentorViewModel.Email2;
            scUsr.FaxNo = mentorViewModel.FaxNo1 + "-" + mentorViewModel.FaxNo2 + "-" + mentorViewModel.FaxNo3;
            scUsr.MbNo = mentorViewModel.MbNo1 + "-" + mentorViewModel.MbNo2 + "-" + mentorViewModel.MbNo3;
            scUsr.Name = mentorViewModel.Name;
            scUsr.PostNo = mentorViewModel.PostNo;
            scUsr.TelNo = mentorViewModel.TelNo1 + "-" + mentorViewModel.TelNo2 + "-" + mentorViewModel.TelNo3;
            scUsr.UsrTypeDetail = mentorViewModel.UsrTypeDetail;

            //_scUsrService.ModifyScUsr(scUsr);

            //파일정보 업데이트
            if (!string.IsNullOrEmpty(param.DeleteFileSn))
            {
                scUsr.ScUsrResume.ScFileInfo.Status = "D";
            }

            //신규파일정보저장 및 파일업로드
            foreach (var file in files)
            {
                if(file !=null)
                {
                    var fileHelper = new FileHelper();

                    var savedFileName = fileHelper.GetUploadFileName(file);

                    var subDirectoryPath = Path.Combine(FileType.Resume.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                    var savedFilePath = Path.Combine(subDirectoryPath, savedFileName);

                    var scFileInfo = new ScFileInfo { FileNm = Path.GetFileName(file.FileName), FilePath = savedFilePath, Status = "N", RegId = param.LoginIdChk, RegDt = DateTime.Now };
                    var scUsrResume = new ScUsrResume { ScFileInfo = scFileInfo };
                    scUsr.ScUsrResume = scUsrResume;

                    //_scUsrService.ModifyMentorInfo(scUsr);

                    await fileHelper.UploadFile(file, subDirectoryPath, savedFileName);
                }
            }

            _scUsrService.ModifyScUsr(scUsr);

            //다성공시 커밋
            await _scUsrService.SaveDbContextAsync();

            return RedirectToAction("MyInfo", "Main");
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