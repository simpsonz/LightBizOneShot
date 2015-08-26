using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using PagedList;
using AutoMapper;

namespace BizOneShot.Light.Web.Controllers
{
    public class CsController : Controller
    {
        private readonly IScFaqService _scFaqService;
        private readonly IScNtcService _scNtcService;

        public CsController(IScFaqService scFaqService, IScNtcService scNtcServcie)
        {
            this._scFaqService = scFaqService;
            this._scNtcService = scNtcServcie;
        }

        public CsController()
        {

        }

        // GET: Cs
        public ActionResult Index()
        {
            return View();
        }

        #region FAQ 
        public ActionResult Faq()
        {

            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };

            ViewBag.SelectList = searchBy;

            var faqs = _scFaqService.GetFaqs();

            var faqViews =
               Mapper.Map<List<FaqViewModel>>(faqs);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<FaqViewModel>(faqViews.ToPagedList(1, pagingSize), 1, pagingSize, faqViews.Count));
        }

        [HttpPost]
        public ActionResult Faq(string SelectList, string Query, string curPage)
        {
            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };
            ViewBag.SelectList = searchBy;

            var faqs = _scFaqService.GetFaqs(SelectList, Query);

            var faqViews =
               Mapper.Map<List<FaqViewModel>>(faqs);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<FaqViewModel>(faqViews.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, faqViews.Count));
        }

        #endregion

        #region Notice(공지사항)
        public ActionResult Notice()
        {

            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };

            ViewBag.SelectList = searchBy;

            var listScNtc = _scNtcService.GetNotices();

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(listScNtc);

            return View(new StaticPagedList<NoticeViewModel>(noticeViews.ToPagedList(1, 1), 1, 1, noticeViews.Count));
        }

        [HttpPost]
        public ActionResult Notice(string SelectList, string Query, string curPage)
        {
            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };
            ViewBag.SelectList = searchBy;

            var listScNtc = _scNtcService.GetNotices(SelectList, Query);

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(listScNtc);

            return View(new StaticPagedList<NoticeViewModel>(noticeViews.ToPagedList(int.Parse(curPage), 10), int.Parse(curPage), 10, noticeViews.Count));
        }

        [HttpPost]
        public ActionResult NoticeDetail(int noticeSn)
        {
            var scNtc = _scNtcService.GetNoticeById(noticeSn);

            var noticeView =
                Mapper.Map<NoticeViewModel>(scNtc);

            return View(noticeView);
        }
        #endregion
    }
}