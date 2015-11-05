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
    public class SHUSER_SboMonthlyProperCostSelectReturnModel
    {
        public string SET_YEAR { get; set; }
        public string SET_MONTH { get; set; }
        public decimal? TM_BILL_AMT { get; set; }
        public decimal? PERSONNEL_AMT { get; set; }
        public decimal? STATIC_EMT_AMT { get; set; }
        public decimal? CARD_AMT { get; set; }
        public decimal? CASH_AMT { get; set; }
        public decimal? UNFIXING_AMT { get; set; }
    }
}