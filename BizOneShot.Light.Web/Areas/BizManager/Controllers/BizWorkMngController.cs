using System;
using System.Collections.Generic;
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

        public ActionResult BizWorkList()
        {
            ViewBag.LeftMenu = Global.BizWorkMng;
            return View();
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
                scBizWork.CompSn = int.Parse(Session[Global.CompSN].ToString());
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