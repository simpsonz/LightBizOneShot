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

namespace BizOneShot.Light.Web.Controllers
{
    public class CsController : Controller
    {
        private readonly IScFaqService _scFaqService;
        private readonly IScNtcService _scNtcService;
        private readonly IScFormService _scFormService;

        public CsController(IScFaqService scFaqService, IScNtcService scNtcServcie, IScFormService scFormService)
        {
            this._scFaqService = scFaqService;
            this._scNtcService = scNtcServcie;
            this._scFormService = scFormService;
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

            var faqs = await _scFaqService.GetFaqsAsync();

            var faqViews =
               Mapper.Map<List<FaqViewModel>>(faqs);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<FaqViewModel>(faqViews.ToPagedList(1, pagingSize), 1, pagingSize, faqViews.Count));
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

            var faqs = await _scFaqService.GetFaqsAsync(SelectList, Query);

            var faqViews =
               Mapper.Map<List<FaqViewModel>>(faqs);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<FaqViewModel>(faqViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, faqViews.Count));
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

            //var listScNtc = _scNtcService.GetNotices();
            var listScNtc = await _scNtcService.GetNoticesAsync();

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(listScNtc);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<NoticeViewModel>(noticeViews.ToPagedList(1, pagingSize), 1, pagingSize, noticeViews.Count));
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

            //var listScNtc = _scNtcService.GetNotices(SelectList, Query);
            var listScNtc = await _scNtcService.GetNoticesAsync(SelectList, Query);

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(listScNtc);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<NoticeViewModel>(noticeViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, noticeViews.Count));
        }

        public  async Task<ActionResult> NoticeDetail(int noticeSn)
        {
            ViewBag.LeftMenu = Global.Cs;
            //var dicScNtc = _scNtcService.GetNoticeDetailById(noticeSn);
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

            return  View(noticeDetailView);
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


            var listScForm = await _scFormService.GetManualsAsync();

            var manualViews =
                Mapper.Map<List<ManualViewModel>>(listScForm);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<ManualViewModel>(manualViews.ToPagedList(1, pagingSize), 1, pagingSize, manualViews.Count));
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


            var listScForm = await _scFormService.GetManualsAsync(SelectList, Query);

            var manualViews =
                Mapper.Map<List<ManualViewModel>>(listScForm);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<ManualViewModel>(manualViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, manualViews.Count));
        }
        #endregion
    }
}