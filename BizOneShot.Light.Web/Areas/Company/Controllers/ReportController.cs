﻿using System;
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
    public class ReportController : BaseController
    {
        private readonly IQuesMasterService _quesMasterService;
        private readonly IQuesCompInfoService _quesCompInfoService;
        private readonly IQuesCheckListService _quesCheckListService;
        private readonly IQuesResult1Service _quesResult1Service;
        private readonly IQuesResult2Service _quesResult2Service;
        private readonly IScUsrService _scUsrService;

        public ReportController(IQuesMasterService _quesMasterService, IQuesCompInfoService _quesCompInfoService, IScUsrService _scUsrService, IQuesCheckListService _quesCheckListService, IQuesResult1Service _quesResult1Service, IQuesResult2Service _quesResult2Service)
        {
            this._quesMasterService = _quesMasterService;
            this._scUsrService = _scUsrService;
            this._quesCompInfoService = _quesCompInfoService;
            this._quesCheckListService = _quesCheckListService;
            this._quesResult1Service = _quesResult1Service;
            this._quesResult2Service = _quesResult2Service;
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
                    quesMaster.SaveStatus = 1;
                }
                else
                {
                    quesMaster.SaveStatus = 2;
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
                    quesMaster.SaveStatus = 1;
                }
                else
                {
                    quesMaster.SaveStatus = 2;
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
                    quesMaster.SaveStatus = 3;
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
                    quesMaster.SaveStatus = 4;
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

        public ActionResult BizCheck01(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            var viewModel = new QuesViewModel();
            viewModel.QuestionSn = int.Parse(questionSn);
            return View(viewModel);
        }

        public async Task<ActionResult> BizCheck02(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if (string.IsNullOrEmpty(questionSn))
            {
                // 오류 처리해야함.
                return View();
            }

            var bizCheck02 = new BizCheck02ViewModel();

            // A1A101 : 경영목표 및 전략 코드
            var quesResult1sPurpose = await _quesResult1Service.GetQuesResult1sAsync(int.Parse(questionSn), "A1A101");

            if (quesResult1sPurpose.Count() != 5)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1A101");
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesCheckList);

                foreach (var item in quesCheckListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.AnsVal = false;
                }

                bizCheck02.BizPurpose = quesCheckListView;
            }
            else
            {
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesResult1sPurpose);
                bizCheck02.BizPurpose = quesCheckListView;
            }

            // A1A102 : 경영자의 리더쉽 코드
            var quesResult1sLeadership = await _quesResult1Service.GetQuesResult1sAsync(int.Parse(questionSn), "A1A102");

            if (quesResult1sLeadership.Count() != 5)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1A102");
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesCheckList);

                foreach (var item in quesCheckListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.AnsVal = false;
                }

                bizCheck02.Leadership = quesCheckListView;
            }
            else
            {
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesResult1sLeadership);
                bizCheck02.Leadership = quesCheckListView;
            }

            bizCheck02.QuestionSn = int.Parse(questionSn);
            return View(bizCheck02);
        }

        [HttpPost]
        public async Task<ActionResult> BizCheck02(BizCheck02ViewModel bizCheck02ViewModel)
        {
            ViewBag.LeftMenu = Global.Report;
            int questionSn = bizCheck02ViewModel.QuestionSn;

            if (bizCheck02ViewModel.QuestionSn > 0)
            {
                var quesMaster = await _quesMasterService.GetQuesResult1Async(questionSn);

                if (bizCheck02ViewModel.SubmitType == "N")
                {
                    quesMaster.SaveStatus = 5;
                }

                foreach(var item in bizCheck02ViewModel.BizPurpose)
                {
                    var checkItem = quesMaster.QuesResult1.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == item.CheckListSn);
                    if(checkItem == null)
                    {
                        var result1 = new QuesResult1();
                        result1.QuestionSn = questionSn;
                        result1.CheckListSn = item.CheckListSn;
                        result1.AnsVal = item.AnsVal;
                        result1.RegDt = DateTime.Now;
                        result1.RegId = Session[Global.LoginID].ToString();
                        quesMaster.QuesResult1.Add(result1);
                    }
                    else
                    {
                        checkItem.AnsVal = item.AnsVal;
                        checkItem.UpdDt = DateTime.Now;
                        checkItem.UpdId = Session[Global.LoginID].ToString();
                    }
                }

                foreach (var item in bizCheck02ViewModel.Leadership)
                {
                    var checkItem = quesMaster.QuesResult1.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == item.CheckListSn);
                    if (checkItem == null)
                    {
                        var result1 = new QuesResult1();
                        result1.QuestionSn = questionSn;
                        result1.CheckListSn = item.CheckListSn;
                        result1.AnsVal = item.AnsVal;
                        result1.RegDt = DateTime.Now;
                        result1.RegId = Session[Global.LoginID].ToString();
                        quesMaster.QuesResult1.Add(result1);
                    }
                    else
                    {
                        checkItem.AnsVal = item.AnsVal;
                        checkItem.UpdDt = DateTime.Now;
                        checkItem.UpdId = Session[Global.LoginID].ToString();
                    }
                }

                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                //에러처리 필요
                return View(bizCheck02ViewModel);
            }

            if (bizCheck02ViewModel.SubmitType == "T")
            {
                return RedirectToAction("BizCheck02", "Report", new { @questionSn = questionSn });
            }
            else
            {
                return RedirectToAction("BizCheck03", "Report", new { @questionSn = questionSn });
            }
        }


        public async Task<ActionResult> BizCheck03(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if (string.IsNullOrEmpty(questionSn))
            {
                // 오류 처리해야함.
                return View();
            }

            var bizCheck03 = new BizCheck03ViewModel();

            // A1A103 : 경영자의 신뢰성
            var quesResult1s = await _quesResult1Service.GetQuesResult1sAsync(int.Parse(questionSn), "A1A103");

            if (quesResult1s.Count() != 4)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1A103");
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesCheckList);

                foreach (var item in quesCheckListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.AnsVal = false;
                }

                bizCheck03.LeaderReliability = quesCheckListView;
            }
            else
            {
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesResult1s);
                bizCheck03.LeaderReliability = quesCheckListView;
            }

            bizCheck03.QuestionSn = int.Parse(questionSn);
            return View(bizCheck03);
        }

        [HttpPost]
        public async Task<ActionResult> BizCheck03(BizCheck03ViewModel bizCheck03ViewModel)
        {
            ViewBag.LeftMenu = Global.Report;
            int questionSn = bizCheck03ViewModel.QuestionSn;

            if (bizCheck03ViewModel.QuestionSn > 0)
            {
                var quesMaster = await _quesMasterService.GetQuesResult1Async(questionSn);

                if (bizCheck03ViewModel.SubmitType == "N")
                {
                    quesMaster.SaveStatus = 6;
                }

                foreach (var item in bizCheck03ViewModel.LeaderReliability)
                {
                    var checkItem = quesMaster.QuesResult1.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == item.CheckListSn);
                    if (checkItem == null)
                    {
                        var result1 = new QuesResult1();
                        result1.QuestionSn = questionSn;
                        result1.CheckListSn = item.CheckListSn;
                        result1.AnsVal = item.AnsVal;
                        result1.RegDt = DateTime.Now;
                        result1.RegId = Session[Global.LoginID].ToString();
                        quesMaster.QuesResult1.Add(result1);
                    }
                    else
                    {
                        checkItem.AnsVal = item.AnsVal;
                        checkItem.UpdDt = DateTime.Now;
                        checkItem.UpdId = Session[Global.LoginID].ToString();
                    }
                }

                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                //에러처리 필요
                return View(bizCheck03ViewModel);
            }

            if (bizCheck03ViewModel.SubmitType == "T")
            {
                return RedirectToAction("BizCheck03", "Report", new { @questionSn = questionSn });
            }
            else
            {
                return RedirectToAction("BizCheck04", "Report", new { @questionSn = questionSn });
            }
        }


        public async Task<ActionResult> BizCheck04(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if (string.IsNullOrEmpty(questionSn))
            {
                // 오류 처리해야함.
                return View();
            }

            var bizCheck04 = new BizCheck04ViewModel();

            // A1A201 : 근로환경
            var quesResult1s = await _quesResult1Service.GetQuesResult1sAsync(int.Parse(questionSn), "A1A201");

            if (quesResult1s.Count() != 6)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1A201");
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesCheckList);

                foreach (var item in quesCheckListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.AnsVal = false;
                }

                bizCheck04.WorkEnv = quesCheckListView;
            }
            else
            {
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesResult1s);
                bizCheck04.WorkEnv = quesCheckListView;
            }

            // A1A202 : 조직만족도
            var quesResult2s = await _quesResult2Service.GetQuesResult2sAsync(int.Parse(questionSn), "A1A202");

            if (quesResult2s.Count() != 3)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1A202");
                var quesYearListView = Mapper.Map<List<QuesYearListViewModel>>(quesCheckList);

                foreach (var item in quesYearListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.BasicYear = DateTime.Now.Year;
                    item.D = "0";
                    item.D451 = "0";
                    item.D452 = "0";
                    item.D453 = "0";
                }
                //총직원
                bizCheck04.TotalEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1A20201");
                //이직직원
                bizCheck04.MoveEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1A20202");
                //신규직원
                bizCheck04.NewEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1A20203");
            }
            else
            {
                var quesYearListView = Mapper.Map<List<QuesYearListViewModel>>(quesResult2s);
                bizCheck04.TotalEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1A20201");
                //이직직원
                bizCheck04.MoveEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1A20202");
                //신규직원
                bizCheck04.NewEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1A20203");
            }

            bizCheck04.QuestionSn = int.Parse(questionSn);
            return View(bizCheck04);
        }

        [HttpPost]
        public async Task<ActionResult> BizCheck04(BizCheck04ViewModel bizCheck04ViewModel)
        {
            ViewBag.LeftMenu = Global.Report;
            int questionSn = bizCheck04ViewModel.QuestionSn;

            if (bizCheck04ViewModel.QuestionSn > 0)
            {
                var quesMaster = await _quesMasterService.GetQuesResult1Async(questionSn);

                if (bizCheck04ViewModel.SubmitType == "N")
                {
                    quesMaster.SaveStatus = 7;
                }

                //근로환경 저장 또는 업데이트 정보 설정
                foreach (var item in bizCheck04ViewModel.WorkEnv)
                {
                    var checkItem = quesMaster.QuesResult1.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == item.CheckListSn);
                    if (checkItem == null)
                    {
                        var result1 = new QuesResult1();
                        result1.QuestionSn = questionSn;
                        result1.CheckListSn = item.CheckListSn;
                        result1.AnsVal = item.AnsVal;
                        result1.RegDt = DateTime.Now;
                        result1.RegId = Session[Global.LoginID].ToString();
                        quesMaster.QuesResult1.Add(result1);
                    }
                    else
                    {
                        checkItem.AnsVal = item.AnsVal;
                        checkItem.UpdDt = DateTime.Now;
                        checkItem.UpdId = Session[Global.LoginID].ToString();
                    }
                }

                //조직만족도 저장 또는 업데이트 정보 설정
                var quesYearMaster = await _quesMasterService.GetQuesResult2Async(questionSn);

                //총직원
                var yearTotalItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck04ViewModel.TotalEmp.CheckListSn);
                if (yearTotalItem == null)
                {
                    var quesYearTotalEmp = Mapper.Map<QuesResult2>(bizCheck04ViewModel.TotalEmp);
                    quesYearTotalEmp.QuestionSn = questionSn;
                    quesYearTotalEmp.RegDt = DateTime.Now;
                    quesYearTotalEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearTotalEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearTotalEmp);
                }
                else
                {
                    yearTotalItem.D = bizCheck04ViewModel.TotalEmp.D;
                    yearTotalItem.D451 = bizCheck04ViewModel.TotalEmp.D451;
                    yearTotalItem.D452 = bizCheck04ViewModel.TotalEmp.D452;
                    yearTotalItem.D453 = bizCheck04ViewModel.TotalEmp.D453;
                    yearTotalItem.UpdDt = DateTime.Now;
                    yearTotalItem.UpdId = Session[Global.LoginID].ToString();
                }


                //이직직원
                var yearMoveItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck04ViewModel.MoveEmp.CheckListSn);
                if (yearMoveItem == null)
                {
                    var quesYearMoveEmp = Mapper.Map<QuesResult2>(bizCheck04ViewModel.MoveEmp);
                    quesYearMoveEmp.QuestionSn = questionSn;
                    quesYearMoveEmp.RegDt = DateTime.Now;
                    quesYearMoveEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearMoveEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearMoveEmp);
                }
                else
                {
                    yearMoveItem.D = bizCheck04ViewModel.MoveEmp.D;
                    yearMoveItem.D451 = bizCheck04ViewModel.MoveEmp.D451;
                    yearMoveItem.D452 = bizCheck04ViewModel.MoveEmp.D452;
                    yearMoveItem.D453 = bizCheck04ViewModel.MoveEmp.D453;
                    yearMoveItem.UpdDt = DateTime.Now;
                    yearMoveItem.UpdId = Session[Global.LoginID].ToString();
                }

                //신규직원
                var yearNewItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck04ViewModel.NewEmp.CheckListSn);
                if (yearNewItem == null)
                {
                    var quesYearNewEmp = Mapper.Map<QuesResult2>(bizCheck04ViewModel.NewEmp);
                    quesYearNewEmp.QuestionSn = questionSn;
                    quesYearNewEmp.RegDt = DateTime.Now;
                    quesYearNewEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearNewEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearNewEmp);
                }
                else
                {
                    yearNewItem.D = bizCheck04ViewModel.NewEmp.D;
                    yearNewItem.D451 = bizCheck04ViewModel.NewEmp.D451;
                    yearNewItem.D452 = bizCheck04ViewModel.NewEmp.D452;
                    yearNewItem.D453 = bizCheck04ViewModel.NewEmp.D453;
                    yearNewItem.UpdDt = DateTime.Now;
                    yearNewItem.UpdId = Session[Global.LoginID].ToString();
                }

                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                //에러처리 필요
                return View(bizCheck04ViewModel);
            }

            if (bizCheck04ViewModel.SubmitType == "T")
            {
                return RedirectToAction("BizCheck04", "Report", new { @questionSn = questionSn });
            }
            else
            {
                return RedirectToAction("BizCheck05", "Report", new { @questionSn = questionSn });
            }
        }


        public async Task<ActionResult> BizCheck05(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if (string.IsNullOrEmpty(questionSn))
            {
                // 오류 처리해야함.
                return View();
            }

            var bizCheck05 = new BizCheck05ViewModel();

            // A1A103 : 경영자의 신뢰성
            var quesResult1s = await _quesResult1Service.GetQuesResult1sAsync(int.Parse(questionSn), "A1A203");

            if (quesResult1s.Count() != 6)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1A203");
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesCheckList);

                foreach (var item in quesCheckListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.AnsVal = false;
                }

                bizCheck05.InfoSystem = quesCheckListView;
            }
            else
            {
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesResult1s);
                bizCheck05.InfoSystem = quesCheckListView;
            }

            bizCheck05.QuestionSn = int.Parse(questionSn);
            return View(bizCheck05);
        }

        [HttpPost]
        public async Task<ActionResult> BizCheck05(BizCheck05ViewModel bizCheck05ViewModel)
        {
            ViewBag.LeftMenu = Global.Report;
            int questionSn = bizCheck05ViewModel.QuestionSn;

            if (bizCheck05ViewModel.QuestionSn > 0)
            {
                var quesMaster = await _quesMasterService.GetQuesResult1Async(questionSn);

                if (bizCheck05ViewModel.SubmitType == "N")
                {
                    quesMaster.SaveStatus = 8;
                }

                foreach (var item in bizCheck05ViewModel.InfoSystem)
                {
                    var checkItem = quesMaster.QuesResult1.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == item.CheckListSn);
                    if (checkItem == null)
                    {
                        var result1 = new QuesResult1();
                        result1.QuestionSn = questionSn;
                        result1.CheckListSn = item.CheckListSn;
                        result1.AnsVal = item.AnsVal;
                        result1.RegDt = DateTime.Now;
                        result1.RegId = Session[Global.LoginID].ToString();
                        quesMaster.QuesResult1.Add(result1);
                    }
                    else
                    {
                        checkItem.AnsVal = item.AnsVal;
                        checkItem.UpdDt = DateTime.Now;
                        checkItem.UpdId = Session[Global.LoginID].ToString();
                    }
                }

                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                //에러처리 필요
                return View(bizCheck05ViewModel);
            }

            if (bizCheck05ViewModel.SubmitType == "T")
            {
                return RedirectToAction("BizCheck05", "Report", new { @questionSn = questionSn });
            }
            else
            {
                return RedirectToAction("BizCheck06", "Report", new { @questionSn = questionSn });
            }
        }


        public async Task<ActionResult> BizCheck06(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if (string.IsNullOrEmpty(questionSn))
            {
                // 오류 처리해야함.
                return View();
            }

            var bizCheck06 = new BizCheck06ViewModel();

            //인력의 비율
            var quesResult2sEmpRate = await _quesResult2Service.GetQuesResult2sAsync(int.Parse(questionSn), "A1B102");

            if (quesResult2sEmpRate.Count() != 2)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1B102");
                var quesYearListView = Mapper.Map<List<QuesYearListViewModel>>(quesCheckList);

                foreach (var item in quesYearListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.BasicYear = DateTime.Now.Year;
                    item.D = "0";
                    item.D451 = "0";
                    item.D452 = "0";
                    item.D453 = "0";
                }
                //전체임직원수
                bizCheck06.TotalEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10202");
                //연구개발인력
                bizCheck06.RndEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10201");
            }
            else
            {
                var quesYearListView = Mapper.Map<List<QuesYearListViewModel>>(quesResult2sEmpRate);
                //전체임직원수
                bizCheck06.TotalEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10202");
                //연구개발인력
                bizCheck06.RndEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10201");
            }

            //연구개발 인력의 능력
            var quesResult2sEmpCapa = await _quesResult2Service.GetQuesResult2sAsync(int.Parse(questionSn), "A1B103");

            if (quesResult2sEmpCapa.Count() != 5)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1B103");
                var quesYearListView = Mapper.Map<List<QuesYearListViewModel>>(quesCheckList);

                foreach (var item in quesYearListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.BasicYear = DateTime.Now.Year;
                    item.D = "0";
                    item.D451 = "0";
                    item.D452 = "0";
                    item.D453 = "0";
                }
                //박사급
                bizCheck06.DoctorEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10301");
                //석사급
                bizCheck06.MasterEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10302");
                //학사급
                bizCheck06.CollegeEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10303");
                //기능사급
                bizCheck06.TechEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10304");
                //고졸이하급
                bizCheck06.HighEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10305");
            }
            else
            {
                var quesYearListView = Mapper.Map<List<QuesYearListViewModel>>(quesResult2sEmpCapa);
                //박사급
                bizCheck06.DoctorEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10301");
                //석사급
                bizCheck06.MasterEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10302");
                //학사급
                bizCheck06.CollegeEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10303");
                //기능사급
                bizCheck06.TechEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10304");
                //고졸이하급
                bizCheck06.HighEmp = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10305");
            }

            bizCheck06.QuestionSn = int.Parse(questionSn);
            return View(bizCheck06);
        }

        [HttpPost]
        public async Task<ActionResult> BizCheck06(BizCheck06ViewModel bizCheck06ViewModel)
        {
            ViewBag.LeftMenu = Global.Report;
            int questionSn = bizCheck06ViewModel.QuestionSn;

            if (bizCheck06ViewModel.QuestionSn > 0)
            {
                //인력의 비율, 연구개발 인력의 능력 저장 또는 업데이트 정보 설정
                var quesYearMaster = await _quesMasterService.GetQuesResult2Async(questionSn);

                if (bizCheck06ViewModel.SubmitType == "N")
                {
                    quesYearMaster.SaveStatus = 9;
                }

                //연구개발 인력수
                var yearRndItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck06ViewModel.RndEmp.CheckListSn);
                if (yearRndItem == null)
                {
                    var quesYearRndEmp = Mapper.Map<QuesResult2>(bizCheck06ViewModel.RndEmp);
                    quesYearRndEmp.QuestionSn = questionSn;
                    quesYearRndEmp.RegDt = DateTime.Now;
                    quesYearRndEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearRndEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearRndEmp);
                }
                else
                {
                    yearRndItem.D = bizCheck06ViewModel.RndEmp.D;
                    yearRndItem.D451 = bizCheck06ViewModel.RndEmp.D451;
                    yearRndItem.D452 = bizCheck06ViewModel.RndEmp.D452;
                    yearRndItem.D453 = bizCheck06ViewModel.RndEmp.D453;
                    yearRndItem.UpdDt = DateTime.Now;
                    yearRndItem.UpdId = Session[Global.LoginID].ToString();
                }


                //전체임직원수
                var yearTotalItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck06ViewModel.TotalEmp.CheckListSn);
                if (yearTotalItem == null)
                {
                    var quesYearTotalEmp = Mapper.Map<QuesResult2>(bizCheck06ViewModel.TotalEmp);
                    quesYearTotalEmp.QuestionSn = questionSn;
                    quesYearTotalEmp.RegDt = DateTime.Now;
                    quesYearTotalEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearTotalEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearTotalEmp);
                }
                else
                {
                    yearTotalItem.D = bizCheck06ViewModel.TotalEmp.D;
                    yearTotalItem.D451 = bizCheck06ViewModel.TotalEmp.D451;
                    yearTotalItem.D452 = bizCheck06ViewModel.TotalEmp.D452;
                    yearTotalItem.D453 = bizCheck06ViewModel.TotalEmp.D453;
                    yearTotalItem.UpdDt = DateTime.Now;
                    yearTotalItem.UpdId = Session[Global.LoginID].ToString();
                }

                //박사급
                var yearDoctorItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck06ViewModel.DoctorEmp.CheckListSn);
                if (yearDoctorItem == null)
                {
                    var quesYearDoctorEmp = Mapper.Map<QuesResult2>(bizCheck06ViewModel.DoctorEmp);
                    quesYearDoctorEmp.QuestionSn = questionSn;
                    quesYearDoctorEmp.RegDt = DateTime.Now;
                    quesYearDoctorEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearDoctorEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearDoctorEmp);
                }
                else
                {
                    yearDoctorItem.D = bizCheck06ViewModel.DoctorEmp.D;
                    yearDoctorItem.UpdDt = DateTime.Now;
                    yearDoctorItem.UpdId = Session[Global.LoginID].ToString();
                }

                //석사급
                var yearMasterItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck06ViewModel.MasterEmp.CheckListSn);
                if (yearMasterItem == null)
                {
                    var quesYearMasterEmp = Mapper.Map<QuesResult2>(bizCheck06ViewModel.MasterEmp);
                    quesYearMasterEmp.QuestionSn = questionSn;
                    quesYearMasterEmp.RegDt = DateTime.Now;
                    quesYearMasterEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearMasterEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearMasterEmp);
                }
                else
                {
                    yearMasterItem.D = bizCheck06ViewModel.MasterEmp.D;
                    yearMasterItem.UpdDt = DateTime.Now;
                    yearMasterItem.UpdId = Session[Global.LoginID].ToString();
                }

                //학사급
                var yearCollegeItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck06ViewModel.CollegeEmp.CheckListSn);
                if (yearCollegeItem == null)
                {
                    var quesYearCollegeEmp = Mapper.Map<QuesResult2>(bizCheck06ViewModel.CollegeEmp);
                    quesYearCollegeEmp.QuestionSn = questionSn;
                    quesYearCollegeEmp.RegDt = DateTime.Now;
                    quesYearCollegeEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearCollegeEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearCollegeEmp);
                }
                else
                {
                    yearCollegeItem.D = bizCheck06ViewModel.CollegeEmp.D;
                    yearCollegeItem.UpdDt = DateTime.Now;
                    yearCollegeItem.UpdId = Session[Global.LoginID].ToString();
                }

                //기능사급
                var yearTechItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck06ViewModel.TechEmp.CheckListSn);
                if (yearTechItem == null)
                {
                    var quesYearTechEmp = Mapper.Map<QuesResult2>(bizCheck06ViewModel.TechEmp);
                    quesYearTechEmp.QuestionSn = questionSn;
                    quesYearTechEmp.RegDt = DateTime.Now;
                    quesYearTechEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearTechEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearTechEmp);
                }
                else
                {
                    yearTechItem.D = bizCheck06ViewModel.TechEmp.D;
                    yearTechItem.UpdDt = DateTime.Now;
                    yearTechItem.UpdId = Session[Global.LoginID].ToString();
                }

                //고졸이하
                var yearHighItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck06ViewModel.HighEmp.CheckListSn);
                if (yearHighItem == null)
                {
                    var quesYearHighEmp = Mapper.Map<QuesResult2>(bizCheck06ViewModel.HighEmp);
                    quesYearHighEmp.QuestionSn = questionSn;
                    quesYearHighEmp.RegDt = DateTime.Now;
                    quesYearHighEmp.RegId = Session[Global.LoginID].ToString();
                    quesYearHighEmp.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearHighEmp);
                }
                else
                {
                    yearHighItem.D = bizCheck06ViewModel.HighEmp.D;
                    yearHighItem.UpdDt = DateTime.Now;
                    yearHighItem.UpdId = Session[Global.LoginID].ToString();
                }

                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                //에러처리 필요
                return View(bizCheck06ViewModel);
            }

            if (bizCheck06ViewModel.SubmitType == "T")
            {
                return RedirectToAction("BizCheck06", "Report", new { @questionSn = questionSn });
            }
            else
            {
                return RedirectToAction("BizCheck07", "Report", new { @questionSn = questionSn });
            }
        }



        public async Task<ActionResult> BizCheck07(string questionSn)
        {
            ViewBag.LeftMenu = Global.Report;

            if (string.IsNullOrEmpty(questionSn))
            {
                // 오류 처리해야함.
                return View();
            }

            var bizCheck07 = new BizCheck07ViewModel();

            // A1B104 : 사업화역량
            var quesResult1s = await _quesResult1Service.GetQuesResult1sAsync(int.Parse(questionSn), "A1B104");

            if (quesResult1s.Count() != 5)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1B104");
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesCheckList);

                foreach (var item in quesCheckListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.AnsVal = false;
                }

                bizCheck07.BizCapa = quesCheckListView;
            }
            else
            {
                var quesCheckListView = Mapper.Map<List<QuesCheckListViewModel>>(quesResult1s);
                bizCheck07.BizCapa = quesCheckListView;
            }

            // A1A202 : 조직만족도
            var quesResult2s = await _quesResult2Service.GetQuesResult2sAsync(int.Parse(questionSn), "A1B105");

            if (quesResult2s.Count() != 2)
            {
                var quesCheckList = await _quesCheckListService.GetQuesCheckListAsync("A1B105");
                var quesYearListView = Mapper.Map<List<QuesYearListViewModel>>(quesCheckList);

                foreach (var item in quesYearListView)
                {
                    item.QuestionSn = int.Parse(questionSn);
                    item.BasicYear = DateTime.Now.Year;

                    if (item.DetailCd == "A1B10502")
                    { 
                        item.D = "0";
                        item.D451 = "0";
                        item.D452 = "0";
                        item.D453 = "0";
                    }
                }

                //사업화실적
                bizCheck07.BizResult = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10501");
                //사업화실적 총 건수
                bizCheck07.BizResultCnt = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10502");
            }
            else
            {
                var quesYearListView = Mapper.Map<List<QuesYearListViewModel>>(quesResult2s);
                //사업화실적
                bizCheck07.BizResult = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10501");
                //사업화실적 총 건수
                bizCheck07.BizResultCnt = quesYearListView.SingleOrDefault(i => i.DetailCd == "A1B10502");
            }

            bizCheck07.QuestionSn = int.Parse(questionSn);
            return View(bizCheck07);
        }

        [HttpPost]
        public async Task<ActionResult> BizCheck07(BizCheck07ViewModel bizCheck07ViewModel)
        {
            ViewBag.LeftMenu = Global.Report;
            int questionSn = bizCheck07ViewModel.QuestionSn;

            if (bizCheck07ViewModel.QuestionSn > 0)
            {
                var quesMaster = await _quesMasterService.GetQuesResult1Async(questionSn);

                if (bizCheck07ViewModel.SubmitType == "N")
                {
                    quesMaster.SaveStatus = 10;
                }

                //사업화 역량 저장 또는 업데이트 정보 설정
                foreach (var item in bizCheck07ViewModel.BizCapa)
                {
                    var checkItem = quesMaster.QuesResult1.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == item.CheckListSn);
                    if (checkItem == null)
                    {
                        var result1 = new QuesResult1();
                        result1.QuestionSn = questionSn;
                        result1.CheckListSn = item.CheckListSn;
                        result1.AnsVal = item.AnsVal;
                        result1.RegDt = DateTime.Now;
                        result1.RegId = Session[Global.LoginID].ToString();
                        quesMaster.QuesResult1.Add(result1);
                    }
                    else
                    {
                        checkItem.AnsVal = item.AnsVal;
                        checkItem.UpdDt = DateTime.Now;
                        checkItem.UpdId = Session[Global.LoginID].ToString();
                    }
                }

                //사업화 실적 저장 또는 업데이트 정보 설정
                var quesYearMaster = await _quesMasterService.GetQuesResult2Async(questionSn);

                //사업화실적
                var yearBizResultItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck07ViewModel.BizResult.CheckListSn);
                if (yearBizResultItem == null)
                {
                    var quesYearBizResult = Mapper.Map<QuesResult2>(bizCheck07ViewModel.BizResult);
                    quesYearBizResult.QuestionSn = questionSn;
                    quesYearBizResult.RegDt = DateTime.Now;
                    quesYearBizResult.RegId = Session[Global.LoginID].ToString();
                    quesYearBizResult.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearBizResult);
                }
                else
                {
                    yearBizResultItem.D = bizCheck07ViewModel.BizResult.D;
                    yearBizResultItem.D451 = bizCheck07ViewModel.BizResult.D451;
                    yearBizResultItem.D452 = bizCheck07ViewModel.BizResult.D452;
                    yearBizResultItem.D453 = bizCheck07ViewModel.BizResult.D453;
                    yearBizResultItem.UpdDt = DateTime.Now;
                    yearBizResultItem.UpdId = Session[Global.LoginID].ToString();
                }


                //사업화실적 총건수
                var yearBizResultCntItem = quesYearMaster.QuesResult2.SingleOrDefault(m => m.QuestionSn == questionSn && m.CheckListSn == bizCheck07ViewModel.BizResultCnt.CheckListSn);
                if (yearBizResultCntItem == null)
                {
                    var quesYearBizResultCnt = Mapper.Map<QuesResult2>(bizCheck07ViewModel.BizResultCnt);
                    quesYearBizResultCnt.QuestionSn = questionSn;
                    quesYearBizResultCnt.RegDt = DateTime.Now;
                    quesYearBizResultCnt.RegId = Session[Global.LoginID].ToString();
                    quesYearBizResultCnt.BasicYear = quesYearMaster.BasicYear;
                    quesYearMaster.QuesResult2.Add(quesYearBizResultCnt);
                }
                else
                {
                    yearBizResultCntItem.D = bizCheck07ViewModel.BizResultCnt.D;
                    yearBizResultCntItem.D451 = bizCheck07ViewModel.BizResultCnt.D451;
                    yearBizResultCntItem.D452 = bizCheck07ViewModel.BizResultCnt.D452;
                    yearBizResultCntItem.D453 = bizCheck07ViewModel.BizResultCnt.D453;
                    yearBizResultCntItem.UpdDt = DateTime.Now;
                    yearBizResultCntItem.UpdId = Session[Global.LoginID].ToString();
                }

                await _quesMasterService.SaveDbContextAsync();
            }
            else
            {
                //에러처리 필요
                return View(bizCheck07ViewModel);
            }

            if (bizCheck07ViewModel.SubmitType == "T")
            {
                return RedirectToAction("BizCheck07", "Report", new { @questionSn = questionSn });
            }
            else
            {
                return RedirectToAction("BizCheck08", "Report", new { @questionSn = questionSn });
            }
        }

        public ActionResult FinanceMng()
        {
            ViewBag.LeftMenu = Global.Report;
            return View();
        }



        
    }
}