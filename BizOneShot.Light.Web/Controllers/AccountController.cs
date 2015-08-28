using System.Web.Mvc;
using System;
using System.Collections.Generic;
using BizOneShot.Light.Services;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.DareModels;
using BizOneShot.Light.Util.Security;
using AutoMapper;
using System.Threading.Tasks;

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
        public async Task<ActionResult> CompanyJoin(JoinCompanyViewModel joinCompanyViewModel)
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

                SHACryptography sha2 = new SHACryptography();
                scUsr.LoginPw = sha2.EncryptString(scUsr.LoginPw);

                //회사정보 추가 정보 설정
                scCompInfo.Status = "N";
                scCompInfo.RegId = scUsr.LoginId;
                scCompInfo.RegDt = DateTime.Now;

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
                syUser.Pwd = scUsr.LoginPw;

                //저장
                IList<ScUsr> scUsrs = new List<ScUsr>();
                scUsrs.Add(scUsr);
                scCompInfo.ScUsrs = scUsrs;

                //bool result = _scUsrService.AddCompanyUser(scCompInfo, scUsr, syUser);
                int result = await _scUsrService.AddCompanyUserAsync(scCompInfo, scUsr, syUser);

                if (result != -1)
                    return RedirectToAction("CompanyJoinComplete", "Account");
                else
                    return View(joinCompanyViewModel);

                //if (result)
                //    return RedirectToAction("CompanyJoinComplete", "Account");
                //else
                //    return View(joinCompanyViewModel);
            }
            // 이 경우 오류가 발생한 것이므로 폼을 다시 표시하십시오.
            return View(joinCompanyViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
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

        [AllowAnonymous]
        public ActionResult CompanyJoinComplete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> DoLogin(LoginViewModel loginViewModel)
        {
            ScUsr scUsr = await _scUsrService.SelectScUsr(loginViewModel.ID);
            if (scUsr != null)
            {
                //패스워드비교
                SHACryptography sha2 = new SHACryptography();
                if (scUsr.LoginPw == sha2.EncryptString(loginViewModel.Password))
                //if (user.LOGIN_PW == param.LOGIN_PW)
                {
                    //base.LogOn(scUsr);
                    //string usrType = user.USR_TYPE;
                    switch(scUsr.UsrType)
                    {
                        case "C": //기업
                            return RedirectToAction("index", "Commpany/Main");
                        case "M": //멘토
                            return RedirectToAction("index", "Mentor/Main");
                        case "P": //전문가
                            return RedirectToAction("index", "Expert/Main");
                        case "S": //SCP
                            return RedirectToAction("index", "SysManager/Main");
                        case "B": //사업관리자
                            return RedirectToAction("index", "BizManager/Main");
                        default:
                            return RedirectToAction("index", "Home");
                    }
                    
                }
                else
                {
                    return RedirectToAction("index", "Home");
                }
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }
    }
}