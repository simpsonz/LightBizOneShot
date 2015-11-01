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
    // RPT_MNG_COMMENT
    public class RptMngComment
    {
        public int MngCompSn { get; set; } // MNG_COMP_SN (Primary key). 관리기업식별자
        public int BasicYear { get; set; } // BASIC_YEAR (Primary key). 기준년도
        public string DetailCd { get; set; } // DETAIL_CD (Primary key). 상세코드
        public string Comment { get; set; } // COMMENT. 관리자작성내용

        // Foreign keys
        public virtual RptMngCode RptMngCode { get; set; } // FK_RPT_MNG_CODE_TO_RPT_MNG_COMMENT
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_RPT_MNG_COMMENT
    }

}
