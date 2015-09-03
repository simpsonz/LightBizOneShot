using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using System.Threading.Tasks;
using AutoMapper;
using System.Configuration;
using PagedList;
using BizOneShot.Light.Util.Security;

namespace BizOneShot.Light.Web.Areas.SysManager.Controllers
{
    [UserAuthorize(Order = 1)]
    public class BizManagerController : BaseController
    {
        private readonly IScUsrService _scUsrService;
        private readonly IScBizWorkService _scBizWorkService;

        public BizManagerController(IScUsrService scUsrService, IScBizWorkService _scBizWorkService)
        {
            this._scUsrService = scUsrService;
            this._scBizWorkService = _scBizWorkService;
        }

        // GET: SysManager/BizManager
        public ActionResult Index()
        {
            return View();
        }

        [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
        public async Task<ActionResult> BizManager()
        {
            ViewBag.LeftMenu = Global.BizMng;

            //var listScNtc = _scNtcService.GetNotices(SelectList, Query);
            var listScUsr = await _scUsrService.GetBizManagerAsync();

            var usrViews =
                Mapper.Map<List<CompanyMyInfoViewModel>>(listScUsr);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<CompanyMyInfoViewModel>(usrViews.ToPagedList(1, pagingSize), 1, pagingSize, usrViews.Count));
        }

        [HttpPost]
        [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
        public async Task<ActionResult> BizManager(string Query, string curPage)
        {
            ViewBag.LeftMenu = Global.BizMng;

            //var listScNtc = _scNtcService.GetNotices(SelectList, Query);
            var listScUsr = await _scUsrService.GetBizManagerByComNameAsync(Query);

            var usrViews =
                Mapper.Map<List<CompanyMyInfoViewModel>>(listScUsr);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<CompanyMyInfoViewModel>(usrViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, usrViews.Count));
        }


        [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
        public ActionResult RegBizManager()
        {
            ViewBag.LeftMenu = Global.BizMng;

            return View();
        }
        [HttpPost]
        [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegBizManager(JoinCompanyViewModel joinCompanyViewModel)
        {
            ViewBag.LeftMenu = Global.BizMng;

            var scUsr = Mapper.Map<ScUsr>(joinCompanyViewModel);
            var scCompInfo = Mapper.Map<ScCompInfo>(joinCompanyViewModel);

            //회원정보 추가 정보 설정
            scUsr.RegId = Session[Global.LoginID].ToString();
            scUsr.RegDt = DateTime.Now;
            scUsr.Status = "N";
            scUsr.UsrType = "B";
            scUsr.UsrTypeDetail = "A";

            SHACryptography sha2 = new SHACryptography();
            scUsr.LoginPw = sha2.EncryptString(scUsr.LoginPw);

            //회사정보 추가 정보 설정
            scCompInfo.Status = "N";
            scCompInfo.RegId = Session[Global.LoginID].ToString();
            scCompInfo.RegDt = DateTime.Now;

            //저장
            IList<ScUsr> scUsrs = new List<ScUsr>();
            scUsrs.Add(scUsr);
            scCompInfo.ScUsrs = scUsrs;

            //bool result = _scUsrService.AddCompanyUser(scCompInfo, scUsr, syUser);
            int result = await _scUsrService.AddBizManagerAsync(scCompInfo);

            if (result != -1)
                return RedirectToAction("BizManager", "BizManager");
            else
                return View(joinCompanyViewModel);

        }

        [HttpPost]
        [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
        public async Task<JsonResult> DoLoginIdSelect(string LoginId)
        {
            bool result = await _scUsrService.ChkLoginId(LoginId);

            if (result.Equals(true))
            {
                return Json(new { result = true });
            }
            else
            {
                return Json(new { result = false });
            }

        }

        [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
        public async Task<ActionResult> BizManagerDetail(string loginId)
        {
            ViewBag.LeftMenu = Global.BizMng;
            //var dicScNtc = _scNtcService.GetNoticeDetailById(noticeSn);
            var scUsr = await _scUsrService.SelectScUsr(loginId);

            var joinCompanyView =
                Mapper.Map<JoinCompanyViewModel>(scUsr);

            return View(joinCompanyView);
        }

        [HttpPost]
        public JsonResult GetBizList(string comSn)
        {
            var bizWork = _scBizWorkService.GetBizWorkList(int.Parse(comSn));

            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(bizWork);

            return Json(bizWorkDropDown);
        }


    }
}