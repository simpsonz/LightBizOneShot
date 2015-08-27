using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Security;
using BizOneShot.Light.Web.ComLib;
using Microsoft.AspNet.Identity.Owin;

namespace BizOneShot.Light.Web.Controllers
{

    public class HomeController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private readonly IScUsrService _scUsrService;

        public HomeController(IScUsrService scUsrService)
        {
            this._scUsrService = scUsrService;
        }

        public ActionResult Index()
        {
            ViewBag.LogoCSS = "headerwrap_main";
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.LogoCSS = "headerwrap_main";
            return View();
        }

        //
        // GET: /Account/Login
        //[AllowAnonymous]
        //public ActionResult index(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View();
        //}

        //
        // POST: /Account/Login


        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> index(LoginViewModel model, string returnUrl)
        //{
        //    ViewBag.LogoCSS = "headerwrap_main";

        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // 계정이 잠기는 로그인 실패로 간주되지 않습니다.
        //    // 암호 오류 시 계정 잠금을 트리거하도록 설정하려면 shouldLockout: true로 변경하십시오.
        //    var result = await SignInManager.PasswordSignInAsync(model.ID, model.Password, model.RememberMe, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "잘못된 로그인 시도입니다.");
        //            return View(model);
        //    }
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Index(LoginViewModel loginViewModel)
        {
            ScUsr scUsr = await _scUsrService.SelectScUsr(loginViewModel.ID);
            if (scUsr != null)
            {
                //패스워드비교
                SHACryptography sha2 = new SHACryptography();
                if (scUsr.LoginPw == sha2.EncryptString(loginViewModel.Password))
                //if (user.LOGIN_PW == param.LOGIN_PW)
                {
                    base.LogOn(scUsr);
                    switch (scUsr.UsrType)
                    {
                        case Global.Company : //기업
                            return RedirectToAction("index", "Commpany/Main");
                        case Global.Mentor: //멘토
                            return RedirectToAction("index", "Mentor/Main");
                        case Global.Expert: //전문가
                            return RedirectToAction("index", "Expert/Main");
                        case Global.SysManager: //SCP
                            return RedirectToAction("index", "SysManager/Main");
                        case Global.BizManager: //사업관리자
                            return RedirectToAction("index", "BizManager/Main");
                        default:
                            return View(loginViewModel);
                    }

                }
                else
                {
                    return View(loginViewModel);
                }
            }
            else
            {
                return View(loginViewModel);
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.LogoCSS = "headerwrap_main";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 계정이 잠기는 로그인 실패로 간주되지 않습니다.
            // 암호 오류 시 계정 잠금을 트리거하도록 설정하려면 shouldLockout: true로 변경하십시오.
            var result = await SignInManager.PasswordSignInAsync(model.ID, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "잘못된 로그인 시도입니다.");
                    return View(model);
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}