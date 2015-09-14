using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Areas.Expert.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Expert, Order = 2)]
    public class CompanyMngController : BaseController
    {
        private readonly IScExpertMappingService _scExpertMappingService;

        public CompanyMngController(IScExpertMappingService _scExpertMappingService)
        {
            this._scExpertMappingService = _scExpertMappingService;
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
            return View();
        }
    }
}