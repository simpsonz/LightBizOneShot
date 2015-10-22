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
        public IList<CommentViewModel> CommentList { get; set; }
        //public string OrgDivision { get; set; } // 조직역량 -> 조직분화도
        //public string MarketingLevel { get; set; } // 상품화역량 -> 고객의수, 상품의 질 및 마케팅 수준
        //public string FinanceMng { get; set; } // 위험관리역량 -> 제무회계 관리체계 및 제도수준
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



}
