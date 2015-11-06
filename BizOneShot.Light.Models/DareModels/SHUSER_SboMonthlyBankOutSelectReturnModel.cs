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
    public class SHUSER_SboMonthlyBankOutSelectReturnModel
    {
        public String BANK_CD { get; set; }
        public String TRANDATE { get; set; }
        public Decimal? HISTCD_O { get; set; }
        public String HISTCD_4 { get; set; }
    }

}
