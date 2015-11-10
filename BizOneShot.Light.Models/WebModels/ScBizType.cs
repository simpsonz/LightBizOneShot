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

namespace BizOneShot.Light.Models.WebModels
{
    // SC_BIZ_TYPE
    public class ScBizType
    {
        public int CompSn { get; set; } // COMP_SN (Primary key). 기업식별자
        public string BizTypeCd { get; set; } // BIZ_TYPE_CD (Primary key). 업종코드
        public string BizTypeNm { get; set; } // BIZ_TYPE_NM. 업종이름

        // Foreign keys
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_SC_BIZ_TYPE
    }

}
