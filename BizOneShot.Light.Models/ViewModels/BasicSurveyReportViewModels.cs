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
        public string OrgDivision { get; set; } // 조직역량 -> 조직분화도
        public string MarketingLevel { get; set; } // 상품화역량 -> 고객의수, 상품의 질 및 마케팅 수준
        public string FinanceMng { get; set; } // 위험관리역량 -> 제무회계 관리체계 및 제도수준
    }



}
