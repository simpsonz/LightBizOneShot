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
        public int BizWorkYear { get; set; }
        public int QuestionSn { get; set; }
        public DateTime? QuestionCompleteDt { get; set; }
        public string BizWorkNm { get; set; }
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

    public class OrgHR01ViewModel
    {
        public string SubmitType { get; set; }
        public CommentViewModel Comment { get; set; }
        public IList<CheckListViewModel> CheckList { get; set; }
    }

    public class CheckListViewModel
    {
        public string Count { get; set; }
        public string Title { get; set; } //항목
        public bool AnsVal { get; set; } //해당기업 문진표
        public string StartUpAvg { get; set; } //창업보육단계평균
        public string GrowthAvg { get; set; } // 보육성장단계 평균
        public string IndependentAvg { get; set; } //자립성장단계 평균
        public string BizInCompanyAvg { get; set; } // 참여기업 평균
        public string TotalAvg { get; set; } //전체평균
    }


    public class GrowthStrategyTypeViewModel
    {
        public string SubmitType { get; set; }
        public IList<CommentViewModel> CommentList { get; set; }
    }


}
