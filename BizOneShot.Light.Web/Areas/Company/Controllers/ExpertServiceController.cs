using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;

namespace BizOneShot.Light.Web.Areas.Company.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Company, Order = 2)]
    public class ExpertServiceController : Controller
    {
        private readonly IScReqDocService _scReqDocService;

        public ExpertServiceController(IScReqDocService _scReqDocService)
        {
            this._scReqDocService = _scReqDocService;
        }

        // GET: Company/ExpertService
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReqReceiveList(string expertType)
        {
            ViewBag.LeftMenu = Global.ExpertService;

            string receiverId = Session[Global.LoginID].ToString();

            
            return View();

            //동기화
        }
    }
}