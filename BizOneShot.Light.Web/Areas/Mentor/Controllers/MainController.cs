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

            ScUsr scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());

            var myInfo =
               Mapper.Map<MentorMyInfoViewModel>(scUsr);

            //myInfo.BizWorkNm = scUsr.ScMentorMappiings.FirstOrDefault().ScBizWork.BizWorkNm;

            return View(myInfo);
        }
    }
}