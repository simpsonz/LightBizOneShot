using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Helper;
using BizOneShot.Light.Util.Security;
using BizOneShot.Light.Web.ComLib;
using PagedList;

namespace BizOneShot.Light.Web.Areas.SysManager.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
    public class ExpertManagerController : BaseController
    {
        private readonly IScUsrService _scUsrService;
        private readonly IScBizWorkService _scBizWorkService;
        private readonly IScExpertMappingService _scExpertMappingService;
        private readonly IScBizTypeService _scBizTypeService;

        public ExpertManagerController(IScUsrService scUsrService, IScBizWorkService scBizWorkService, IScExpertMappingService scExpertMappingService, IScBizTypeService scBizTypeService)
        {
            _scUsrService = scUsrService;
            _scBizWorkService = scBizWorkService;
            _scExpertMappingService = scExpertMappingService;
            _scBizTypeService = scBizTypeService;
        }
        // GET: SysManager/ExpertManager
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ExpertManager()
        {
            ViewBag.LeftMenu = Global.ExpertMng;


            //사업관리자 DropDown List Data
            var bizMngList = await _scUsrService.GetBizManagerAsync();

            var bizMngDropDown =
                Mapper.Map<List<BizMngDropDownModel>>(bizMngList);

            BizMngDropDownModel title = new BizMngDropDownModel();
            title.CompSn = 0;
            title.CompNm = "사업관리자 선택";
            bizMngDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizMngDropDown, "CompSn", "CompNm");

            ViewBag.SelectBizList = bizList;

            //전문가 분야 DropDown List Data
            var expertPart = new List<SelectListItem>(){
                new SelectListItem { Value = "", Text = "전문분야", Selected = true },
                new SelectListItem { Value = "T", Text = "세무" },
                new SelectListItem { Value = "L", Text = "법무" },
                new SelectListItem { Value = "W", Text = "노무" },
                new SelectListItem { Value = "P", Text = "특허" },
                new SelectListItem { Value = "M", Text = "마케팅" }
            };

            SelectList expertPartList = new SelectList(expertPart, "Value", "Text");

            ViewBag.SelectBizPartList = expertPartList;

            //전문가 리스트 조회
            var listScUsr = await _scUsrService.GetExpertManagerAsync();

            var usrViews =
                Mapper.Map<List<JoinExpertViewModel>>(listScUsr);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<JoinExpertViewModel>(usrViews.ToPagedList(1, pagingSize), 1, pagingSize, usrViews.Count));
        }

        [HttpPost]
        public async Task<ActionResult> ExpertManager(string BizMngList, string ExpertPartList, string curPage)
        {
            ViewBag.LeftMenu = Global.ExpertMng;


            //사업관리자 DropDown List Data
            var bizMngList = await _scUsrService.GetBizManagerAsync();

            var bizMngDropDown =
                Mapper.Map<List<BizMngDropDownModel>>(bizMngList);

            BizMngDropDownModel title = new BizMngDropDownModel();
            title.CompSn = 0;
            title.CompNm = "사업관리자 선택";
            bizMngDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizMngDropDown, "CompSn", "CompNm");

            ViewBag.SelectBizList = bizList;

            //전문가 분야 DropDown List Data
            var expertPart = new List<SelectListItem>(){
                new SelectListItem { Value = "", Text = "전문분야", Selected = true },
                new SelectListItem { Value = "T", Text = "세무" },
                new SelectListItem { Value = "L", Text = "법무" },
                new SelectListItem { Value = "W", Text = "노무" },
                new SelectListItem { Value = "P", Text = "특허" },
                new SelectListItem { Value = "M", Text = "마케팅" }
            };

            SelectList expertPartList = new SelectList(expertPart, "Value", "Text");

            ViewBag.SelectBizPartList = expertPartList;

            IList<ScUsr> listScUsr;
            IList<ScExpertMapping> listExpert;
            IList<JoinExpertViewModel> usrViews;

            //전문가 리스트 조회
            if ((string.IsNullOrEmpty(BizMngList) && string.IsNullOrEmpty(ExpertPartList)) || ((BizMngList == "0") && string.IsNullOrEmpty(ExpertPartList)))
            {
                listScUsr = await _scUsrService.GetExpertManagerAsync(BizMngList, ExpertPartList);
                usrViews = Mapper.Map<List<JoinExpertViewModel>>(listScUsr);
            }
            else if ((BizMngList != "0") && string.IsNullOrEmpty(ExpertPartList))
            {
                listExpert = await _scExpertMappingService.GetExpertManagerAsync(BizMngList, ExpertPartList);
                usrViews = Mapper.Map<List<JoinExpertViewModel>>(listExpert);
            }
            else if ((BizMngList == "0") && !string.IsNullOrEmpty(ExpertPartList))
            {
                listScUsr = await _scUsrService.GetExpertManagerAsync(BizMngList, ExpertPartList);
                usrViews = Mapper.Map<List<JoinExpertViewModel>>(listScUsr);
            }
            else
            {
                listExpert = await _scExpertMappingService.GetExpertManagerAsync(BizMngList, ExpertPartList);
                usrViews = Mapper.Map<List<JoinExpertViewModel>>(listExpert);
            }


            //var listScUsr = await _scUsrService.GetExpertManagerAsync(BizMngList, ExpertPartList);

            //var usrViews =
            //    Mapper.Map<List<JoinExpertViewModel>>(listScUsr);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<JoinExpertViewModel>(usrViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, usrViews.Count));
        }


        public async Task<ActionResult> RegExpertManager()
        {
            ViewBag.LeftMenu = Global.ExpertMng;

            //사업관리자 DropDown List Data
            var bizMngList = await _scUsrService.GetBizManagerAsync();

            var bizMngDropDown =
                Mapper.Map<List<BizMngDropDownModel>>(bizMngList);

            BizMngDropDownModel title = new BizMngDropDownModel();
            title.CompSn = 0;
            title.CompNm = "사업관리기관 선택";
            bizMngDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizMngDropDown, "CompSn", "CompNm");

            ViewBag.SelectBizMngList = bizList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegExpertManager(JoinExpertViewModel joinExpertViewModel, string BizList)
        {
            ViewBag.LeftMenu = Global.ExpertMng;

            var bizMngList = await _scUsrService.GetBizManagerAsync();
            var bizMngDropDown =
                Mapper.Map<List<BizMngDropDownModel>>(bizMngList);
            BizMngDropDownModel title = new BizMngDropDownModel();
            title.CompSn = 0;
            title.CompNm = "사업관리기관 선택";
            bizMngDropDown.Insert(0, title);
            
            SelectList bizList = new SelectList(bizMngDropDown, "CompSn", "CompNm");
            ViewBag.SelectBizMngList = bizList;

            if (ModelState.IsValid)
            {
                if(string.IsNullOrEmpty(BizList) || BizList == "0" )
                {
                    ModelState.AddModelError("", "사업명을 선택하지 않았습니다.");
                    return View(joinExpertViewModel);
                }

                var expert = await _scExpertMappingService.GetSameDetailTypeExpertAsync(int.Parse(BizList), joinExpertViewModel.UsrTypeDetail);
                if(expert != null)
                {
                    ModelState.AddModelError("", "동일한 분야의 전문가가 이미 사업에 맵핑되어 있습니다.");
                    return View(joinExpertViewModel);
                }

                var scUsr = Mapper.Map<ScUsr>(joinExpertViewModel);
                var scCompInfo = Mapper.Map<ScCompInfo>(joinExpertViewModel);

                //회원정보 추가 정보 설정
                scUsr.RegId = Session[Global.LoginID].ToString();
                scUsr.RegDt = DateTime.Now;
                scUsr.Status = "N";
                scUsr.UsrType = "P";

                SHACryptography sha2 = new SHACryptography();
                scUsr.LoginPw = sha2.EncryptString(scUsr.LoginPw);

                //회사정보 추가 정보 설정
                scCompInfo.Status = "N";
                scCompInfo.RegId = Session[Global.LoginID].ToString();
                scCompInfo.RegDt = DateTime.Now;

                //전문가 매핑정보 생성
                IList<ScExpertMapping> scExpertMappings = new List<ScExpertMapping>();
                ScExpertMapping scExpertMapping = new ScExpertMapping();
                scExpertMapping.BizWorkSn = int.Parse(BizList);
                scExpertMapping.ExpertId = scUsr.LoginId;
                scExpertMapping.Status = "N";
                scExpertMapping.RegId = scUsr.RegId;
                scExpertMapping.RegDt = scUsr.RegDt;
                scExpertMappings.Add(scExpertMapping);
                scUsr.ScExpertMappings = scExpertMappings;

                //저장
                IList<ScUsr> scUsrs = new List<ScUsr>();
                scUsrs.Add(scUsr);
                scCompInfo.ScUsrs = scUsrs;


                //bool result = _scUsrService.AddCompanyUser(scCompInfo, scUsr, syUser);
                int result = await _scUsrService.AddBizManagerAsync(scCompInfo);

                if (result != -1)
                    return RedirectToAction("ExpertManager", "ExpertManager");
                else
                { 
                    ModelState.AddModelError("", "전문가 등록 실패.");
                    return View(joinExpertViewModel);
                }
            }
            return View(joinExpertViewModel);
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

        [HttpPost]
        public async Task<JsonResult> GetBizList(string CompSn)
        {
            var bizWork = await _scBizWorkService.GetBizWorkList(int.Parse(CompSn));

            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(bizWork);

            return Json(bizWorkDropDown);
        }

        public async Task<ActionResult> ExpertManagerDetail(string loginId)
        {
            ViewBag.LeftMenu = Global.ExpertMng;

            ScUsr scUsr = await _scUsrService.SelectScUsr(loginId);
            var myInfo =
              Mapper.Map<JoinExpertViewModel>(scUsr);

            //업종, 종목
            var listScBizType = await _scBizTypeService.GetScBizTypeByCompSn(myInfo.CompSn);
            var bizTypeViewModel =
               Mapper.Map<List<BizTypeViewModel>>(listScBizType);

            myInfo.BizTypes = bizTypeViewModel;

            return View(myInfo);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteExpert(string[] loginIds)
        {
            ViewBag.LeftMenu = Global.ExpertMng;

            foreach (var loginId in loginIds)
            {
                ScUsr scUsr = await _scUsrService.SelectScUsr(loginId);
                scUsr.Status = "D";
                await _scUsrService.SaveDbContextAsync();
            }
            return Json(new { result = true });
        }

        public void DownloadResumeFile()
        {
            //System.Collections.Specialized.NameValueCollection col = Request.QueryString;
            string fileNm = Request.QueryString["FileNm"];
            string filePath = Request.QueryString["FilePath"];

            string archiveName = fileNm;

            var files = new List<FileContent>();

            var file = new FileContent
            {
                FileNm = fileNm,
                FilePath = filePath
            };
            files.Add(file);

            new FileHelper().DownloadFile(files, archiveName);
        }
    }
}