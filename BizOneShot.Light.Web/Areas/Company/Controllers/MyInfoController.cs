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


namespace BizOneShot.Light.Web.Areas.Company.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Company, Order = 2)]
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
        public async Task<ActionResult> MyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;

            ScUsr scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());

            var myInfo =
               Mapper.Map<CompanyMyInfoViewModel>(scUsr);

            return View(myInfo);
        }


        public ActionResult ModifyMyInfo(CompanyMyInfoViewModel myInfo)
        {
            ViewBag.LeftMenu = Global.MyInfo;

            return View(myInfo);
        }


        [HttpPost]
        public async Task<ActionResult> ModifyMyInfo(CompanyMyInfoViewModel companyInfoViewModel, ModifyCompanyParamModel param)
        {
            ViewBag.LeftMenu = Global.MyInfo;

            ScUsr scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());

            if (Session[Global.LoginID].ToString() != param.LoginIdChk)
            {
                companyInfoViewModel =
                   Mapper.Map<CompanyMyInfoViewModel>(scUsr);
                ModelState.AddModelError("", "로그인된 아이디가 아닙니다.");
                return View(companyInfoViewModel);
            }

            //실제패스워드와 입력패스워드 비교
            SHACryptography sha2 = new SHACryptography();
            if (param.LoginPwChk != sha2.EncryptString(companyInfoViewModel.LoginPw))
            {
                companyInfoViewModel =
                   Mapper.Map<CompanyMyInfoViewModel>(scUsr);

                ModelState.AddModelError("", "비밀번호가 일치하지 않습니다.");
                return View(companyInfoViewModel);
            }

            //담당자정보
            scUsr.Name = companyInfoViewModel.Name;
            scUsr.Email = companyInfoViewModel.Email1 + "@" + companyInfoViewModel.Email2;
            //scUsr.FaxNo = mentorViewModel.FaxNo1 + "-" + mentorViewModel.FaxNo2 + "-" + mentorViewModel.FaxNo3;
            scUsr.MbNo = companyInfoViewModel.MbNo1 + "-" + companyInfoViewModel.MbNo2 + "-" + companyInfoViewModel.MbNo3;
            scUsr.TelNo = companyInfoViewModel.TelNo1 + "-" + companyInfoViewModel.TelNo2 + "-" + companyInfoViewModel.TelNo3;

            //회사정보
            scUsr.ScCompInfo.CompNm = companyInfoViewModel.CompNm;
            scUsr.ScCompInfo.OwnNm = companyInfoViewModel.ComOwnNm;
            scUsr.ScCompInfo.TelNo = companyInfoViewModel.ComTelNo1 + "-" + companyInfoViewModel.ComTelNo2 + "-" + companyInfoViewModel.ComTelNo3;
            scUsr.ScCompInfo.PostNo = companyInfoViewModel.ComPostNo;
            scUsr.ScCompInfo.Addr1 = companyInfoViewModel.ComAddr1;
            scUsr.ScCompInfo.Addr2 = companyInfoViewModel.ComAddr2;
            //업태업종 처리해야함

            _scUsrService.ModifyScUsr(scUsr);

            //다성공시 커밋
            await _scUsrService.SaveDbContextAsync();

            return RedirectToAction("MyInfo", "MyInfo");
        }


    }
}