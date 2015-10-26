using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Services;
using BizOneShot.Light.Web.ComLib;
using PagedList;
using AutoMapper;

namespace BizOneShot.Light.Web.Controllers
{
    [UserAuthorize(Order = 1)]
    public class BasicSurveyReportController : BaseController
    {
        
        private readonly IQuesCompInfoService quesCompInfoService;
        private readonly IQuesResult1Service quesResult1Service;
        private readonly IQuesResult2Service quesResult2Service;
        private readonly IQuesMasterService quesMasterService;
        private readonly IScCompMappingService scCompMappingService;
        private readonly IScBizWorkService scBizWorkService;
        private readonly IScMentorMappingService scMentorMappingService;
        private readonly IRptMasterService rptMasterService;
        private readonly IRptMentorCommentService rptMentorCommentService;
        private readonly IRptCheckListService rptCheckListService;
        public BasicSurveyReportController(
            IScCompMappingService scCompMappingService,
            IQuesCompInfoService quesCompInfoService,
            IScMentorMappingService scMentorMappingService,
            IRptCheckListService rptCheckListService,
            IRptMasterService rptMasterService,
            IRptMentorCommentService rptMentorCommentService,
            IQuesResult1Service quesResult1Service,
            IQuesResult2Service quesResult2Service,
            IQuesMasterService quesMasterService,
            IScBizWorkService scBizWorkService)
        {
            this.scCompMappingService = scCompMappingService;
            this.quesCompInfoService = quesCompInfoService;
            this.scMentorMappingService = scMentorMappingService;
            this.rptCheckListService = rptCheckListService;
            this.rptMasterService = rptMasterService;
            this.rptMentorCommentService = rptMentorCommentService;
            this.quesResult1Service = quesResult1Service;
            this.quesMasterService = quesMasterService;
            this.quesResult2Service = quesResult2Service;
            this.scBizWorkService = scBizWorkService;
        }

        // GET: BasicSurveyReport
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Cover(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            if (paramModel.CompSn == 0 || paramModel.BizWorkSn == 0)
            {
                return View(paramModel);
            }

            var scCompMapping = await scCompMappingService.GetCompMappingAsync(paramModel.BizWorkSn, paramModel.CompSn);

            paramModel.CompNm = scCompMapping.ScCompInfo.CompNm;
            paramModel.BizWorkNm = scCompMapping.ScBizWork.BizWorkNm;

            return View(paramModel);

        }

        public async Task<ActionResult> CompanyInfo(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            if(paramModel.Status == "T")
            {
                var rptMaster = await rptMasterService.GetRptMasterAsync(paramModel.QuestionSn, paramModel.CompSn, paramModel.BizWorkYear);
                rptMaster.Status = "P";
                await rptMasterService.SaveDbContextAsync();
            }

            var quesCompInfo = await quesCompInfoService.GetQuesCompInfoAsync(paramModel.QuestionSn);
            var quesCompInfoView = Mapper.Map<QuesCompanyInfoViewModel>(quesCompInfo);
            if (quesCompInfoView.PublishDt == "0001-01-01")
                quesCompInfoView.PublishDt = null;

            ViewBag.paramModel = paramModel;
            return View(quesCompInfoView);

        }


        public ActionResult OverallSummaryCover(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            return View(paramModel);

        }

               

        public ActionResult BasicSurveyCompanyList(string curPage)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;
            //사업년도 DownDown List Data
            ViewBag.SelectBizWorkYearList = ReportHelper.MakeYear(2015);
            
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(null);
            ViewBag.SelectCompInfoList = ReportHelper.MakeCompanyList(null);
            ViewBag.SelectStatusList = ReportHelper.MakeReportStatusList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BasicSurveyCompanyList(BasicSurveyReportViewModel paramModel, string curPage)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;
            //사업년도 DownDown List Data
            ViewBag.SelectBizWorkYearList = ReportHelper.MakeYear(2015);

            var mentorId = Session[Global.LoginID].ToString();
            if (string.IsNullOrEmpty(paramModel.Status))
                paramModel.Status = "";

            //사업 DropDown List Data
            var listScMentorMapping = await scMentorMappingService.GetMentorMappingListByMentorId(mentorId, paramModel.BizWorkYear);
            var listScBizWork = listScMentorMapping.Select(mmp => mmp.ScBizWork).ToList();
            ViewBag.SelectBizWorkList = ReportHelper.MakeBizWorkList(listScBizWork);

            var listScCompMapping = await scCompMappingService.GetCompMappingListByMentorId(mentorId, "A", paramModel.BizWorkSn, paramModel.BizWorkYear);
            var listScCompInfo = listScCompMapping.Select(cmp => cmp.ScCompInfo).ToList();
            ViewBag.SelectCompInfoList = ReportHelper.MakeCompanyList(listScCompInfo);
            ViewBag.SelectStatusList = ReportHelper.MakeReportStatusList();


            //기초역량 보고서 조회
            int pagingSize = int.Parse(ConfigurationManager.AppSettings["PagingSize"]);

            var rptMsters = rptMasterService.GetRptMasterList(int.Parse(curPage ?? "1"), pagingSize, mentorId, paramModel.BizWorkYear, paramModel.BizWorkSn, paramModel.CompSn, paramModel.Status);

            //뷰모델 맵핑
            var rptMasterListView = Mapper.Map<List<BasicSurveyReportViewModel>>(rptMsters.ToList());

            return View(new StaticPagedList<BasicSurveyReportViewModel>(rptMasterListView, int.Parse(curPage ?? "1"), pagingSize, rptMsters.TotalCount));

        }

        public async Task<ActionResult> OverallSummary(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            //double totalPoint = 0;
            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            OverallSummaryViewModel viewModel = new OverallSummaryViewModel();
            viewModel.CommentList = new List<CommentViewModel>();
            ReportUtil reportUtil = new ReportUtil(quesResult1Service, quesResult2Service, quesMasterService);


            //1. 경영역량총괄 전체평균
            //1) 종료된 모든 사업에 포함된 기업들의 사업기간내의 기초역량 점수 계산 
            // 확인사항 현재 사업이 장기간의 사업이라면 해당사업이 진행될 수록 사업전 사업은 작아짐. 확인 필요
            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);
            var endBizWorks = await scBizWorkService.GetEndBizWorkList(curBizWork.BizWorkStDt.Value);
            foreach(var bizWork in endBizWorks)
            {
                var compMappings = bizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    var quesMasters = await quesMasterService.GetQuesMastersAsync(compMapping.ScCompInfo.RegistrationNo);
                    if (quesMasters.Count == 0)
                    {
                        continue;
                    }
                    for (int i = bizWork.BizWorkStDt.Value.Year; i <= bizWork.BizWorkEdDt.Value.Year; i++)
                    {
                        if(dictionary.ContainsKey(compMapping.ScCompInfo.RegistrationNo + i.ToString()).Equals(false))
                        {
                            var quesMaster = quesMasters.Where(qm => qm.BasicYear == i).SingleOrDefault();
                            if(quesMaster != null)
                            { 
                                dictionary.Add(compMapping.ScCompInfo.RegistrationNo + i.ToString(), await reportUtil.GetCompanyTotalPoint(quesMaster.QuestionSn));
                            }
                        }
                    }
                }
            }

            //2) 현재 사업에 참여한 업체 평균
            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    var quesMasters = await quesMasterService.GetQuesMastersAsync(compMapping.ScCompInfo.RegistrationNo);
                    if(quesMasters.Count == 0)
                    {
                        continue;
                    }
                    for (int i = curBizWork.BizWorkStDt.Value.Year; i <= curBizWork.BizWorkEdDt.Value.Year; i++)
                    {
                        if (dictionary.ContainsKey(compMapping.ScCompInfo.RegistrationNo + i.ToString()).Equals(false))
                        {
                            var quesMaster = quesMasters.Where(qm => qm.BasicYear == i).SingleOrDefault();
                            if (quesMaster != null)
                            {
                                dictionary.Add(compMapping.ScCompInfo.RegistrationNo + i.ToString(), await reportUtil.GetCompanyTotalPoint(quesMaster.QuestionSn));
                            }
                        }
                    }
                }
            }

            // 3) 기초자료 전체 평균
            // 설명자료에 해당 내용 없음.


            // 4) 전체 평균정수 계산
            double totalPoint = 0;
            foreach(var item in dictionary.Values)
            {
                totalPoint = totalPoint + item;
            }

            totalPoint = totalPoint  / dictionary.Count;
            viewModel.AvgTotalPoint = Math.Round(totalPoint, 1);

            //2. 해당 기업의 기초역량 점수 계산
            double companyPoint = await reportUtil.GetCompanyTotalPoint(paramModel.QuestionSn);
            viewModel.CompanyPoint = Math.Round(companyPoint, 1);

            //3. 경영역량 총괄 화살표
            viewModel.BizCapaType = ReportHelper.GetArrowTypeA(companyPoint);

            #region 주석 추후 제거 필요

            ////경영목표 및 전략
            //// A1A101 : 경영목표 및 전략 코드
            //var quesResult1sPurpose = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1A101");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeA(ReportHelper.CalcCheckCount(quesResult1sPurpose)), 0.5);


            //// A1A102 : 경영자의 리더쉽 코드
            //var quesResult1sLeadership = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1A102");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeA(ReportHelper.CalcCheckCount(quesResult1sLeadership)), 0.5);


            //// 조직구성 조회
            //var quesMaster = await quesMasterService.GetQuesOgranAnalysisAsync(paramModel.QuestionSn);
            //int officerCnt = 0;
            //foreach(var item in quesMaster.QuesOgranAnalysis)
            //{
            //    officerCnt = officerCnt + item.OfficerCount.Value;
            //}
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeB(officerCnt), 1);


            //// A1A103 : 경영자의 신뢰성
            //var quesResult1s = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1A103");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeC(ReportHelper.CalcCheckCount(quesResult1s)), 2);


            //// A1A201 : 근로환경
            //var quesResult1sWorkEnv = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1A201");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeA(ReportHelper.CalcCheckCount(quesResult1sWorkEnv)), 1);


            //// A1A202 : 조직만족도
            //var quesResult2s = await quesResult2Service.GetQuesResult2sAsync(paramModel.QuestionSn, "A1A202");
            ////총직원
            //var totalEmp = quesResult2s.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1A20201");
            ////이직직원
            //var moveEmp = quesResult2s.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1A20202");

            //if(totalEmp.D451 == "0")
            //{
            //    double avg = (int.Parse(moveEmp.D) / int.Parse(totalEmp.D)) * 100;
            //    totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeD(avg), 3);

            //}
            //else if(totalEmp.D452 == "0")
            //{
            //    double avg = ((int.Parse(moveEmp.D) / int.Parse(totalEmp.D)) + (int.Parse(moveEmp.D451) / int.Parse(totalEmp.D451))) / 2  * 100;
            //    totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeD(avg), 3);
            //}
            //else
            //{
            //    double avg = ((int.Parse(moveEmp.D) / int.Parse(totalEmp.D)) + (int.Parse(moveEmp.D451) / int.Parse(totalEmp.D451)) + (int.Parse(moveEmp.D452) / int.Parse(totalEmp.D452))) / 3 * 100;
            //    totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeD(avg), 3);
            //}


            //// A1A203 : 정보시스템 활용
            //var quesResult1sInfoSystem = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1A203");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeA(ReportHelper.CalcCheckCount(quesResult1sInfoSystem)), 2);


            //// 연구개발 투자
            //// 구현해야 함.(다래 DB 정리되면..............................)


            ////연구개발 인력의 비율
            //var quesResult2sEmpRate = await quesResult2Service.GetQuesResult2sAsync(paramModel.QuestionSn, "A1B102");
            ////전체임직원수
            //var TotalEmp = quesResult2sEmpRate.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10202");
            ////연구개발인력
            //var RndEmp = quesResult2sEmpRate.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10201");
            //if(int.Parse(RndEmp.D) != 0)
            //{
            //    double avg = (int.Parse(TotalEmp.D) / int.Parse(RndEmp.D)) * 100;
            //    totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeE(avg), 1);
            //}


            ////연구개발 인력의 능력
            //var quesResult2sEmpCapa = await quesResult2Service.GetQuesResult2sAsync(paramModel.QuestionSn, "A1B103");
            ////박사급
            //var DoctorEmp = quesResult2sEmpCapa.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10301");
            ////석사급
            //var MasterEmp = quesResult2sEmpCapa.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10302");
            ////학사급
            //var CollegeEmp = quesResult2sEmpCapa.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10303");
            ////기능사급
            //var TechEmp = quesResult2sEmpCapa.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10304");
            ////고졸이하급
            //var HighEmp = quesResult2sEmpCapa.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10305");
            //if((int.Parse(DoctorEmp.D) + int.Parse(MasterEmp.D) + int.Parse(CollegeEmp.D) + int.Parse(TechEmp.D) + int.Parse(HighEmp.D)) != 0)
            //{
            //    double avg = (int.Parse(DoctorEmp.D) * 5) + (int.Parse(MasterEmp.D) * 4) + (int.Parse(CollegeEmp.D) * 3) + (int.Parse(TechEmp.D) * 2) + (int.Parse(HighEmp.D) * 1) / (int.Parse(DoctorEmp.D) + int.Parse(MasterEmp.D) + int.Parse(CollegeEmp.D) + int.Parse(TechEmp.D) + int.Parse(HighEmp.D));
            //    totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeF(avg), 3);
            //}


            //// A1B104 : 사업화역량
            //var quesResult1sBizCapa = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1B104");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeC(ReportHelper.CalcCheckCount(quesResult1sBizCapa)), 5);


            //// A1B105 : 사업화실적
            //var quesResult2sBizResult = await quesResult2Service.GetQuesResult2sAsync(paramModel.QuestionSn, "A1B105");
            ////사업화실적 총 건수
            //var BizResultCnt = quesResult2sBizResult.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10502");
            //{
            //    double avg = int.Parse(BizResultCnt.D) + int.Parse(BizResultCnt.D451) + int.Parse(BizResultCnt.D452) / 3;
            //    totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeE(avg), 4);
            //}


            //// A1B106 : 생산설비의 운영체제 및 관리
            //var quesResult1sFacMng = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1B106");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeA(ReportHelper.CalcCheckCount(quesResult1sFacMng)), 2);


            //// A1B107 : 공정관리
            //var quesResult1sProcess = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1B107");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeC(ReportHelper.CalcCheckCount(quesResult1sProcess)), 2);


            //// A1B108 : 품질관리
            //var quesResult1sQaMng = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1B108");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeC(ReportHelper.CalcCheckCount(quesResult1sQaMng)), 3);


            //// A1C101 : 마케팅 전략의 수립 및 실행
            //var quesResult1sMarketing = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1C101");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeG(ReportHelper.CalcCheckCount(quesResult1sMarketing)), 8);


            //// A1C102 : 고객관리
            //var quesResult1sCustMng = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1C102");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeA(ReportHelper.CalcCheckCount(quesResult1sCustMng)), 9);


            //// A1D101 : 인적자윈의 확보와 개발관리
            //var quesResult1sHrMng = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1D101");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeA(ReportHelper.CalcCheckCount(quesResult1sHrMng)), 11);


            //// A1D102 : 이적자원의 보상 및 유지관리
            //var quesResult1sMaintenance = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1D102");
            //totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeA(ReportHelper.CalcCheckCount(quesResult1sMaintenance)), 8);


            ////재무적성과
            //// 구현해야 함.(다래 DB 정리되면..............................)


            //// A1E102 : 지적재산권성과
            //var quesResult2sPatent = await quesResult2Service.GetQuesResult2sAsync(paramModel.QuestionSn, "A1E102");
            ////등록 특허
            //var RegPatent = quesResult2sPatent.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1E10201");
            ////등록 실용신안
            //var RegUtilityModel = quesResult2sPatent.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1E10202");
            ////출원 특허
            //var ApplyPatent = quesResult2sPatent.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1E10203");
            ////출원 실용신안
            //var ApplyUtilityModel = quesResult2sPatent.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1E10204");
            ////기타
            //var Etc = quesResult2sPatent.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1E10205");
            //{
            //    double avg = (int.Parse(RegPatent.D)*3) + (int.Parse(ApplyPatent.D)*2) + (int.Parse(RegUtilityModel.D)*2) + int.Parse(ApplyUtilityModel.D) + int.Parse(Etc.D);
            //    totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeH(avg), 3);
            //}


            //// A1E103 : 임직원 수
            //var quesResult2sTotalEmp = await quesResult2Service.GetQuesResult2sAsync(paramModel.QuestionSn, "A1E103");
            ////전체 임직원
            //var TotalEmploy = quesResult2sTotalEmp.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1E10301");
            //if(int.Parse(TotalEmploy.D451) != 0)
            //{
            //    double avg = (int.Parse(TotalEmploy.D) / int.Parse(TotalEmploy.D451))-1;
            //    totalPoint = totalPoint + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeI(avg), 3);
            //}

            #endregion

            var comments = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "01");

            //조직역량->조직분화도
            var comment0 = comments.SingleOrDefault(i => i.DetailCd == "01010401");
            viewModel.CommentList.Add(ReportHelper.MakeCommentViewModel(paramModel, "01010401", comment0));

            // 상품화역량 -> 고객의수, 상품의 질 및 마케팅 수준
            var comment1 = comments.SingleOrDefault(i => i.DetailCd == "01010402");
            viewModel.CommentList.Add(ReportHelper.MakeCommentViewModel(paramModel, "01010402", comment1));

            // 위험관리역량 -> 제무회계 관리체계 및 제도수준
            var comment2 = comments.SingleOrDefault(i => i.DetailCd == "01010403");
            viewModel.CommentList.Add(ReportHelper.MakeCommentViewModel(paramModel, "01010403", comment2));


            ViewBag.paramModel = paramModel;
            return View(viewModel);

        }

        [HttpPost]
        public async Task<ActionResult> OverallSummary(OverallSummaryViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;
            var comments = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "01");

            foreach(var item in viewModel.CommentList)
            {
                var comment = comments.SingleOrDefault(i => i.DetailCd == item.DetailCd);
                if(comment == null)
                {
                    rptMentorCommentService.Insert(ReportHelper.MakeRptMentorcomment(item, paramModel));
                }
                else
                {
                    comment.Comment = item.Comment;
                }
            }

            await rptMentorCommentService.SaveDbContextAsync();

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("OverallSummary", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("OverallResultCover", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        public ActionResult OverallResultCover(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;
            return View(paramModel);
        }



        public async Task<ActionResult> OrgHR01(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            OrgHR01ViewModel viewModel = new OrgHR01ViewModel();
            //리스트 데이터 생성
            //var quesResult1s = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1D101");





            //검토결과 데이터 생성
            var comments = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "02");

            //조직역량->인적자원의 확보와 개발관리
            var comment = comments.SingleOrDefault(i => i.DetailCd == "02010201");
            viewModel.Comment = ReportHelper.MakeCommentViewModel(paramModel, "02010201", comment);

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> OrgHR01(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;
            var comments = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "02");

            var comment = comments.SingleOrDefault(i => i.DetailCd == viewModel.Comment.DetailCd);
            if (comment == null)
            {
                rptMentorCommentService.Insert(ReportHelper.MakeRptMentorcomment(viewModel.Comment, paramModel));
            }
            else
            {
                comment.Comment = viewModel.Comment.Comment;
            }

            await rptMentorCommentService.SaveDbContextAsync();

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("OrgHR01", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("OrgHR02", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }
        #region 성장 로드맵
        public ActionResult GrowthRoadMapCover(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            //임시로 나중에 삭제
            paramModel.BizWorkSn = 1;
            paramModel.BizWorkYear = 2015;
            paramModel.CompSn = 94;
            paramModel.QuestionSn = 16;
            paramModel.Status = "P";

            return View(paramModel);

        }


        //유형별 성장전략
        public async Task<ActionResult> GrowthStrategyType(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            GrowthStrategyViewModel viewModel = new GrowthStrategyViewModel();
        
            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "33");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("33");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

          
            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GrowthStrategyType(BasicSurveyReportViewModel paramModel, GrowthStrategyViewModel viewModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "33");

            foreach (var item in viewModel.CommentList)
            {
                var comment = listRptMentorComment.SingleOrDefault(i => i.DetailCd == item.DetailCd);
                if (comment == null)
                {
                    rptMentorCommentService.Insert(ReportHelper.MakeRptMentorcomment(item, paramModel));
                }
                else
                {
                    comment.Comment = item.Comment;
                }
            }
          

            await rptMentorCommentService.SaveDbContextAsync();
         

            if (viewModel.SubmitType == "T") //임시저장
            {
                return RedirectToAction("GrowthStrategyType", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("GrowthStrategyStep", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //단계 성장전략
        public async Task<ActionResult> GrowthStrategyStep(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            GrowthStrategyViewModel viewModel = new GrowthStrategyViewModel();

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "34");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("34");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);


            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GrowthStrategyStep(BasicSurveyReportViewModel paramModel, GrowthStrategyViewModel viewModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "34");

            foreach (var item in viewModel.CommentList)
            {
                var comment = listRptMentorComment.SingleOrDefault(i => i.DetailCd == item.DetailCd);
                if (comment == null)
                {
                    rptMentorCommentService.Insert(ReportHelper.MakeRptMentorcomment(item, paramModel));
                }
                else
                {
                    comment.Comment = item.Comment;
                }
            }


            await rptMentorCommentService.SaveDbContextAsync();


            if (viewModel.SubmitType == "T") //임시저장
            {
                return RedirectToAction("GrowthStrategyStep", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("GrowthCapabilityProposal", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //역량강화제안
        public async Task<ActionResult> GrowthCapabilityProposal(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            GrowthStrategyViewModel viewModel = new GrowthStrategyViewModel();

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "35");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("35");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);


            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GrowthCapabilityProposal(BasicSurveyReportViewModel paramModel, GrowthStrategyViewModel viewModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "35");

            foreach (var item in viewModel.CommentList)
            {
                var comment = listRptMentorComment.SingleOrDefault(i => i.DetailCd == item.DetailCd);
                if (comment == null)
                {
                    rptMentorCommentService.Insert(ReportHelper.MakeRptMentorcomment(item, paramModel));
                }
                else
                {
                    comment.Comment = item.Comment;
                }
            }


            await rptMentorCommentService.SaveDbContextAsync();


            if (viewModel.SubmitType == "T") //임시저장
            {
                return RedirectToAction("GrowthCapabilityProposal", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("GrowthTotalProposal", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //회사핵심내용
        public async Task<ActionResult> GrowthTotalProposal(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            GrowthStrategyViewModel viewModel = new GrowthStrategyViewModel();

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "36");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("36");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);


            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GrowthTotalProposal(BasicSurveyReportViewModel paramModel, GrowthStrategyViewModel viewModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "36");

            foreach (var item in viewModel.CommentList)
            {
                var comment = listRptMentorComment.SingleOrDefault(i => i.DetailCd == item.DetailCd);
                if (comment == null)
                {
                    rptMentorCommentService.Insert(ReportHelper.MakeRptMentorcomment(item, paramModel));
                }
                else
                {
                    comment.Comment = item.Comment;
                }
            }

            if (viewModel.SubmitType == "T") //임시저장
            {
                await rptMentorCommentService.SaveDbContextAsync();
                return RedirectToAction("GrowthTotalProposal", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                var rptMater = await rptMasterService.GetRptMasterAsync(paramModel.QuestionSn, paramModel.CompSn, paramModel.BizWorkYear);
                rptMater.Status = "C";
                rptMasterService.ModifyRptMaster(rptMater);

                await rptMentorCommentService.SaveDbContextAsync();
                return RedirectToAction("BasicSurveyCompanyList", "BasicSurveyReport", new { area = "" });
            }

        }
        #endregion


        #region 드롭다운박스 처리 controller
        [HttpPost]
        public async Task<JsonResult> GetBizWorkNm(int Year)
        {
            var mentorId = Session[Global.LoginID].ToString();

            //사업 DropDown List Data
            var listScMentorMapping = await scMentorMappingService.GetMentorMappingListByMentorId(mentorId, Year);
            var listScBizWork = listScMentorMapping.Select(mmp => mmp.ScBizWork).ToList();

            var bizList = ReportHelper.MakeBizWorkList(listScBizWork);

            return Json(bizList);
        }

        [HttpPost]
        public async Task<JsonResult> GetCompanyNm(int BizWorkSn, int Year)
        {
            var mentorId = Session[Global.LoginID].ToString();

            var listScCompMapping = await scCompMappingService.GetCompMappingListByMentorId(mentorId, "A", BizWorkSn, Year);
            var listScCompInfo = listScCompMapping.Select(cmp => cmp.ScCompInfo).ToList();

            var bizList = ReportHelper.MakeCompanyList(listScCompInfo);

            return Json(bizList);
        }
        #endregion
    }
}