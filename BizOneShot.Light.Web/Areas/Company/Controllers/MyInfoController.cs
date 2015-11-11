using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Security;
using BizOneShot.Light.Web.ComLib;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;


namespace BizOneShot.Light.Web.Areas.Company.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Company, Order = 2)]
    public class MyInfoController : BaseController
    {
        private readonly IScUsrService _scUsrService;
        private readonly IScBizTypeService _scBizTypeService;
        private readonly IDareAcStdIncmrateBseIdstTService _dareAcStdIncmrateBseIdstTService;
        
        public MyInfoController(IScUsrService scUsrService, IScBizTypeService scBizTypeService, IDareAcStdIncmrateBseIdstTService dareAcStdIncmrateBseIdstTService)
        {
            _scUsrService = scUsrService;
            _scBizTypeService = scBizTypeService;
            _dareAcStdIncmrateBseIdstTService = dareAcStdIncmrateBseIdstTService;
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

            //업종, 종목
            var listScBizType = await _scBizTypeService.GetScBizTypeByCompSn(int.Parse(Session[Global.CompSN].ToString()));
            var bizTypeViewModel =
               Mapper.Map<List<BizTypeViewModel>>(listScBizType);

            myInfo.BizTypes = bizTypeViewModel;

            return View(myInfo);
        }


        public async Task<ActionResult> ModifyMyInfo(CompanyMyInfoViewModel myInfo)
        {
            ViewBag.LeftMenu = Global.MyInfo;

            //업종, 종목
            var listScBizType = await _scBizTypeService.GetScBizTypeByCompSn(int.Parse(Session[Global.CompSN].ToString()));
            var bizTypeViewModel =
               Mapper.Map<List<BizTypeViewModel>>(listScBizType);

            myInfo.BizTypes = bizTypeViewModel;

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

            _scUsrService.ModifyScUsr(scUsr);

            //업종 ,종목
            int compSn = int.Parse(Session[Global.CompSN].ToString());
            if (companyInfoViewModel.BizTypes.Count > 0)
            {
                _scBizTypeService.DeleteScBizTypeByCompSn(compSn);

                foreach (var item in companyInfoViewModel.BizTypes)
                {
                    var scBizType = new ScBizType
                    {
                       CompSn = compSn,
                       BizTypeCd = item.BizTypeCd,
                       BizTypeNm = item.BizTypeNm
                    };

                    _scBizTypeService.AddScBizType(scBizType);
                }
            }
            

            //다성공시 커밋
            await _scUsrService.SaveDbContextAsync();

            return RedirectToAction("MyInfo", "MyInfo");
        }


        [AllowAnonymous]
        public async Task<ActionResult> SearchBizTypePopup(string Id, string curPage=null, string QUERY = null)
        {
            ViewBag.QUERY = QUERY;
            ViewBag.Id = Id;

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pagedListBizType = await _dareAcStdIncmrateBseIdstTService.GetBizTypes(int.Parse(curPage ?? "1"), pagingSize, QUERY);

            var bizTypeView =
                Mapper.Map<List<BizTypeViewModel>>(pagedListBizType);

            return View(new StaticPagedList<BizTypeViewModel>(bizTypeView, int.Parse(curPage ?? "1"), pagingSize, pagedListBizType.TotalItemCount));

        }


    }
}