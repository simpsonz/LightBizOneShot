using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;
using System.Threading.Tasks;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Helper;
using PagedList;
using AutoMapper;

namespace BizOneShot.Light.Web.Areas.Mentor.Controllers
{

    [UserAuthorize(Order = 1)]
    public class MainController : BaseController
    {
        private readonly IScNtcService _scNtcService;

        private readonly IScUsrService _scUsrService;

        public MainController(IScNtcService scNtcServcie, IScUsrService scUsrService)
        {
            this._scNtcService = scNtcServcie;
            this._scUsrService = scUsrService;

        }
        // GET: Company/Main
        [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
        public async Task<ActionResult> Index()
        {
            //var listScNtc = _scNtcService.GetNotices();
            var listScNtc = await _scNtcService.GetNoticesForMainAsync();

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(listScNtc);
            return View(noticeViews);
        }

        [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
        public async Task<ActionResult> MyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;

            ScUsr scUsr = await _scUsrService.SelectMentorInfo(Session[Global.LoginID].ToString());

            var myInfo =
               Mapper.Map<MentorMyInfoViewModel>(scUsr);

            return View(myInfo);
        }


        [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
        public ActionResult ModifyMyInfo(MentorMyInfoViewModel myInfo)
        {
            ViewBag.LeftMenu = Global.MyInfo;

            //ScUsr scUsr = await _scUsrService.SelectMentorInfo(Session[Global.LoginID].ToString());

            //var myInfo =
            //   Mapper.Map<MentorMyInfoViewModel>(scUsr);

            return View(myInfo);
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