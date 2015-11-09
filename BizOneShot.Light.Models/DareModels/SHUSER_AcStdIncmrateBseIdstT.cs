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
    // AC_STD_INCMRATE_BSE_IDST_T
    public class SHUSER_AcStdIncmrateBseIdstT
    {
        public string IdstCd { get; set; } // IDST_CD (Primary key). 업종코드
        public string IdstLgClsfCd { get; set; } // IDST_LG_CLSF_CD. 업종대분류코드
        public string IdstDtlNm { get; set; } // IDST_DTL_NM. 업종세부명
    }

}
