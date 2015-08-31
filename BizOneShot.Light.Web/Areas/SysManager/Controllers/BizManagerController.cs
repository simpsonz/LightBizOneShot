using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using System.Threading.Tasks;
using AutoMapper;
using System.Configuration;
using PagedList;

namespace BizOneShot.Light.Web.Areas.SysManager.Controllers
{
    [UserAuthorize(Order = 1)]
    public class BizManagerController : Controller
    {
        private readonly IScUsrService _scUsrService;

        public BizManagerController(IScUsrService scUsrService)
        {
            this._scUsrService = scUsrService;
        }

        // GET: SysManager/BizManager
        public ActionResult Index()
        {
            return View();
        }

        [MenuAuthorize(Roles = UserType.SysManager, Order = 2)]
        public async Task<ActionResult> BizManager()
        {
            ViewBag.LeftMenu = Global.BizMng;

            //var listScNtc = _scNtcService.GetNotices(SelectList, Query);
            var listScUsr = await _scUsrService.GetBizManagerAsync();

            var usrViews =
                Mapper.Map<List<CompanyMyInfoViewModel>>(listScUsr);

            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            return View(new StaticPagedList<CompanyMyInfoViewModel>(usrViews.ToPagedList(1, pagingSize), 1, pagingSize, usrViews.Count));
        }
    }
}