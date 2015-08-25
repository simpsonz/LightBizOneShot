using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;
using BizOneShot.Light.ViewModels;
using BizOneShot.Light.Services;
using PagedList;
using AutoMapper;

namespace BizOneShot.Light.Web.Controllers
{
    public class CsController : Controller
    {
        private readonly IScFaqService _scFaqService;

        public CsController(IScFaqService scFaqService)
        {
            this._scFaqService = scFaqService;
        }

        public CsController()
        {
            
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

            var faqVM = _scFaqService.GetFaqs();

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            return View(new StaticPagedList<FaqViewModel>(faqVM.ToPagedList(1, pagingSize), 1, pagingSize, faqVM.Count));
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

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<FaqViewModel>(faqs.ToPagedList(int.Parse(curPage), pagingSize), int.Parse(curPage), pagingSize, faqs.Count));
        }
    }
}