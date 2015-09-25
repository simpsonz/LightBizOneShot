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
        private readonly IQuesCompInfoService _quesCompInfoService;
        private readonly IScUsrService _scUsrService;

        public ReportController(IQuesMasterService _quesMasterService, IQuesCompInfoService _quesCompInfoService, IScUsrService _scUsrService)
        {
            this._quesMasterService = _quesMasterService;
            this._scUsrService = _scUsrService;
            this._quesCompInfoService = _quesCompInfoService;
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

            if (questionDropDown.Count() == 0 || questionDropDown[0].BasicYear < DateTime.Now.Year)
            {
                //사업담당자 일 경우 담당 사업만 조회
                QuestionDropDownModel title = new QuestionDropDownModel();
                title.SnStatus = "#P";
                title.BasicYear = DateTime.Now.Year;
                questionDropDown.Insert(0, title);
            }

            SelectList questionList = new SelectList(questionDropDown, "SnStatus", "BasicYear");
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
                ScUsr scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());
                var quesMasterViewModel = new QuesMasterViewModel();
                var quesWriterViewModel = new QuesWriterViewModel();
                quesMasterViewModel.RegistrationNo = scUsr.ScCompInfo.RegistrationNo;
                quesWriterViewModel.CompNm = scUsr.ScCompInfo.CompNm;
                quesWriterViewModel.DeptNm = scUsr.DeptNm;
                quesWriterViewModel.Email = scUsr.Email;
                quesWriterViewModel.Name = scUsr.Name;
                quesMasterViewModel.QuesWriter = quesWriterViewModel;
                return View(quesMasterViewModel);
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

                if (quesMasterViewModel.SubmitType == "T")
                {
                    quesMaster.SaveStatus = "1";
                }
                else
                {
                    quesMaster.SaveStatus = "2";
                }

                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                var quesMaster = new QuesMaster();
                quesMaster.BasicYear = DateTime.Now.Year;
                quesMaster.RegistrationNo = Session[Global.CompRegistrationNo].ToString();
                quesMaster.Status = "P";
                if (quesMasterViewModel.SubmitType == "T")
                {
                    quesMaster.SaveStatus = "1";
                }
                else
                {
                    quesMaster.SaveStatus = "2";
                }

                var quesWriter = Mapper.Map<QuesWriter>(quesMasterViewModel.QuesWriter);
                quesWriter.RegDt = DateTime.Now;
                quesWriter.RegId = Session[Global.LoginID].ToString();
                quesMaster.QuesWriter = quesWriter;

                var saveQuesMaster = await _quesMasterService.AddQuesMasterAsync(quesMaster);
                questionSn = saveQuesMaster.QuestionSn;
            }

            if (quesMasterViewModel.SubmitType == "T")
            {
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
            var quesViewModel = new QuesViewModel();
            quesViewModel.QuestionSn = int.Parse(questionSn);
            return View(quesViewModel);
        }

        public async Task<ActionResult> CompanyInfo01(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if (string.IsNullOrEmpty(questionSn))
            {
                // 오류 처리해야함.
                return View();
            }

            var quesCompInfo = await _quesCompInfoService.GetQuesCompInfoAsync(int.Parse(questionSn));

            if (quesCompInfo == null)
            {
                ScUsr scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());
                var quesCompInfoViewModel = new QuesCompanyInfoViewModel();
                quesCompInfoViewModel.QuestionSn = int.Parse(questionSn);
                quesCompInfoViewModel.CompAddr = "(" + scUsr.ScCompInfo.PostNo + ") " + scUsr.ScCompInfo.Addr1 + " " + scUsr.ScCompInfo.Addr2;
                quesCompInfoViewModel.CompNm = scUsr.ScCompInfo.CompNm;
                quesCompInfoViewModel.TelNo = scUsr.ScCompInfo.TelNo;
                quesCompInfoViewModel.Name = scUsr.ScCompInfo.OwnNm;
                quesCompInfoViewModel.Email = scUsr.ScCompInfo.Email;
                quesCompInfoViewModel.RegistrationNo = scUsr.ScCompInfo.RegistrationNo;
                return View(quesCompInfoViewModel);
            }
            else
            {
                var quesCompInfoView = Mapper.Map<QuesCompanyInfoViewModel>(quesCompInfo);
                if (quesCompInfoView.PublishDt == "0001-01-01")
                    quesCompInfoView.PublishDt = null;
                return View(quesCompInfoView);
            }

        }

        [HttpPost]
        public async Task<ActionResult> CompanyInfo01(QuesCompanyInfoViewModel quesCompanyInfoViewModel)
        {
            ViewBag.LeftMenu = Global.Report;
            int questionSn = quesCompanyInfoViewModel.QuestionSn;

            if (quesCompanyInfoViewModel.QuestionSn > 0)
            {
                var quesMaster = await _quesMasterService.GetQuesCompInfoAsync(questionSn);

                if (quesCompanyInfoViewModel.SubmitType == "N")
                {
                    quesMaster.SaveStatus = "3";
                }

                var quesCompInfo = Mapper.Map<QuesCompInfo>(quesCompanyInfoViewModel);
                if (quesMaster.QuesCompInfo == null)
                {
                    quesCompInfo.RegDt = DateTime.Now;
                    quesCompInfo.RegId = Session[Global.LoginID].ToString();
                }
                else
                {
                    quesCompInfo.RegDt = quesMaster.QuesCompInfo.RegDt;
                    quesCompInfo.RegId = quesMaster.QuesCompInfo.RegId;
                    quesCompInfo.UpdDt = DateTime.Now;
                    quesCompInfo.UpdId = Session[Global.LoginID].ToString();
                }
                quesMaster.QuesCompInfo = quesCompInfo;
                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                //에러처리 필요
                return View(quesCompanyInfoViewModel);
            }

            if (quesCompanyInfoViewModel.SubmitType == "T")
            {
                return RedirectToAction("CompanyInfo01", "Report", new { @questionSn = questionSn });
            }
            else
            {
                return RedirectToAction("CompanyInfo02", "Report", new { @questionSn = questionSn });
            }
        }

        public async Task<ActionResult> CompanyInfo02(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if (string.IsNullOrEmpty(questionSn))
            {
                // 오류 처리해야함.
                return View();
            }

            var quesMaster = await _quesMasterService.GetQuesCompExtentionAsync(int.Parse(questionSn));

            if (quesMaster.QuesCompExtention == null)
            {
                ScUsr scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());
                var quesCompExtentionViewModel = new QuesCompExtentionViewModel();
                quesCompExtentionViewModel.QuestionSn = int.Parse(questionSn);
                quesCompExtentionViewModel.PresidentNm = scUsr.ScCompInfo.OwnNm;
                return View(quesCompExtentionViewModel);
            }
            else
            {
                var quesCompExtentionView = Mapper.Map<QuesCompExtentionViewModel>(quesMaster.QuesCompExtention);
                return View(quesCompExtentionView);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CompanyInfo02(QuesCompExtentionViewModel quesCompExtentionViewModel)
        {
            ViewBag.LeftMenu = Global.Report;
            int questionSn = quesCompExtentionViewModel.QuestionSn;

            if (quesCompExtentionViewModel.QuestionSn > 0)
            {
                var quesMaster = await _quesMasterService.GetQuesCompExtentionAsync(questionSn);

                if (quesCompExtentionViewModel.SubmitType == "N")
                {
                    quesMaster.SaveStatus = "4";
                }

                var quesCompExtention = Mapper.Map<QuesCompExtention>(quesCompExtentionViewModel);
                if (quesMaster.QuesCompExtention == null)
                {
                    quesCompExtention.RegDt = DateTime.Now;
                    quesCompExtention.RegId = Session[Global.LoginID].ToString();
                }
                else
                {
                    quesCompExtention.RegDt = quesMaster.QuesCompExtention.RegDt;
                    quesCompExtention.RegId = quesMaster.QuesCompExtention.RegId;
                    quesCompExtention.UpdDt = DateTime.Now;
                    quesCompExtention.UpdId = Session[Global.LoginID].ToString();
                }
                quesMaster.QuesCompExtention = quesCompExtention;
                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                //에러처리 필요
                return View(quesCompExtentionViewModel);
            }

            if (quesCompExtentionViewModel.SubmitType == "T")
            {
                return RedirectToAction("CompanyInfo02", "Report", new { @questionSn = questionSn });
            }
            else
            {
                return RedirectToAction("BizCheck01", "Report", new { @questionSn = questionSn });
            }
        }

        public async Task<ActionResult> BizCheck01(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if (string.IsNullOrEmpty(questionSn))
            {
                // 오류 처리해야함.
                return View();
            }

            var quesMaster = await _quesMasterService.GetQuesCompExtentionAsync(int.Parse(questionSn));

            if (quesMaster.QuesCompExtention == null)
            {
                ScUsr scUsr = await _scUsrService.SelectScUsr(Session[Global.LoginID].ToString());
                var quesCompExtentionViewModel = new QuesCompExtentionViewModel();
                quesCompExtentionViewModel.QuestionSn = int.Parse(questionSn);
                quesCompExtentionViewModel.PresidentNm = scUsr.ScCompInfo.OwnNm;
                return View(quesCompExtentionViewModel);
            }
            else
            {
                var quesCompExtentionView = Mapper.Map<QuesCompExtentionViewModel>(quesMaster.QuesCompExtention);
                return View(quesCompExtentionView);
            }
        }

        public ActionResult BizCheck02()
        {
            ViewBag.LeftMenu = Global.Report;
            return View();
        }

        public ActionResult BizCheck03()
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