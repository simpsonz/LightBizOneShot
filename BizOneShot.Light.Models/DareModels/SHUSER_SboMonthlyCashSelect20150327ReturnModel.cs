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
    public class SHUSER_SboMonthlyCashSelect20150327ReturnModel
    {
        public decimal? INPUT_AMT { get; set; }
        public decimal? OUTPUT_AMT { get; set; }
        public decimal? CASH_AMT { get; set; }
        public string ACC_YEAR { get; set; }
        public string ACC_MONTH { get; set; }
        public long? ROW_NUM { get; set; }
        public decimal? LAST_AMT { get; set; }
    }
}