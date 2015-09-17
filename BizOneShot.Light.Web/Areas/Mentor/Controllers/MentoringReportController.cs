using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using System.IO;
using BizOneShot.Light.Web.ComLib;
using BizOneShot.Light.Models.ViewModels;
using System.Threading.Tasks;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Util.Helper;
using BizOneShot.Light.Util.Security;
using PagedList;
using AutoMapper;

namespace BizOneShot.Light.Web.Areas.Mentor.Controllers
{
   

    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Mentor, Order = 2)]
    public class MentoringReportController : BaseController
    {
        private readonly IScCompMappingService _scCompMappingService;
        private readonly IScMentorMappingService _scMentorMappingService;
        private readonly IScMentoringTotalReportService _scMentoringTotalReportService;

        public MentoringReportController(IScCompMappingService scCompMappingService, IScMentorMappingService scMentorMappingService, IScMentoringTotalReportService scMentoringTotalReportService)
        {
            this._scCompMappingService = scCompMappingService;
            this._scMentorMappingService = scMentorMappingService;
            this._scMentoringTotalReportService = scMentoringTotalReportService;
        }

        public async Task<ActionResult> MentoringTotalReportList(SelectedMentorTotalReportParmModel param)
        {
            ViewBag.LeftMenu = Global.MentoringReport;

            var mentorId = Session[Global.LoginID].ToString();

            //사업 DropDown List Data
            var listScMentorMapping = await _scMentorMappingService.GetMentorMappingListByMentorId(mentorId);
            var listScBizWork = listScMentorMapping.Select(mmp => mmp.ScBizWork);//.ToList();

            var bizWorkDropDown =
                Mapper.Map<List<BizWorkDropDownModel>>(listScBizWork);

            //사업드롭다운 타이틀 추가
            BizWorkDropDownModel titleBizWork = new BizWorkDropDownModel
            {
                BizWorkSn = 0,
                BizWorkNm = "사업명 선택"
            };
            
            bizWorkDropDown.Insert(0, titleBizWork);
          
            SelectList bizList = new SelectList(bizWorkDropDown, "BizWorkSn", "BizWorkNm");

            ViewBag.SelectBizWorkList = bizList;


            //기업 DropDwon List Data
            var listScCompMapping = await _scCompMappingService.GetCompMappingListByMentorId(mentorId, "A");
            var listScCompInfo = listScCompMapping.Select(cmp => cmp.ScCompInfo);//.ToList();

            var compInfoDropDown =
                Mapper.Map<List<CompInfoDropDownModel>>(listScCompInfo);

            //기업 드롭다운 타이틀 추가
            CompInfoDropDownModel titleCompInfo = new CompInfoDropDownModel
            {
                CompSn = 0,
                CompNm="기업명 선택"
            };

            compInfoDropDown.Insert(0, titleCompInfo);

            SelectList compInfoList = new SelectList(compInfoDropDown, "CompSn", "CompNm");

            ViewBag.SelectCompInfoList = compInfoList;

            //제출년도 DownDown List Data
            var listSubmitDt = await _scMentoringTotalReportService.GetMentoringTotalReportSubmitDt(mentorId);

            var submitDtDropDown = new List<SubmitDtDropDownModel>();

            foreach (var submitDt in listSubmitDt.AsEnumerable())
            {
                //SubmitDtDropDownModel submitModel = new SubmitDtDropDownModel
                //{
                //    SubmitDt = submitDt,
                //    SubmitYear = submitDt.ToString()
                //};

                submitDtDropDown.Add(
                    new SubmitDtDropDownModel
                    {
                        SubmitDt = submitDt,
                        SubmitYear = submitDt.ToString()
                    });
            }


            SubmitDtDropDownModel titleSubmitDt = new SubmitDtDropDownModel
            {
                SubmitDt = 0,
                SubmitYear = "제출일 선택"
            };

            submitDtDropDown.Insert(0, titleSubmitDt);

            SelectList submitDtList = new SelectList(submitDtDropDown, "SubmitDt", "SubmitYear");

            ViewBag.SelectSubmitList = submitDtList;


            //실제 쿼리
            var listTotalReport = await _scMentoringTotalReportService.GetMentoringTotalReportAsync(mentorId, param.SubmitDt, param.BizWorkSn, param.CompSn);


            //var usrViews =
            //   Mapper.Map<List<CompanyMngViewModel>>(listCompany);

            //int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);
            //return View(new StaticPagedList<CompanyMngViewModel>(listTotalReport.ToPagedList(1, pagingSize), 1, pagingSize, usrViews.Count));

            return View(listTotalReport);
        }
    }
}