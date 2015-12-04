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
        private readonly IScCompInfoService scCompInfoService;
        private readonly IRptMasterService rptMasterService;
        private readonly IRptMentorCommentService rptMentorCommentService;
        private readonly IRptCheckListService rptCheckListService;
        private readonly IRptMngCodeService rptMngCodeService;
        private readonly IRptMngCommentService rptMngCommentService;
        private readonly ISboFinancialIndexTService sboFinancialIndexTService;

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
            IScBizWorkService scBizWorkService,
            IRptMngCodeService rptMngCodeService,
            IRptMngCommentService rptMngCommentService,
            ISboFinancialIndexTService sboFinancialIndexTService,
            IScCompInfoService scCompInfoService)
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
            this.rptMngCodeService = rptMngCodeService;
            this.rptMngCommentService = rptMngCommentService;
            this.sboFinancialIndexTService = sboFinancialIndexTService;
            this.scCompInfoService = scCompInfoService;
        }

        // GET: BasicSurveyReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BasicSurveyReportPrint(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;
            return View(paramModel);
        }

        public async Task<ActionResult> Cover(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;


            return View(paramModel);

        }

               

        

        public async Task<ActionResult> OverallSummary(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            //double totalPoint = 0;
            OverallSummaryViewModel viewModel = new OverallSummaryViewModel();
            viewModel.CommentList = new List<CommentViewModel>();
            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

            //해당기업 기초역량
            double basicCapa = 0.0;
            //해당기업 기술경영 마케팅관리
            double mkt = 0.0;
            //해당기업 인적자원관리
            double hrMng = 0.0;
            //해당기업 재무관리
            double financeMng = 0.0;
            //해당기업 1인당 노동생산성
            double workProductivity = 0.0;
            //해당기업 매출영업이익률
            double salesEarning = 0.0;
            //해당기업 유동비율
            double current = 0.0;


            //1) 현재 사업에 참여한 업체 평균
            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);

            //인적자원관리
            Dictionary<string, double> dicBizInHrMng = new Dictionary<string, double>();
            //재무관리
            Dictionary<string, double> dicBizInFinanceMng = new Dictionary<string, double>();
            //기술경영마케팅
            Dictionary<string, double> dicBizInMkt = new Dictionary<string, double>();
            //기초역량
            Dictionary<string, double> dicBizInBasicCpas = new Dictionary<string, double>();
            //매출액
            Dictionary<string, decimal> dicSales = new Dictionary<string, decimal>();
            //재료비
            Dictionary<string, decimal> dicMaterrial = new Dictionary<string, decimal>();
            //종업원수
            Dictionary<string, decimal> dicQtEmp = new Dictionary<string, decimal>();
            //영업이익
            Dictionary<string, decimal> dicOperatingErning = new Dictionary<string, decimal>();
            //유동자산
            Dictionary<string, decimal> dicCurrentAsset = new Dictionary<string, decimal>();
            //유동부채
            Dictionary<string, decimal> dicCurrentLiability = new Dictionary<string, decimal>();

            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    //문진표 작성내역 조회
                    var quesMaster = await quesMasterService.GetQuesMasterAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMaster == null)
                    {
                        continue;
                    }
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    //참여기업의 점수 계산
                    var bizInHrMng = await reportUtil.GetHumanResourceMng(quesMaster.QuestionSn);
                    var bizInMkt = await reportUtil.GetTechMng(quesMaster.QuestionSn, sboFinacialIndexT);
                    var bizInBasicCapa = await reportUtil.GetOverAllManagementTotalPoint(quesMaster.QuestionSn);
                    var bizInFinance = await reportUtil.GetFinanceMng(quesMaster.QuestionSn, sboFinacialIndexT);

                    // A1A202 : 조직만족도
                    var quesResult2s = await quesResult2Service.GetQuesResult2sAsync(quesMaster.QuestionSn, "A1A202");
                    // 총직원은 다래 재무정보가 아닌 문진표상에 총인원을 가지고 온다.
                    var totalEmp = quesResult2s.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1A20201");

                    //해당기업을 찾아 점수를 별도로 저장한다.
                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {
                        basicCapa = bizInBasicCapa;
                        mkt = bizInMkt;
                        hrMng = bizInHrMng;
                        financeMng = bizInFinance;

                        if (sboFinacialIndexT.QtEmp != 0)
                        {
                            workProductivity = Math.Truncate(Convert.ToDouble(((sboFinacialIndexT.CurrentSale - sboFinacialIndexT.MaterialCost) / decimal.Parse(totalEmp.D)) / 1000));
                        }

                        if (sboFinacialIndexT.CurrentSale != 0)
                        {
                            salesEarning = Math.Round(Convert.ToDouble((sboFinacialIndexT.OperatingEarning / sboFinacialIndexT.CurrentSale) * 100), 1);
                        }

                        if (sboFinacialIndexT.CurrentLiability != 0)
                        {
                            current = Math.Round(Convert.ToDouble((sboFinacialIndexT.CurrentAsset / sboFinacialIndexT.CurrentLiability) * 100), 1);
                        }
                    }


                    dicBizInHrMng.Add(compMapping.ScCompInfo.RegistrationNo, bizInHrMng);
                    dicBizInMkt.Add(compMapping.ScCompInfo.RegistrationNo, bizInMkt);
                    dicBizInBasicCpas.Add(compMapping.ScCompInfo.RegistrationNo, bizInBasicCapa);
                    dicBizInFinanceMng.Add(compMapping.ScCompInfo.RegistrationNo, bizInFinance);

                    dicSales.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentSale.Value);
                    dicMaterrial.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.MaterialCost.Value);
                    dicQtEmp.Add(compMapping.ScCompInfo.RegistrationNo, decimal.Parse(totalEmp.D));
                    dicOperatingErning.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.OperatingEarning.Value);
                    dicCurrentAsset.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentAsset.Value);
                    dicCurrentLiability.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentLiability.Value);


                }
            }

            //1-A. 사업평균 점수 계산
            double totalPoint = 0;
            totalPoint = totalPoint + dicBizInHrMng.Values.Sum();
            totalPoint = totalPoint + dicBizInMkt.Values.Sum();
            totalPoint = totalPoint + dicBizInBasicCpas.Values.Sum();
            totalPoint = totalPoint + dicBizInFinanceMng.Values.Sum();
            viewModel.AvgTotalPoint = (dicBizInHrMng.Count == 0) ? 0 : Math.Round(totalPoint / dicBizInHrMng.Count, 1);

            //1-B. 해당 기업의 기초역량 점수 계산 
            double companyPoint = 0;
            companyPoint = basicCapa + mkt + hrMng + financeMng;
            viewModel.CompanyPoint = Math.Round(companyPoint, 1);

            //2. 경영역량 총괄 화살표
            viewModel.BizCapaType = ReportHelper.GetArrowTypeA(companyPoint);

            //3. 인적자원관리 화살표(해당기업)
            viewModel.HRMngType = ReportHelper.GetArrowTypeB(hrMng);

            //4. 기술경영, 마케팅 화살표(해당기업)
            viewModel.MarketingType = ReportHelper.GetArrowTypeC(mkt);

            //5. 기초역량 화살표(해당기업)
            viewModel.BasicCapaType = ReportHelper.GetArrowTypeD(basicCapa);

            //6. 조직문화도 화살표  -------------> 해당 페이지 개발 후 적용 해야함.
            var orgDivided = await rptMentorCommentService.GetRptMentorCommentAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "02030901");

            if(orgDivided != null)
            { 
                viewModel.OrgType = ReportHelper.GetArrowTypeE(int.Parse(orgDivided.Comment));
            }
            else
            {
                viewModel.OrgType = "C";
            }
            //7. 고객의수, 상품의질 화살표 -------------> 해당 페이지 개발 후 적용 해야함.
            var custMng = await rptMentorCommentService.GetRptMentorCommentAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "02022112");
            if (custMng != null)
            {
                viewModel.CustMngType = ReportHelper.GetArrowTypeE(int.Parse(custMng.Comment));
            }
            else
            {
                viewModel.CustMngType = "C";
            }
            //8. 전반적 제도 및 규정관리체계 화살표 -------------> 해당 페이지 개발 후 적용 해야함.02033128
            var rool = await rptMentorCommentService.GetRptMentorCommentAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "02033128");

            if (rool != null)
            {
                viewModel.RoolType = ReportHelper.GetArrowTypeE(int.Parse(rool.Comment));
            }
            else
            {
                viewModel.RoolType = "C";
            }

            //9. 조직역량-인적자원관리 해당기업 점수
            OverallSummaryPointViewModel orgCapa = new OverallSummaryPointViewModel();
            orgCapa.CompanyPoint = Math.Round(hrMng, 1);
            //12. 조직역량-인적자원관리 참여기업 평균 점수
            orgCapa.AvgBizInCompanyPoint = Math.Round(dicBizInHrMng.Values.Average(), 1);
            //15. 조직역량-인적자원관리 전체평균 점수
            orgCapa.AvgTotalPoint = Math.Round((dicBizInHrMng.Values.Sum()+ 277.75)/(dicBizInHrMng.Count + 39), 1);
            //18. 조직역량-1인당노동생산성 해당기업점수
            orgCapa.CompanyPoint2 = workProductivity;
            //21. 조직역량-1인당노동생산성 참여기업 평균
            orgCapa.AvgBizInCompanyPoint2 = (dicQtEmp.Values.Sum() == 0) ? 0 : Math.Truncate(Convert.ToDouble(((dicSales.Values.Sum() - dicMaterrial.Values.Sum()) / dicQtEmp.Values.Sum()) / 1000));
            //24. 조직역량-1인당노동생산성 전체 평균
            orgCapa.AvgTotalPoint2 = Math.Truncate(Convert.ToDouble((((dicSales.Values.Sum() + 111710064106) - (dicMaterrial.Values.Sum() + 43571068769)) / (dicQtEmp.Values.Sum() + 718 )) / 1000));
            //27. 조직역량-1인당노동생산성 중소기업평균
            orgCapa.AvgSMCompanyPoint = 64342;
            viewModel.OrgCapa = orgCapa;

            //10. 상품화역량-기술경영 마케팅관리 해당기업 점수
            OverallSummaryPointViewModel prductionCapa = new OverallSummaryPointViewModel();
            prductionCapa.CompanyPoint = Math.Round(mkt, 1);
            //13. 상품화역량-기술경영 마케팅관리 참여기업 평균 점수
            prductionCapa.AvgBizInCompanyPoint = Math.Round(dicBizInMkt.Values.Average(), 1);
            //16. 상품화역량-기술경영 마케팅관리 전체평균 점수
            prductionCapa.AvgTotalPoint = Math.Round((dicBizInMkt.Values.Sum() + 770.25) / (dicBizInMkt.Count + 39), 1);
            //19. 상품화역량-매출영업이익률 해당기업 점수
            prductionCapa.CompanyPoint2 = salesEarning;
            //22. 상품화역량-매출영업이익률 참여기업 평균
            prductionCapa.AvgBizInCompanyPoint2 = (dicSales.Values.Sum() == 0) ? 0 : Math.Round(Convert.ToDouble((dicOperatingErning.Values.Sum() / dicSales.Values.Sum()) * 100), 1);
            //25. 상품화역량-매출영업이익률 전체평균
            prductionCapa.AvgTotalPoint2 = Math.Round(Convert.ToDouble(((dicOperatingErning.Values.Sum() + 6689265895) / (dicSales.Values.Sum() + 111710064106)) * 100), 1);
            //28. 상품화역량-매출영업이익률 중소기업평균
            prductionCapa.AvgSMCompanyPoint = 5.2;
            viewModel.ProductionCapa = prductionCapa;

            //11. 위험관리역량-기초역량 해당기업 점수
            OverallSummaryPointViewModel riskMngCapa = new OverallSummaryPointViewModel();
            riskMngCapa.CompanyPoint = Math.Round(basicCapa, 1);
            //14. 위험관리역량-기초역량 참여기업 평균 점수
            riskMngCapa.AvgBizInCompanyPoint = Math.Round(dicBizInBasicCpas.Values.Average(), 1);
            //17. 위험관리역량-기초역량 전체평균 점수
            riskMngCapa.AvgTotalPoint = Math.Round((dicBizInBasicCpas.Values.Sum() + 238.38) / (dicBizInBasicCpas.Count + 39), 1);
            //20. 위험관리역량-유동비율 해당기업 점수
            riskMngCapa.CompanyPoint2 = current;
            //23. 위험관리역량-유동비율 참여기업평균 점수
            riskMngCapa.AvgBizInCompanyPoint2 = (dicCurrentLiability.Values.Sum() == 0) ? 0 : Math.Round(Convert.ToDouble((dicCurrentAsset.Values.Sum() / dicCurrentLiability.Values.Sum()) * 100), 1);
            //26. 위험관리역량-유동비율 전체평균 점수
            riskMngCapa.AvgTotalPoint2 = Math.Round(Convert.ToDouble(((dicCurrentAsset.Values.Sum() + 58220981909) / (dicCurrentLiability.Values.Sum() + 23152799577)) * 100), 1);
            //29. 위험관리역량-유동비율 중소기업평균 점수
            riskMngCapa.AvgSMCompanyPoint = 136.3;
            viewModel.RiskMngCapa = riskMngCapa;

            //멘토 작성내용 조회
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
            ViewBag.LeftMenu = Global.Report;
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
            ViewBag.LeftMenu = Global.Report;
            return View(paramModel);
        }


        public async Task<ActionResult> OrgHR01(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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

                    //다래 재무정보 유무 체크하는 로직 추가해야함.(문진표정보, 재무정보가 있어야 보고서 생성가능.)


                    //종합점수 조회하여 분류별로 딕셔너리 저장
                    //var point = await reportUtil.GetCompanyTotalPoint(quesMasters.QuestionSn);

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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

                    //다래 재무정보 유무 체크하는 로직 추가해야함.(문진표정보, 재무정보가 있어야 보고서 생성가능.)


                    //종합점수 조회하여 분류별로 딕셔너리 저장
                    //var point = await reportUtil.GetCompanyTotalPoint(quesMasters.QuestionSn);

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

            OrgProductivityViewModel viewModel = new OrgProductivityViewModel();
            viewModel.CheckList = new List<CheckListViewModel>();
            viewModel.Productivity = new BarChartViewModel();
            viewModel.Activity = new BarChartViewModel();

            //1) 현재 사업에 참여한 업체 평균
            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);

            Dictionary<string, decimal> dicSales = new Dictionary<string, decimal>();
            Dictionary<string, decimal> dicMaterrial = new Dictionary<string, decimal>();
            Dictionary<string, decimal> dicQtEmp = new Dictionary<string, decimal>();
            Dictionary<string, decimal> dicTotalAsset = new Dictionary<string, decimal>();

            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    //문진표 작성내역 조회
                    var quesMaster = await quesMasterService.GetQuesOgranAnalysisAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMaster == null)
                    {
                        continue;
                    }
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    // A1A202 : 조직만족도
                    var quesResult2s = await quesResult2Service.GetQuesResult2sAsync(quesMaster.QuestionSn, "A1A202");
                    // 총직원은 다래 재무정보가 아닌 문진표상에 총인원을 가지고 온다.
                    var totalEmp = quesResult2s.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1A20201");

                    //해당기업을 찾아 점수를 별도로 저장한다.
                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {

                        viewModel.Productivity.Dividend = Math.Truncate(Convert.ToDouble((sboFinacialIndexT.CurrentSale.Value - sboFinacialIndexT.MaterialCost.Value) / 1000));
                        viewModel.Productivity.Divisor = Math.Round(Convert.ToDouble(totalEmp.D), 0);
                        viewModel.Productivity.Result = (viewModel.Productivity.Divisor == 0) ? 0 : Math.Truncate(viewModel.Productivity.Dividend / viewModel.Productivity.Divisor);
                        viewModel.Productivity.Company = viewModel.Productivity.Result;
                        viewModel.Productivity.AvgSMCompany = 135547;

                        viewModel.Activity.Dividend = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.CurrentSale.Value / 1000));
                        viewModel.Activity.Divisor = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.TotalAsset.Value / 1000));
                        viewModel.Activity.Result = (viewModel.Activity.Divisor == 0) ? 0 : Math.Round((viewModel.Activity.Dividend / viewModel.Activity.Divisor) * 100, 1);
                        viewModel.Activity.Company = viewModel.Activity.Result;
                        viewModel.Activity.AvgSMCompany = 114.8;
                    }

                    dicSales.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentSale.Value);
                    dicMaterrial.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.MaterialCost.Value);
                    dicQtEmp.Add(compMapping.ScCompInfo.RegistrationNo, decimal.Parse(totalEmp.D));
                    dicTotalAsset.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.TotalAsset.Value);

                }
            }

            //평균값 계산
            viewModel.Productivity.AvgBizInCompany = (dicQtEmp.Values.Sum() == 0) ? 0 : Math.Truncate(Convert.ToDouble(((dicSales.Values.Sum() - dicMaterrial.Values.Sum()) / dicQtEmp.Values.Sum()) / 1000));
            viewModel.Productivity.AvgTotal = Math.Truncate(Convert.ToDouble((((dicSales.Values.Sum() - dicMaterrial.Values.Sum()) + 68138995337) / (dicQtEmp.Values.Sum() + 718)) / 1000));

            viewModel.Activity.AvgBizInCompany = (dicTotalAsset.Values.Sum() == 0) ? 0 : Math.Round(Convert.ToDouble(dicSales.Values.Sum() / dicTotalAsset.Values.Sum() * 100));
            viewModel.Activity.AvgTotal = Math.Round(Convert.ToDouble((dicSales.Values.Sum() + 58431124392) / (dicTotalAsset.Values.Sum() + 46885784174) * 100));



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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

            var viewModel = new OrgDividedViewModel();

            //1) 현재 사업에 참여한 업체 평균
            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);

            Dictionary<string, int> dicManagement = new Dictionary<string, int>();
            Dictionary<string, int> dicProduce = new Dictionary<string, int>();
            Dictionary<string, int> dicRnd = new Dictionary<string, int>();
            Dictionary<string, int> dicSalse = new Dictionary<string, int>();

            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    //문진표 작성내역 조회
                    var quesMaster = await quesMasterService.GetQuesOgranAnalysisAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMaster == null)
                    {
                        continue;
                    }
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    foreach(var item in quesMaster.QuesOgranAnalysis)
                    {
                        var cnt = item.ChiefCount + item.OfficerCount + item.StaffCount + item.BeginnerCount;

                        //기획관리
                        if(item.DeptCd == "M")
                        {
                            dicManagement.Add(compMapping.ScCompInfo.RegistrationNo, cnt.Value);

                            if(quesMaster.QuestionSn == paramModel.QuestionSn)
                            {
                                viewModel.Management = Mapper.Map<OrgEmpCompositionViewModel>(item);
                            }
                        }
                        //생산 / 생산관리
                        else if (item.DeptCd == "P")
                        {
                            dicProduce.Add(compMapping.ScCompInfo.RegistrationNo, cnt.Value);
                            if (quesMaster.QuestionSn == paramModel.QuestionSn)
                            {
                                viewModel.Produce = Mapper.Map<OrgEmpCompositionViewModel>(item);
                            }
                        }
                        //연구개발/연구지원
                        else if (item.DeptCd == "R")
                        {
                            dicRnd.Add(compMapping.ScCompInfo.RegistrationNo, cnt.Value);
                            if (quesMaster.QuestionSn == paramModel.QuestionSn)
                            {
                                viewModel.RND = Mapper.Map<OrgEmpCompositionViewModel>(item);
                            }
                        }
                        //마케팅기획/판매영업
                        else if (item.DeptCd == "S")
                        {
                            dicSalse.Add(compMapping.ScCompInfo.RegistrationNo, cnt.Value);
                            if (quesMaster.QuestionSn == paramModel.QuestionSn)
                            {
                                viewModel.Salse = Mapper.Map<OrgEmpCompositionViewModel>(item);
                            }
                        }
                    }
                }
            }

            viewModel.StaffSumCount = viewModel.Management.StaffCount + viewModel.Produce.StaffCount + viewModel.RND.StaffCount + viewModel.Salse.StaffCount;

            viewModel.ChiefSumCount = viewModel.Management.ChiefCount + viewModel.Produce.ChiefCount + viewModel.RND.ChiefCount + viewModel.Salse.ChiefCount;

            viewModel.OfficerSumCount = viewModel.Management.OfficerCount + viewModel.Produce.OfficerCount + viewModel.RND.OfficerCount + viewModel.Salse.OfficerCount;

            viewModel.BeginnerSumCount = viewModel.Management.BeginnerCount + viewModel.Produce.BeginnerCount + viewModel.RND.BeginnerCount + viewModel.Salse.BeginnerCount;

            viewModel.TotalSumCount = viewModel.StaffSumCount + viewModel.ChiefSumCount + viewModel.OfficerSumCount + viewModel.BeginnerSumCount;
            //평균값생성

            double companyTotalCnt = viewModel.TotalSumCount.Value;
            double bizInCompanyTotalCnt = dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum() + dicManagement.Values.Sum();

            //기획관리 평균값 생성
            if (viewModel.TotalSumCount == 0)
            {
                viewModel.Management.Company = 0;
                viewModel.Produce.Company = 0;
                viewModel.RND.Company = 0;
                viewModel.Salse.Company = 0;
                viewModel.CompanySum = 0;
            }
            else
            { 
                viewModel.Management.Company = Math.Round(viewModel.Management.PartialSum / companyTotalCnt * 100, 1);
                viewModel.Produce.Company = Math.Round(viewModel.Produce.PartialSum / companyTotalCnt * 100, 1);
                viewModel.RND.Company = Math.Round(viewModel.RND.PartialSum / companyTotalCnt * 100, 1);
                viewModel.Salse.Company = Math.Round(viewModel.Salse.PartialSum / companyTotalCnt * 100, 1);
                viewModel.CompanySum = 100;
            }

            if((dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum() + dicManagement.Values.Sum()) == 0)
            {
                viewModel.Management.AvgBizInCompany = 0;
                viewModel.Produce.AvgBizInCompany = 0;
                viewModel.RND.AvgBizInCompany = 0;
                viewModel.Salse.AvgBizInCompany = 0;
                viewModel.AvgBizInCompanySum = 0;
            }
            else
            { 
                viewModel.Management.AvgBizInCompany = Math.Round(dicManagement.Values.Sum() / bizInCompanyTotalCnt * 100, 1);
                viewModel.Produce.AvgBizInCompany = Math.Round(dicProduce.Values.Sum() / bizInCompanyTotalCnt * 100, 1);
                viewModel.RND.AvgBizInCompany = Math.Round(dicRnd.Values.Sum() / bizInCompanyTotalCnt * 100, 1);
                viewModel.Salse.AvgBizInCompany = Math.Round(dicSalse.Values.Sum() / bizInCompanyTotalCnt * 100, 1);
                viewModel.AvgBizInCompanySum = 100;
            }

            viewModel.Management.AvgTotal = Math.Round((dicManagement.Values.Sum() + 112) / (bizInCompanyTotalCnt + 718) * 100, 1);
            viewModel.Produce.AvgTotal = Math.Round((dicProduce.Values.Sum() + 305) / (bizInCompanyTotalCnt + 718) * 100, 1);
            viewModel.RND.AvgTotal = Math.Round((dicRnd.Values.Sum() + 180) / (bizInCompanyTotalCnt + 718) * 100, 1);
            viewModel.Salse.AvgTotal = Math.Round((dicSalse.Values.Sum() + 121) / (bizInCompanyTotalCnt + 718) * 100, 1);
            viewModel.AvgTotalSum = 100;


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
            ViewBag.LeftMenu = Global.Report;

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
                return RedirectToAction("RndCost", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }



        public async Task<ActionResult> RndCost(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

            var viewModel = new RndCostViewModel();
            viewModel.value = new CheckListViewModel();
            viewModel.percent = new CheckListViewModel();

            //1) 현재 사업에 참여한 업체 평균
            //사업참여 기업들의 레벨(창업보육, 보육성장, 자립성장) 분류
            Dictionary<int, decimal> dicStartUpRnd = new Dictionary<int, decimal>();
            Dictionary<int, decimal> dicGrowthRnd = new Dictionary<int, decimal>();
            Dictionary<int, decimal> dicIndependentRnd = new Dictionary<int, decimal>();
            Dictionary<int, decimal> dicStartUpSales = new Dictionary<int, decimal>();
            Dictionary<int, decimal> dicGrowthSales = new Dictionary<int, decimal>();
            Dictionary<int, decimal> dicIndependentSales = new Dictionary<int, decimal>();

            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);
            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    var quesMaster = await quesMasterService.GetQuesMasterAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMaster == null)
                    {
                        continue;
                    }

                    //다래 재무정보 유무 체크하는 로직 추가해야함.(문진표정보, 재무정보가 있어야 보고서 생성가능.)
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {
                        viewModel.value.Company = Math.Truncate(sboFinacialIndexT.ReserchAmt.Value / 1000).ToString();
                        viewModel.percent.Company = (sboFinacialIndexT.CurrentSale.Value == 0) ? "0" : Math.Round((sboFinacialIndexT.ReserchAmt.Value / sboFinacialIndexT.CurrentSale.Value * 100), 1).ToString();
                    }

                    //종합점수 조회하여 분류별로 딕셔너리 저장
                    var point = await reportUtil.GetCompanyTotalPoint(quesMaster.QuestionSn, sboFinacialIndexT);

                    if (point >= 0 && point <= 50)
                    { 
                        dicStartUpRnd.Add(compMapping.CompSn, sboFinacialIndexT.ReserchAmt.Value);
                        dicStartUpSales.Add(compMapping.CompSn, sboFinacialIndexT.CurrentSale.Value);
                    }
                    else if (point > 50 && point <= 75)
                    {
                        dicGrowthRnd.Add(compMapping.CompSn, sboFinacialIndexT.ReserchAmt.Value);
                        dicGrowthSales.Add(compMapping.CompSn, sboFinacialIndexT.CurrentSale.Value);
                    }
                    else
                    {
                        dicIndependentRnd.Add(compMapping.CompSn, sboFinacialIndexT.ReserchAmt.Value);
                        dicIndependentSales.Add(compMapping.CompSn, sboFinacialIndexT.CurrentSale.Value);
                    }
                }
            }


            viewModel.value.StartUpAvg = Math.Truncate(((dicStartUpRnd.Values.Sum() + 1095260958) /(dicStartUpRnd.Count + 19)) / 1000).ToString();
            viewModel.percent.StartUpAvg = Math.Round(((dicStartUpRnd.Values.Sum() + 1095260958) / (dicStartUpSales.Values.Sum() + 34089625773) * 100), 1).ToString();

            viewModel.value.GrowthAvg = Math.Truncate(((dicGrowthRnd.Values.Sum() + 3405773678) / (dicGrowthRnd.Count + 18)) / 1000).ToString();
            viewModel.percent.GrowthAvg = Math.Round(((dicGrowthRnd.Values.Sum() + 3405773678) / (dicGrowthSales.Values.Sum() + 124570092683) * 100), 1).ToString();

            viewModel.value.IndependentAvg = Math.Truncate(((dicIndependentRnd.Values.Sum() + 1998306254) / (dicIndependentRnd.Count + 2)) / 1000).ToString();
            viewModel.percent.IndependentAvg = Math.Round(((dicIndependentRnd.Values.Sum() + 1998306254) / (dicIndependentSales.Values.Sum() + 9817345650) * 100), 1).ToString();

            viewModel.value.BizInCompanyAvg = Math.Truncate(((dicIndependentRnd.Values.Sum() + dicGrowthRnd.Values.Sum() + dicStartUpRnd.Values.Sum()) / (dicIndependentRnd.Count + dicStartUpRnd.Count + dicGrowthRnd.Count)) / 1000).ToString();
            viewModel.percent.BizInCompanyAvg = Math.Round(((dicIndependentRnd.Values.Sum() + dicGrowthRnd.Values.Sum() + dicStartUpRnd.Values.Sum()) / (dicIndependentSales.Values.Sum() + dicGrowthSales.Values.Sum() + dicStartUpSales.Values.Sum()) * 100), 1).ToString();

            viewModel.value.TotalAvg = Math.Truncate(((dicIndependentRnd.Values.Sum() + dicGrowthRnd.Values.Sum() + dicStartUpRnd.Values.Sum() + 6499340890) / (dicIndependentRnd.Count + dicStartUpRnd.Count + dicGrowthRnd.Count + 39)) / 1000).ToString();
            viewModel.percent.TotalAvg = Math.Round(((dicIndependentRnd.Values.Sum() + dicGrowthRnd.Values.Sum() + dicStartUpRnd.Values.Sum() + 6499340890) / (dicIndependentSales.Values.Sum() + dicGrowthSales.Values.Sum() + dicStartUpSales.Values.Sum() + 168477064106) * 100), 1).ToString();



            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "10");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("10");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RndCost(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "10");

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
                return RedirectToAction("RndCost", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("RndEmp", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        public async Task<ActionResult> RndEmp(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

            var viewModel = new RndEmpViewModel();
            viewModel.rndEmpRatio = new CheckListViewModel();
            viewModel.rndEmpLevelRatio = new CheckListViewModel();

            //1) 현재 사업에 참여한 업체 평균
            //사업참여 기업들의 레벨(창업보육, 보육성장, 자립성장) 분류
            Dictionary<int, int> dicStartUpRndEmp = new Dictionary<int, int>();
            Dictionary<int, int> dicGrowthRndEmp = new Dictionary<int, int>();
            Dictionary<int, int> dicIndependentRndEmp = new Dictionary<int, int>();
            Dictionary<int, int> dicStartUpTotalEmp = new Dictionary<int, int>();
            Dictionary<int, int> dicGrowthTotalEmp = new Dictionary<int, int>();
            Dictionary<int, int> dicIndependentTotalEmp = new Dictionary<int, int>();
            Dictionary<int, int> dicStartUpHighRnd = new Dictionary<int, int>();
            Dictionary<int, int> dicGrowthHighRnd = new Dictionary<int, int>();
            Dictionary<int, int> dicIndependentHighRnd = new Dictionary<int, int>();

            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);
            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    var quesMaster = await quesMasterService.GetQuesMasterAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMaster == null)
                    {
                        continue;
                    }

                    //다래 재무정보 유무 체크하는 로직 추가해야함.(문진표정보, 재무정보가 있어야 보고서 생성가능.)
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    //연구개발 인력의 비율
                    var quesResult2sEmpRate = await quesResult2Service.GetQuesResult2sAsync(quesMaster.QuestionSn, "A1B102");
                    //전체임직원수
                    var TotalEmp = quesResult2sEmpRate.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10202");
                    //연구개발인력
                    var RndEmp = quesResult2sEmpRate.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10201");

                    //연구개발 인력의 능력
                    var quesResult2sEmpCapa = await quesResult2Service.GetQuesResult2sAsync(quesMaster.QuestionSn, "A1B103");
                    //박사급
                    var DoctorEmp = quesResult2sEmpCapa.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10301");
                    //석사급
                    var MasterEmp = quesResult2sEmpCapa.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10302");


                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {
                        if(TotalEmp.D == "0")
                        {
                            viewModel.rndEmpRatio.Company = "0";
                        }
                        else
                        { 
                            viewModel.rndEmpRatio.Company = Math.Round((double.Parse(RndEmp.D) / double.Parse(TotalEmp.D) * 100), 1).ToString();
                        }

                        if (RndEmp.D == "0")
                        {
                            viewModel.rndEmpLevelRatio.Company = "0";
                        }
                        else
                        {
                            viewModel.rndEmpLevelRatio.Company = Math.Round(((double.Parse(DoctorEmp.D) + double.Parse(MasterEmp.D)) / double.Parse(RndEmp.D) * 100), 1).ToString();
                        }
                    }

                    //종합점수 조회하여 분류별로 딕셔너리 저장
                    var point = await reportUtil.GetCompanyTotalPoint(quesMaster.QuestionSn, sboFinacialIndexT);

                    if (point >= 0 && point <= 50)
                    {
                        dicStartUpRndEmp.Add(compMapping.CompSn, int.Parse(RndEmp.D));
                        dicStartUpTotalEmp.Add(compMapping.CompSn, int.Parse(TotalEmp.D));
                        dicStartUpHighRnd.Add(compMapping.CompSn, int.Parse(DoctorEmp.D) + int.Parse(MasterEmp.D));
                    }
                    else if (point > 50 && point <= 75)
                    {
                        dicGrowthRndEmp.Add(compMapping.CompSn, int.Parse(RndEmp.D));
                        dicGrowthTotalEmp.Add(compMapping.CompSn, int.Parse(TotalEmp.D));
                        dicGrowthHighRnd.Add(compMapping.CompSn, int.Parse(DoctorEmp.D) + int.Parse(MasterEmp.D));
                    }
                    else
                    {
                        dicIndependentRndEmp.Add(compMapping.CompSn, int.Parse(RndEmp.D));
                        dicIndependentTotalEmp.Add(compMapping.CompSn, int.Parse(TotalEmp.D));
                        dicIndependentHighRnd.Add(compMapping.CompSn, int.Parse(DoctorEmp.D) + int.Parse(MasterEmp.D));
                    }
                }
            }


            viewModel.rndEmpRatio.StartUpAvg = Math.Round(((dicStartUpRndEmp.Values.Sum() + 45.0) / (dicStartUpTotalEmp.Values.Sum() + 207) * 100), 1).ToString();
            viewModel.rndEmpLevelRatio.StartUpAvg = Math.Round(((dicStartUpHighRnd.Values.Sum() + 22.0) / (dicStartUpRndEmp.Values.Sum() + 45) * 100), 1).ToString();

            viewModel.rndEmpRatio.GrowthAvg = Math.Round(((dicGrowthRndEmp.Values.Sum() + 107.0) / (dicGrowthTotalEmp.Values.Sum() + 439) * 100), 1).ToString();
            viewModel.rndEmpLevelRatio.GrowthAvg = Math.Round(((dicGrowthHighRnd.Values.Sum() + 50.0) / (dicGrowthRndEmp.Values.Sum() + 107) * 100), 1).ToString();

            viewModel.rndEmpRatio.IndependentAvg = Math.Round(((dicIndependentRndEmp.Values.Sum() + 30.0) / (dicIndependentTotalEmp.Values.Sum() + 79) * 100), 1).ToString();
            viewModel.rndEmpLevelRatio.IndependentAvg = Math.Round(((dicIndependentHighRnd.Values.Sum() + 15.0) / (dicIndependentRndEmp.Values.Sum() + 30) * 100), 1).ToString();


            if(dicStartUpTotalEmp.Values.Sum() + dicGrowthTotalEmp.Values.Sum() + dicIndependentTotalEmp.Values.Sum() == 0)
            {
                viewModel.rndEmpRatio.BizInCompanyAvg = "0";
            }
            else
            { 
                viewModel.rndEmpRatio.BizInCompanyAvg = Math.Round(((dicStartUpRndEmp.Values.Sum() + dicGrowthRndEmp.Values.Sum() + dicIndependentRndEmp.Values.Sum() + 0.0) / (dicStartUpTotalEmp.Values.Sum() + dicGrowthTotalEmp.Values.Sum() + dicIndependentTotalEmp.Values.Sum()) * 100), 1).ToString();
            }

            if (dicStartUpRndEmp.Values.Sum() + dicGrowthRndEmp.Values.Sum() + dicIndependentRndEmp.Values.Sum() == 0)
            {
                viewModel.rndEmpLevelRatio.BizInCompanyAvg = "0";
            }
            else
            {
                viewModel.rndEmpLevelRatio.BizInCompanyAvg = Math.Round(((dicStartUpHighRnd.Values.Sum() + dicGrowthHighRnd.Values.Sum() + dicIndependentHighRnd.Values.Sum() + 0.0) / (dicStartUpRndEmp.Values.Sum() + dicGrowthRndEmp.Values.Sum() + dicIndependentRndEmp.Values.Sum()) * 100), 1).ToString();
            }

            viewModel.rndEmpRatio.TotalAvg = Math.Round(((dicStartUpRndEmp.Values.Sum() + dicGrowthRndEmp.Values.Sum() + dicIndependentRndEmp.Values.Sum() + 182.0) / (dicStartUpTotalEmp.Values.Sum() + dicGrowthTotalEmp.Values.Sum() + dicIndependentTotalEmp.Values.Sum() + 725) * 100), 1).ToString();
            viewModel.rndEmpLevelRatio.TotalAvg = Math.Round(((dicStartUpHighRnd.Values.Sum() + dicGrowthHighRnd.Values.Sum() + dicIndependentHighRnd.Values.Sum() + 87.0) / (dicStartUpRndEmp.Values.Sum() + dicGrowthRndEmp.Values.Sum() + dicIndependentRndEmp.Values.Sum() + 182) * 100), 1).ToString();

            viewModel.rndEmpRatio.SMCompanyAvg = "17.1";



            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "11");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("11");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RndEmp(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "11");

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
                return RedirectToAction("RndEmp", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityCommercialize", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }




        public async Task<ActionResult> ProductivityResult(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

            var viewModel = new ProductivityResultViewModel();
            viewModel.BizResultCnt = new CheckListViewModel();
            viewModel.BizResultPoint = new CheckListViewModel();

            //1) 현재 사업에 참여한 업체 평균
            //사업참여 기업들의 레벨(창업보육, 보육성장, 자립성장) 분류
            Dictionary<int, int> dicStartUpCnt = new Dictionary<int, int>();
            Dictionary<int, int> dicGrowthCnt = new Dictionary<int, int>();
            Dictionary<int, int> dicIndependentCnt = new Dictionary<int, int>();
            Dictionary<int, double> dicStartUpPoint = new Dictionary<int, double>();
            Dictionary<int, double> dicGrowthPoint = new Dictionary<int, double>();
            Dictionary<int, double> dicIndependentPoint = new Dictionary<int, double>();

            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);
            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    var quesMaster = await quesMasterService.GetQuesMasterAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMaster == null)
                    {
                        continue;
                    }

                    //다래 재무정보 유무 체크하는 로직 추가해야함.(문진표정보, 재무정보가 있어야 보고서 생성가능.)
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    // A1B105 : 사업화실적
                    var quesResult2s = await quesResult2Service.GetQuesResult2sAsync(quesMaster.QuestionSn, "A1B105");
                    var BizResultCnt = quesResult2s.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1B10502");

                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {
                        viewModel.BizResultCnt.Company = (int.Parse(BizResultCnt.D) + int.Parse(BizResultCnt.D451) + int.Parse(BizResultCnt.D452)).ToString();

                        double avg = (int.Parse(BizResultCnt.D) + int.Parse(BizResultCnt.D451) + int.Parse(BizResultCnt.D452)) / 3.0;
                        viewModel.BizResultPoint.Company = Math.Round(ReportHelper.CalcPoint(ReportHelper.GetCodeTypeE(avg), 4), 1).ToString();
                    }


                    //종합점수 조회하여 분류별로 딕셔너리 저장
                    var point = await reportUtil.GetCompanyTotalPoint(quesMaster.QuestionSn, sboFinacialIndexT);

                    if (point >= 0 && point <= 50)
                    {
                        dicStartUpCnt.Add(compMapping.CompSn, (int.Parse(BizResultCnt.D) + int.Parse(BizResultCnt.D451) + int.Parse(BizResultCnt.D452)));
                    }
                    else if (point > 50 && point <= 75)
                    {
                        dicGrowthCnt.Add(compMapping.CompSn, (int.Parse(BizResultCnt.D) + int.Parse(BizResultCnt.D451) + int.Parse(BizResultCnt.D452)));
                    }
                    else
                    {
                        dicIndependentCnt.Add(compMapping.CompSn, (int.Parse(BizResultCnt.D) + int.Parse(BizResultCnt.D451) + int.Parse(BizResultCnt.D452)));
                    }
                }
            }

            //참업보육 평균
            viewModel.BizResultCnt.StartUpAvg = Math.Round((dicStartUpCnt.Values.Sum() + 46) / (dicStartUpCnt.Count + 19.0), 1).ToString();
            double startUpSum = 0;
            foreach(var item in dicStartUpCnt.Values)
            {
                startUpSum = startUpSum + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeE(item / 3.0), 4);
            }
            viewModel.BizResultPoint.StartUpAvg = Math.Round((startUpSum + 4) / (dicStartUpCnt.Count + 19.0), 1).ToString();
            //보육성장 평균
            viewModel.BizResultCnt.GrowthAvg = Math.Round((dicGrowthCnt.Values.Sum() + 72) / (dicGrowthCnt.Count + 18.0), 1).ToString();
            double growthSum = 0;
            foreach (var item in dicGrowthCnt.Values)
            {
                growthSum = growthSum + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeE(item / 3.0), 4);
            }
            viewModel.BizResultPoint.GrowthAvg = Math.Round((growthSum + 8) / (dicGrowthCnt.Count + 18.0), 1).ToString();
            //자립성장 평균
            viewModel.BizResultCnt.IndependentAvg = Math.Round((dicIndependentCnt.Values.Sum() + 4) / (dicIndependentCnt.Count + 2.0), 1).ToString();
            double independentSum = 0;
            foreach (var item in dicIndependentCnt.Values)
            {
                independentSum = independentSum + ReportHelper.CalcPoint(ReportHelper.GetCodeTypeE(item / 3.0), 4);
            }
            viewModel.BizResultPoint.IndependentAvg = Math.Round((independentSum + 0) / (dicIndependentCnt.Count + 2.0), 1).ToString();
            //참여기업 평균
            viewModel.BizResultCnt.BizInCompanyAvg = Math.Round((dicIndependentCnt.Values.Sum() + dicStartUpCnt.Values.Sum() + dicGrowthCnt.Values.Sum() + 0.0) / (dicIndependentCnt.Count + dicStartUpCnt.Count + dicGrowthCnt.Count), 1).ToString();
            viewModel.BizResultPoint.BizInCompanyAvg = Math.Round((independentSum + startUpSum + growthSum) / (dicIndependentCnt.Count + dicGrowthCnt.Count + dicStartUpCnt.Count + 0.0), 1).ToString();

            //전체 평균
            viewModel.BizResultCnt.TotalAvg = Math.Round((dicIndependentCnt.Values.Sum() + dicStartUpCnt.Values.Sum() + dicGrowthCnt.Values.Sum() + 122.0) / (dicIndependentCnt.Count + dicStartUpCnt.Count + dicGrowthCnt.Count + 39.0), 1).ToString();
            viewModel.BizResultPoint.TotalAvg = Math.Round((independentSum + startUpSum + growthSum + 12) / (dicIndependentCnt.Count + dicGrowthCnt.Count + dicStartUpCnt.Count + 39.0), 1).ToString();


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "11");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("11");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductivityResult(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "11");

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
                return RedirectToAction("RndEmp", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityMgmtFacility", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }



        public async Task<ActionResult> ProductivityProfitability(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

            ProductivityProfitabilityViewModel viewModel = new ProductivityProfitabilityViewModel();
            viewModel.Profitability = new BarChartViewModel();
            viewModel.Growth = new BarChartViewModel();

            //1) 현재 사업에 참여한 업체 평균
            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);

            //당기매출액
            Dictionary<string, decimal> dicSales = new Dictionary<string, decimal>();
            //영업이익
            Dictionary<string, decimal> dicOperatingEarning = new Dictionary<string, decimal>();
            //전기매출액
            Dictionary<string, decimal> dicPrevSales = new Dictionary<string, decimal>();

            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    //문진표 작성내역 조회
                    var quesMaster = await quesMasterService.GetQuesOgranAnalysisAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMaster == null)
                    {
                        continue;
                    }
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    //해당기업을 찾아 점수를 별도로 저장한다.
                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {

                        viewModel.Profitability.Dividend = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.OperatingEarning.Value / 1000));
                        viewModel.Profitability.Divisor = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.CurrentSale.Value / 1000));
                        viewModel.Profitability.Result = (sboFinacialIndexT.CurrentSale.Value == 0) ? 0 : Math.Round(Convert.ToDouble(sboFinacialIndexT.OperatingEarning.Value / sboFinacialIndexT.CurrentSale.Value * 100), 1);
                        viewModel.Profitability.Company = viewModel.Profitability.Result;
                        viewModel.Profitability.AvgSMCompany = 5.2;

                        viewModel.Growth.Dividend = Math.Truncate(Convert.ToDouble((sboFinacialIndexT.CurrentSale.Value - sboFinacialIndexT.PrevSale.Value) / 1000));
                        viewModel.Growth.Divisor = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.PrevSale.Value / 1000));
                        viewModel.Growth.Result = (sboFinacialIndexT.PrevSale.Value == 0) ? 0 : Math.Round(Convert.ToDouble((sboFinacialIndexT.CurrentSale.Value - sboFinacialIndexT.PrevSale.Value) / sboFinacialIndexT.PrevSale.Value * 100), 1);
                        viewModel.Growth.Company = viewModel.Growth.Result;
                        viewModel.Growth.AvgSMCompany = 4.9;
                    }

                    dicSales.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentSale.Value);
                    dicOperatingEarning.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.OperatingEarning.Value);
                    dicPrevSales.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.PrevSale.Value);

                }
            }

            //평균값 계산
            viewModel.Profitability.AvgBizInCompany = (dicSales.Values.Sum() == 0) ? 0 : Math.Round(Convert.ToDouble(dicOperatingEarning.Values.Sum() / dicSales.Values.Sum() * 100), 1);
            viewModel.Profitability.AvgTotal = Math.Round(Convert.ToDouble((dicOperatingEarning.Values.Sum() + 6748926334) / (dicSales.Values.Sum() + 111666772288) * 100), 1);

            viewModel.Growth.AvgBizInCompany = (dicPrevSales.Values.Sum() == 0) ? 0 : Math.Round(Convert.ToDouble((dicSales.Values.Sum() - dicPrevSales.Values.Sum()) / dicPrevSales.Values.Sum() * 100), 1);
            viewModel.Growth.AvgTotal = Math.Round(Convert.ToDouble(((dicSales.Values.Sum() - dicPrevSales.Values.Sum()) + 9517105574) / (dicPrevSales.Values.Sum() + 102192958532) * 100), 1);


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "19");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("19");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);

        }

        [HttpPost]
        public async Task<ActionResult> ProductivityProfitability(OrgProductivityViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "19");

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
                return RedirectToAction("ProductivityProfitability", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityValueChain", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }



        public async Task<ActionResult> RiskMgmtOrgSatisfaction(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

            var viewModel = new RiskMgmtOrgSatisfactionViewModel();
            viewModel.orgSatisfaction = new CheckListViewModel();

            //1) 현재 사업에 참여한 업체 평균
            //사업참여 기업들의 레벨(창업보육, 보육성장, 자립성장) 분류
            Dictionary<int, int> dicStartUpTotal = new Dictionary<int, int>();
            Dictionary<int, int> dicStartUpMove = new Dictionary<int, int>();

            Dictionary<int, int> dicGrowthTotal = new Dictionary<int, int>();
            Dictionary<int, int> dicGrowthMove = new Dictionary<int, int>();

            Dictionary<int, int> dicIndependentTotal = new Dictionary<int, int>();
            Dictionary<int, int> dicIndependentMove = new Dictionary<int, int>();

            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);
            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    var quesMaster = await quesMasterService.GetQuesMasterAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMaster == null)
                    {
                        continue;
                    }

                    //다래 재무정보 유무 체크하는 로직 추가해야함.(문진표정보, 재무정보가 있어야 보고서 생성가능.)
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    // A1A202 : 조직만족도
                    var quesResult2s = await quesResult2Service.GetQuesResult2sAsync(quesMaster.QuestionSn, "A1A202");
                    //총직원
                    var totalEmp = quesResult2s.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1A20201");
                    //이직직원
                    var moveEmp = quesResult2s.SingleOrDefault(i => i.QuesCheckList.DetailCd == "A1A20202");

                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {
                        

                        if (totalEmp.D451 == "0")
                        {
                            viewModel.orgSatisfaction.Company = Math.Round(Convert.ToDouble((int.Parse(moveEmp.D) / double.Parse(totalEmp.D)) * 100), 1).ToString();

                        }
                        else if (totalEmp.D452 == "0")
                        {
                            viewModel.orgSatisfaction.Company = Math.Round(Convert.ToDouble((((int.Parse(moveEmp.D) / double.Parse(totalEmp.D)) + (int.Parse(moveEmp.D451) / double.Parse(totalEmp.D451))) / 2) * 100), 1).ToString();
                        }
                        else
                        {
                            viewModel.orgSatisfaction.Company = Math.Round(Convert.ToDouble((((int.Parse(moveEmp.D) / double.Parse(totalEmp.D)) + (int.Parse(moveEmp.D451) / double.Parse(totalEmp.D451)) + (int.Parse(moveEmp.D452) / double.Parse(totalEmp.D452))) / 3) * 100), 1).ToString();
                        }
                    }

                    //종합점수 조회하여 분류별로 딕셔너리 저장
                    var point = await reportUtil.GetCompanyTotalPoint(quesMaster.QuestionSn, sboFinacialIndexT);

                    if (point >= 0 && point <= 50)
                    {
                        dicStartUpTotal.Add(compMapping.CompSn, int.Parse(totalEmp.D));
                        dicStartUpMove.Add(compMapping.CompSn, int.Parse(moveEmp.D));
                    }
                    else if (point > 50 && point <= 75)
                    {
                        dicGrowthTotal.Add(compMapping.CompSn, int.Parse(totalEmp.D));
                        dicGrowthMove.Add(compMapping.CompSn, int.Parse(moveEmp.D));
                    }
                    else
                    {
                        dicIndependentTotal.Add(compMapping.CompSn, int.Parse(totalEmp.D));
                        dicIndependentMove.Add(compMapping.CompSn, int.Parse(moveEmp.D));
                    }
                }
            }

            viewModel.orgSatisfaction.StartUpAvg = Math.Round(Convert.ToDouble(((dicStartUpMove.Values.Sum() + 28.0) / (dicStartUpTotal.Values.Sum() + 207)) * 100), 1).ToString();

            viewModel.orgSatisfaction.GrowthAvg = Math.Round(Convert.ToDouble(((dicGrowthMove.Values.Sum() + 57.0) / (dicGrowthTotal.Values.Sum() + 443)) * 100), 1).ToString();

            viewModel.orgSatisfaction.IndependentAvg = Math.Round(Convert.ToDouble(((dicIndependentMove.Values.Sum() + 12.0) / (dicIndependentTotal.Values.Sum() + 79)) * 100), 1).ToString();

            viewModel.orgSatisfaction.BizInCompanyAvg = Math.Round(Convert.ToDouble(((dicStartUpMove.Values.Sum() + dicGrowthMove.Values.Sum() + dicIndependentMove.Values.Sum() + 0.0) / (dicStartUpTotal.Values.Sum() + dicGrowthTotal.Values.Sum() + dicIndependentTotal.Values.Sum())) * 100), 1).ToString();

            viewModel.orgSatisfaction.TotalAvg = Math.Round(Convert.ToDouble(((dicStartUpMove.Values.Sum() + dicGrowthMove.Values.Sum() + dicIndependentMove.Values.Sum() + 97.0) / (dicStartUpTotal.Values.Sum() + dicGrowthTotal.Values.Sum() + dicIndependentTotal.Values.Sum() + 729)) * 100), 1).ToString();


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "28");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("28");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RiskMgmtOrgSatisfaction(OrgHR01ViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "28");

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
                return RedirectToAction("RiskMgmtOrgSatisfaction", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("RiskMgmtITSystem", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }



        public async Task<ActionResult> RiskMgmtLiquidity(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

            RiskMgmtLiquidityViewModel viewModel = new RiskMgmtLiquidityViewModel();
            viewModel.Liquidity = new BarChartViewModel();
            viewModel.Stability = new BarChartViewModel();

            //1) 현재 사업에 참여한 업체 평균
            var curBizWork = await scBizWorkService.GetBizWorkByBizWorkSn(paramModel.BizWorkSn);

            //유동자산
            Dictionary<string, decimal> dicCurrentAsset = new Dictionary<string, decimal>();
            //유동부채
            Dictionary<string, decimal> dicCurrentLiability = new Dictionary<string, decimal>();
            //부채총계
            Dictionary<string, decimal> dicTotalLiability = new Dictionary<string, decimal>();
            //자본총계
            Dictionary<string, decimal> dicTotalCapital = new Dictionary<string, decimal>();

            {
                var compMappings = curBizWork.ScCompMappings;
                foreach (var compMapping in compMappings)
                {
                    //문진표 작성내역 조회
                    var quesMaster = await quesMasterService.GetQuesOgranAnalysisAsync(compMapping.ScCompInfo.RegistrationNo, paramModel.BizWorkYear);
                    if (quesMaster == null)
                    {
                        continue;
                    }
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    //해당기업을 찾아 점수를 별도로 저장한다.
                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {

                        viewModel.Liquidity.Dividend = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.CurrentAsset.Value / 1000));
                        viewModel.Liquidity.Divisor = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.CurrentLiability.Value / 1000));
                        viewModel.Liquidity.Result = (sboFinacialIndexT.CurrentLiability.Value == 0) ? 0 : Math.Round(Convert.ToDouble(sboFinacialIndexT.CurrentAsset.Value / sboFinacialIndexT.CurrentLiability.Value * 100), 1);
                        viewModel.Liquidity.Company = viewModel.Liquidity.Result;
                        viewModel.Liquidity.AvgSMCompany = 136.3;

                        viewModel.Stability.Dividend = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.TotalLiability.Value / 1000));
                        viewModel.Stability.Divisor = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.TotalCapital.Value / 1000));
                        viewModel.Stability.Result = (sboFinacialIndexT.TotalCapital.Value == 0) ? 0 : Math.Round(Convert.ToDouble(sboFinacialIndexT.TotalLiability.Value / sboFinacialIndexT.TotalCapital.Value * 100), 1);
                        viewModel.Stability.Company = viewModel.Stability.Result;
                        viewModel.Stability.AvgSMCompany = 141.7;
                    }

                    dicCurrentAsset.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentSale.Value);
                    dicCurrentLiability.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.OperatingEarning.Value);
                    dicTotalLiability.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.PrevSale.Value);
                    dicTotalCapital.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.PrevSale.Value);

                }
            }

            //평균값 계산
            viewModel.Liquidity.AvgBizInCompany = (dicCurrentLiability.Values.Sum() == 0) ? 0 : Math.Round(Convert.ToDouble(dicCurrentAsset.Values.Sum() / dicCurrentLiability.Values.Sum() * 100), 1);
            viewModel.Liquidity.AvgTotal = Math.Round(Convert.ToDouble((dicCurrentAsset.Values.Sum() + 26161408957) / (dicCurrentLiability.Values.Sum() + 5869940384) * 100), 1);

            viewModel.Stability.AvgBizInCompany = (dicTotalCapital.Values.Sum() == 0) ? 0 : Math.Round(Convert.ToDouble(dicTotalLiability.Values.Sum() / dicTotalCapital.Values.Sum() * 100), 1);
            viewModel.Stability.AvgTotal = Math.Round(Convert.ToDouble((dicTotalLiability.Values.Sum() + 21887099526) / (dicTotalCapital.Values.Sum() + 24998683648) * 100), 1);


            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "30");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("30");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList, listRptMentorComment);

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;
            return View(viewModel);

        }

        [HttpPost]
        public async Task<ActionResult> RiskMgmtLiquidity(OrgProductivityViewModel viewModel, BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "30");

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
                return RedirectToAction("RiskMgmtLiquidity", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("RiskMgmtEvalProfession", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        #region 2. 기초역량 검토 종합결과

        //P12 2.상품화역량 - 사업화역량
        public async Task<ActionResult> ProductivityCommercialize(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
                return RedirectToAction("ProductivityResult", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        //P14 2.상품화역량 - 생산설비의 운영체계 및 관리
        public async Task<ActionResult> ProductivityMgmtFacility(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
                return RedirectToAction("ProductivityProfitability", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        // p20 2.상품화 역량 -  역량별 검토결과 - 타깃고객검토
        public async Task<ActionResult> ProductivityTargetCustomer(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "20");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("20");

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
        public async Task<ActionResult> ProductivityTargetCustomer(BasicSurveyReportViewModel paramModel, RiskMgmtViewModel viewModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "20");

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
                return RedirectToAction("ProductivityTargetCustomer", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityValueChain", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        // p21 2.상품화 역량 -  역량별 검토결과 - 상품화구조 Check
        public async Task<ActionResult> ProductivityValueChain(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            RiskMgmtViewModel viewModel = new RiskMgmtViewModel();

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "21");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("21");

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
        public async Task<ActionResult> ProductivityValueChain(BasicSurveyReportViewModel paramModel, RiskMgmtViewModel viewModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "21");

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
                return RedirectToAction("ProductivityValueChain", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityRelation", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        // p22 2.상품화 역량 -  역량별 검토결과 - 제품생산.판매 관계망검토
        public async Task<ActionResult> ProductivityRelation(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            ProductivityRelationViewModel viewModel = new ProductivityRelationViewModel();

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "22");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("22");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList.Where(cl => cl.Type == "C"), listRptMentorComment.Where(rmc => rmc.RptCheckList.Type == "C").ToList());


            //SCP 입력값 가져오는 로직 있어야함(테이블도 생성해야함)
            //검토결과 데이터 생성
            var listRptMngComment = await rptMngCommentService.GetRptMngCommentListAsync(paramModel.BizWorkYear, "22");

            //레포트 체크리스트
            var enumRptMngCode = await rptMngCodeService.GetRptMngCodeBySmallClassCd("22");

            //CommentList 채우기
            var MngCommentList = ReportHelper.MakeCommentViewModel(enumRptMngCode, listRptMngComment.ToList());


            viewModel.CommentList = CommentList;
            viewModel.MngComment = MngCommentList;

            ViewBag.paramModel = paramModel;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductivityRelation(BasicSurveyReportViewModel paramModel, ProductivityRelationViewModel viewModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "22");

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
            try {
                await rptMentorCommentService.SaveDbContextAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            if (viewModel.SubmitType == "T") //임시저장
            {
                return RedirectToAction("ProductivityRelation", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("ProductivityRelation2", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }

        // p23 2.상품화 역량 -  역량별 검토결과 - 제품생산.판매 관계망검토
        public async Task<ActionResult> ProductivityRelation2(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            ProductivityRelationViewModel viewModel = new ProductivityRelationViewModel();

            //검토결과 데이터 생성
            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "23");

            //레포트 체크리스트
            var enumRptCheckList = await rptCheckListService.GetRptCheckListBySmallClassCd("23");

            //CommentList 채우기
            var CommentList = ReportHelper.MakeCommentViewModel(enumRptCheckList.Where(cl => cl.Type == "C"), listRptMentorComment.Where(rmc => rmc.RptCheckList.Type == "C").ToList());


            //SCP 입력값 가져오는 로직 있어야함(테이블도 생성해야함)

            viewModel.CommentList = CommentList;

            ViewBag.paramModel = paramModel;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ProductivityRelation2(BasicSurveyReportViewModel paramModel, ProductivityRelationViewModel viewModel)
        {
            ViewBag.LeftMenu = Global.Report;

            var listRptMentorComment = await rptMentorCommentService.GetRptMentorCommentListAsync(paramModel.QuestionSn, paramModel.BizWorkSn, paramModel.BizWorkYear, "23");

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
            try
            {
                await rptMentorCommentService.SaveDbContextAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (viewModel.SubmitType == "T") //임시저장
            {
                return RedirectToAction("ProductivityRelation2", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
            else
            {
                return RedirectToAction("RiskMgmtVisionStrategy", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        //P24 3.위험관리역량 - [CEO역량]경영목표 및 전략
        public async Task<ActionResult> RiskMgmtVisionStrategy(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
                return RedirectToAction("RiskMgmtOrgSatisfaction", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }



        //P29 3.위험관리역량 - 정보시스템활용
        public async Task<ActionResult> RiskMgmtITSystem(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;


            ReportUtil reportUtil = new ReportUtil(scBizWorkService, quesResult1Service, quesResult2Service, quesMasterService, sboFinancialIndexTService);

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
            ViewBag.LeftMenu = Global.Report;

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
                return RedirectToAction("RiskMgmtLiquidity", "BasicSurveyReport", new { BizWorkSn = paramModel.BizWorkSn, CompSn = paramModel.CompSn, BizWorkYear = paramModel.BizWorkYear, Status = paramModel.Status, QuestionSn = paramModel.QuestionSn });
            }
        }


        //31p 3.위험관리 역량 - 31p. 전문가 평가
        public async Task<ActionResult> RiskMgmtEvalProfession(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;

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
        //32p
        public ActionResult GrowthRoadMapCover(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

            return View(paramModel);

        }


        //p33 유형별 성장전략
        public async Task<ActionResult> GrowthStrategyType(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;

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

        //p34 단계 성장전략
        public async Task<ActionResult> GrowthStrategyStep(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;

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

        //p35 역량강화제안
        public async Task<ActionResult> GrowthCapabilityProposal(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;

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

        //p36 회사핵심내용
        public async Task<ActionResult> GrowthTotalProposal(BasicSurveyReportViewModel paramModel)
        {
            ViewBag.LeftMenu = Global.Report;

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
            ViewBag.LeftMenu = Global.Report;

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
                return RedirectToAction("BasicSurveyReport", "Report", new { area = "Mentor" });
            }

        }
        #endregion


        

        [HttpPost]
        public async Task<JsonResult> CheckFinanceData(int CompSn, int BasicYear)
        {
            var compInfo = await scCompInfoService.GetScCompInfoByCompSn(CompSn);
            //다래 재무정보 조회해야 함.
            var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compInfo.RegistrationNo, ConfigurationManager.AppSettings["CorpCode"], ConfigurationManager.AppSettings["BizCode"], BasicYear.ToString());
            if (sboFinacialIndexT != null)
            {
                return Json(new { result = true });
            }
            else
            {
                return Json(new { result = false });
            }
        }
    }
}