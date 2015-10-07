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

namespace BizOneShot.Light.Web.Areas.Company.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Company, Order = 2)]
    public class ExpertServiceController : BaseController
    {
        private readonly IScReqDocService _scReqDocService;
        private readonly IScReqDocFileService _scReqDocFileService;

        private readonly IScExpertMappingService _scExpertMappingService;
        private readonly IScCompMappingService _scCompMappingService;
        private readonly IScQaService _scQaService;

        public ExpertServiceController(IScReqDocService scReqDocService, IScReqDocFileService scReqDocFileService 
            , IScExpertMappingService scExpertMappingService, IScCompMappingService scCompMappingService
            , IScQaService scQaService)
        {
            _scReqDocService = scReqDocService;
            _scReqDocFileService = scReqDocFileService;
            _scExpertMappingService = scExpertMappingService;
            _scCompMappingService = scCompMappingService;
            _scQaService = scQaService;
        }


        // GET: Company/ExpertService
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ReceiveList(string expertType, string curPage = null)
        {
            ViewBag.LeftMenu = Global.ExpertService;

            string receiverId = Session[Global.LoginID].ToString();
            string compSn = Session[Global.CompSN].ToString();

            //승인된 사업이 없으면 리다이렉트 함
            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(compSn), "A");
            if(scCompMapping == null)
            {
                TempData["alert"] = "승인된 사업이 없습니다.";

                return RedirectToAction("Index", "Main");
            }

            var listScReqDoc = await _scReqDocService.GetReceiveDocs(receiverId, expertType);

            var dataRequestList =
                Mapper.Map<List<DataRequstViewModels>>(listScReqDoc);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            ViewBag.ExpertType = expertType;

            return View(new StaticPagedList<DataRequstViewModels>(dataRequestList.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, dataRequestList.Count));

        }

        public async Task<ActionResult> ReceiveDetail(string reqDocSn, string expertType)
        {
            ViewBag.LeftMenu = Global.ExpertService;

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

            //전문가 타입 리턴
            ViewBag.ExpertType = expertType;

            return View(dataRequest);
        }

        public async Task<ActionResult> ModifyReceive(string reqDocSn, string expertType)
        {
            ViewBag.LeftMenu = Global.ExpertService;

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

            //전문가 타입 리턴
            ViewBag.ExpertType = expertType;

            return View(dataRequest);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyReceive(DataRequstViewModels dataRequestViewModel, string expertType, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.LeftMenu = Global.ExpertService;

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
                return RedirectToAction("ReceiveDetail", "ExpertService", new { reqDocSn = dataRequestViewModel.ReqDocSn, expertType = expertType });
            else
            {
                ModelState.AddModelError("", "답변 등록 실패.");
                return View(dataRequestViewModel);
            }
        }


        public async Task<ActionResult> SendList(string expertType, string curPage = null)
        {
            ViewBag.LeftMenu = Global.ExpertService;

            string senderId = Session[Global.LoginID].ToString();

            var listScReqDoc = await _scReqDocService.GetSendDocs(senderId, expertType);

            var dataRequestList =
                Mapper.Map<List<DataRequstViewModels>>(listScReqDoc);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            //전문가 타입 뷰로 전달
            ViewBag.ExpertType = expertType;

            return View(new StaticPagedList<DataRequstViewModels>(dataRequestList.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, dataRequestList.Count));

        }

        public async Task<ActionResult> SendDetail(string reqDocSn, string expertType)
        {
            ViewBag.LeftMenu = Global.ExpertService;

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

            //전문가 타입 리턴
            ViewBag.ExpertType = expertType;

            return View(dataRequest);
        }

        public async Task<ActionResult> RegSend(string expertType)
        {
            ViewBag.LeftMenu = Global.ExpertService;
            string senderId = Session[Global.LoginID].ToString();
            string compSn = Session[Global.CompSN].ToString();

            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(compSn), "A");

            var scExpertMapping =  await _scExpertMappingService.GetExpertAsync(scCompMapping.BizWorkSn, expertType);

            var dataRequest = new DataRequstViewModels
            {
                 ReceiverName = scExpertMapping.ScUsr.Name,
                  ReceiverId = scExpertMapping.ScUsr.LoginId
            };
                 
            //전문가 타입 리턴
            ViewBag.ExpertType = expertType;

            return View(dataRequest);
        }

        [HttpPost]
        public async Task<ActionResult> RegSend(DataRequstViewModels dataRequestViewModel, string expertType, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.LeftMenu = Global.ExpertService;

            if (ModelState.IsValid)
            {
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
                    return RedirectToAction("SendList", "ExpertService", new {expertType = expertType });
                else
                {
                    ModelState.AddModelError("", "자료요청 등록 실패.");
                    return View(dataRequestViewModel);
                }
            }
            ModelState.AddModelError("", "입력값 검증 실패.");
            return View(dataRequestViewModel);
        }


        public async Task<ActionResult> CompanyQAList(string expertType, string curPage = null)
        {
            ViewBag.LeftMenu = Global.ExpertService;

            string questionId = Session[Global.LoginID].ToString();

            //수신함 조회
            var scQas = await _scQaService.GetReceiveQAsByQuestionId(questionId, expertType);

            var qaList =
                Mapper.Map<List<QaRequstViewModels>>(scQas);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            //전문가 타입 뷰로 전달
            ViewBag.ExpertType = expertType;

            return View(new StaticPagedList<QaRequstViewModels>(qaList.ToPagedList(int.Parse(curPage ?? "1"), pagingSize), int.Parse(curPage ?? "1"), pagingSize, qaList.Count));
        }

        public async Task<ActionResult> CompanyQADetail(string usrQaSn, string expertType)
        {
            ViewBag.LeftMenu = Global.ExpertService;

            var scQa = await _scQaService.GetQAAsync(int.Parse(usrQaSn));

            var dataQa =
                Mapper.Map<QaRequstViewModels>(scQa);

            //전문가 타입 리턴
            ViewBag.ExpertType = expertType;

            return View(dataQa);
        }

        public async Task<ActionResult> RegCompanyQA(string expertType)
        {
            ViewBag.LeftMenu = Global.ExpertService;
            string questionId = Session[Global.LoginID].ToString();
            string compSn = Session[Global.CompSN].ToString();

            var scCompMapping = await _scCompMappingService.GetCompMappingAsync(int.Parse(compSn), "A");

            var scExpertMapping = await _scExpertMappingService.GetExpertAsync(scCompMapping.BizWorkSn, expertType);

            var dataQa = new QaRequstViewModels
            {
                AnswerId = scExpertMapping.ScUsr.LoginId,
                QuestionId = questionId,
                QuestionComNm = scCompMapping.ScCompInfo.CompNm
            };

            //전문가 타입 리턴
            ViewBag.ExpertType = expertType;

            return View(dataQa);
        }

        #region 파일 다운로드
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
        #endregion
    }
}