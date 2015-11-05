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

namespace BizOneShot.Light.Models.WebModels
{
    // RPT_FINANCE_COMMENT
    public class RptFinanceComment
    {
        public int CompSn { get; set; } // COMP_SN (Primary key). 기업식별자
        public string ExpertId { get; set; } // EXPERT_ID (Primary key). 전문가식별자
        public int BizWorkSn { get; set; } // BIZ_WORK_SN. 사업식별자
        public int BasicYear { get; set; } // BASIC_YEAR (Primary key). 기준년도
        public int BasicMonth { get; set; } // BASIC_MONTH (Primary key). 기준월
        public string Comment { get; set; } // COMMENT. 커맨트
        public DateTime? WriteDt { get; set; } // WRITE_DT. 등록일

        // Foreign keys
        public virtual ScBizWork ScBizWork { get; set; } // FK_SC_BIZ_WORK_TO_RPT_FINANCE_COMMENT
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_RPT_FINANCE_COMMENT
        public virtual ScUsr ScUsr { get; set; } // FK_SC_USR_TO_RPT_FINANCE_COMMENT
    }
}