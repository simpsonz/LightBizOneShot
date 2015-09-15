using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;
using PagedList;

namespace BizOneShot.Light.Web.Areas.Expert.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Expert, Order = 2)]
    public class CompanyMngController : BaseController
    {
        private readonly IScExpertMappingService _scExpertMappingService;
        private readonly IScCompInfoService _scCompInfoService;

        public CompanyMngController(IScExpertMappingService _scExpertMappingService, IScCompInfoService _scCompInfoService)
        {
            this._scExpertMappingService = _scExpertMappingService;
            this._scCompInfoService = _scCompInfoService;
        }

        // GET: Expert/CompanyMng
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CompanyList()
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            //사업 DropDown List Data
            var scExpertsMapping = await _scExpertMappingService.GetExpertsAsync(Session[Global.LoginID].ToString());


            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(scExpertsMapping);

            //사업담당자 일 경우 담당 사업만 조회
            BizWorkDropDownModel title = new BizWorkDropDownModel();
            title.BizWorkSn = 0;
            title.BizWorkNm = "사업명 선택";
            bizWorkDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;

            var scCompMappings = await _scCompInfoService.GetExpertCompMappingsAsync(Session[Global.LoginID].ToString());

            var companyList =
                Mapper.Map<List<ExpertCompanyViewModel>>(scCompMappings);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<ExpertCompanyViewModel>(companyList.ToPagedList(1, pagingSize), 1, pagingSize, companyList.Count));
        }

        [HttpPost]
        public async Task<ActionResult> CompanyList(string BizWorkList, string QUERY, string curPage)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            //사업 DropDown List Data
            var scExpertsMapping = await _scExpertMappingService.GetExpertsAsync(Session[Global.LoginID].ToString());


            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(scExpertsMapping);

            //사업담당자 일 경우 담당 사업만 조회
            BizWorkDropDownModel title = new BizWorkDropDownModel();
            title.BizWorkSn = 0;
            title.BizWorkNm = "사업명 선택";
            bizWorkDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;

            var scCompMappings = await _scCompInfoService.GetExpertCompMappingsAsync(Session[Global.LoginID].ToString(), int.Parse(BizWorkList), QUERY);

            var companyList =
                Mapper.Map<List<ExpertCompanyViewModel>>(scCompMappings);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<ExpertCompanyViewModel>(companyList.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, companyList.Count));
        }

        public async Task<ActionResult> ReceiveLsit()
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            //사업 DropDown List Data
            var scExpertsMapping = await _scExpertMappingService.GetExpertsAsync(Session[Global.LoginID].ToString());


            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(scExpertsMapping);

            //사업담당자 일 경우 담당 사업만 조회
            BizWorkDropDownModel title = new BizWorkDropDownModel();
            title.BizWorkSn = 0;
            title.BizWorkNm = "사업명 선택";
            bizWorkDropDown.Insert(0, title);

            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;

            var scCompMappings = await _scCompInfoService.GetExpertCompMappingsAsync(Session[Global.LoginID].ToString());

            var companyList =
                Mapper.Map<List<ExpertCompanyViewModel>>(scCompMappings);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<ExpertCompanyViewModel>(companyList.ToPagedList(1, pagingSize), 1, pagingSize, companyList.Count));
        }
    }
}