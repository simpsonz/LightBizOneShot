using System.Web.Mvc;
using BizOneShot.Light.Services;
using BizOneShot.Light.Models.ViewModels;

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
        [ValidateAntiForgeryToken]
        public ActionResult CompanyJoin(JoinCompanyViewModel joinCompanyViewModel)
        {
            if (ModelState.IsValid)
            {

            }
                // 이 경우 오류가 발생한 것이므로 폼을 다시 표시하십시오.
            return View(joinCompanyViewModel);
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