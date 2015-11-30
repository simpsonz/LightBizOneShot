using System;
using System.Collections.Generic;

namespace BizOneShot.Light.Models.ViewModels
{
    public class BasicSurveyReportViewModels
    {
    }

    public class BasicSurveyReportViewModel
    {
        public int BizWorkSn { get; set; }
        public int CompSn { get; set; }
        public int BizWorkMngr { get; set; }
        public string BizWorkMngrNm { get; set; }
        public int BizWorkYear { get; set; }
        public int QuestionSn { get; set; }
        public DateTime? QuestionCompleteDt { get; set; }
        public string BizWorkNm { get; set; }
        public string RegistrationNo { get; set; }
        public string OwnNm { get; set; }
        public string CompNm { get; set; }
        public string Status { get; set; } // STATUS
    }

    public class BasicSurveyPrintViewModel
    {
        /// <summary>
        /// page1 - 표지 및 기초 데이터 모델
        /// </summary>
        public BasicSurveyReportViewModel cover { get; set; } //Page1
        /// <summary>
        /// page2 - 기업 기본정보
        /// </summary>
        public QuesCompanyInfoViewModel companyInfo { get; set; } //Page2
        /// <summary>
        /// page4 - 역량검토 결과 요약 (전체 결과요약)
        /// </summary>
        public OverallSummaryViewModel overallSummary { get; set; } //Page4
        /// <summary>
        /// page6 - 인적자원의 확보와 개발관리 (인적자원의 확보와 개발의 체계성 정도)
        /// </summary>
        public OrgHR01ViewModel orgHR01 { get; set; } //Page6
        /// <summary>
        /// page7 - 인적자원의 보상 및 유지관리 (인적자원의 보상, 유지관리의 적절성 정도)
        /// </summary>
        public OrgHR01ViewModel orgHR02 { get; set; } //Page7
        /// <summary>
        /// page8 - 조직역량 (생산성)
        /// </summary>
        public OrgProductivityViewModel orgProductivity { get; set; } //Page8
        /// <summary>
        /// page9 - 조직역량 (조직분화도)
        /// </summary>
        public OrgDividedViewModel orgDivided { get; set; } //Page9
        /// <summary>
        /// page10 - 연구개발 투자 (연구개발 투자의 수준)
        /// </summary>
        public RndCostViewModel rndCost { get; set; } //Page10
        /// <summary>
        /// page11 - 연구개발 인력의 비율 (연구개발 인력의 구성비율 및 석사급 이상 인력의 비율)
        /// </summary>
        public RndEmpViewModel rndEmp { get; set; } //Page11
        /// <summary>
        /// Page12 - 사업화역량 (기술개발의 결과를 사업화로 연결시킬 수 있는 역량을 갖추고 있는지의 여부)
        /// </summary>
        public RiskMgmtViewModel productivityCommercialize { get; set; } //Page12
        /// <summary>
        /// page13 - [상품화역량] 사업화실적 (기술개발의 사업화 및 상용화 건수)
        /// </summary>
        public ProductivityResultViewModel prductivityResult { get; set; } //Page13
        /// <summary>
        /// page15 생산설비의 운영체계 및 관리 (생산설비의 효율적인 운영 관리 수준)
        /// </summary>
        public RiskMgmtViewModel productivityMgmtFacility { get; set; } //Page14
        /// <summary>
        /// page15 - 공정관리(공정관리의 적절성 수준)
        /// </summary>
        public RiskMgmtViewModel productivityProcessControl { get; set; } //Page15
        /// <summary>
        /// page16 - 품질관리(품질검사․측정 및 시험장비의 관리의 체계성 여부, 검사 및 품질보증 활동의 적절성 여부)
        /// </summary>
        public RiskMgmtViewModel productivityQC { get; set; } //Page16
        /// <summary>
        /// page17 - 마케팅 전략의 수립 및 실행(개발 제품의 목표시장을 분석하고 가격결정, 판매예측 등 마케팅 전략이 수립, 실행되고 있는지의 여부)
        /// </summary>
        public RiskMgmtViewModel productivityMgmtMarketing { get; set; } //Page17
        /// <summary>
        /// page18 - 고객 관리 (고객의 충성도를 높이도록 하는 효율적인 고객관리 체제 여부 )
        /// </summary>
        public RiskMgmtViewModel productivityMgmtCustomer { get; set; } //Page18
        /// <summary>
        /// page19 - 상폼화역량(수익성)
        /// </summary>
        public ProductivityProfitabilityViewModel productivityProfitability { get; set; } //Page19
        /// <summary>
        /// page20 - 상품화역량(1차 타킷고객 설정)
        /// </summary>
        //public RiskMgmtViewModel productivityTargetCustomer { get; set; } //Page20(보고서 삭제됨)
        /// <summary>
        /// page21 - 상품화역량(사업모델 도출과정)
        /// </summary>
        public RiskMgmtViewModel productivityValueChain { get; set; } //Page21
        /// <summary>
        /// page22 - 상품화역량(제품생산 판매 관계망 검토) 
        /// </summary>
        public ProductivityRelationViewModel productivityRelation { get; set; } //Page22
        /// <summary>
        /// page23 - 상품화역량(경쟁력 검토)
        /// </summary>
        public ProductivityRelationViewModel productivityRelation2 { get; set; } //Page23
        /// <summary>
        /// page24 - [CEO 역량] 경영목표 및 전략 (기업의 경영목표 및 경영전략이 적절하게 설정되어 있는지의 여부)
        /// </summary>
        public RiskMgmtViewModel riskMgmtVisionStategy { get; set; } //Page24
        /// <summary>
        /// page25 - [CEO 역량] 경영목표의 리더십 (최고경영자가 성공적인 리더십을 발휘할 수 있도록 하는데 필요한 요건 )
        /// </summary>
        public RiskMgmtViewModel riskMgmtLeadership { get; set; } //Page25
        /// <summary>
        /// page26 - [CEO 역량] 경영목표의 신뢰성 (최고경영자(기업)의 대내외적인 신뢰성 정도)
        /// </summary>
        public RiskMgmtViewModel riskMgmtRelCEO { get; set; } //Page26
        /// <summary>
        /// page27 - [조직역량] 근로환경 (종업원의 생산성 극대화를 위해 꼭 필요한 근무 환경 및 복리후생 수준)
        /// </summary>
        public RiskMgmtViewModel riskMgmtWorkingEnv { get; set; } //Page27
        /// <summary>
        /// page28 - [조직역량] 조직만족도 (종업원이 조직에 만족하는 정도)
        /// </summary>
        public RiskMgmtOrgSatisfactionViewModel riskMgmtOrgSatisfaction { get; set; } //Page28
        /// <summary>
        /// page29 - [조직역량] 정보시스템 활용 (정보시스템 활용 정도)
        /// </summary>
        public RiskMgmtViewModel riskMgmtITSystem { get; set; } //Page29
        /// <summary>
        /// page30 - 위험관리 역량(유동성)
        /// </summary>
        public RiskMgmtLiquidityViewModel riskMgmtLiquidity { get; set; } //Page30
        /// <summary>
        /// page31 - 위험관리역량(위험관리 역량)
        /// </summary>
        public RiskMgmtViewModel riskMgmtEvalProfession { get; set; } //Page31
        /// <summary>
        /// page33 - 성장전량 유형검토
        /// </summary>
        public GrowthStrategyViewModel growthStrategyType { get; set; } //Page33
        /// <summary>
        /// page34 - 단계별 성장전략
        /// </summary>
        public GrowthStrategyViewModel growthStrategyStep { get; set; } //Page34
        /// <summary>
        /// page35 - 단계별 역량강화 산업별 모범 체계
        /// </summary>
        public GrowthStrategyViewModel growthCapabilityProposal { get; set; } //Page35
        /// <summary>
        /// page36 - 회사의 핵심내요을 기술합니다.
        /// </summary>
        public GrowthStrategyViewModel growthTotalProposal { get; set; } //Page36
    }

    public class OverallSummaryViewModel
    {
        public string SubmitType { get; set; }
        public double AvgTotalPoint { get; set; } //전체 평균점수
        public double CompanyPoint { get; set; } //해당 기업점수
        public string BizCapaType { get; set; } //경영역량 총괄 화살표
        public string HRMngType { get; set; } //인적자원관리 화살표
        public string OrgType { get; set; } //조직분화도 화살표
        public string MarketingType { get; set; } //기술경영, 마케팅 화살표
        public string CustMngType { get; set; } //고객의수, 상품의질, 마케팅 수준 화살표
        public string BasicCapaType { get; set; } //기초역량 화살표
        public string RoolType { get; set; } //전반적제도, 규정관리 체계 화살표
        public OverallSummaryPointViewModel OrgCapa { get; set; } //항목별 역량검토결과 - 조직역량
        public OverallSummaryPointViewModel ProductionCapa { get; set; } //항목별 역량검토결과 - 상품화역량
        public OverallSummaryPointViewModel RiskMngCapa { get; set; } //항목별 역량검토결과 - 위험관리역량
        public IList<CommentViewModel> CommentList { get; set; }
    }

    public class RndCostViewModel
    {
        public string SubmitType { get; set; }
        public CheckListViewModel value { get; set; }
        public CheckListViewModel percent { get; set; }
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }

    public class ProductivityResultViewModel
    {
        public string SubmitType { get; set; }
        public CheckListViewModel BizResultCnt { get; set; }
        public CheckListViewModel BizResultPoint { get; set; }
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }


    public class RndEmpViewModel
    {
        public string SubmitType { get; set; }
        public CheckListViewModel rndEmpRatio { get; set; }
        public CheckListViewModel rndEmpLevelRatio { get; set; }
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }

    public class RiskMgmtOrgSatisfactionViewModel
    {
        public string SubmitType { get; set; }
        public CheckListViewModel orgSatisfaction { get; set; }
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }

    public class OverallSummaryPointViewModel
    {
        public double CompanyPoint { get; set; } //해당기업
        public double AvgBizInCompanyPoint { get; set; } //참여기업 평균
        public double AvgTotalPoint { get; set; } //전체평균
        public double CompanyPoint2 { get; set; } //해당기업
        public double AvgBizInCompanyPoint2 { get; set; } //참여기업 평균
        public double AvgTotalPoint2 { get; set; } //전체평균
        public double AvgSMCompanyPoint { get; set; } //중소기업 평균
    }

    public class CommentViewModel
    {
        public string DetailCd { get; set; }
        public string Comment { get; set; }
    }

    public class CheckBoxViewModel
    {
        public string DetailCd { get; set; }
        public bool CheckVal { get; set; }
    }

    public class OrgHR01ViewModel
    {
        public string SubmitType { get; set; }
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }


    public class OrgProductivityViewModel
    {
        public string SubmitType { get; set; }
        public BarChartViewModel Productivity { get; set; } //생산성
        public BarChartViewModel Activity { get; set; } //활동성
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }

    public class ProductivityProfitabilityViewModel
    {
        public string SubmitType { get; set; }
        public BarChartViewModel Profitability { get; set; } //수익성
        public BarChartViewModel Growth { get; set; } //성장성
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }

    public class RiskMgmtLiquidityViewModel
    {
        public string SubmitType { get; set; }
        public BarChartViewModel Liquidity { get; set; } //유동성
        public BarChartViewModel Stability { get; set; } //안정성
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }

    public class BarChartViewModel
    {
        public double Result { get; set; } //결과
        public double Dividend { get; set; } //분자
        public double Divisor { get; set; } // 분모
        public double Company { get; set; }
        public double AvgBizInCompany { get; set; }
        public double AvgTotal { get; set; }
        public double AvgSMCompany { get; set; }
    }

    public class CheckListViewModel
    {
        public string Count { get; set; }
        public string DetailCd { get; set; }
        public string Title { get; set; } //항목
        public bool AnsVal { get; set; } //해당기업 문진표
        public string Company { get; set; } //해당기업 
        public string StartUpAvg { get; set; } //창업보육단계평균
        public string GrowthAvg { get; set; } // 보육성장단계 평균
        public string IndependentAvg { get; set; } //자립성장단계 평균
        public string BizInCompanyAvg { get; set; } // 참여기업 평균
        public string TotalAvg { get; set; } //전체평균
        public string SMCompanyAvg { get; set; } //중소기업 평균
    }


    public class OrgDividedViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public OrgEmpCompositionViewModel Management { get; set; } //기획/관리
        public OrgEmpCompositionViewModel Produce { get; set; } // 생산/생산관리
        public OrgEmpCompositionViewModel RND { get; set; } // 연구개발/연구지원
        public OrgEmpCompositionViewModel Salse { get; set; } // 마케팅기획 / 판매영업
        public int? OfficerSumCount { get; set; } // OFFICER_COUNT
        public int? ChiefSumCount { get; set; } // CHIEF_COUNT
        public int? StaffSumCount { get; set; } // STAFF_COUNT
        public int? BeginnerSumCount { get; set; } // BEGINNER_COUNT
        public int? TotalSumCount { get; set; } //부문별 합계
        public double CompanySum { get; set; } //해당기업 합계
        public double AvgBizInCompanySum { get; set; } //참여기업 합계 평균
        public double AvgTotalSum { get; set; } //전체 합계 평균
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }

    public class OrgEmpCompositionViewModel
    {
        public string Dept1 { get; set; } // DEPT1
        public string Dept2 { get; set; } // DEPT2
        public int OfficerCount { get; set; } // OFFICER_COUNT
        public int ChiefCount { get; set; } // CHIEF_COUNT
        public int StaffCount { get; set; } // STAFF_COUNT
        public int BeginnerCount { get; set; } // BEGINNER_COUNT
        public int PartialSum { get; set; } // 부문별 합계
        public double Company { get; set; } // 해당기업
        public double AvgBizInCompany { get; set; } // 참여기업 평균
        public double AvgTotal { get; set; } // 전체평균
    }


    public class GrowthStrategyViewModel
    {
        public string SubmitType { get; set; }
        public IList<CommentViewModel> CommentList { get; set; }
    }

    public class RiskMgmtViewModel
    {
        public string SubmitType { get; set; }
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckBoxViewModel> CheckBoxList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }

    public class ProductivityRelationViewModel
    {
        public string SubmitType { get; set; }
        public IList<CommentViewModel> CommentList { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }

        //관리자 입력값
        public IList<CommentViewModel> MngComment { get; set; }
    }

    public class RegCommentViewModel
    {
        public int CompSn { get; set; } // COMP_SN (Primary key). 기업식별자
        public string ExpertId { get; set; } // EXPERT_ID (Primary key). 전문가식별자
        public int BizWorkSn { get; set; } // BIZ_WORK_SN. 사업식별자
        public int BasicYear { get; set; } // BASIC_YEAR (Primary key). 기준년도
        public int BasicMonth { get; set; } // BASIC_MONTH (Primary key). 기준월
        public string Comment { get; set; } // COMMENT. 커맨트
        public DateTime? WriteDt { get; set; } // WRITE_DT. 등록일
        public string ExpertNm { get; set; } // EXPERT_ID (Primary key). 전문가이름
        public string BizWorkNm { get; set; } // EXPERT_ID (Primary key). 사업명
        public string WriteYN { get; set; } //Commant 작성여부 Y:작성, N:미작성
    }
}