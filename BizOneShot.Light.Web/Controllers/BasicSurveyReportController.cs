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
            Dictionary<string, double> dicTotalHrMng = new Dictionary<string, double>();
            Dictionary<string, double> dicTotalMkt = new Dictionary<string, double>();
            Dictionary<string, double> dicTotalBasicCpas = new Dictionary<string, double>();

            OverallSummaryViewModel viewModel = new OverallSummaryViewModel();
            viewModel.CommentList = new List<CommentViewModel>();
            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);


            //1-A. 경영역량총괄 전체평균
            //1) 종료된 모든 사업에 포함된 기업들의 사업기간내의 기초역량 점수 계산 
            // 확인사항 현재 사업이 장기간의 사업이라면 해당사업이 진행될 수록 사업전 사업은 작아짐. 확인 필요
            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);
            var endBizWorks = await scBizWorkService.GetEndBizWorkList(curBizWork.BizWorkStDt.Value);

            foreach (var bizWork in endBizWorks)
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
                        var dicKey = compMapping.ScCompInfo.RegistrationNo + i.ToString();

                        if (dicTotalHrMng.ContainsKey(dicKey).Equals(false))
                        {
                            var quesMaster = quesMasters.Where(qm => qm.BasicYear == i).SingleOrDefault();
                            if(quesMaster != null)
                            {
                                var totalHrMng = await reportUtil.GetHumanResourceMng(paramModel.QuestionSn);
                                var totalMkt = await reportUtil.GetTechMng(paramModel.QuestionSn);
                                var totalBasicCapa = await reportUtil.GetOverAllManagementTotalPoint(paramModel.QuestionSn);

                                dicTotalHrMng.Add(dicKey, totalHrMng);
                                dicTotalMkt.Add(dicKey, totalMkt);
                                dicTotalBasicCpas.Add(dicKey, totalBasicCapa);
                            }
                        }
                    }
                }
            }

            //2) 현재 사업에 참여한 업체 평균
            Dictionary<string, double> dicBizInHrMng = new Dictionary<string, double>();
            Dictionary<string, double> dicBizInMkt = new Dictionary<string, double>();
            Dictionary<string, double> dicBizInBasicCpas = new Dictionary<string, double>();

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
                        var dicKey = compMapping.ScCompInfo.RegistrationNo + i.ToString();
                        var quesMaster = quesMasters.Where(qm => qm.BasicYear == i).SingleOrDefault();

                        if (quesMaster != null)
                        {
                            var bizInHrMng = await reportUtil.GetHumanResourceMng(paramModel.QuestionSn);
                            var bizInMkt = await reportUtil.GetTechMng(paramModel.QuestionSn);
                            var bizInBasicCapa = await reportUtil.GetOverAllManagementTotalPoint(paramModel.QuestionSn);

                            if (dicTotalHrMng.ContainsKey(dicKey).Equals(false))
                            {
                                dicTotalHrMng.Add(dicKey, bizInHrMng);
                                dicTotalMkt.Add(dicKey, bizInMkt);
                                dicTotalBasicCpas.Add(dicKey, bizInBasicCapa);
                            }

                            dicBizInHrMng.Add(dicKey, bizInHrMng);
                            dicBizInMkt.Add(dicKey, bizInMkt);
                            dicBizInBasicCpas.Add(dicKey, bizInBasicCapa);
                        }
                    }
                }
            }

            // 3) 기초자료 전체 평균
            // 설명자료에 해당 내용 없음.

            // 4) 전체 평균정수 계산
            double totalPoint = 0;
            totalPoint = totalPoint + dicTotalHrMng.Values.Sum();
            totalPoint = totalPoint + dicTotalMkt.Values.Sum();
            totalPoint = totalPoint + dicTotalBasicCpas.Values.Sum();
            viewModel.AvgTotalPoint = Math.Round(totalPoint / dicTotalHrMng.Count, 1);

            //1-B. 해당 기업의 기초역량 점수 계산
            double companyPoint = 0;
            var mnaagementTotalPoint =  await reportUtil.GetOverAllManagementTotalPoint(paramModel.QuestionSn);
            var techMng = await reportUtil.GetTechMng(paramModel.QuestionSn);
            var HrMng = await reportUtil.GetHumanResourceMng(paramModel.QuestionSn);
            companyPoint = mnaagementTotalPoint + techMng + HrMng;
            viewModel.CompanyPoint = Math.Round(companyPoint, 1);

            //2. 경영역량 총괄 화살표
            viewModel.BizCapaType = ReportHelper.GetArrowTypeA(companyPoint);

            //3. 인적자원관리 화살표(해당기업)
            viewModel.HRMngType = ReportHelper.GetArrowTypeB(HrMng);

            //4. 기술경영, 마케팅 화살표(해당기업)
            viewModel.MarketingType = ReportHelper.GetArrowTypeC(techMng);

            //5. 기초역량 화살표(해당기업)
            viewModel.BasicCapaType = ReportHelper.GetArrowTypeD(mnaagementTotalPoint);

            //6. 조직문화도 화살표  -------------> 해당 페이지 개발 후 적용 해야함.
            viewModel.OrgType = "A";
            //7. 고객의수, 상품의질 화살표 -------------> 해당 페이지 개발 후 적용 해야함.
            viewModel.CustMngType = "A";
            //8. 전반적 제도 및 규정관리체계 화살표 -------------> 해당 페이지 개발 후 적용 해야함.
            viewModel.RoolType = "A";

            //9. 조직역량-인적자원관리 해당기업 점수
            OverallSummaryPointViewModel orgCapa = new OverallSummaryPointViewModel();
            orgCapa.CompanyPoint = Math.Round(HrMng, 1);
            //12. 조직역량-인적자원관리 참여기업 평균 점수
            orgCapa.AvgBizInCompanyPoint = Math.Round(dicBizInHrMng.Values.Average(), 1);
            //15. 조직역량-인적자원관리 전체평균 점수
            orgCapa.AvgTotalPoint = Math.Round(dicTotalHrMng.Values.Average(), 1);
            viewModel.OrgCapa = orgCapa;

            //10. 상품화역량-기술경영 마케팅관리 해당기업 점수
            OverallSummaryPointViewModel prductionCapa = new OverallSummaryPointViewModel();
            prductionCapa.CompanyPoint = Math.Round(techMng, 1);
            //13. 상품화역량-기술경영 마케팅관리 참여기업 평균 점수
            prductionCapa.AvgBizInCompanyPoint = Math.Round(dicBizInMkt.Values.Average(), 1);
            //16. 상품화역량-기술경영 마케팅관리 전체평균 점수
            prductionCapa.AvgTotalPoint = Math.Round(dicTotalMkt.Values.Average(), 1);
            viewModel.ProductionCapa = prductionCapa;

            //11. 위험관리역량-기초역량 해당기업 점수
            OverallSummaryPointViewModel riskMngCapa = new OverallSummaryPointViewModel();
            riskMngCapa.CompanyPoint = Math.Round(mnaagementTotalPoint, 1);
            //14. 상품화역량-기술경영 마케팅관리 참여기업 평균 점수
            riskMngCapa.AvgBizInCompanyPoint = Math.Round(dicBizInBasicCpas.Values.Average(), 1);
            //17. 상품화역량-기술경영 마케팅관리 전체평균 점수
            riskMngCapa.AvgTotalPoint = Math.Round(dicTotalBasicCpas.Values.Average(), 1);
            viewModel.RiskMngCapa = riskMngCapa;


            var comments = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "04");

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
            var comments = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "04");

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

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            OrgHR01ViewModel viewModel = new OrgHR01ViewModel();
            //viewModel.CheckList = new List<CheckListViewModel>();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1D101");

            ////사업참여 기업들의 레벨(창업보육, 보육성장, 자립성장) 분류
            //Dictionary<int, int> dicStartUp = new Dictionary<int, int>();
            //Dictionary<int, int> dicGrowth = new Dictionary<int, int>();
            //Dictionary<int, int> dicIndependent = new Dictionary<int, int>();

            //var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);
            //{
            //    var compMappings = curBizWork.ScCompMappings;
            //    foreach (var compMapping in compMappings)
            //    {
            //        var quesMasters = await quesMasterService.GetQuesMasterAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
            //        if (quesMasters == null)
            //        {
            //            continue;
            //        }

            //        //다래 재무정보 유무 체크하는 로직 추가해야함.(문진표정보, 재무정보가 있어야 보고서 생성가능.)


            //        //종합점수 조회하여 분류별로 딕셔너리 저장
            //        var point = await reportUtil.GetCompanyTotalPoint(quesMasters.QuestionSn);

            //        if (point >= 0 && point <= 50)
            //            dicStartUp.Add(compMapping.CompSn, quesMasters.QuestionSn);
            //        else if (point > 50 && point <= 75)
            //            dicGrowth.Add(compMapping.CompSn, quesMasters.QuestionSn);
            //        else
            //            dicIndependent.Add(compMapping.CompSn, quesMasters.QuestionSn);
            //    }
            //}



            ////리스트 데이터 생성
            //var quesResult1s = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1D101");

            //int count = 1;
            //foreach(var item in quesResult1s)
            //{
            //    CheckListViewModel checkListViewModel = new CheckListViewModel();
            //    checkListViewModel.Count = count.ToString();
            //    checkListViewModel.AnsVal = item.AnsVal.Value;
            //    checkListViewModel.DetailCd = item.QuesCheckList.DetailCd;
            //    checkListViewModel.Title = item.QuesCheckList.ReportTitle;
            //    //창업보육단계 평균
            //    int startUpCnt =  await reportUtil.GetCheckListCnt(dicStartUp, checkListViewModel.DetailCd);
            //    checkListViewModel.StartUpAvg = Math.Round(((startUpCnt + item.QuesCheckList.StartUpStep.Value + 0.0) / ( 39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    //보육성장단계 평균
            //    int growthCnt = await reportUtil.GetCheckListCnt(dicGrowth, checkListViewModel.DetailCd);
            //    checkListViewModel.GrowthAvg = Math.Round(((growthCnt + item.QuesCheckList.GrowthStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    //자립성장단계 평균
            //    int IndependentCnt = await reportUtil.GetCheckListCnt(dicIndependent, checkListViewModel.DetailCd);
            //    checkListViewModel.IndependentAvg = Math.Round(((IndependentCnt + item.QuesCheckList.IndependentStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    //참여기업 평균
            //    checkListViewModel.BizInCompanyAvg = Math.Round(((IndependentCnt + growthCnt + startUpCnt + 0.0) / (dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    //전체 평균
            //    checkListViewModel.TotalAvg = Math.Round(((IndependentCnt + growthCnt + startUpCnt + item.QuesCheckList.TotalStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    viewModel.CheckList.Add(checkListViewModel);
            //    count++;
            //}

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "06");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("06");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> OrgHR01(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "06");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("OrgHR01", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("OrgHR02", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        public async Task<ActionResult> OrgHR02(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            OrgHR01ViewModel viewModel = new OrgHR01ViewModel();
            //viewModel.CheckList = new List<CheckListViewModel>();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1D102");

            ////사업참여 기업들의 레벨(창업보육, 보육성장, 자립성장) 분류
            //Dictionary<int, int> dicStartUp = new Dictionary<int, int>();
            //Dictionary<int, int> dicGrowth = new Dictionary<int, int>();
            //Dictionary<int, int> dicIndependent = new Dictionary<int, int>();

            //var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);

            //{
            //    var compMappings = curBizWork.ScCompMappings;
            //    foreach (var compMapping in compMappings)
            //    {
            //        var quesMasters = await quesMasterService.GetQuesMasterAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
            //        if (quesMasters == null)
            //        {
            //            continue;
            //        }

            //        //다래 재무정보 유무 체크하는 로직 추가해야함.(문진표정보, 재무정보가 있어야 보고서 생성가능.)


            //        //종합점수 조회하여 분류별로 딕셔너리 저장
            //        var point = await reportUtil.GetCompanyTotalPoint(quesMasters.QuestionSn);

            //        if (point >= 0 && point <= 50)
            //            dicStartUp.Add(compMapping.CompSn, quesMasters.QuestionSn);
            //        else if (point > 50 && point <= 75)
            //            dicGrowth.Add(compMapping.CompSn, quesMasters.QuestionSn);
            //        else
            //            dicIndependent.Add(compMapping.CompSn, quesMasters.QuestionSn);
            //    }
            //}



            ////리스트 데이터 생성
            //var quesResult1s = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1D102");

            //int count = 1;
            //foreach (var item in quesResult1s)
            //{
            //    CheckListViewModel checkListViewModel = new CheckListViewModel();
            //    checkListViewModel.Count = count.ToString();
            //    checkListViewModel.AnsVal = item.AnsVal.Value;
            //    checkListViewModel.DetailCd = item.QuesCheckList.DetailCd;
            //    checkListViewModel.Title = item.QuesCheckList.ReportTitle;
            //    //창업보육단계 평균
            //    int startUpCnt = await reportUtil.GetCheckListCnt(dicStartUp, checkListViewModel.DetailCd);
            //    checkListViewModel.StartUpAvg = Math.Round(((startUpCnt + item.QuesCheckList.StartUpStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    //보육성장단계 평균
            //    int growthCnt = await reportUtil.GetCheckListCnt(dicGrowth, checkListViewModel.DetailCd);
            //    checkListViewModel.GrowthAvg = Math.Round(((growthCnt + item.QuesCheckList.GrowthStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    //자립성장단계 평균
            //    int IndependentCnt = await reportUtil.GetCheckListCnt(dicIndependent, checkListViewModel.DetailCd);
            //    checkListViewModel.IndependentAvg = Math.Round(((IndependentCnt + item.QuesCheckList.IndependentStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    //참여기업 평균
            //    checkListViewModel.BizInCompanyAvg = Math.Round(((IndependentCnt + growthCnt + startUpCnt + 0.0) / (dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    //전체 평균
            //    checkListViewModel.TotalAvg = Math.Round(((IndependentCnt + growthCnt + startUpCnt + item.QuesCheckList.TotalStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
            //    viewModel.CheckList.Add(checkListViewModel);
            //    count++;
            //}

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "07");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("07");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> OrgHR02(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "07");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("OrgHR02", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("OrgProductivity", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        public async Task<ActionResult> OrgProductivity(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            OrgProductivityViewModel viewModel = new OrgProductivityViewModel();
            viewModel.CheckList = new List<CheckListViewModel>();
            viewModel.Productivity = new BarChartViewModel();
            viewModel.Activity = new BarChartViewModel();
            viewModel.Activity.Dividend = 12;
            viewModel.Activity.Divisor = 100;
            viewModel.Activity.Result = viewModel.Activity.Dividend / viewModel.Activity.Divisor * 100;
            viewModel.Activity.Company = viewModel.Activity.Result;
            viewModel.Activity.AvgBizInCompany = 12.1;
            viewModel.Activity.AvgSMCompany = 34.2;
            viewModel.Activity.AvgTotal = 70;

            viewModel.Productivity.Dividend = 79;
            viewModel.Productivity.Divisor = 158;
            viewModel.Productivity.Result = viewModel.Productivity.Dividend / viewModel.Productivity.Divisor;
            viewModel.Productivity.Company = viewModel.Productivity.Result;
            viewModel.Productivity.AvgBizInCompany = 62.1;
            viewModel.Productivity.AvgSMCompany = 54.2;
            viewModel.Productivity.AvgTotal = 48;
            viewModel.CheckList = new List<CheckListViewModel>();

            //사업참여 기업들의 레벨(창업보육, 보육성장, 자립성장) 분류
            Dictionary<int, int> dicStartUp = new Dictionary<int, int>();
            Dictionary<int, int> dicGrowth = new Dictionary<int, int>();
            Dictionary<int, int> dicIndependent = new Dictionary<int, int>();

            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);

            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    var quesMasters = await quesMasterService.GetQuesMasterAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMasters == null)
                    {
                        continue;
                    }

                    //다래 재무정보 유무 체크하는 로직 추가해야함.(문진표정보, 재무정보가 있어야 보고서 생성가능.)


                    //종합점수 조회하여 분류별로 딕셔너리 저장
                    var point = await reportUtil.GetCompanyTotalPoint(quesMasters.QuestionSn);

                    if (point >= 0 && point <= 50)
                        dicStartUp.Add(compMapping.CompSn, quesMasters.QuestionSn);
                    else if (point > 50 && point <= 75)
                        dicGrowth.Add(compMapping.CompSn, quesMasters.QuestionSn);
                    else
                        dicIndependent.Add(compMapping.CompSn, quesMasters.QuestionSn);
                }
            }

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "08");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("08");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> OrgProductivity(OrgProductivityViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "08");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("OrgProductivity", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("OrgDivided", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        public async Task<ActionResult> OrgDivided(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            var viewModel = new OrgDividedViewModel();

            // 조직구성 조회
            var quesMaster = await quesMasterService.GetQuesOgranAnalysisAsync(paramModel.QuestionSn);

            //기획관리
            var management = quesMaster.QuesOgranAnalysis.SingleOrDefault(i => i.DeptCd == "M");
            viewModel.Management = Mapper.Map<OrgEmpCompositionViewModel>(management);

            //생산관리
            var produce = quesMaster.QuesOgranAnalysis.SingleOrDefault(i => i.DeptCd == "P");
            viewModel.Produce = Mapper.Map<OrgEmpCompositionViewModel>(produce);

            //연구개발
            var rnd = quesMaster.QuesOgranAnalysis.SingleOrDefault(i => i.DeptCd == "R");
            viewModel.RND = Mapper.Map<OrgEmpCompositionViewModel>(rnd);

            //마케팅
            var salse = quesMaster.QuesOgranAnalysis.SingleOrDefault(i => i.DeptCd == "S");
            viewModel.Salse = Mapper.Map<OrgEmpCompositionViewModel>(salse);


            viewModel.StaffSumCount = viewModel.Management.StaffCount + viewModel.Produce.StaffCount + viewModel.RND.StaffCount + viewModel.Salse.StaffCount;

            viewModel.ChiefSumCount = viewModel.Management.ChiefCount + viewModel.Produce.ChiefCount + viewModel.RND.ChiefCount + viewModel.Salse.ChiefCount;

            viewModel.OfficerSumCount = viewModel.Management.OfficerCount + viewModel.Produce.OfficerCount + viewModel.RND.OfficerCount + viewModel.Salse.OfficerCount;

            viewModel.BeginnerSumCount = viewModel.Management.BeginnerCount + viewModel.Produce.BeginnerCount + viewModel.RND.BeginnerCount + viewModel.Salse.BeginnerCount;

            viewModel.TotalSumCount = viewModel.StaffSumCount + viewModel.ChiefSumCount + viewModel.OfficerSumCount + viewModel.BeginnerSumCount;

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "09");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("09");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> OrgDivided(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "09");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("OrgDivided", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductTechMgmt", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        #region 2. 기초역량 검토 종합결과

        //P14 2.상품화역량 - 생산설비의 운영체계 및 관리
        public async Task<ActionResult> ProductivityCommercialize(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1B104");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "12");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("12");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductivityCommercialize(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "12");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("ProductivityCommercialize", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityMgmtFacility", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        //P14 2.상품화역량 - 생산설비의 운영체계 및 관리
        public async Task<ActionResult> ProductivityMgmtFacility(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1B106");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "14");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("14");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductivityMgmtFacility(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "14");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("ProductivityMgmtFacility", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityProcessControl", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //P15 2.상품화역량 - 공정관리
        public async Task<ActionResult> ProductivityProcessControl(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1B107");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "15");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("15");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductivityProcessControl(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "15");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("ProductivityProcessControl", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityQC", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //P16 2.상품화역량 - 품질관리
        public async Task<ActionResult> ProductivityQC(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1B108");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "16");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("16");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductivityQC(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "16");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("ProductivityQC", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityMgmtMarketing", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //P17 2.상품화역량 - 마케팅 전략의 수립 및 실행
        public async Task<ActionResult> ProductivityMgmtMarketing(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1C101");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "17");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("17");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductivityMgmtMarketing(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "17");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("ProductivityMgmtMarketing", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityMgmtCustomer", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //P18 2.상품화역량 - 고객관리
        public async Task<ActionResult> ProductivityMgmtCustomer(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1C102");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "18");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("18");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductivityMgmtCustomer(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "18");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("ProductivityMgmtCustomer", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("RiskMgmtVisionStrategy", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        //P24 3.위험관리역량 - [CEO역량]경영목표 및 전략
        public async Task<ActionResult> RiskMgmtVisionStrategy(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1A101");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "24");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("24");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RiskMgmtVisionStrategy(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "24");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("RiskMgmtVisionStrategy", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("RiskMgmtLeadership", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //P25 3.위험관리역량 - [CEO역량]경영자의 리더쉽
        public async Task<ActionResult> RiskMgmtLeadership(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1A102");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "25");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("25");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RiskMgmtLeadership(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "25");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("RiskMgmtLeadership", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("RiskMgmtRelCEO", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //P26 3.위험관리역량 - 경영목표의 신뢰성
        public async Task<ActionResult> RiskMgmtRelCEO(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1A103");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "26");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("26");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RiskMgmtRelCEO(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "26");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("RiskMgmtRelCEO", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("RiskMgmtWorkingEnv", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        //P27 3.위험관리역량 - 근로환경
        public async Task<ActionResult> RiskMgmtWorkingEnv(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1A201");


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "27");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("27");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RiskMgmtWorkingEnv(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "27");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("RiskMgmtWorkingEnv", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("??", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }



        //P29 3.위험관리역량 - 정보시스템활용
        public async Task<ActionResult> RiskMgmtITSystem(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService);

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();
            viewModel.CheckList = await reportUtil.getGrowthStepPointCheckList(paramModel, "A1A203");
            

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "29");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("29");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RiskMgmtITSystem(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "29");

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

            if (viewModel.SubmitType == "T")
            {
                return RedirectToAction("RiskMgmtITSystem", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("??", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        //3.위험관리 역량 - 31p. 전문가 평가
        public async Task<ActionResult> RiskMgmtEvalProfession(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "31");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("31");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList.Where(cl => cl.Type == "C"), listRptMentorComment.Where(rmc => rmc.RptCheckList.Type == "C").ToList());

            //체크박스 List 채우기
            var ChekcBoxList = ReportHelper.MakeCheckBoxViewModel(enumRptCheckList.Where(cl => cl.Type == "B"), listRptMentorComment.Where(rmc => rmc.RptCheckList.Type == "B").ToList());

            viewModel.CommentList = CommentList;
            viewModel.CheckBoxList = ChekcBoxList;

            ViewBag.paramModel = paramModel;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RiskMgmtEvalProfession(BasicSurveyReportViewModel paramModel, RiskMgmtViewModel viewModel)
        {
            ViewBag.LeftMenu = Global.CapabilityReport;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "31");

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

            foreach (var item in viewModel.CheckBoxList)
            {
                var comment = listRptMentorComment.SingleOrDefault(i => i.DetailCd == item.DetailCd);
                if (comment == null)
                {
                    rptMentorCommentService.Insert(ReportHelper.MakeRptMentorcomment(item, paramModel));
                }
                else
                {
                    comment.Comment = item.CheckVal.ToString();
                }
            }


            await rptMentorCommentService.SaveDbContextAsync();


            if (viewModel.SubmitType == "T") //임시저장
            {
                return RedirectToAction("RiskMgmtEvalProfession", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("GrowthRoadMapCover", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        #endregion


        #region 3. 성장 로드맵제안
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