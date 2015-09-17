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
        private readonly IPostService _postService;

        public HomeController(IScUsrService scUsrService, IPostService _postService)
        {
            this._scUsrService = scUsrService;
            this._postService = _postService;
        }

        public ActionResult Index()
        {
            if(Session[Global.UserLogo] == null)
            { 
                base.SetLogo("headerwrap_main");
            }
            return View();
        }

        public ActionResult Login()
        {
            if (Session[Global.UserLogo] == null)
            {
                base.SetLogo("headerwrap_main");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Index(LoginViewModel loginViewModel)
        {
            if (Session[Global.UserLogo] == null)
            {
                base.SetLogo("headerwrap_main");
            }

            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

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
                        case Global.Company: //기업
                            return RedirectToAction("index", "Company/Main");
                        case Global.Mentor: //멘토
                            return RedirectToAction("index", "Mentor/Main");
                        case Global.Expert: //전문가
                            return RedirectToAction("index", "Expert/Main");
                        case Global.SysManager: //SCP
                            return RedirectToAction("index", "SysManager/Main");
                        case Global.BizManager: //사업관리자
                            return RedirectToAction("index", "BizManager/Main");
                        default:
                            ModelState.AddModelError("", "정의되지 않은 사용자 타입입니다.");
                            return View(loginViewModel);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "비밀번호가 일치하지 않습니다.");
                    return View(loginViewModel);
                }
            }
            else
            {
                ModelState.AddModelError("", "아이디가 존재하지 않습니다.");
                return View(loginViewModel);
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            if (Session[Global.UserLogo] == null)
            {
                base.SetLogo("headerwrap_main");
            }

            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

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
                        case Global.Company: //기업
                            return RedirectToAction("index", "Company/Main");
                        case Global.Mentor: //멘토
                            return RedirectToAction("index", "Mentor/Main");
                        case Global.Expert: //전문가
                            return RedirectToAction("index", "Expert/Main");
                        case Global.SysManager: //SCP
                            return RedirectToAction("index", "SysManager/Main");
                        case Global.BizManager: //사업관리자
                            return RedirectToAction("index", "BizManager/Main");
                        default:
                            ModelState.AddModelError("", "정의되지 않은 사용자 타입입니다.");
                            return View(loginViewModel);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "비밀번호가 일치하지 않습니다.");
                    return View(loginViewModel);
                }
            }
            else
            {
                ModelState.AddModelError("", "아이디가 존재하지 않습니다.");
                return View(loginViewModel);
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


        public ActionResult Logoff()
        {
            LogOff();
            return RedirectToAction("Login");
        }

        public  ActionResult WoonjooUniv()
        {
            base.SetLogo("headerwrap_woonjoouniv");
            return RedirectToAction("Login", "Home");
        }

        public ActionResult SmartBizOn()
        {
            base.SetLogo("headerwrap_main");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult zipSearchPopup()
        {
            var sidoList = _postService.GetSidosAsync();
            return View(sidoList);
        }

    }
}