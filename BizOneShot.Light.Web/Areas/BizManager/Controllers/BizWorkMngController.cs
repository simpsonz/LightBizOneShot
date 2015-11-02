using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Security;
using BizOneShot.Light.Web.ComLib;
using PagedList;

namespace BizOneShot.Light.Web.Areas.BizManager.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.BizManager, Order = 2)]
    public class BizWorkMngController : BaseController
    {

        private readonly IScUsrService _scUsrService;
        private readonly IScBizWorkService _scBizWorkService;

        public BizWorkMngController(IScUsrService scUsrService, IScBizWorkService _scBizWorkService)
        {
            this._scUsrService = scUsrService;
            this._scBizWorkService = _scBizWorkService;
        }
        // GET: BizManager/BizWorkMng
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> BizWorkList()
        {
            ViewBag.LeftMenu = Global.BizWorkMng;

            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(Session[Global.CompSN].ToString()));

            var bizWorkViews =
                Mapper.Map<List<BizWorkViewModel>>(listScBizWork);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<BizWorkViewModel>(bizWorkViews.ToPagedList(1, pagingSize), 1, pagingSize, bizWorkViews.Count));
        }

        [HttpPost]
        public async Task<ActionResult> BizWorkList(string Query, string curPage)
        {
            ViewBag.LeftMenu = Global.BizWorkMng;

            //var listScNtc = _scNtcService.GetNotices(SelectList, Query);
            var listScBizWork = await _scBizWorkService.GetBizWorkListByBizWorkNm(int.Parse(Session[Global.CompSN].ToString()), Query);

            var bizWorkViews =
                Mapper.Map<List<BizWorkViewModel>>(listScBizWork);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<BizWorkViewModel>(bizWorkViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, bizWorkViews.Count));
        }

        //public async Task<ActionResult> BizWorkComList(string bizWorkSn, string bizWorkNm)
        //{
        //    ViewBag.LeftMenu = Global.BizWorkMng;
        //    ViewBag.BizWorkNm = bizWorkNm;

        //    var comListScBizWork = await _scBizWorkService.GetBizWorkComList(int.Parse(bizWorkSn));

        //    var comListViews =
        //        Mapper.Map<List<JoinCompanyViewModel>>(comListScBizWork);

        //    int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
        //    pagingSize = 2;
        //    return View(new StaticPagedList<JoinCompanyViewModel>(comListViews.ToPagedList(1, pagingSize), 1, pagingSize, comListViews.Count));
        //}

        public async Task<ActionResult> BizWorkComList(string bizWorkSn, string bizWorkNm, string curPage)
        {
            ViewBag.LeftMenu = Global.BizWorkMng;
            ViewBag.BizWorkNm = bizWorkNm;

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pagedListScBizWork = await _scBizWorkService.GetPagedListBizWorkComList(int.Parse(bizWorkSn), int.Parse(curPage ?? "1"), pagingSize);

            var comListViews =
                Mapper.Map<List<JoinCompanyViewModel>>(pagedListScBizWork);

            ViewBag.BizWorkSN = bizWorkSn;

            return View(new StaticPagedList<JoinCompanyViewModel>(comListViews, pagedListScBizWork.PageNumber, pagingSize, pagedListScBizWork.TotalItemCount));
        }


        public async Task<ActionResult> BizWorkDetail(string bizWorkSn)
        {
            ViewBag.LeftMenu = Global.BizWorkMng;

            //var listScNtc = _scNtcService.GetNotices(SelectList, Query);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(int.Parse(bizWorkSn));

            var bizWorkViews =
               Mapper.Map<BizWorkViewModel>(scBizWork);

            return View(bizWorkViews);
        }

        public async Task<ActionResult> ModifyBizWork(string bizWorkSn)
        {
            ViewBag.LeftMenu = Global.BizWorkMng;

            //var listScNtc = _scNtcService.GetNotices(SelectList, Query);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(int.Parse(bizWorkSn));

            var bizWorkViews =
               Mapper.Map<BizWorkViewModel>(scBizWork);

            return View(bizWorkViews);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyBizWork(BizWorkViewModel bizWorkViewModel)
        {
            ViewBag.LeftMenu = Global.BizWorkMng;

            //var listScNtc = _scNtcService.GetNotices(SelectList, Query);
            var scBizWork = await _scBizWorkService.GetBizWorkByBizWorkSn(bizWorkViewModel.BizWorkSn);

            scBizWork.BizWorkSummary = bizWorkViewModel.BizWorkSummary;
            scBizWork.BizWorkStDt = DateTime.Parse(bizWorkViewModel.BizWorkStDt);
            scBizWork.BizWorkEdDt = DateTime.Parse(bizWorkViewModel.BizWorkEdDt);
            scBizWork.MngDept = bizWorkViewModel.MngDept;

            int result = await _scBizWorkService.SaveDbContextAsync();

            if (result != -1)
                return RedirectToAction("BizWorkDetail", "BizWorkMng", new { bizWorkSn = scBizWork.BizWorkSn });
            else
            {
                ModelState.AddModelError("", "사업 수정 실패.");
                return View(bizWorkViewModel);
            }
        }

        public ActionResult RegBizWork()
        {
            ViewBag.LeftMenu = Global.BizWorkMng;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegBizWork(BizWorkViewModel bizWorkViewModel)
        {
            ViewBag.LeftMenu = Global.BizWorkMng;

            if (ModelState.IsValid)
            {
                var scUsr = Mapper.Map<ScUsr>(bizWorkViewModel);
                var scBizWork = Mapper.Map<ScBizWork>(bizWorkViewModel);

                //회원정보 추가 정보 설정
                scUsr.RegId = Session[Global.LoginID].ToString();
                scUsr.RegDt = DateTime.Now;
                scUsr.Status = "N";
                scUsr.UsrType = "B";
                scUsr.UsrTypeDetail = "M";
                scUsr.CompSn = int.Parse(Session[Global.CompSN].ToString());

                SHACryptography sha2 = new SHACryptography();
                scUsr.LoginPw = sha2.EncryptString(scUsr.LoginPw);

                //사업정보 추가 정보 설정
                scBizWork.Status = "N";
                scBizWork.MngCompSn = int.Parse(Session[Global.CompSN].ToString());
                scBizWork.RegId = Session[Global.LoginID].ToString();
                scBizWork.RegDt = DateTime.Now;

                //저장
                IList<ScBizWork> scBizWorks = new List<ScBizWork>();
                scUsr.ScBizWorks.Add(scBizWork);

                int result = await _scUsrService.AddBizManagerMemberAsync(scUsr);

                if (result != -1)
                    return RedirectToAction("BizWorkList", "BizWorkMng");
                else
                {
                    ModelState.AddModelError("", "사업 등록 실패.");
                    return View(bizWorkViewModel);
                }
            }
            ModelState.AddModelError("", "입력값 검증 실패.");
            return View(bizWorkViewModel);
        }

        [HttpPost]
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


    }
}