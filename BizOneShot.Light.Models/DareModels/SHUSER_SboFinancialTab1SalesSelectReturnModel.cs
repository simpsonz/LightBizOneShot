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
    public class SHUSER_SboFinancialTab1SalesSelectReturnModel
    {
        public Decimal? TERM_SALE { get; set; }
        public Decimal? TERM_SALE_AVR { get; set; }
        public Decimal? TO_SALE { get; set; }
        public Decimal? PRE_TO_SALE { get; set; }
        public Decimal? QT_EMP { get; set; }
    }

}
