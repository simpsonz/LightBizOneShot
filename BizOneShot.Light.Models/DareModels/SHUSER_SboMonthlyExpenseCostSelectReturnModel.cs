// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.51

#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace BizOneShot.Light.Models.DareModels
{
    public class SHUSER_SboMonthlyExpenseCostSelectReturnModel
    {
        public string SET_YEAR { get; set; }
        public string SET_MONTH { get; set; }
        public string SET_DATE { get; set; }
        public decimal? MAN_AMT { get; set; }
        public decimal? SALES_AMT { get; set; }
        public decimal? STATIC_ETC_AMT { get; set; }
        public decimal? WELFARE_AMT { get; set; }
        public decimal? TAX_AMT { get; set; }
        public decimal? WASTE_AMT { get; set; }
        public decimal? FLOAT_ETC_AMT { get; set; }
    }
}