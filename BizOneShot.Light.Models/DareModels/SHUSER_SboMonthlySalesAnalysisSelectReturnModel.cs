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
    public class SHUSER_SboMonthlySalesAnalysisSelectReturnModel
    {
        public String CUST_CD { get; set; }
        public String CUST_NM { get; set; }
        public Decimal? TOTAL_AMT { get; set; }
        public Int32? SET_MONTH { get; set; }
        public Decimal? SALES_AMT { get; set; }
    }

}
