using System.Web.Mvc;
using BizOneShot.Light.Services;

namespace BizOneShot.Light.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IScUsrService _scUsrService;

        public AccountController(IScUsrService scUsrService)
        {
            this._scUsrService = scUsrService;
        }

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
            bool result = _scUsrService.ChkLoginId(LoginId);

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