using System.Collections.Generic;
using System.Web.Mvc;
using BizOneShot.Light.Web.ViewModels;
using PagedList;

namespace BizOneShot.Light.Web.Controllers
{
    public class CsController : Controller
    {
        private IScFaqService _scFaqService;

        public CsController(IScFaqService scFaqService)
        {
            this._scFaqService = scFaqService;
        }

        // GET: Cs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Faq()
        {
            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };

            ViewBag.SelectList = searchBy;

            List<FaqViewModel> faqs = new List<FaqViewModel>();

            FaqViewModel faq1 = new FaqViewModel();
            faq1.QCL_NM = "이용문의";
            faq1.QST_TXT = "질문1";
            faq1.ANS_TXT = "답변1";
            faqs.Add(faq1);

            FaqViewModel faq2 = new FaqViewModel();
            faq2.QCL_NM = "이용문의";
            faq2.QST_TXT = "질문2";
            faq2.ANS_TXT = "답변2";
            faqs.Add(faq2);


            return View(new StaticPagedList<FaqViewModel>((IList<FaqViewModel>)faqs, 1, 10, 2));
        }

        [HttpPost]
        public ActionResult Faq(string SelectList, string Query, string curPage)
        {

            var faqs = _scFaqService.GetFaqs(SelectList, Query);


            //var searchBy = new List<SelectListItem>(){
            //    new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
            //    new SelectListItem { Value = "1", Text = "제목" },
            //    new SelectListItem { Value = "2", Text = "내용" }
            //};

            //ViewBag.SelectList = searchBy;

            //List<FaqViewModel> faqs = new List<FaqViewModel>();

            //FaqViewModel faq1 = new FaqViewModel();
            //faq1.QCL_NM = "이용문의";
            //faq1.QST_TXT = "질문3";
            //faq1.ANS_TXT = "답변3";
            //faqs.Add(faq1);

            //FaqViewModel faq2 = new FaqViewModel();
            //faq2.QCL_NM = "이용문의";
            //faq2.QST_TXT = "질문4";
            //faq2.ANS_TXT = "답변4";
            //faqs.Add(faq2);


            return View(new StaticPagedList<FaqViewModel>((IList<FaqViewModel>)faqs.ToPagedList(1,10), 2, 10, 50));
        }
    }
}