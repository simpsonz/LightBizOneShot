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
    public class SHUSER_SboMonthlyTaxSalesSelectReturnModel
    {
        public String ACPT_TR_NM { get; set; }
        public String JNLYZ_DT { get; set; }
        public Decimal? SUM_AMT { get; set; }
        public String ITM_NM { get; set; }
    }

}
