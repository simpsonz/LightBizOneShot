using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Security;
using BizOneShot.Light.Web.ComLib;
using PagedList;

namespace BizOneShot.Light.Web.Areas.BizManager.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.BizManager, Order = 2)]
    public class MentorMngController : BaseController
    {
        private readonly IScUsrService _scUsrService;
        private readonly IScBizWorkService _scBizWorkService;
        private readonly IScMentorMappingService _scMentorMappingService;

        public MentorMngController(IScUsrService scUsrService, IScBizWorkService _scBizWorkService, IScMentorMappingService _scMentorMappingService)
        {
            this._scUsrService = scUsrService;
            this._scBizWorkService = _scBizWorkService;
            this._scMentorMappingService = _scMentorMappingService;
        }

        // GET: BizManager/MentorMng
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> MentorList()
        {
            ViewBag.LeftMenu = Global.MentorMng;

            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(Session[Global.CompSN].ToString()));


            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);

            BizWorkDropDownModel title = new BizWorkDropDownModel();
            title.BizWorkSn = 0;
            title.BizWorkNm = "사업명 선택";
            bizWorkDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;


            //멘토 분야 DropDown List Data
            var mentorPart = new List<SelectListItem>(){
                new SelectListItem { Value = "", Text = "멘토분야", Selected = true },
                new SelectListItem { Value = "F", Text = "자금" },
                new SelectListItem { Value = "D", Text = "기술개발" },
                new SelectListItem { Value = "T", Text = "세무회계" },
                new SelectListItem { Value = "L", Text = "법무" },
                new SelectListItem { Value = "W", Text = "노무" },
                new SelectListItem { Value = "P", Text = "특허" },
                new SelectListItem { Value = "M", Text = "마케팅" },
                new SelectListItem { Value = "E", Text = "기타" }
            };

            SelectList mentorPartList = new SelectList(mentorPart, "Value", "Text");

            ViewBag.SelectMentorPartList = mentorPartList;

            //전문가 리스트 조회
            var listMentor = await _scMentorMappingService.GetMentorListAsync(int.Parse(Session[Global.CompSN].ToString()));

            var usrViews =
                Mapper.Map<List<JoinMentorViewModel>>(listMentor);
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            return View(new StaticPagedList<JoinMentorViewModel>(usrViews.ToPagedList(1, pagingSize), 1, pagingSize, usrViews.Count));

        }

        [HttpPost]
        public async Task<ActionResult> MentorList(string BizWorkList, string MentorPartList, string curPage)
        {
            ViewBag.LeftMenu = Global.MentorMng;

            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(Session[Global.CompSN].ToString()));


            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);

            BizWorkDropDownModel title = new BizWorkDropDownModel();
            title.BizWorkSn = 0;
            title.BizWorkNm = "사업명 선택";
            bizWorkDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;


            //멘토 분야 DropDown List Data
            var mentorPart = new List<SelectListItem>(){
                new SelectListItem { Value = "", Text = "멘토분야", Selected = true },
                new SelectListItem { Value = "F", Text = "자금" },
                new SelectListItem { Value = "D", Text = "기술개발" },
                new SelectListItem { Value = "T", Text = "세무회계" },
                new SelectListItem { Value = "L", Text = "법무" },
                new SelectListItem { Value = "W", Text = "노무" },
                new SelectListItem { Value = "P", Text = "특허" },
                new SelectListItem { Value = "M", Text = "마케팅" },
                new SelectListItem { Value = "E", Text = "기타" }
            };

            SelectList mentorPartList = new SelectList(mentorPart, "Value", "Text");

            ViewBag.SelectMentorPartList = mentorPartList;

            //전문가 리스트 조회
            var listMentor = await _scMentorMappingService.GetMentorListAsync(int.Parse(Session[Global.CompSN].ToString()), int.Parse(BizWorkList), MentorPartList);

            var usrViews =
                Mapper.Map<List<JoinMentorViewModel>>(listMentor);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            return View(new StaticPagedList<JoinMentorViewModel>(usrViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, usrViews.Count));

        }

        public async Task<ActionResult> RegMentor()
        {
            ViewBag.LeftMenu = Global.MentorMng;

            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(Session[Global.CompSN].ToString()));


            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);

            BizWorkDropDownModel title = new BizWorkDropDownModel();
            title.BizWorkSn = 0;
            title.BizWorkNm = "사업명 선택";
            bizWorkDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegMentor(JoinMentorViewModel joinMentorViewModel)
        {
            ViewBag.LeftMenu = Global.MentorMng;

            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(Session[Global.CompSN].ToString()));


            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);

            BizWorkDropDownModel title = new BizWorkDropDownModel();
            title.BizWorkSn = 0;
            title.BizWorkNm = "사업명 선택";
            bizWorkDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;


            if (ModelState.IsValid)
            {
                if (joinMentorViewModel.BizWorkSn == 0)
                {
                    ModelState.AddModelError("", "사업명을 선택하지 않았습니다.");
                    return View(joinMentorViewModel);
                }

                var scUsr = Mapper.Map<ScUsr>(joinMentorViewModel);
                var scCompInfo = Mapper.Map<ScCompInfo>(joinMentorViewModel);

                //회원정보 추가 정보 설정
                scUsr.RegId = Session[Global.LoginID].ToString();
                scUsr.RegDt = DateTime.Now;
                scUsr.Status = "N";
                scUsr.UsrType = "M";
                scUsr.UsrTypeDetail = "E";

                SHACryptography sha2 = new SHACryptography();
                scUsr.LoginPw = sha2.EncryptString(scUsr.LoginPw);

                //회사정보 추가 정보 설정
                scCompInfo.Status = "N";
                scCompInfo.RegId = Session[Global.LoginID].ToString();
                scCompInfo.RegDt = DateTime.Now;

                //멘토 매핑정보 생성
                ScMentorMappiing scMentorMappiing = new ScMentorMappiing();
                scMentorMappiing.BizWorkSn = joinMentorViewModel.BizWorkSn;
                scMentorMappiing.MentorId = scUsr.LoginId;
                scMentorMappiing.Status = "N";
                scMentorMappiing.CompSn = int.Parse(Session[Global.CompSN].ToString());
                scMentorMappiing.RegId = scUsr.RegId;
                scMentorMappiing.RegDt = scUsr.RegDt;
                scUsr.ScMentorMappiings.Add(scMentorMappiing);

                //저장
                scCompInfo.ScUsrs.Add(scUsr);


                //bool result = _scUsrService.AddCompanyUser(scCompInfo, scUsr, syUser);
                int result = await _scMentorMappingService.AddMentorAsync(scCompInfo);

                if (result != -1)
                    return RedirectToAction("MentorList", "MentorMng");
                else
                {
                    ModelState.AddModelError("", "멘토 등록 실패.");
                    return View(joinMentorViewModel);
                }
            }
            return View(joinMentorViewModel);
        }

        public async Task<ActionResult> MentorDetail(string bizWorkSn, string mentorId)
        {
            ViewBag.LeftMenu = Global.MentorMng;

            ScMentorMappiing scMentorMapping = await _scMentorMappingService.GetMentor(int.Parse(bizWorkSn), mentorId);

            var usrView =
                Mapper.Map<JoinMentorViewModel>(scMentorMapping);

            return View(usrView);
        }

        [HttpPost]
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
    }
}