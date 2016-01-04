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
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;

namespace BizOneShot.Light.Models.DareModels
{
    public class SHUSER_SboBosLiteMonthlyCashReturnModel
    {
        public Decimal? LAST_MONTH_CASH { get; set; }
        public Decimal? CUR_MONTH_CASH { get; set; }
        public Decimal? INPUT_AMT { get; set; }
        public Decimal? OUTPUT_AMT { get; set; }
        public Decimal? LAST_QUARTER_CASH_AVG { get; set; }
    }

}
