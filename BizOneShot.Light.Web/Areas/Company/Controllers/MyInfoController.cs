using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Security;


namespace BizOneShot.Light.Web.Areas.Company.Controllers
{
    [UserAuthorize(Order = 1)]
    public class MyInfoController : BaseController
    {
        private readonly IScUsrService _scUsrService;

        public MyInfoController(IScUsrService scUsrService)
        {
            this._scUsrService = scUsrService;
        }

        // GET: Company/MyInfo
        public ActionResult Index()
        {
            return View();
        }

        // GET: Company/MyInfo
        [MenuAuthorize(Roles = UserType.Company, Order = 2)]
        public async Task<ActionResult> MyInfo()
        {
            ViewBag.LeftMenu = Global.MyInfo;

            ScUsr scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());

            var myInfo =
               Mapper.Map<CompanyMyInfoViewModel>(scUsr);

            return View(myInfo);
        }

        

        
    }
}