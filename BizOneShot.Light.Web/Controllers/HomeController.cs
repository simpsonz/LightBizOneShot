using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
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
using Microsoft.AspNet.Identity.Owin;
using PagedList;

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

        public async Task<ActionResult> zipSearchPopup()
        {
            var sidoList = await _postService.GetSidoListAsync();

            var sidoViews =
              Mapper.Map<List<SelectAddressListViewModel>>(sidoList);
            return View(sidoViews);
        }

        [HttpPost]
        public async Task<JsonResult> getGunguList(string SIDO)
        {
            SqlParameter sidoParam = new SqlParameter("SIDO", SIDO);

            object[] parameters = new object[] { sidoParam };

            var gunguList = await _postService.GetGunguListAsync(parameters);

            var gunguViews =
              Mapper.Map<List<SelectGunguListViewModel>>(gunguList);

            return Json(gunguViews);
        }

        /// <summary>
        /// [기능] : 동(읍/면) + 지번 우편번호 검색 리스트 호출
        /// [작성] : 2014-11-26 김가은
        /// [수정] :  
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> getAddressByDongSearchList(SelectAddressByDongSearchParamModel param)
        {
            SqlParameter sidoParam = new SqlParameter("SIDO", param.SIDO);
            SqlParameter gunguParam = new SqlParameter("GUNGU", param.GUNGU);
            SqlParameter dongParam = new SqlParameter("DONG", param.DONG);
            SqlParameter mnNoParam = new SqlParameter("MN_NO", param.MN_NO);
            SqlParameter subNoParam = new SqlParameter("SUB_NO", param.SUB_NO);

            object[] parameters = new object[] { sidoParam, gunguParam, dongParam, mnNoParam, subNoParam };

            var resultList = await _postService.GetAddressByDongSearchListAsync(parameters);

            var resultViews =
              Mapper.Map<List<SelectAddressListViewModel>>(resultList);

            ViewBag.SIZE = resultViews.Count();

            return PartialView("getZipListPartial", resultViews);
        }

        /// <summary>
        /// [기능] : 도로명 주소 + 건물번호 우편번호 검색 리스트 호출
        /// [작성] : 2014-11-26 김가은
        /// [수정] :  
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> getAddressByStreetSearchList(SelectAddressByStreetSearchParamModel param)
        {
            SqlParameter sidoParam = new SqlParameter("SIDO", param.SIDO);
            SqlParameter gunguParam = new SqlParameter("GUNGU", param.GUNGU);
            SqlParameter rdNmParam = new SqlParameter("RD_NM", param.RD_NM);
            SqlParameter mnNoParam = new SqlParameter("MN_NO", param.MN_NO);
            SqlParameter subNoParam = new SqlParameter("SUB_NO", param.SUB_NO);

            object[] parameters = new object[] { sidoParam, gunguParam, rdNmParam, mnNoParam, subNoParam };

            var resultList = await _postService.GetAddressByStreetSearchListAsync(parameters);

            var resultViews =
              Mapper.Map<List<SelectAddressListViewModel>>(resultList);

            ViewBag.SIZE = resultViews.Count();

            return PartialView("getZipListPartial", resultViews);
        }

        /// <summary>
        /// [기능] : 건물명(아파트명) 우편번호 검색 리스트 호출
        /// [작성] : 2014-11-26 김가은
        /// [수정] :  
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> getAddressByBuildingSearchList(SelectAddressByBuildingSearchParamModel param)
        {
            SqlParameter sidoParam = new SqlParameter("SIDO", param.SIDO);
            SqlParameter gunguParam = new SqlParameter("GUNGU", param.GUNGU);
            SqlParameter bldNmParam = new SqlParameter("BLD_NM", param.BLD_NM);

            object[] parameters = new object[] { sidoParam, gunguParam, bldNmParam };

            var resultList = await _postService.GetAddressByBuildingSearchListAsync(parameters);

            var resultViews =
              Mapper.Map<List<SelectAddressListViewModel>>(resultList);

            ViewBag.SIZE = resultViews.Count();

            return PartialView("getZipListPartial", resultViews);

        }

    }
}