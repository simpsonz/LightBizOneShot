using System.Collections.Generic;
using System.Web.Mvc;
using BizOneShot.Light.Web.Models;
using PagedList;

namespace BizOneShot.Light.Web.Controllers
{
    public class CsController : Controller
    {
        // GET: Cs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Faq()
        {
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
    }
}