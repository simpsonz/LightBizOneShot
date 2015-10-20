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


    #region 기업지원통계
    public class MentoringCompanyStatsViewModel
    {
        public string BizWorkNm { get; set; } // BIZ_WORK_NM. 사업명
        public DateTime? BizWorkStDt { get; set; } // BIZ_WORK_ST_DT. 사업시작일
        public DateTime? BizWorkEdDt { get; set; } // BIZ_WORK_ED_DT. 사업종료일

        public string StartYear { get; set; } // 시작년
        public string EndYear { get; set; } // 종료년
        public string StartMonth { get; set; } // 시작월
        public string EndMonth { get; set; } // 종료월

        public int SumMentoringDays { get; set; } //전체 멘통링 일수
        public double AvgMentoringDays { get; set; } //메토링 일수 평균(전체 멘토링 일수 / 기업수)

        public int SumMentoring_F { get; set; } //자금 일수
        public int SumMentoring_D { get; set; } //기술개발 일수
        public int SumMentoring_P { get; set; } //특허 일수
        public int SumMentoring_M { get; set; } //마케팅 일수
        public int SumMentoring_L { get; set; } //법무 일수
        public int SumMentoring_T { get; set; } //세무회계 일수
        public int SumMentoring_W { get; set; } //노무 일수
        public int SumMentoring_Z { get; set; } //기타 일수

        public string Display { get; set; } // Display 

        public IList<MentoringStatByCompanyViewModel> MentoringStatByCompanyViewModel { get; set; } // 기업별 멘토링 통계
    }

    public class MentoringStatByCompanyViewModel
    {
        public int ComSn { get; set; }
        public string CompNm { get; set; } // COMP_NM. 회사명
        public int SumMentoringDays { get; set; } //기간 멘토일 일수
        public double AvgMentoringDays { get; set; } //기간 월 평균

        public int SumMentoring_F { get; set; } //자금 일수
        public int SumMentoring_D { get; set; } //기술개발 일수
        public int SumMentoring_P { get; set; } //특허 일수
        public int SumMentoring_M { get; set; } //마케팅 일수
        public int SumMentoring_L { get; set; } //법무 일수
        public int SumMentoring_T { get; set; } //세무회계 일수
        public int SumMentoring_W { get; set; } //노무 일수
        public int SumMentoring_Z { get; set; } //기타 일수
    }

    public class MentoringStatsByCompanyGroupModel
    {
        public int CompSn { get; set; }
        public string ComNm { get; set; }
        public string MentoringAreaCd { get; set; }
        public int Count { get; set; }
    }
    #endregion

}
