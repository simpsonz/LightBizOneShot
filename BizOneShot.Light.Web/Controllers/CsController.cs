using System.Collections.Generic;
using System.Web.Mvc;
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

            var faqVM = _scFaqService.GetFaqs();

            return View(new StaticPagedList<FaqViewModel>(faqVM.ToPagedList(1, 10), 1, 10, faqVM.Count));
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

            return View(new StaticPagedList<FaqViewModel>(faqs.ToPagedList(1, 10), 1, 10, faqs.Count));
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

            var scNtc = _scNtcService.GetNotices();

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(scNtc);

            return View(new StaticPagedList<NoticeViewModel>(noticeViews.ToPagedList(1, 10), 1, 10, noticeViews.Count));
        }
        #endregion
    }
}