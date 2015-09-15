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

namespace BizOneShot.Light.Web.Areas.Expert.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Expert, Order = 2)]
    public class MainController : Controller
    {
        private readonly IScExpertMappingService _scExpertMappingService;
        private readonly IScUsrService _scUsrService;

        public MainController(IScExpertMappingService _scExpertMappingService, IScUsrService _scUsrService)
        {
            this._scExpertMappingService = _scExpertMappingService;
            this._scUsrService = _scUsrService;
        }
        // GET: Expert/Main
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> MyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;
            var scExpertMapping = await _scExpertMappingService.GetExpertAsync(Session[Global.LoginID].ToString());

            var usrView =
               Mapper.Map<JoinExpertViewModel>(scExpertMapping);

            return View(usrView);
        }


        public async Task<ActionResult> ModifyMyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;
            var scExpertMapping = await _scExpertMappingService.GetExpertAsync(Session[Global.LoginID].ToString());

            var usrView =
               Mapper.Map<JoinExpertViewModel>(scExpertMapping);

            return View(usrView);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyMyInfo(JoinExpertViewModel joinExpertViewModel)
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
            //업태업종 코드 추가해야함.
            scExpertMapping.ScUsr.ScCompInfo.TelNo = joinExpertViewModel.ComTelNo1 + "-" + joinExpertViewModel.ComTelNo2 + "-" + joinExpertViewModel.ComTelNo3;
            scExpertMapping.ScUsr.ScCompInfo.PostNo = joinExpertViewModel.ComPostNo;
            scExpertMapping.ScUsr.ScCompInfo.Addr1 = joinExpertViewModel.ComAddr1;
            scExpertMapping.ScUsr.ScCompInfo.Addr2 = joinExpertViewModel.ComAddr2;
            scExpertMapping.ScUsr.ScUsrResume.ScFileInfo.FileNm = joinExpertViewModel.ResumeName;
            scExpertMapping.ScUsr.ScUsrResume.ScFileInfo.FilePath = joinExpertViewModel.ResumePath;

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
    }
}