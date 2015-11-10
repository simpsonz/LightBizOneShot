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
using BizOneShot.Light.Web.ComLib;
using PagedList;

namespace BizOneShot.Light.Web.Areas.BizManager.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.BizManager, Order = 2)]
    public class CompanyMngController : BaseController
    {
        private readonly IScBizWorkService _scBizWorkService;
        private readonly IScMentorMappingService _scMentorMappingService;
        private readonly IScUsrService _scUsrService;
        private readonly IScCompMappingService _scCompMappingService;
        private readonly IScBizTypeService _scBizTypeService;

        public CompanyMngController(IScBizWorkService _scBizWorkService, IScMentorMappingService _scMentorMappingService, IScUsrService _scUsrService
            , IScCompMappingService _scCompMappingService, IScBizTypeService scBizTypeService)
        {
            this._scBizWorkService = _scBizWorkService;
            this._scMentorMappingService = _scMentorMappingService;
            this._scUsrService = _scUsrService;
            this._scCompMappingService = _scCompMappingService;
            _scBizTypeService = scBizTypeService;
        }

        // GET: BizManager/CompanyMng
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CompanyList()
        {
            ViewBag.LeftMenu = Global.ComMng;

            string excutorId = null;
            int bizWorkSn = 0;

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(Session[Global.CompSN].ToString()), excutorId);
           

            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);

            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                bizWorkSn = listScBizWork.First().BizWorkSn;
            }
            else
            { 
                BizWorkDropDownModel title = new BizWorkDropDownModel();
                title.BizWorkSn = 0;
                title.BizWorkNm = "사업명 선택";
                bizWorkDropDown.Insert(0, title);
            }

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;


            //멘토 분야 DropDown List Data
            var status = new List<SelectListItem>(){
                new SelectListItem { Value = "", Text = "승인상태", Selected = true },
                new SelectListItem { Value = "R", Text = "승인대기" },
                new SelectListItem { Value = "A", Text = "승인" }
            };

            SelectList statusList = new SelectList(status, "Value", "Text");

            ViewBag.SelectStatusList = statusList;

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            //사업참여기업 리스트 조회(pagedList 적용)
            var pagedListCompany = await _scCompMappingService.GetPagedListCompMappingsAsync(1, pagingSize, int.Parse(Session[Global.CompSN].ToString()), bizWorkSn);

            var usrViews =
                Mapper.Map<List<CompanyMngViewModel>>(pagedListCompany);

            
            return View(new StaticPagedList<CompanyMngViewModel>(usrViews, 1, pagingSize, pagedListCompany.TotalItemCount));

            //var listCompany = await _scCompMappingService.GetCompMappingsAsync(int.Parse(Session[Global.CompSN].ToString()), bizWorkSn);

            //var usrViews =
            //    Mapper.Map<List<CompanyMngViewModel>>(listCompany);

            //int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            //return View(new StaticPagedList<CompanyMngViewModel>(usrViews.ToPagedList(1, pagingSize), 1, pagingSize, usrViews.Count));
        }

        [HttpPost]
        public async Task<ActionResult> CompanyList(string BizWorkList, string StatusList, string QUERY, string curPage)
        {
            ViewBag.LeftMenu = Global.ComMng;

            string excutorId = null;

            //사업담당자 일 경우 담당 사업만 조회
            if(Session[Global.UserDetailType].ToString() == "M")
            {
                excutorId = Session[Global.LoginID].ToString();
            }

            //사업 DropDown List Data
            var listScBizWork = await _scBizWorkService.GetBizWorkList(int.Parse(Session[Global.CompSN].ToString()), excutorId);

            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);


            //사업담당자 일 경우 담당 사업만 조회
            if (Session[Global.UserDetailType].ToString() == "M")
            {
                BizWorkList = listScBizWork.First().BizWorkSn.ToString();
            }
            else
            {
                BizWorkDropDownModel title = new BizWorkDropDownModel();
                title.BizWorkSn = 0;
                title.BizWorkNm = "사업명 선택";
                bizWorkDropDown.Insert(0, title);
            }

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;


            //멘토 분야 DropDown List Data
            var status = new List<SelectListItem>(){
                new SelectListItem { Value = "", Text = "승인상태", Selected = true },
                new SelectListItem { Value = "R", Text = "승인대기" },
                new SelectListItem { Value = "A", Text = "승인" }
            };

            SelectList statusList = new SelectList(status, "Value", "Text");

            ViewBag.SelectStatusList = statusList;



            //사업참여기업 리스트 조회(PagedList 적용)
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pagedListCompany = await _scCompMappingService.GetPagedListCompMappingsAsync(int.Parse(curPage), pagingSize,int.Parse(Session[Global.CompSN].ToString()), int.Parse(BizWorkList), StatusList, QUERY);

            var usrViews =
                Mapper.Map<List<CompanyMngViewModel>>(pagedListCompany);

            
            return View(new StaticPagedList<CompanyMngViewModel>(usrViews, int.Parse(curPage), pagingSize, pagedListCompany.TotalItemCount));


            //var listCompany = await _scCompMappingService.GetCompMappingsAsync(int.Parse(Session[Global.CompSN].ToString()), int.Parse(BizWorkList), StatusList, QUERY);

            //var usrViews =
            //    Mapper.Map<List<CompanyMngViewModel>>(listCompany);

            //int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            //return View(new StaticPagedList<CompanyMngViewModel>(usrViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, usrViews.Count));

        }

        public async Task<ActionResult> CompanyDetail(string bizWorkSn, string compSn)
        {
            ViewBag.LeftMenu = Global.ComMng;

            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(bizWorkSn), int.Parse(compSn));
            var usrView =
                Mapper.Map<CompanyMngViewModel>(scCompMapping);

            //업태, 업종
            var listScBizType = await _scBizTypeService.GetScBizTypeByCompSn(int.Parse(compSn));
            var bizTypeViewModel =
               Mapper.Map<List< BizTypeViewModel>> (listScBizType);

            usrView.BizTypes = bizTypeViewModel;

            if (scCompMapping.Status == "R")
            {
                //해당 사업의 멘토 리스트 조회
                var scMentorMappings = await _scMentorMappingService.GetMentorListAsync(int.Parse(Session[Global.CompSN].ToString()), int.Parse(bizWorkSn));

                var mentorDropDown =
                    Mapper.Map<List<MentorDropDownModel>>(scMentorMappings);

                MentorDropDownModel title = new MentorDropDownModel();
                title.LoginId = "";
                title.Name = "멘토 선택";
                mentorDropDown.Insert(0, title);

                SelectList mentorList = new SelectList(mentorDropDown, "LoginId", "Name");

                ViewBag.SelectMentorList = mentorList;

                usrView.MentorEmail = "";
                usrView.MentorLoginId = "";
                usrView.MentorMbNo = "";
                usrView.MentorName = "";
                usrView.MentorTelNo = "";
                usrView.MngCompSn = 0;

                return View("ApproveCompany", usrView);
            }
            else
            {
                return View(usrView);
            }

        }

        [HttpPost]
        public async Task<JsonResult> GetMentor(string MentorID)
        {
            ScUsr result = await _scUsrService.SelectScUsr(MentorID);

            var mentor =
                    Mapper.Map<JoinMentorViewModel>(result);

            return Json(mentor);

        }

        [HttpPost]
        public async Task<ActionResult> ApproveCompany(CompanyMngViewModel companyViewModel)
        {
            ViewBag.LeftMenu = Global.ComMng;

            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(companyViewModel.BizWorkSn, companyViewModel.CompSn);

            scCompMapping.MentorId = companyViewModel.MentorLoginId;
            scCompMapping.Status = "A";
            scCompMapping.UpdId = Session[Global.LoginID].ToString();
            scCompMapping.UpdDt = DateTime.Now;

            int result = await _scCompMappingService.SaveDbContextAsync();

            if (result != -1)
                return RedirectToAction("CompanyList", "CompanyMng");
            else
            {
                return RedirectToAction("CompanyDetail", "CompanyMng", new { bizWorkSn = companyViewModel.BizWorkSn, compSn = companyViewModel.CompSn });
            }

            
        }

        public async Task<ActionResult> ModifyCompany(string bizWorkSn, string compSn)
        {
            ViewBag.LeftMenu = Global.ComMng;

            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(bizWorkSn), int.Parse(compSn));
            var usrView =
                Mapper.Map<CompanyMngViewModel>(scCompMapping);

            //업태, 업종
            var listScBizType = await _scBizTypeService.GetScBizTypeByCompSn(int.Parse(compSn));
            var bizTypeViewModel =
               Mapper.Map<List<BizTypeViewModel>>(listScBizType);

            usrView.BizTypes = bizTypeViewModel;

            //해당 사업의 멘토 리스트 조회
            var scMentorMappings = await _scMentorMappingService.GetMentorListAsync(int.Parse(Session[Global.CompSN].ToString()), int.Parse(bizWorkSn));

            var mentorDropDown =
                Mapper.Map<List<MentorDropDownModel>>(scMentorMappings);

            MentorDropDownModel title = new MentorDropDownModel();
            title.LoginId = "";
            title.Name = "멘토 선택";
            mentorDropDown.Insert(0, title);

            SelectList mentorList = new SelectList(mentorDropDown, "LoginId", "Name");

            ViewBag.SelectMentorList = mentorList;

            return View(usrView);

        }

        [HttpPost]
        public async Task<ActionResult> ModifyCompany(CompanyMngViewModel companyViewModel)
        {
            ViewBag.LeftMenu = Global.ComMng;
            

            if(string.IsNullOrEmpty(companyViewModel.MentorLoginId))
            {
                return RedirectToAction("ModifyCompany", "CompanyMng", new { bizWorkSn = companyViewModel.BizWorkSn, compSn = companyViewModel.CompSn });
            }

            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(companyViewModel.BizWorkSn, companyViewModel.CompSn);

            scCompMapping.MentorId = companyViewModel.MentorLoginId;
            scCompMapping.UpdId = Session[Global.LoginID].ToString();
            scCompMapping.UpdDt = DateTime.Now;

            int result = await _scCompMappingService.SaveDbContextAsync();

            if (result != -1)
                return RedirectToAction("CompanyList", "CompanyMng");
            else
            {
                return RedirectToAction("CompanyDetail", "CompanyMng", new { bizWorkSn = companyViewModel.BizWorkSn, compSn = companyViewModel.CompSn });
            }

        }


        [HttpPost]
        public async Task<JsonResult> CancelApprove(string[] compSns)
        {
            ViewBag.LeftMenu = Global.ComMng;

            await _scCompMappingService.CancelApproveCompMapping(compSns);

            return Json(new { result = true });
        }

    }
}