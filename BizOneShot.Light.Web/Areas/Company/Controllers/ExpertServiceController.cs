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

            ViewBag.ExpertType = expertType;

            return View(dataRequest);
        }
    }
}