using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
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
        private readonly IScReqDocService _scReqDocService;
        private readonly IScCompMappingService _scCompMappingService;

        public CompanyMngController(IScExpertMappingService _scExpertMappingService, IScReqDocService _scReqDocService, IScCompMappingService _scCompMappingService)
        {
            this._scExpertMappingService = _scExpertMappingService;
            this._scReqDocService = _scReqDocService;
            this._scCompMappingService = _scCompMappingService;
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

            var scCompMappings = await _scCompMappingService.GetExpertCompMappingsAsync(Session[Global.LoginID].ToString());

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

            var scCompMappings = await _scCompMappingService.GetExpertCompMappingsAsync(Session[Global.LoginID].ToString(), int.Parse(BizWorkList), QUERY);

            var companyList =
                Mapper.Map<List<ExpertCompanyViewModel>>(scCompMappings);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<ExpertCompanyViewModel>(companyList.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, companyList.Count));
        }

        public async Task<ActionResult> ReceiveList()
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            ViewBag.StartDate = DateTime.Now.AddMonths(-1).ToShortDateString();
            ViewBag.EndDate = DateTime.Now.ToShortDateString();

            //답변여부 DropDown List  생성
            var checkYN = new List<SelectListItem>(){
                new SelectListItem { Value = "N", Text = "미답변", Selected = true },
                new SelectListItem { Value = "Y", Text = "답변" },
                new SelectListItem { Value = "", Text = "전체" }
            };

            SelectList checkYNList = new SelectList(checkYN, "Value", "Text");

            ViewBag.SelectCheckYNList = checkYNList;

            DateTime startDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString() + " 00:00:00");
            DateTime endDate = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");

            //수신함 조회
            var scReqDocs = await _scReqDocService.GetReceiveDocs(Session[Global.LoginID].ToString(), "N", startDate, endDate, "", "");


            var dataRequestList =
                Mapper.Map<List<DataRequstViewModels>>(scReqDocs);


            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<DataRequstViewModels>(dataRequestList.ToPagedList(1, pagingSize), 1, pagingSize, dataRequestList.Count));
        }

        [HttpPost]
        public async Task<ActionResult> ReceiveList(string ComName, string RegistrationNo, string START_DATE, string END_DATE, string CheckYNList, string curPage)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            //답변여부 DropDown List  생성
            var checkYN = new List<SelectListItem>(){
                new SelectListItem { Value = "N", Text = "미답변", Selected = true },
                new SelectListItem { Value = "Y", Text = "답변" },
                new SelectListItem { Value = "", Text = "전체" }
            };

            SelectList checkYNList = new SelectList(checkYN, "Value", "Text");

            foreach(var item in checkYNList)
            {
                if(item.Value == CheckYNList)
                {
                    item.Selected = true;
                    break;
                }
            }

            ViewBag.SelectCheckYNList = checkYNList;

            DateTime startDate = DateTime.Parse(START_DATE + " 00:00:00");
            DateTime endDate = DateTime.Parse(END_DATE + " 23:59:59");

            //수신함 조회
            var scReqDocs = await _scReqDocService.GetReceiveDocs(Session[Global.LoginID].ToString(), CheckYNList, startDate, endDate, ComName, RegistrationNo);

            var dataRequestList =
                Mapper.Map<List<DataRequstViewModels>>(scReqDocs);


            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<DataRequstViewModels>(dataRequestList.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, dataRequestList.Count));
        }


        public async Task<ActionResult> ReceiveDetail(string reqDocSn)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            var scReqDoc = await _scReqDocService.GetReqDoc(int.Parse(reqDocSn));


            var dataRequest =
                Mapper.Map<DataRequstViewModels>(scReqDoc);

            return View(dataRequest);
        }

        public async Task<ActionResult> ModifyReceive(string reqDocSn)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            var scReqDoc = await _scReqDocService.GetReqDoc(int.Parse(reqDocSn));


            var dataRequest =
                Mapper.Map<DataRequstViewModels>(scReqDoc);

            return View(dataRequest);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyReceive(DataRequstViewModels dataRequestViewModel)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            var scReqDoc = await _scReqDocService.GetReqDoc(dataRequestViewModel.ReqDocSn);
            scReqDoc.ResContents = dataRequestViewModel.ResContents;
            scReqDoc.ChkYn = "Y";
            scReqDoc.ResDt = DateTime.Now;

            int result = await _scReqDocService.SaveDbContextAsync();

            if (result != -1)
                return RedirectToAction("ReceiveDetail", "CompanyMng", new { reqDocSn = dataRequestViewModel.ReqDocSn });
            else
            {
                ModelState.AddModelError("", "답변 등록 실패.");
                return View(dataRequestViewModel);
            }
        }


        public async Task<ActionResult> SendList()
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            ViewBag.StartDate = DateTime.Now.AddMonths(-1).ToShortDateString();
            ViewBag.EndDate = DateTime.Now.ToShortDateString();

            //답변여부 DropDown List  생성
            var checkYN = new List<SelectListItem>(){
                new SelectListItem { Value = "N", Text = "미답변", Selected = true },
                new SelectListItem { Value = "Y", Text = "답변" },
                new SelectListItem { Value = "", Text = "전체" }
            };

            SelectList checkYNList = new SelectList(checkYN, "Value", "Text");

            ViewBag.SelectCheckYNList = checkYNList;

            DateTime startDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString() + " 00:00:00");
            DateTime endDate = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");

            //수신함 조회
            //var scReqDocs = await _scReqDocService.GetSendDocs(Session[Global.LoginID].ToString(), "N", DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString()), DateTime.Parse(DateTime.Now.ToShortDateString()), "", "");
            var scReqDocs = await _scReqDocService.GetSendDocs(Session[Global.LoginID].ToString(), "N", startDate, endDate, "", "");


            var dataRequestList =
                Mapper.Map<List<DataRequstViewModels>>(scReqDocs);


            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<DataRequstViewModels>(dataRequestList.ToPagedList(1, pagingSize), 1, pagingSize, dataRequestList.Count));
        }


        [HttpPost]
        public async Task<ActionResult> SendList(string ComName, string RegistrationNo, string START_DATE, string END_DATE, string CheckYNList, string curPage)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            //답변여부 DropDown List  생성
            var checkYN = new List<SelectListItem>(){
                new SelectListItem { Value = "N", Text = "미답변", Selected = true },
                new SelectListItem { Value = "Y", Text = "답변" },
                new SelectListItem { Value = "", Text = "전체" }
            };

            SelectList checkYNList = new SelectList(checkYN, "Value", "Text");

            foreach (var item in checkYNList)
            {
                if (item.Value == CheckYNList)
                {
                    item.Selected = true;
                    break;
                }
            }

            ViewBag.SelectCheckYNList = checkYNList;

            DateTime startDate = DateTime.Parse(START_DATE + " 00:00:00");
            DateTime endDate = DateTime.Parse(END_DATE + " 23:59:59");

            //수신함 조회
            //var scReqDocs = await _scReqDocService.GetSendDocs(Session[Global.LoginID].ToString(), CheckYNList, DateTime.Parse(START_DATE), DateTime.Parse(END_DATE), ComName, RegistrationNo);
            var scReqDocs = await _scReqDocService.GetSendDocs(Session[Global.LoginID].ToString(), CheckYNList, startDate, endDate, ComName, RegistrationNo);

            var dataRequestList =
                Mapper.Map<List<DataRequstViewModels>>(scReqDocs);


            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<DataRequstViewModels>(dataRequestList.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, dataRequestList.Count));


        }

        public async Task<ActionResult> SendDetail(string reqDocSn)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            var scReqDoc = await _scReqDocService.GetReqDoc(int.Parse(reqDocSn));


            var dataRequest =
                Mapper.Map<DataRequstViewModels>(scReqDoc);

            return View(dataRequest);
        }

        public ActionResult RegSend()
        {
            ViewBag.LeftMenu = Global.CompanyMng;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegSend(DataRequstViewModels dataRequestViewModel)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            if (ModelState.IsValid)
            {
                var scReqDoc = Mapper.Map<ScReqDoc>(dataRequestViewModel);

                //회원정보 추가 정보 설정
                scReqDoc.ChkYn = "N";
                scReqDoc.ReqDt = DateTime.Now;
                scReqDoc.SenderId = Session[Global.LoginID].ToString();
                scReqDoc.Status = "N";

                //저장
                int result = await _scReqDocService.AddReqDocAsync(scReqDoc);

                if (result != -1)
                    return RedirectToAction("SendList", "CompanyMng");
                else
                {
                    ModelState.AddModelError("", "자료요청 등록 실패.");
                    return View(dataRequestViewModel);
                }
            }
            ModelState.AddModelError("", "입력값 검증 실패.");
            return View(dataRequestViewModel);
        }

        public async Task<ActionResult> SearchCompanyPopup(string QUERY)
        {
            ViewBag.QUERY = QUERY;

            var scCompMappings = await _scCompMappingService.GetExpertCompMappingsForPopupAsync(Session[Global.LoginID].ToString(), QUERY);

            var companyList =
                Mapper.Map<List<ExpertCompanyViewModel>>(scCompMappings);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<ExpertCompanyViewModel>(companyList.ToPagedList(1, pagingSize), 1, pagingSize, companyList.Count));
            
        }
    }
}