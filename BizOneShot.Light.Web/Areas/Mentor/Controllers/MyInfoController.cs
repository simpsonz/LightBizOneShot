using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Security;


namespace BizOneShot.Light.Web.Areas.Mentor.Controllers
{
    [UserAuthorize(Order = 1)]
    public class MyInfoController : BaseController
    {
        private readonly IScUsrService _scUsrService;
    

        public MyInfoController(IScUsrService scUsrService)
        {
            this._scUsrService = scUsrService;
        }

        // GET: Company/MyInfo
        public ActionResult Index()
        {
            return View();
        }

        // GET: Company/MyInfo
        [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
        public async Task<ActionResult> MyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;

            ScUsr scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());

            var myInfo =
               Mapper.Map<MentorMyInfoViewModel>(scUsr);

            myInfo.BizWorkNm = scUsr.ScMentorMappiings.FirstOrDefault().ScBizWork.BizWorkNm;

            return View(myInfo);
        }

        [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
        public ActionResult ChangePassword()
        {
            ViewBag.LeftMenu = Global.MyInfo;
            return View();
        }

        [HttpPost]
        [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewBag.LeftMenu = Global.MyInfo;

            if(Session[Global.LoginID].ToString() != model.ID)
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
                    return RedirectToAction("MyInfo", "MyInfo", new { area = "Company" });
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

        
    }
}