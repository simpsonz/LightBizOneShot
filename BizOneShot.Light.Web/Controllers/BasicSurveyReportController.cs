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
            ISboFinancialIndexTService sboFinancialIndexTService)
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
            this.sboFinancialIndexTService = sboFinancialIndexTService;
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
            OverallSummaryViewModel viewModel = new OverallSummaryViewModel();
            viewModel.CommentList = new List<CommentViewModel>();
            ReportUtil reportUtil = new ReportUtil(quesResult1Service, quesResult2Service, quesMasterService);

            //해당기업 기초역량
            double basicCapa = 0.0;
            //해당기업 기술경영 마케팅관리
            double mkt = 0.0;
            //해당기업 인적자원관리
            double hrMng = 0.0;
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
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, "1000", "1100", paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }


                    //참여기업의 점수 계산
                    var bizInHrMng = await reportUtil.GetHumanResourceMng(quesMaster.QuestionSn, sboFinacialIndexT);
                    var bizInMkt = await reportUtil.GetTechMng(quesMaster.QuestionSn, sboFinacialIndexT);
                    var bizInBasicCapa = await reportUtil.GetOverAllManagementTotalPoint(quesMaster.QuestionSn);

                    //해당기업을 찾아 점수를 별도로 저장한다.
                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {
                        basicCapa = bizInBasicCapa;
                        mkt = bizInMkt;
                        hrMng = bizInHrMng;
                        workProductivity = Math.Truncate(Convert.ToDouble(((sboFinacialIndexT.CurrentSale - sboFinacialIndexT.MaterialCost) / sboFinacialIndexT.QtEmp) / 1000));
                        salesEarning = Math.Round(Convert.ToDouble((sboFinacialIndexT.OperatingEarning / sboFinacialIndexT.CurrentSale) * 100), 1);
                        current = Math.Round(Convert.ToDouble((sboFinacialIndexT.CurrentAsset / sboFinacialIndexT.CurrentLiability) * 100), 1);
                    }

                    dicBizInHrMng.Add(compMapping.ScCompInfo.RegistrationNo, bizInHrMng);
                    dicBizInMkt.Add(compMapping.ScCompInfo.RegistrationNo, bizInMkt);
                    dicBizInBasicCpas.Add(compMapping.ScCompInfo.RegistrationNo, bizInBasicCapa);

                    dicSales.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentSale.Value);
                    dicMaterrial.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.MaterialCost.Value);
                    dicQtEmp.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.QtEmp.Value);
                    dicOperatingErning.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.OperatingEarning.Value);
                    dicCurrentAsset.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentAsset.Value);
                    dicCurrentLiability.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentLiability.Value);

                }
            }

            // 3) 기초자료 전체 평균
            // 설명자료에 해당 내용 없음.

            // 4) 전체 평균정수 계산
            double totalPoint = 0;
            totalPoint = totalPoint + dicBizInHrMng.Values.Sum();
            totalPoint = totalPoint + dicBizInMkt.Values.Sum();
            totalPoint = totalPoint + dicBizInBasicCpas.Values.Sum();
            viewModel.AvgTotalPoint = Math.Round(totalPoint / dicBizInHrMng.Count, 1);

            //1-B. 해당 기업의 기초역량 점수 계산
            double companyPoint = 0;
            companyPoint = basicCapa + mkt + hrMng;
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
            viewModel.OrgType = "A";
            //7. 고객의수, 상품의질 화살표 -------------> 해당 페이지 개발 후 적용 해야함.
            viewModel.CustMngType = "A";
            //8. 전반적 제도 및 규정관리체계 화살표 -------------> 해당 페이지 개발 후 적용 해야함.
            viewModel.RoolType = "A";

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
            orgCapa.AvgBizInCompanyPoint2 = Math.Truncate(Convert.ToDouble(((dicSales.Values.Sum() - dicMaterrial.Values.Sum()) / dicQtEmp.Values.Sum()) / 1000));
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
            prductionCapa.AvgBizInCompanyPoint2 = Math.Round(Convert.ToDouble((dicOperatingErning.Values.Sum() / dicSales.Values.Sum()) * 100), 1);
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
            riskMngCapa.AvgBizInCompanyPoint2 = Math.Round(Convert.ToDouble((dicCurrentAsset.Values.Sum() / dicCurrentLiability.Values.Sum()) * 100), 1);
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

            ReportUtil reportUtil = new ReportUtil(quesResult1Service, quesResult2Service, quesMasterService);

            OrgHR01ViewModel viewModel = new OrgHR01ViewModel();
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
                    //다래 재무정보 조회해야 함.
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, "1000", "1100", paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    //종합점수 조회하여 분류별로 딕셔너리 저장
                    var point = await reportUtil.GetCompanyTotalPoint(quesMasters.QuestionSn, sboFinacialIndexT);

                    if (point >= 0 && point <= 50)
                        dicStartUp.Add(compMapping.CompSn, quesMasters.QuestionSn);
                    else if (point > 50 && point <= 75)
                        dicGrowth.Add(compMapping.CompSn, quesMasters.QuestionSn);
                    else
                        dicIndependent.Add(compMapping.CompSn, quesMasters.QuestionSn);
                }
            }



            //리스트 데이터 생성
            var quesResult1s = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1D101");

            int count = 1;
            foreach(var item in quesResult1s)
            {
                CheckListViewModel checkListViewModel = new CheckListViewModel();
                checkListViewModel.Count = count.ToString();
                checkListViewModel.AnsVal = item.AnsVal.Value;
                checkListViewModel.DetailCd = item.QuesCheckList.DetailCd;
                checkListViewModel.Title = item.QuesCheckList.ReportTitle;
                //창업보육단계 평균
                int startUpCnt =  await reportUtil.GetCheckListCnt(dicStartUp, checkListViewModel.DetailCd);
                checkListViewModel.StartUpAvg = Math.Round(((startUpCnt + item.QuesCheckList.StartUpStep.Value + 0.0) / ( 39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                //보육성장단계 평균
                int growthCnt = await reportUtil.GetCheckListCnt(dicGrowth, checkListViewModel.DetailCd);
                checkListViewModel.GrowthAvg = Math.Round(((growthCnt + item.QuesCheckList.GrowthStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                //자립성장단계 평균
                int IndependentCnt = await reportUtil.GetCheckListCnt(dicIndependent, checkListViewModel.DetailCd);
                checkListViewModel.IndependentAvg = Math.Round(((IndependentCnt + item.QuesCheckList.IndependentStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                //참여기업 평균
                checkListViewModel.BizInCompanyAvg = Math.Round(((IndependentCnt + growthCnt + startUpCnt + 0.0) / (dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                //전체 평균
                checkListViewModel.TotalAvg = Math.Round(((IndependentCnt + growthCnt + startUpCnt + item.QuesCheckList.TotalStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                viewModel.CheckList.Add(checkListViewModel);
                count++;
            }

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

            ReportUtil reportUtil = new ReportUtil(quesResult1Service, quesResult2Service, quesMasterService);

            OrgHR01ViewModel viewModel = new OrgHR01ViewModel();
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
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, "1000", "1100", paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    //종합점수 조회하여 분류별로 딕셔너리 저장
                    var point = await reportUtil.GetCompanyTotalPoint(quesMasters.QuestionSn, sboFinacialIndexT);

                    if (point >= 0 && point <= 50)
                        dicStartUp.Add(compMapping.CompSn, quesMasters.QuestionSn);
                    else if (point > 50 && point <= 75)
                        dicGrowth.Add(compMapping.CompSn, quesMasters.QuestionSn);
                    else
                        dicIndependent.Add(compMapping.CompSn, quesMasters.QuestionSn);
                }
            }



            //리스트 데이터 생성
            var quesResult1s = await quesResult1Service.GetQuesResult1sAsync(paramModel.QuestionSn, "A1D102");

            int count = 1;
            foreach (var item in quesResult1s)
            {
                CheckListViewModel checkListViewModel = new CheckListViewModel();
                checkListViewModel.Count = count.ToString();
                checkListViewModel.AnsVal = item.AnsVal.Value;
                checkListViewModel.DetailCd = item.QuesCheckList.DetailCd;
                checkListViewModel.Title = item.QuesCheckList.ReportTitle;
                //창업보육단계 평균
                int startUpCnt = await reportUtil.GetCheckListCnt(dicStartUp, checkListViewModel.DetailCd);
                checkListViewModel.StartUpAvg = Math.Round(((startUpCnt + item.QuesCheckList.StartUpStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                //보육성장단계 평균
                int growthCnt = await reportUtil.GetCheckListCnt(dicGrowth, checkListViewModel.DetailCd);
                checkListViewModel.GrowthAvg = Math.Round(((growthCnt + item.QuesCheckList.GrowthStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                //자립성장단계 평균
                int IndependentCnt = await reportUtil.GetCheckListCnt(dicIndependent, checkListViewModel.DetailCd);
                checkListViewModel.IndependentAvg = Math.Round(((IndependentCnt + item.QuesCheckList.IndependentStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                //참여기업 평균
                checkListViewModel.BizInCompanyAvg = Math.Round(((IndependentCnt + growthCnt + startUpCnt + 0.0) / (dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                //전체 평균
                checkListViewModel.TotalAvg = Math.Round(((IndependentCnt + growthCnt + startUpCnt + item.QuesCheckList.TotalStep.Value + 0.0) / (39 + dicStartUp.Count + dicGrowth.Count + dicIndependent.Count)) * 100, 0).ToString();
                viewModel.CheckList.Add(checkListViewModel);
                count++;
            }

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

            ReportUtil reportUtil = new ReportUtil(quesResult1Service, quesResult2Service, quesMasterService);
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
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, "1000", "1100", paramModel.BizWorkYear.ToString());
                    if (sboFinacialIndexT == null)
                    {
                        continue;
                    }

                    //해당기업을 찾아 점수를 별도로 저장한다.
                    if (quesMaster.QuestionSn == paramModel.QuestionSn)
                    {

                        viewModel.Productivity.Dividend = Math.Truncate(Convert.ToDouble((sboFinacialIndexT.CurrentSale.Value - sboFinacialIndexT.MaterialCost.Value) / 1000));
                        viewModel.Productivity.Divisor = Math.Round(Convert.ToDouble(sboFinacialIndexT.QtEmp.Value), 0);
                        viewModel.Productivity.Result = Math.Truncate(viewModel.Productivity.Dividend / viewModel.Productivity.Divisor);
                        viewModel.Productivity.Company = viewModel.Productivity.Result;
                        viewModel.Productivity.AvgSMCompany = 135547;

                        viewModel.Activity.Dividend = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.CurrentSale.Value / 1000));
                        viewModel.Activity.Divisor = Math.Truncate(Convert.ToDouble(sboFinacialIndexT.TotalAsset.Value / 1000));
                        viewModel.Activity.Result = Math.Round((viewModel.Activity.Dividend / viewModel.Activity.Divisor) * 100, 1);
                        viewModel.Activity.Company = viewModel.Activity.Result;
                        viewModel.Activity.AvgSMCompany = 114.8;
                    }

                    dicSales.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.CurrentSale.Value);
                    dicMaterrial.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.MaterialCost.Value);
                    dicQtEmp.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.QtEmp.Value);
                    dicTotalAsset.Add(compMapping.ScCompInfo.RegistrationNo, sboFinacialIndexT.TotalAsset.Value);

                }
            }
            
            //평균값 계산
            viewModel.Productivity.AvgBizInCompany = Math.Truncate(Convert.ToDouble(((dicSales.Values.Sum() - dicMaterrial.Values.Sum()) / dicQtEmp.Values.Sum()) / 1000));
            viewModel.Productivity.AvgTotal = Math.Truncate(Convert.ToDouble((((dicSales.Values.Sum() - dicMaterrial.Values.Sum()) + 68138995337) / (dicQtEmp.Values.Sum() + 718)) / 1000));

            viewModel.Activity.AvgBizInCompany = Math.Round(Convert.ToDouble(dicSales.Values.Sum() / dicTotalAsset.Values.Sum() * 100));
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

            ReportUtil reportUtil = new ReportUtil(quesResult1Service, quesResult2Service, quesMasterService);
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
                    var sboFinacialIndexT = await sboFinancialIndexTService.GetSHUSER_SboFinancialIndexT(compMapping.ScCompInfo.RegistrationNo, "1000", "1100", paramModel.BizWorkYear.ToString());
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

            //기획관리 평균값 생성
            if(viewModel.TotalSumCount == 0)
            {
                viewModel.Management.Company = 0;
                viewModel.Produce.Company = 0;
                viewModel.RND.Company = 0;
                viewModel.Salse.Company = 0;
                viewModel.CompanySum = 0;
            }
            else
            { 
                viewModel.Management.Company = Math.Round(Convert.ToDouble((viewModel.Management.PartialSum / viewModel.TotalSumCount)) * 100, 1);
                viewModel.Produce.Company = Math.Round(Convert.ToDouble((viewModel.Produce.PartialSum / viewModel.TotalSumCount)) * 100, 1);
                viewModel.RND.Company = Math.Round(Convert.ToDouble((viewModel.RND.PartialSum / viewModel.TotalSumCount)) * 100, 1);
                viewModel.Salse.Company = Math.Round(Convert.ToDouble((viewModel.Salse.PartialSum / viewModel.TotalSumCount)) * 100, 1);
                viewModel.CompanySum = 100;
            }

            if((dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum()) == 0)
            {
                viewModel.Management.AvgBizInCompany = 0;
                viewModel.Produce.AvgBizInCompany = 0;
                viewModel.RND.AvgBizInCompany = 0;
                viewModel.Salse.AvgBizInCompany = 0;
                viewModel.AvgBizInCompanySum = 0;
            }
            else
            { 
                viewModel.Management.AvgBizInCompany = Math.Round(Convert.ToDouble((dicManagement.Values.Sum() / (dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum()))) * 100, 1);
                viewModel.Produce.AvgBizInCompany = Math.Round(Convert.ToDouble((dicProduce.Values.Sum() / (dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum()))) * 100, 1);
                viewModel.RND.AvgBizInCompany = Math.Round(Convert.ToDouble((dicRnd.Values.Sum() / (dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum()))) * 100, 1);
                viewModel.Salse.AvgBizInCompany = Math.Round(Convert.ToDouble((dicSalse.Values.Sum() / (dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum()))) * 100, 1);
                viewModel.AvgBizInCompanySum = 100;
            }

            viewModel.Management.AvgTotal = Math.Round(((dicManagement.Values.Sum() + 112.0) / (dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum() + 718)) * 100, 1);
            viewModel.Produce.AvgTotal = Math.Round(Convert.ToDouble(((dicProduce.Values.Sum() + 305.0) / (dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum() + 718))) * 100, 1);
            viewModel.RND.AvgTotal = Math.Round(Convert.ToDouble(((dicRnd.Values.Sum() + 180.0) / (dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum() + 718))) * 100, 1);
            viewModel.Salse.AvgTotal = Math.Round(Convert.ToDouble(((dicSalse.Values.Sum() + 121.0) / (dicProduce.Values.Sum() + dicRnd.Values.Sum() + dicSalse.Values.Sum() + 718))) * 100, 1);
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