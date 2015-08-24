using System.Web.Mvc;

namespace BizOneShot.Light.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult CompanyAgreement()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult CompanyJoin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DoLoginIdSelect(string LoginId)
        {
            bool result = true; // _memberService.GetLoginIdCheck(LoginId);

            if (result == true)
            {
                return Json(new { result = true });
            }
            else
            {
                return Json(new { result = false });
            }

        }
    }
}