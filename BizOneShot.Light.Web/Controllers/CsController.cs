using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using PagedList;
using AutoMapper;
using System.Threading.Tasks;

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
        [HttpGet]
        public async Task<ActionResult> Notice()
        {

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

        [HttpGet]
        public  async Task<ActionResult> NoticeDetail(int noticeSn)
        {
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
    }
}