using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int BizWorkYear { get; set; }
        public int QuestionSn { get; set; }
        public DateTime? QuestionCompleteDt { get; set; }
        public string BizWorkNm { get; set; }
        public string RegistrationNo { get; set; }
        public string OwnNm { get; set; }
        public string CompNm { get; set; }
        public string Status { get; set; } // STATUS
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


}
