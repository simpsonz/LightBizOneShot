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
    public class SHUSER_SboMonthlyCashSelect20150327ReturnModel
    {
        public Decimal? INPUT_AMT { get; set; }
        public Decimal? OUTPUT_AMT { get; set; }
        public Decimal? CASH_AMT { get; set; }
        public String ACC_YEAR { get; set; }
        public String ACC_MONTH { get; set; }
        public Int64? ROW_NUM { get; set; }
        public Decimal? LAST_AMT { get; set; }
    }

}
