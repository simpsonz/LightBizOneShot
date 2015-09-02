using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Areas.SysManager.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
    public class ExpertManagerController : BaseController
    {
        // GET: SysManager/ExpertManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExpertManager()
        {
            ViewBag.LeftMenu = Global.ExpertMng;

            var searchBy = new List<SelectListItem>(){
                new SelectListItem { Value = "0", Text = "제목 + 내용", Selected = true },
                new SelectListItem { Value = "1", Text = "제목" },
                new SelectListItem { Value = "2", Text = "내용" }
            };

            ViewBag.SelectList = searchBy;


            return View();
        }
    }
}