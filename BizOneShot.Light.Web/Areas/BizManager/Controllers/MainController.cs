using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Areas.BizManager.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.BizManager, Order = 2)]
    public class MainController : BaseController
    {
        private readonly IScBizWorkService _scBizWorkService;
        private readonly IScUsrService _scUsrService;

        public MainController(IScBizWorkService _scBizWorkService, IScUsrService _scUsrService)
        {
            this._scBizWorkService = _scBizWorkService;
            this._scUsrService = _scUsrService;
        }
        // GET: BizManager/Main
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> MyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;
            var scBizWork = await _scBizWorkService.GetBizWorkByloginId(Session[Global.LoginID].ToString());

            var usrView =
               Mapper.Map<BizWorkViewModel>(scBizWork);

            return View(usrView);
        }

        public async Task<ActionResult> ModifyMyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;
            var scBizWork = await _scBizWorkService.GetBizWorkByloginId(Session[Global.LoginID].ToString());

            var usrView =
               Mapper.Map<BizWorkViewModel>(scBizWork);

            return View(usrView);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyMyInfo(BizWorkViewModel bizWorkViewModel)
        {
            ViewBag.LeftMenu = Global.MyInfo;
            var scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());

            scUsr.Name = bizWorkViewModel.Name;
            scUsr.DeptNm = bizWorkViewModel.DeptNm;
            scUsr.TelNo = bizWorkViewModel.TelNo1 + "-" + bizWorkViewModel.TelNo2 + "-" + bizWorkViewModel.TelNo3;
            scUsr.MbNo = bizWorkViewModel.MbNo1 + "-" + bizWorkViewModel.MbNo2 + "-" + bizWorkViewModel.MbNo3;
            scUsr.FaxNo = bizWorkViewModel.FaxNo1 + "-" + bizWorkViewModel.FaxNo2 + "-" + bizWorkViewModel.FaxNo3;
            scUsr.Email = bizWorkViewModel.Email1 + "@" + bizWorkViewModel.Email2;

            int result = await _scUsrService.SaveDbContextAsync();

            if (result != -1)
                return RedirectToAction("MyInfo", "Main");
            else
            {
                ModelState.AddModelError("", "내정보 수정 실패.");
                var scBizWork = await _scBizWorkService.GetBizWorkByloginId(Session[Global.LoginID].ToString());

                var usrView =
                   Mapper.Map<BizWorkViewModel>(scBizWork);

                return View(usrView);
            }

        }
    }
}