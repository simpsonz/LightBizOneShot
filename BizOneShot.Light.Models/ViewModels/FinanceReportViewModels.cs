using System.Collections.Generic;

namespace BizOneShot.Light.Models.ViewModels
{
    internal class FinanceReportViewModels
    {
    }

    public class FinanceMngViewModel
    {
        public string Year { get; set; } // year
        public string Month { get; set; } // month
        public string Display { get; set; } // Display 
        public string CompNm { get; set; } // COMP_NM. 회사명
        public int CompSn { get; set; } // COMP_NM. 회사 식별자
        public int BizWorkSn { get; set; } // COMP_NM. 사업 식별자

        public CashViewModel cashViewModel { get; set; } // 현금시제
        public SalesViewModel salesViewModel { get; set; } // 매출
        public TotalCostViewModel curMenthTotalCostViewModel { get; set; } //해당 월 이익분석
        public TotalCostViewModel lastMenthTotalCostViewModel { get; set; } //전달 월 이익분석
        public ExpenseCostViewModel expenseCostViewModel { get; set; } //비용분석
        public IList<TaxSalesViewModel> taxSalesListViewModel { get; set; } //주요매출
        public IList<BankOutViewModel> bankOutListViewModel { get; set; } //주요지출
    }

    public class CashViewModel
    {
        public string ForwardAmt { get; set; } // 이월액
        public string ReceivedAmt { get; set; } // 입금액
        public string ContributionAmt { get; set; } // 출금액 
        public string CashBalance { get; set; } // 현재잔고
        public string LastMonthCashBalance { get; set; } // 전월잔고
        public string BeforeQuarterlyCashBalance { get; set; } // 전분기잔고
    }

    public class SalesViewModel
    {
        public string CurMonth { get; set; } // 이월액
        public string CurYear { get; set; } // 입금액
        public string LastMonth { get; set; } // 출금액 
    }

    public class TotalCostViewModel
    {
        public string MaterialAmt { get; set; } // 자재비
        public string ManufacturingAmt { get; set; } // 제조비
        public string OperatingAmt { get; set; } // 판관비
        public string AllOtherAmt { get; set; } // 영업외비용
        public string SalesAmt { get; set; } // 총 매출
        public string ProfitAmt { get; set; } // 순이익
    }

    public class ExpenseCostViewModel
    {
        public string ManAmt { get; set; } // 고정경비 인건비
        public string SalesAmt { get; set; } // 고정경비 지금입차료
        public string StaticEtcAmt { get; set; } // 고정경비 이자비용
        public string FixTotalAmt { get; set; } // 고정경비 총액
        public string WelfareAmt { get; set; } // 유동경비 복리후생비용
        public string TaxAmt { get; set; } // 유동경비 세금공과
        public string WasteAmt { get; set; } // 유동경비 소모품비
        public string FloatEtcAmt { get; set; } // 유동경비 기타
        public string MoveTotalAmt { get; set; } // 유동경비 총액
    }

    public class TaxSalesViewModel
    {
        public string CustName { get; set; } // 거래처명
        public string WriteDate { get; set; } // 전자세금 작성일
        public string TotalAmt { get; set; } // 합계금액
        public string ItemName { get; set; } // 품목명
        public string Share { get; set; } // 점유율
    }

    public class BankOutViewModel
    {
        public string BankName { get; set; } // 은행명
        public string OutDate { get; set; } // 출금일
        public string TotalAmt { get; set; } // 금액
        public string ItemName { get; set; } // 적요
        public string Share { get; set; } // 점유율
    }
}