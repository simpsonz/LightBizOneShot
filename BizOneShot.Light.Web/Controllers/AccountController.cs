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
using BizOneShot.Light.Web.ComLib;


namespace BizOneShot.Light.Web.Controllers
{
    [UserAuthorize(Order = 1)]
    public class AccountController : BaseController
    {
        private readonly IScUsrService _scUsrService;
        private readonly IScCompInfoService _scCompInfoService;
        private readonly IScBizWorkService _scBizWorkService;
        private readonly IScBizTypeService _scBizTypeService;

        public AccountController(IScUsrService scUsrService, IScCompInfoService scCompInfoService, IScBizWorkService scBizWorkService, IScBizTypeService scBizTypeService)
        {
            _scUsrService = scUsrService;
            _scCompInfoService = scCompInfoService;
            _scBizWorkService = scBizWorkService;
            _scBizTypeService = scBizTypeService;
        }

        [AllowAnonymous]
        public ActionResult CompanyAgreement()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> CompanyJoin()
        {
            //사업관리자 DropDown List Data
            var bizMngList = await _scUsrService.GetBizManagerAsync();

            var bizMngDropDown =
                Mapper.Map<List<BizMngDropDownModel>>(bizMngList);

            BizMngDropDownModel title = new BizMngDropDownModel();
            title.CompSn = 0;
            title.CompNm = "사업관리자 선택";
            bizMngDropDown.Insert(0, title);

            SelectList bizManagerList = new SelectList(bizMngDropDown, "CompSn", "CompNm");

            ViewBag.SelectBizMngList = bizManagerList;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompanyJoin(JoinCompanyViewModel joinCompanyViewModel, string BizList)
        {
            if (ModelState.IsValid)
            {
                if(string.IsNullOrEmpty(BizList))
                {
                    ModelState.AddModelError("", "사업명을 선택하지 않았습니다.");
                    return View(joinCompanyViewModel);
                }
                var scUsr = Mapper.Map<ScUsr>(joinCompanyViewModel);
                var syUser = Mapper.Map<SHUSER_SyUser>(joinCompanyViewModel);
                var scCompInfo = Mapper.Map<ScCompInfo>(joinCompanyViewModel);

                //회원정보 추가 정보 설정
                scUsr.RegId = scUsr.LoginId;
                scUsr.RegDt = DateTime.Now;
                scUsr.Status = "N";
                scUsr.UsrType = "C";
                scUsr.UsrTypeDetail = "A";
                scUsr.AgreeYn = "Y";

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

                //사업관리 및 사업등록 요청 설절
                ScCompMapping scm = new ScCompMapping();
                scm.BizWorkSn = int.Parse(BizList);
                scm.Status = "R";
                scm.RegId = scUsr.LoginId;
                scm.RegDt = DateTime.Now;



                //다래 추가정보 설정
                // 기업회원 : 1, 세무회계사 : 2
                syUser.UsrGbn = "1";
                //사용가능 : 1, 사용불가 : 0
                syUser.UserStatus = "1";
                // 무료사용자 : 3, 유료사용자 : 1
                syUser.WebUsrGbn = "3";
                syUser.SmartPwd = scUsr.LoginPw;

                //저장
                scCompInfo.ScUsrs.Add(scUsr);
                scCompInfo.ScCompMappings.Add(scm);

                //bool result = _scUsrService.AddCompanyUser(scCompInfo, scUsr, syUser);
                int result = await _scUsrService.AddCompanyUserAsync(scCompInfo, scUsr, syUser);

                //업종,종목처리
                int compSn = int.Parse(Session[Global.CompSN].ToString());
                if (joinCompanyViewModel.BizTypes.Count > 0)
                {
                    //._scBizTypeService.DeleteScBizTypeByCompSn(compSn);

                    foreach (var item in joinCompanyViewModel.BizTypes)
                    {
                        var scBizType = new ScBizType
                        {
                            CompSn = compSn,
                            BizTypeCd = item.BizTypeCd,
                            BizTypeNm = item.BizTypeNm
                        };

                        _scBizTypeService.AddScBizType(scBizType);
                    }

                    await _scBizTypeService.SaveDbContextAsync();
                }

              

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
                            return RedirectToAction("index", "Commany/Main");
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

        [MenuAuthorize(Roles = UserType.Company, Order = 2)]
        public async Task<ActionResult> CompanyMyInfo()
        {

            return View();
        }


        public ActionResult ChangePassword()
        {
            ViewBag.LeftMenu = Global.MyInfo;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewBag.LeftMenu = Global.MyInfo;

            if (Session[Global.LoginID].ToString() != model.ID)
            {
                ModelState.AddModelError("", "로그인된 아이디가 아닙니다.");
                return View();
            }

            ScUsr scUsr = await _scUsrService.SelectScUsr(model.ID);
            if (scUsr != null)
            {
                //패스워드비교
                SHACryptography sha2 = new SHACryptography();
                if (scUsr.LoginPw == sha2.EncryptString(model.LoginPw))
                {
                    scUsr.LoginPw = sha2.EncryptString(model.NewLoginPw);
                    await _scUsrService.SaveDbContextAsync();

                    string usrArea;

                    if (Session[Global.UserType].ToString().Equals(Global.Company))
                    {// 기업회원
                        usrArea = "Company";
                        return RedirectToAction("MyInfo", "MyInfo", new { area = usrArea });

                    }
                    else if (Session[Global.UserType].ToString().Equals(Global.Mentor))
                    {// 멘토
                        usrArea = "Mentor";
                    }
                    else if (Session[Global.UserType].ToString().Equals(Global.SysManager))
                    {//SCP 관리자
                        usrArea = "SysManager";
                    }
                    else if (Session[Global.UserType].ToString().Equals(Global.BizManager))
                    {//사업 관리자
                        usrArea = "BizManager";
                    }
                    else if (Session[Global.UserType].ToString().Equals(Global.Expert))
                    {//사업 관리자
                        usrArea = "Expert";
                    }
                    else
                    {
                        usrArea = "";
                    }
                    return RedirectToAction("MyInfo", "Main", new { area = usrArea });
                }
                else
                {
                    ModelState.AddModelError("", "비밀번호가 일치하지 않습니다.");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "아이디가 존재하지 않습니다.");
                return View();
            }
        }

        /// <summary>
        /// [기능] : 회원가입 - 사업자번호 중복 확인 AJAX
        /// [작성] : 2014-10-26 유연화
        /// [수정] :  
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> DoCompanyUsrNoSelect(string USR_NO)
        {
            var scCompInfos = await _scCompInfoService.GetScCompInfoByRegistationNo(USR_NO);

            if (scCompInfos.Count == 0)
            {
                return Json(new { result = true });
            }
            else
            {
                return Json(new { result = false });
            }

        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GetBizList(string CompSn)
        {
            var bizWork = await _scBizWorkService.GetBizWorkList(int.Parse(CompSn));

            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(bizWork);

            return Json(bizWorkDropDown);
        }
    }
}