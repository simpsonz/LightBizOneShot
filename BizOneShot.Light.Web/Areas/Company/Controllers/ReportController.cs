using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Areas.Company.Controllers
{
    [UserAuthorize(Order = 1)]
    [MenuAuthorize(Roles = UserType.Company, Order = 2)]
    public class ReportController : Controller
    {
        private readonly IQuesMasterService _quesMasterService;

        public ReportController(IQuesMasterService _quesMasterService)
        {
            this._quesMasterService = _quesMasterService;
        }

        // GET: Company/Report
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> BasicSurvey()
        {
            ViewBag.LeftMenu = Global.Report;

            //문진표 List Data
            var listQuesMaster = await _quesMasterService.GetQuesMastersAsync(Session[Global.CompRegistrationNo].ToString());


            var questionDropDown =
                Mapper.Map<List<QuestionDropDownModel>>(listQuesMaster);

            //사업담당자 일 경우 담당 사업만 조회
            QuestionDropDownModel title = new QuestionDropDownModel();
            title.QuestionSn = 0;
            title.BasicYear = DateTime.Now.Year;
            title.Status = "P";
            questionDropDown.Insert(0, title);

            SelectList questionList = new SelectList(questionDropDown, "Status", "BasicYear");

            ViewBag.SelectQuestionList = questionList;


            return View();
        }

        public async Task<ActionResult> Summary01(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if(!string.IsNullOrEmpty(questionSn))
            { 
                var quesMaster = await _quesMasterService.GetQuesMasterAsync(int.Parse(questionSn));
                var quesWriter = quesMaster.QuesWriter;
                var quesMasterView = Mapper.Map<QuesMasterViewModel>(quesMaster);
                var quesWriterView = Mapper.Map<QuesWriterViewModel>(quesWriter);
                quesMasterView.QuesWriter = quesWriterView;

                return View(quesMasterView);
            }
            else
            { 
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Summary01(QuesMasterViewModel quesMasterViewModel)
        {
            ViewBag.LeftMenu = Global.Report;
            int questionSn = quesMasterViewModel.QuestionSn;

            if (quesMasterViewModel.QuestionSn > 0)
            {
                var quesMaster = await _quesMasterService.GetQuesMasterAsync(quesMasterViewModel.QuestionSn);

                quesMaster.QuesWriter.CompNm = quesMasterViewModel.QuesWriter.CompNm;
                quesMaster.QuesWriter.Name = quesMasterViewModel.QuesWriter.Name;
                quesMaster.QuesWriter.Position = quesMasterViewModel.QuesWriter.Position;
                quesMaster.QuesWriter.TelNo = quesMasterViewModel.QuesWriter.TelNo;
                quesMaster.QuesWriter.Email = quesMasterViewModel.QuesWriter.Email;
                quesMaster.QuesWriter.UpdDt = DateTime.Now;
                quesMaster.QuesWriter.UpdId = Session[Global.LoginID].ToString();

                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                var quesMaster = new QuesMaster();
                quesMaster.BasicYear = DateTime.Now.Year;
                quesMaster.RegistrationNo = Session[Global.CompRegistrationNo].ToString();
                quesMaster.SaveStatus = "1";
                quesMaster.Status = "P";

                var quesWriter = Mapper.Map<QuesWriter>(quesMasterViewModel.QuesWriter);
                quesWriter.RegDt = DateTime.Now;
                quesWriter.RegId = Session[Global.LoginID].ToString();
                quesMaster.QuesWriter = quesWriter;

                var saveQuesMaster = await _quesMasterService.AddQuesMasterAsync(quesMaster);
                questionSn = saveQuesMaster.QuestionSn;
            }

            if (quesMasterViewModel.SubmitType == "T")
            {
                //var quesMaster = await _quesMasterService.GetQuesMasterAsync(questionSn);
                //var quesWriter = quesMaster.QuesWriter;
                //var quesMasterView = Mapper.Map<QuesMasterViewModel>(quesMaster);
                //var quesWriterView = Mapper.Map<QuesWriterViewModel>(quesWriter);
                //quesMasterView.QuesWriter = quesWriterView;

                //return View(quesMasterView);
                return RedirectToAction("Summary01", "Report", new { @questionSn = questionSn });
            }
            else
            {
                return RedirectToAction("Summary02", "Report", new { @questionSn = questionSn });
            }
        }

        public ActionResult Summary02(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;
            return View();
        }

        public ActionResult FinanceMng()
        {
            ViewBag.LeftMenu = Global.Report;
            return View();
        }



        
    }
}