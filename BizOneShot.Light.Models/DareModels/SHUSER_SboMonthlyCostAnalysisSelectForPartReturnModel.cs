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
    public class SHUSER_SboMonthlyCostAnalysisSelectForPartReturnModel
    {
        public String SET_YEAR { get; set; }
        public String SET_MONTH { get; set; }
        public Decimal? MATERIALS_AMT { get; set; }
        public Decimal? MANUFACTURING_AMT { get; set; }
        public Decimal? OPERATING_AMT { get; set; }
        public Decimal? ALL_OTHER_AMT { get; set; }
        public Decimal? SALES_AMT { get; set; }
        public Decimal? PROFIT_AMT { get; set; }
    }

}
