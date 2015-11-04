using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;
using PagedList;
using AutoMapper;
using System.Threading.Tasks;
using BizOneShot.Light.Util.Helper;
using System;
using System.Web;
using System.IO;
using System.Linq;


namespace BizOneShot.Light.Web.Controllers
{
    public class CsController : BaseController
    {
        private readonly IScFaqService _scFaqService;
        private readonly IScNtcService _scNtcService;
        private readonly IScFormService _scFormService;
        private readonly IScFormFileService _scFormFileService;
        private readonly IScQclService _scQclService;

        public CsController(
            IScFaqService scFaqService, IScNtcService scNtcServcie,
            IScFormService scFormService, IScFormFileService scFormFileService,
            IScQclService _scQclService)
        {
            this._scFaqService = scFaqService;
            this._scNtcService = scNtcServcie;
            this._scFormService = scFormService;
            this._scFormFileService = scFormFileService;
            this._scQclService = _scQclService;
        }

        public CsController()
        {

        }

        // GET: Cs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CompanyGuide()
        {
            return View();
        }

        public ActionResult MentorGuide()
        {
            return View();
        }

        public ActionResult ExpertGuide()
        {
            return View();
        }

        #region FAQ 
        public async Task<ActionResult> Faq()
        {
            ViewBag.LeftMenu = Global.Cs;

            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };

            ViewBag.SelectList = searchBy;
           
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pagedListfaqs = await _scFaqService.GetPagedListFaqsAsync(1, pagingSize);

            var faqViews =
               Mapper.Map<List<FaqViewModel>>(pagedListfaqs);

            //return View(new StaticPagedList<FaqViewModel>(faqViews.ToPagedList(1, pagingSize), 1, pagingSize, faqViews.Count));
            return View(new StaticPagedList<FaqViewModel>(faqViews, 1, pagingSize, pagedListfaqs.TotalItemCount));
        }

        [HttpPost]
        public async Task<ActionResult> Faq(string SelectList, string Query, string curPage)
        {
            ViewBag.LeftMenu = Global.Cs;

            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };
            ViewBag.SelectList = searchBy;

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pagedListfaqs = await _scFaqService.GetPagedListFaqsAsync(int.Parse(curPage ?? "1"), pagingSize, SelectList, Query);

            var faqViews =
               Mapper.Map<List<FaqViewModel>>(pagedListfaqs);

            return View(new StaticPagedList<FaqViewModel>(faqViews, int.Parse(curPage), pagingSize, pagedListfaqs.TotalItemCount));
        }


        public async Task<ActionResult> RegFaq()
        {
            ViewBag.LeftMenu = Global.Cs;

            //Faq 분류 조회
            var qclList = await _scQclService.GetScQclsAsync();
            var qclDropDown =
                Mapper.Map<List<QclDropDownModel>>(qclList);

            SelectList qclSelcetList = new SelectList(qclDropDown, "QclSn", "QclNm");

            ViewBag.SelectQclList = qclSelcetList;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegFaq(FaqViewModel faqViewModel)
        {
            ViewBag.LeftMenu = Global.Cs;

            var scFaq =
                Mapper.Map<ScFaq>(faqViewModel);

            scFaq.Stat = "N";
            scFaq.RegDt = DateTime.Now;
            scFaq.RegId = Session[Global.LoginID].ToString();

            //Faq 등록
            int result = await _scFaqService.AddFaqAsync(scFaq);

            if (result != -1)
                return RedirectToAction("Faq", "Cs");
            else
            {
                ModelState.AddModelError("", "FAQ 등록 실패.");
                return View(faqViewModel);
            }
        }


        public async Task<ActionResult> FaqDetail(string faqSn)
        {
            ViewBag.LeftMenu = Global.Cs;

            //Faq 조회
            var scFaq = await _scFaqService.GetFaqAsync(int.Parse(faqSn));
            var scFaqViewModel =
                Mapper.Map<FaqViewModel>(scFaq);

            return View(scFaqViewModel);
        }

        public async Task<ActionResult> ModifyFaq(string faqSn)
        {
            ViewBag.LeftMenu = Global.Cs;
            //Faq 분류 조회
            var qclList = await _scQclService.GetScQclsAsync();
            var qclDropDown =
                Mapper.Map<List<QclDropDownModel>>(qclList);

            SelectList qclSelcetList = new SelectList(qclDropDown, "QclSn", "QclNm");

            ViewBag.SelectQclList = qclSelcetList;
            //Faq 조회
            var scFaq = await _scFaqService.GetFaqAsync(int.Parse(faqSn));
            var scFaqViewModel =
                Mapper.Map<FaqViewModel>(scFaq);

            return View(scFaqViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyFaq(FaqViewModel faqViewModel)
        {
            ViewBag.LeftMenu = Global.Cs;

            //Faq 조회
            var scFaq = await _scFaqService.GetFaqAsync(faqViewModel.FaqSn);

            scFaq.QstTxt = faqViewModel.QstTxt;
            scFaq.AnsTxt = faqViewModel.AnsTxt;
            scFaq.QclSn = faqViewModel.QclSn;
            scFaq.UpdDt = DateTime.Now;
            scFaq.UpdId = Session[Global.LoginID].ToString();

            await _scFaqService.SaveDbContextAsync();

            return RedirectToAction("Faq", "Cs");
        }


        public async Task<ActionResult> DeleteFaq(string faqSn)
        {
            ViewBag.LeftMenu = Global.Cs;
            //Faq 조회
            var scFaq = await _scFaqService.GetFaqAsync(int.Parse(faqSn));
            scFaq.Stat = "D";
            scFaq.UpdDt = DateTime.Now;
            scFaq.UpdId = Session[Global.LoginID].ToString();

            await _scFaqService.SaveDbContextAsync();

            return RedirectToAction("Faq", "Cs");
        }

        #endregion

        #region Notice(공지사항)
        public async Task<ActionResult> Notice()
        {
            ViewBag.LeftMenu = Global.Cs;

            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };

            ViewBag.SelectList = searchBy;

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pagedListScNtc = await _scNtcService.GetNoticesAsync(1, pagingSize);

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(pagedListScNtc);

            //return View(new StaticPagedList<NoticeViewModel>(noticeViews.ToPagedList(1, pagingSize), 1, pagingSize, noticeViews.Count));
            return View(new StaticPagedList<NoticeViewModel>(noticeViews, 1, pagingSize, pagedListScNtc.TotalItemCount));
        }

        [HttpPost]
        public async Task<ActionResult> Notice(string SelectList, string Query, string curPage)
        {
            ViewBag.LeftMenu = Global.Cs;

            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };
            ViewBag.SelectList = searchBy;

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pagedListScNtc = await _scNtcService.GetNoticesAsync(int.Parse(curPage ?? "1"), pagingSize, SelectList, Query);

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(pagedListScNtc);

            //return View(new StaticPagedList<NoticeViewModel>(noticeViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, noticeViews.Count));
            return View(new StaticPagedList<NoticeViewModel>(noticeViews, int.Parse(curPage ?? "1"), pagingSize, pagedListScNtc.TotalItemCount));
        }

        public async Task<ActionResult> NoticeDetail(int noticeSn)
        {
            ViewBag.LeftMenu = Global.Cs;

            var dicScNtc = await _scNtcService.GetNoticeDetailByIdAsync(noticeSn);

            var noticeDetailView =
                Mapper.Map<NoticeDetailViewModel>(dicScNtc["curNotice"]);

            foreach (var key in dicScNtc.Keys)
            {
                var value = dicScNtc[key];

                if (key == "preNotice" && value != null)
                {
                    noticeDetailView.PreNoticeSn = value.NoticeSn;
                    noticeDetailView.PreSubject = value.Subject;
                }
                else if (key == "nextNotice" && value != null)
                {
                    noticeDetailView.NextNoticeSn = value.NoticeSn;
                    noticeDetailView.NextSubject = value.Subject;
                }
            }

            return View(noticeDetailView);
        }

        public async Task<ActionResult> DeleteNotice(int noticeSn)
        {
            ViewBag.LeftMenu = Global.Cs;

            var dicScNtc = await _scNtcService.GetNoticeAsync(noticeSn);
            dicScNtc.Status = "D";
            dicScNtc.UpdDt = DateTime.Now;
            dicScNtc.UpdId = Session[Global.LoginID].ToString();

            await _scNtcService.SaveDbContextAsync();

            return RedirectToAction("Notice", "Cs");
        }

        public ActionResult RegNotice()
        {
            ViewBag.LeftMenu = Global.Cs;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegNotice(NoticeViewModel noticeViewModel)
        {
            ViewBag.LeftMenu = Global.Cs;

            var scNtc =
                Mapper.Map<ScNtc>(noticeViewModel);

            scNtc.Status = "N";
            scNtc.RegDt = DateTime.Now;
            scNtc.RegId = Session[Global.LoginID].ToString();

            //Faq 등록
            int result = await _scNtcService.AddNoticeAsync(scNtc);

            if (result != -1)
                return RedirectToAction("Notice", "Cs");
            else
            {
                ModelState.AddModelError("", "공지 등록 실패.");
                return View(noticeViewModel);
            }
        }


        public async Task<ActionResult> ModifyNotice(int noticeSn)
        {
            ViewBag.LeftMenu = Global.Cs;
            var scNtc = await _scNtcService.GetNoticeAsync(noticeSn);

            var scNtcViewModel =
                Mapper.Map<NoticeViewModel>(scNtc);

            return View(scNtcViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyNotice(NoticeViewModel noticeViewModel)
        {
            ViewBag.LeftMenu = Global.Cs;

            var scNtc = await _scNtcService.GetNoticeAsync(noticeViewModel.NoticeSn);

            scNtc.Subject = noticeViewModel.Subject;
            scNtc.RmkTxt = noticeViewModel.RmkTxt;
            scNtc.UpdDt = DateTime.Now;
            scNtc.UpdId = Session[Global.LoginID].ToString();

            await _scNtcService.SaveDbContextAsync();

            return RedirectToAction("Notice", "Cs");
        }
        #endregion

        #region Manual(매뉴얼)
        public async Task<ActionResult> Manual()
        {
            ViewBag.LeftMenu = Global.Cs;

            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };

            ViewBag.SelectList = searchBy;

            int pageSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pagedListScForm = await _scFormService.GetManualsAsync(1, pageSize);

            var manualViews =
                Mapper.Map<List<ManualViewModel>>(pagedListScForm);

            

            //return View(new StaticPagedList<ManualViewModel>(manualViews.ToPagedList(1, pagingSize), 1, pagingSize, manualViews.Count));
            return View(new StaticPagedList<ManualViewModel>(manualViews, 1, pageSize, pagedListScForm.TotalItemCount));
        }

        [HttpPost]
        public async Task<ActionResult> Manual(string SelectList, string Query, string curPage)
        {
            ViewBag.LeftMenu = Global.Cs;

            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };

            ViewBag.SelectList = searchBy;

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var pageListScForm = await _scFormService.GetManualsAsync(int.Parse(curPage ?? "1"), pagingSize, SelectList, Query);

            var manualViews =
                Mapper.Map<List<ManualViewModel>>(pageListScForm);


            //return View(new StaticPagedList<ManualViewModel>(manualViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, manualViews.Count));
            return View(new StaticPagedList<ManualViewModel>(manualViews, int.Parse(curPage ?? "1"), pagingSize, pageListScForm.TotalItemCount));
        }

        public async Task<ActionResult> ManualDetail(int formSn)
        {
            ViewBag.LeftMenu = Global.Cs;

            var dicScForm = await _scFormService.GetManualDetailByIdAsync(formSn);

            var curScForm = dicScForm["curForm"];

            var manualDetailView =
                Mapper.Map<ManualDetailViewModel>(curScForm);

            foreach (var key in dicScForm.Keys)
            {
                var value = dicScForm[key];

                if (key == "preForm" && value != null)
                {
                    manualDetailView.PreFormSn = value.FormSn;
                    manualDetailView.PreSubject = value.Subject;
                }
                else if (key == "nextForm" && value != null)
                {
                    manualDetailView.NextFormSn = value.FormSn;
                    manualDetailView.NextSubject = value.Subject;
                }
            }


            ////Lazy Loading 방식
            //var listScFileInfo = new List<ScFileInfo>();
            //foreach (var scFormFile in curScForm.ScFormFiles)
            //{
            //    listScFileInfo.Add(scFormFile.ScFileInfo);
            //}

            //var fileInfoViewModel = Mapper.Map<IList<FileInfoViewModel>>(listScFileInfo);
            //manualDetailView.ManualFiles = manualDetailFileInfo;


            //Eager Loading 방식
            var listScFormFile = await _scFormFileService.GetFormFilesByIdAsync(formSn);

            var listScFileInfo = new List<ScFileInfo>();
            foreach (var scFormFile in listScFormFile)
            {
                listScFileInfo.Add(scFormFile.ScFileInfo);
            }

            var fileInfoViewModel = Mapper.Map<IList<FileInfoViewModel>>(listScFileInfo);

            manualDetailView.ManualFiles = fileInfoViewModel;

            return View(manualDetailView);
        }


        public ActionResult RegManual()
        {
            ViewBag.LeftMenu = Global.Cs;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegManual(ManualViewModel manualViewModel, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.LeftMenu = Global.Cs;

            var scForm =
                Mapper.Map<ScForm>(manualViewModel);

            scForm.Status = "N";
            scForm.RegDt = DateTime.Now;
            scForm.RegId = Session[Global.LoginID].ToString();


            //첨부파일
            if (files != null)
            {
                var fileHelper = new FileHelper();
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var savedFileName = fileHelper.GetUploadFileName(file);

                        var subDirectoryPath = Path.Combine(FileType.Manual.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                        var savedFilePath = Path.Combine(subDirectoryPath, savedFileName);

                        var scFileInfo = new ScFileInfo { FileNm = Path.GetFileName(file.FileName), FilePath = savedFilePath, Status = "N", RegId = Session[Global.LoginID].ToString(), RegDt = DateTime.Now };

                        var scFormFile = new ScFormFile { ScFileInfo = scFileInfo };
                        scForm.ScFormFiles.Add(scFormFile);

                        await fileHelper.UploadFile(file, subDirectoryPath, savedFileName);
                    }
                }
            }

            //저장
            int result = await _scFormService.AddScFormAsync(scForm);

            if (result != -1)
                return RedirectToAction("Manual", "Cs");
            else
            {
                ModelState.AddModelError("", "메뉴얼 등록 실패.");
                return View(manualViewModel);
            }

        }

        public async Task<ActionResult> DeleteManual(int formSn)
        {
            ViewBag.LeftMenu = Global.Cs;

            var dicScForm = await _scFormService.GetScFormAsync(formSn);
            dicScForm.Status = "D";
            dicScForm.UpdDt = DateTime.Now;
            dicScForm.UpdId = Session[Global.LoginID].ToString();

            await _scFormService.SaveDbContextAsync();

            return RedirectToAction("Manual", "Cs");
        }

        public async Task<ActionResult> ModifyManual(int formSn)
        {
            ViewBag.LeftMenu = Global.Cs;

            var dicScForm = await _scFormService.GetManualDetailByIdAsync(formSn);

            var curScForm = dicScForm["curForm"];

            var manualDetailView =
                Mapper.Map<ManualDetailViewModel>(curScForm);

            foreach (var key in dicScForm.Keys)
            {
                var value = dicScForm[key];

                if (key == "preForm" && value != null)
                {
                    manualDetailView.PreFormSn = value.FormSn;
                    manualDetailView.PreSubject = value.Subject;
                }
                else if (key == "nextForm" && value != null)
                {
                    manualDetailView.NextFormSn = value.FormSn;
                    manualDetailView.NextSubject = value.Subject;
                }
            }


            //Eager Loading 방식
            var listScFormFile = await _scFormFileService.GetFormFilesByIdAsync(formSn);

            var listScFileInfo = new List<ScFileInfo>();
            foreach (var scFormFile in listScFormFile)
            {
                listScFileInfo.Add(scFormFile.ScFileInfo);
            }

            var fileInfoViewModel = Mapper.Map<IList<FileInfoViewModel>>(listScFileInfo);

            manualDetailView.ManualFiles = fileInfoViewModel;

            return View(manualDetailView);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyManual(ManualDetailViewModel manualDetailViewModel, string deleteFileSns, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.LeftMenu = Global.Cs;

            var scForm = await _scFormService.GetScFormAsync(manualDetailViewModel.Manual.FormSn);
            scForm.FormType = manualDetailViewModel.Manual.FormType;
            scForm.Subject = manualDetailViewModel.Manual.Subject;
            scForm.Contents = manualDetailViewModel.Manual.Contents;
            scForm.UpdDt = DateTime.Now;
            scForm.UpdId = Session[Global.LoginID].ToString();

            //삭제파일 상태 업데이트

            if (!string.IsNullOrEmpty(deleteFileSns.Trim()))
            {
                foreach (var deleteFileSn in deleteFileSns.Split(',').AsEnumerable())
                {
                    scForm.ScFormFiles.Select(fi => fi.ScFileInfo).Where(fi => fi.FileSn == int.Parse(deleteFileSn)).FirstOrDefault().Status = "D";
                }
            }

            //첨부파일
            if (files != null)
            {
                var fileHelper = new FileHelper();
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var savedFileName = fileHelper.GetUploadFileName(file);

                        var subDirectoryPath = Path.Combine(FileType.Manual.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                        var savedFilePath = Path.Combine(subDirectoryPath, savedFileName);

                        var scFileInfo = new ScFileInfo { FileNm = Path.GetFileName(file.FileName), FilePath = savedFilePath, Status = "N", RegId = Session[Global.LoginID].ToString(), RegDt = DateTime.Now };

                        var scFormFile = new ScFormFile { ScFileInfo = scFileInfo };
                        scForm.ScFormFiles.Add(scFormFile);

                        await fileHelper.UploadFile(file, subDirectoryPath, savedFileName);
                    }
                }
            }

            //저장
            await _scFormService.ModifyScFormAsync(scForm);

            return RedirectToAction("Manual", "Cs");
        }
        #endregion



        public void DownloadManualFile()
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

        public async Task DownloadManualFileMulti()
        {
            string formSn = Request.QueryString["FormSn"];

            string archiveName = "download.zip";

            //Eager Loading 방식
            var listScFormFile = await _scFormFileService.GetFormFilesByIdAsync(int.Parse(formSn));

            var listScFileInfo = new List<ScFileInfo>();
            foreach (var scFormFile in listScFormFile)
            {
                listScFileInfo.Add(scFormFile.ScFileInfo);
            }

            var files = Mapper.Map<IList<FileContent>>(listScFileInfo);

            new FileHelper().DownloadFile(files, archiveName);

        }
    }
}