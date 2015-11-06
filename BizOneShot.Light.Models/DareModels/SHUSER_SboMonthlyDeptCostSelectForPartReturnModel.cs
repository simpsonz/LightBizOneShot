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
    public class SHUSER_SboMonthlyDeptCostSelectForPartReturnModel
    {
        public String ACC_DT_YEAR { get; set; }
        public String ACC_DT_MONTH { get; set; }
        public String DEPT_NM { get; set; }
        public Decimal? TOTAL { get; set; }
        public Decimal? MAN_AMT { get; set; }
        public Decimal? SALES_AMT { get; set; }
        public Decimal? ETC_AMT { get; set; }
    }

}
