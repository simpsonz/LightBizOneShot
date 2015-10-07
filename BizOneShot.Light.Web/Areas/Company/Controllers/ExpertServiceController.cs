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

        //private readonly IScExpertMappingService _scExpertMappingService;
        //private readonly IScCompMappingService _scCompMappingService;
        //private readonly IScQaService _scQaService;

        public ExpertServiceController(IScReqDocService scReqDocService, IScReqDocFileService scReqDocFileService)
        {
            this._scReqDocService = scReqDocService;
            this._scReqDocFileService = scReqDocFileService;
        }


        // GET: Company/ExpertService
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ReceiveList(string expertType, string curPage)
        {
            ViewBag.LeftMenu = Global.ExpertService;

            string receiverId = Session[Global.LoginID].ToString();

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