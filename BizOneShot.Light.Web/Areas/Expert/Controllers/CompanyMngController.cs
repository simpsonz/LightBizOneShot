using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Helper;
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
        private readonly IScQaService _scQaService;
        private readonly IScReqDocFileService _scReqDocFileService;

        public CompanyMngController(IScExpertMappingService _scExpertMappingService, IScReqDocService _scReqDocService, IScCompMappingService _scCompMappingService, IScQaService _scQaService, IScReqDocFileService _scReqDocFileService)
        {
            this._scExpertMappingService = _scExpertMappingService;
            this._scReqDocService = _scReqDocService;
            this._scCompMappingService = _scCompMappingService;
            this._scQaService = _scQaService;
            this._scReqDocFileService = _scReqDocFileService;
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

            ViewBag.SelectCompInfoList = ReportHelper.MakeCompanyList(null);

            var scCompMappings = await _scCompMappingService.GetExpertCompMappingsAsync(Session[Global.LoginID].ToString(), 0, 0);

            var companyList =
                Mapper.Map<List<ExpertCompanyViewModel>>(scCompMappings);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<ExpertCompanyViewModel>(companyList.ToPagedList(1, pagingSize), 1, pagingSize, companyList.Count));
        }

        [HttpPost]
        public async Task<ActionResult> CompanyList(string BizWorkSn, string CompSn, string curPage)
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

            if(BizWorkSn == "0")
            {
                ViewBag.SelectCompInfoList = ReportHelper.MakeCompanyList(null);
            }
            else
            {
                var listScCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(BizWorkSn));
                var listScCompInfo = listScCompMapping.Select(cmp => cmp.ScCompInfo).ToList();

                ViewBag.SelectCompInfoList = ReportHelper.MakeCompanyList(listScCompInfo);
            }

            var scCompMappings = await _scCompMappingService.GetExpertCompMappingsAsync(Session[Global.LoginID].ToString(), int.Parse(BizWorkSn), int.Parse(CompSn));

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

            //전송자 첨부파일 처리
            var listSenderScReqDocFile = await _scReqDocFileService.GetReqFilesAsync(int.Parse(reqDocSn), "S");

            var listSenderScFileInfo = new List<ScFileInfo>();
            foreach (var scReqDocFile in listSenderScReqDocFile)
            {
                listSenderScFileInfo.Add(scReqDocFile.ScFileInfo);
            }

            var sndFileInfoViewModel = Mapper.Map<IList<FileInfoViewModel>>(listSenderScFileInfo);

            dataRequest.SenderFiles = sndFileInfoViewModel;

            //수신자 첨부파일 처리
            var listReceivedrScReqDocFile = await _scReqDocFileService.GetReqFilesAsync(int.Parse(reqDocSn), "R");

            var listReceiverScFileInfo = new List<ScFileInfo>();
            foreach (var scReqDocFile in listReceivedrScReqDocFile)
            {
                listReceiverScFileInfo.Add(scReqDocFile.ScFileInfo);
            }

            var rcvFileInfoViewModel = Mapper.Map<IList<FileInfoViewModel>>(listReceiverScFileInfo);

            dataRequest.ReceiverFiles = rcvFileInfoViewModel;

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
        public async Task<ActionResult> ModifyReceive(DataRequstViewModels dataRequestViewModel, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            var scReqDoc = await _scReqDocService.GetReqDoc(dataRequestViewModel.ReqDocSn);
            scReqDoc.ResContents = dataRequestViewModel.ResContents;
            scReqDoc.ChkYn = "Y";
            scReqDoc.ResDt = DateTime.Now;

            


            //신규파일정보저장 및 파일업로드
            foreach (var file in files)
            {
                if (file != null)
                {
                    var fileHelper = new FileHelper();

                    var savedFileName = fileHelper.GetUploadFileName(file);

                    var subDirectoryPath = Path.Combine(FileType.Document.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                    var savedFilePath = Path.Combine(subDirectoryPath, savedFileName);

                    var scFileInfo = new ScFileInfo { FileNm = Path.GetFileName(file.FileName), FilePath = savedFilePath, Status = "N", RegId = Session[Global.LoginID].ToString(), RegDt = DateTime.Now };

                    var scReqDocFile = new ScReqDocFile { ScFileInfo = scFileInfo };
                    scReqDocFile.RegType = "R";
                    scReqDocFile.ReqDocSn = dataRequestViewModel.ReqDocSn;

                    scReqDoc.ScReqDocFiles.Add(scReqDocFile);

                    await fileHelper.UploadFile(file, subDirectoryPath, savedFileName);
                }
            }

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

            //전송자 첨부파일 처리
            var listSenderScReqDocFile = await _scReqDocFileService.GetReqFilesAsync(int.Parse(reqDocSn), "S");

            var listSenderScFileInfo = new List<ScFileInfo>();
            foreach (var scReqDocFile in listSenderScReqDocFile)
            {
                listSenderScFileInfo.Add(scReqDocFile.ScFileInfo);
            }

            var sndFileInfoViewModel = Mapper.Map<IList<FileInfoViewModel>>(listSenderScFileInfo);

            dataRequest.SenderFiles = sndFileInfoViewModel;

            //수신자 첨부파일 처리
            var listReceivedrScReqDocFile = await _scReqDocFileService.GetReqFilesAsync(int.Parse(reqDocSn), "R");

            var listReceiverScFileInfo = new List<ScFileInfo>();
            foreach (var scReqDocFile in listReceivedrScReqDocFile)
            {
                listReceiverScFileInfo.Add(scReqDocFile.ScFileInfo);
            }

            var rcvFileInfoViewModel = Mapper.Map<IList<FileInfoViewModel>>(listReceiverScFileInfo);

            dataRequest.ReceiverFiles = rcvFileInfoViewModel;

            return View(dataRequest);
        }

        public ActionResult RegSend()
        {
            ViewBag.LeftMenu = Global.CompanyMng;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegSend(DataRequstViewModels dataRequestViewModel, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            var scReqDoc = Mapper.Map<ScReqDoc>(dataRequestViewModel);

            //회원정보 추가 정보 설정
            scReqDoc.ChkYn = "N";
            scReqDoc.ReqDt = DateTime.Now;
            scReqDoc.SenderId = Session[Global.LoginID].ToString();
            scReqDoc.Status = "N";

            //신규파일정보저장 및 파일업로드
            foreach (var file in files)
            {
                if (file != null)
                {
                    var fileHelper = new FileHelper();

                    var savedFileName = fileHelper.GetUploadFileName(file);

                    var subDirectoryPath = Path.Combine(FileType.Document.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                    var savedFilePath = Path.Combine(subDirectoryPath, savedFileName);

                    var scFileInfo = new ScFileInfo { FileNm = Path.GetFileName(file.FileName), FilePath = savedFilePath, Status = "N", RegId = Session[Global.LoginID].ToString(), RegDt = DateTime.Now };

                    var scReqDocFile = new ScReqDocFile { ScFileInfo = scFileInfo };
                    scReqDocFile.RegType = "S";

                    scReqDoc.ScReqDocFiles.Add(scReqDocFile);

                    await fileHelper.UploadFile(file, subDirectoryPath, savedFileName);
                }
            }

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

        public async Task<ActionResult> SearchCompanyPopup(string QUERY)
        {
            ViewBag.QUERY = QUERY;

            var scCompMappings = await _scCompMappingService.GetExpertCompMappingsForPopupAsync(Session[Global.LoginID].ToString(), QUERY);

            var companyList =
                Mapper.Map<List<ExpertCompanyViewModel>>(scCompMappings);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<ExpertCompanyViewModel>(companyList.ToPagedList(1, pagingSize), 1, pagingSize, companyList.Count));
            
        }

        public async Task<ActionResult> CompanyQAList()
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

            var scQas = await _scQaService.GetReceiveQAsAsync(Session[Global.LoginID].ToString(), "N", startDate, endDate, "", "");

            var qaList =
                Mapper.Map<List<QaRequstViewModels>>(scQas);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<QaRequstViewModels>(qaList.ToPagedList(1, pagingSize), 1, pagingSize, qaList.Count));
        }


        [HttpPost]
        public async Task<ActionResult> CompanyQAList(string ComName, string RegistrationNo, string START_DATE, string END_DATE, string CheckYNList, string curPage)
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
            var scQas = await _scQaService.GetReceiveQAsAsync(Session[Global.LoginID].ToString(), CheckYNList, startDate, endDate, ComName, RegistrationNo);

            var qaList =
                Mapper.Map<List<QaRequstViewModels>>(scQas);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<QaRequstViewModels>(qaList.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, qaList.Count));
        }

        public async Task<ActionResult> CompanyQADetail(string usrQaSn)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            var scQa = await _scQaService.GetQAAsync(int.Parse(usrQaSn));


            var dataQa =
                Mapper.Map<QaRequstViewModels>(scQa);

            return View(dataQa);
        }

        public async Task<ActionResult> ModifyCompanyQA(string usrQaSn)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            var scQa = await _scQaService.GetQAAsync(int.Parse(usrQaSn));


            var dataQa =
                Mapper.Map<QaRequstViewModels>(scQa);

            return View(dataQa);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyCompanyQA(QaRequstViewModels qaRequestViewModel)
        {
            ViewBag.LeftMenu = Global.CompanyMng;

            if (string.IsNullOrEmpty(qaRequestViewModel.Answer))
            {
                ModelState.AddModelError("", "답변을 작성하지 않았습니다.");
                return View(qaRequestViewModel);
            }

            var scQa = await _scQaService.GetQAAsync(qaRequestViewModel.UsrQaSn);

            scQa.AnsDt = DateTime.Now;
            scQa.AnsYn = "Y";
            scQa.Answer = qaRequestViewModel.Answer;

            int result = await _scQaService.SaveDbContextAsync();

            if (result != -1)
                return RedirectToAction("CompanyQADetail", "CompanyMng", new { usrQaSn = qaRequestViewModel.UsrQaSn });
            else
            {
                ModelState.AddModelError("", "답변 등록 실패.");
                return View(qaRequestViewModel);
            }

        }


        public void DownloadReqDocFile()
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

        public async Task DownloadReqDocFileMulti()
        {
            string reqDocSn = Request.QueryString["reqDocSn"];
            string regType = Request.QueryString["regType"];

            string archiveName = "download.zip";

            //Eager Loading 방식
            var listScReqDocFile = await _scReqDocFileService.GetReqFilesAsync(int.Parse(reqDocSn), regType);

            var listScFileInfo = new List<ScFileInfo>();
            foreach (var scFormFile in listScReqDocFile)
            {
                listScFileInfo.Add(scFormFile.ScFileInfo);
            }

            var files = Mapper.Map<IList<FileContent>>(listScFileInfo);

            new FileHelper().DownloadFile(files, archiveName);

        }

        [HttpPost]
        public async Task<JsonResult> GetBizWorkNm(int BizWorkSn)
        {

            var listScCompMapping = await _scCompMappingService.GetCompMappingAsync(BizWorkSn);
            var listScCompInfo = listScCompMapping.Select(cmp => cmp.ScCompInfo).ToList();

            var bizList = ReportHelper.MakeCompanyList(listScCompInfo);

            return Json(bizList);
        }

    }
}