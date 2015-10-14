using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizOneShot.Light.Models.ViewModels
{
    class FinanceReportViewModels
    {
    }

    public class FinanceMngViewModel
    {
        public string Year { get; set; } // year
        public string Month { get; set; } // month
        public string Display { get; set; } // Display 
        public string CompNm { get; set; } // COMP_NM. 회사명

        public CashViewModel cashViewModel { get; set; } // 현금시제
        public SalesViewModel salesViewModel { get; set; } // 매출
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

}
