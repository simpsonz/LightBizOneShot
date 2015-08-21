using System.Collections.Generic;
using System.Web.Mvc;
using BizOneShot.Light.Web.ViewModels;
using BizOneShot.Light.Services;
using PagedList;

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
            var faqs = _scFaqService.GetFaqs();
            return View(new StaticPagedList<FaqViewModel>((IList<FaqViewModel>)faqs.ToPagedList(1, 10), 1, 10, faqs.Count));
        }

        [HttpPost]
        public ActionResult Faq(string SelectList, string Query, string curPage)
        {

            var faqs = _scFaqService.GetFaqs(SelectList, Query);
            return View(new StaticPagedList<FaqViewModel>((IList<FaqViewModel>)faqs.ToPagedList(1, 10), 1, 10, faqs.Count));
        }
    }
}