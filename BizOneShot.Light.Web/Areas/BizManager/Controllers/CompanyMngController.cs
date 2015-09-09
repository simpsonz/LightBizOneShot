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
        private readonly IScCompInfoService _scCompInfoService;

        public CompanyMngController(IScBizWorkService _scBizWorkService, IScCompInfoService _scCompInfoService)
        {
            this._scBizWorkService = _scBizWorkService;
            this._scCompInfoService = _scCompInfoService;
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



            //사업참여기업 리스트 조회
            var listCompany = await _scCompInfoService.GetCompMappingsAsync(int.Parse(Session[Global.CompSN].ToString()), bizWorkSn);

            var usrViews =
                Mapper.Map<List<CompanyMngViewModel>>(listCompany);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            return View(new StaticPagedList<CompanyMngViewModel>(usrViews.ToPagedList(1, pagingSize), 1, pagingSize, usrViews.Count));
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

            //사업참여기업 리스트 조회
            var listCompany = await _scCompInfoService.GetCompMappingsAsync(int.Parse(Session[Global.CompSN].ToString()), int.Parse(BizWorkList), StatusList, QUERY);

            var usrViews =
                Mapper.Map<List<CompanyMngViewModel>>(listCompany);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            return View(new StaticPagedList<CompanyMngViewModel>(usrViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, usrViews.Count));
        }

        public async Task<ActionResult> CompanyDetail(string bizWorkSn, string compSn)
        {
            ViewBag.LeftMenu = Global.ComMng;

            var scCompMapping = await _scCompInfoService.GetCompMappingAsync(int.Parse(bizWorkSn), int.Parse(compSn) );

            var usrView =
                Mapper.Map<CompanyMngViewModel>(scCompMapping);

            if(scCompMapping.Status == "R")
                return View("ModifyCompany", usrView);
            else 
                return View(usrView);

        }
    }
}