using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizOneShot.Light.Models.ViewModels
{
    class ReportViewModels
    {
    }

    public class QuarterModel
    {
        public int Year { get; set; } // quarter
        public int Quarter { get; set; } // display quarter
    }

    public class BizInCompanyStatsViewModel
    {

        public string BizWorkNm { get; set; } // BIZ_WORK_NM. 사업명
        public DateTime? BizWorkStDt { get; set; } // BIZ_WORK_ST_DT. 사업시작일
        public DateTime? BizWorkEdDt { get; set; } // BIZ_WORK_ED_DT. 사업종료일

        public string SumSales { get; set; } // 참여기업 매출 총합
        public string AvgSales { get; set; } // 참여기업 매출 평균
        public string SumEmploy { get; set; } // 참여기업 총 고용인원
        public string AvgEmploy { get; set; } // 참여기업 평균 고용인원
        public string Display { get; set; } // Display 
        public string StartYear { get; set; } // 시작년
        public string EndYear { get; set; } // 종료년
        public string StartMonth { get; set; } // 시작월
        public string EndMonth { get; set; } // 종료월
        public string StartQuarter { get; set; } // 시작분기
        public string EndQuarter { get; set; } // 종료분기

        public IList<CompnayStatsViewModel> compnayStatsListViewModel { get; set; } //기업매출 및 고용인원
    }

    public class CompnayStatsViewModel
    {
        public string CompNm { get; set; } // COMP_NM. 회사명
        public string SumSales { get; set; } // 참여기업 기간 매출 총합
        public string CntEmploy { get; set; } // 참여기업 고용인원
        public string AvgSales { get; set; } // 참여기업 기간 매출 평균
        public string LastSales { get; set; } // 참여기업 종료 매출 
        public string BeforeSales { get; set; } // 참여기업 종료 전(월, 분기, 년) 매출 
    }
}
