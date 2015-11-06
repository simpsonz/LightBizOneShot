// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;

namespace BizOneShot.Light.Models.DareModels
{
    public class SHUSER_SboMonthlyProperCostSelectReturnModel
    {
        public String SET_YEAR { get; set; }
        public String SET_MONTH { get; set; }
        public Decimal? TM_BILL_AMT { get; set; }
        public Decimal? PERSONNEL_AMT { get; set; }
        public Decimal? STATIC_EMT_AMT { get; set; }
        public Decimal? CARD_AMT { get; set; }
        public Decimal? CASH_AMT { get; set; }
        public Decimal? UNFIXING_AMT { get; set; }
    }

}
