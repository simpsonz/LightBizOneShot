using System.Web.Mvc;
using System;
using System.Collections.Generic;
using BizOneShot.Light.Services;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.DareModels;
using AutoMapper;

namespace BizOneShot.Light.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IScUsrService _scUsrService;

        public AccountController(IScUsrService scUsrService)
        {
            this._scUsrService = scUsrService;
        }

        [AllowAnonymous]
        public ActionResult CompanyAgreement()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult CompanyJoin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyJoin(JoinCompanyViewModel joinCompanyViewModel)
        {
            if (ModelState.IsValid)
            {
                var scUsr = Mapper.Map<ScUsr>(joinCompanyViewModel);
                var syUser = Mapper.Map<SHUSER_SyUser>(joinCompanyViewModel);
                var scCompInfo = Mapper.Map<ScCompInfo>(joinCompanyViewModel);

                //회원정보 추가 정보 설정
                scUsr.RegId = scUsr.LoginId;
                scUsr.RegDt = DateTime.Now;
                scUsr.Status = "N";
                scUsr.UsrType = "C";
                scUsr.UsrTypeDetail = "A";

                //회사정보 추가 정보 설정
                scCompInfo.Status = "N";
                scCompInfo.RegId = scUsr.LoginId;
                scUsr.RegDt = DateTime.Now;

                //개인, 법인사업자 구분 설정
                int bizCode = Convert.ToInt32(scCompInfo.RegistrationNo.Substring(3, 2));
                string bizType = string.Empty; //법인 : L, 개인 : C

                if ((bizCode >= 1 && bizCode <= 79) || (bizCode >= 90 && bizCode <= 99) || bizCode == 89 || bizCode == 80)
                {
                    scCompInfo.CompType = "I"; //개인
                }
                else 
                {
                    scCompInfo.CompType = "C"; //법인
                }

                //다래 추가정보 설정
                syUser.UsrGbn = "1";
                syUser.UserStatus = "1";

                //저장
                IList<ScUsr> scUsrs = new List<ScUsr>();
                scUsrs.Add(scUsr);
                scCompInfo.ScUsrs = scUsrs;

                bool result = _scUsrService.AddCompanyUser(scCompInfo, scUsr, syUser);


                return View(joinCompanyViewModel);
                //return RedirectToAction("CompanyJoinComplete", "Account");
            }
                // 이 경우 오류가 발생한 것이므로 폼을 다시 표시하십시오.
            return View(joinCompanyViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DoLoginIdSelect(string LoginId)
        {
            bool result = _scUsrService.ChkLoginId(LoginId);

            if (result == true)
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