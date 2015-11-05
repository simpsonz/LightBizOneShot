using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;
using System.Threading.Tasks;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using PagedList;
using AutoMapper;

namespace BizOneShot.Light.Web.Areas.Company.Controllers
{
    [UserAuthorize(Order = 1)]
    public class MainController : BaseController
    {
        private readonly IScNtcService _scNtcService;

        public MainController(IScNtcService scNtcServcie)
        {
            this._scNtcService = scNtcServcie;

        }
        // GET: Company/Main
        [MenuAuthorize(Roles = UserType.Company, Order = 2)]
        public async Task<ActionResult> Index()
        {
            //var listScNtc = _scNtcService.GetNotices();
            var listScNtc = await _scNtcService.GetNoticesForMainAsync();

            var noticeViews =
                Mapper.Map<List<NoticeViewModel>>(listScNtc);
            return View(noticeViews);
        }
    }
}