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
using System.Data.Entity;
using System.Threading;

namespace BizOneShot.Light.Models.WebModels
{
    // SC_MENTOR_MAPPIING
    public class ScMentorMappiing
    {
        public int BizWorkSn { get; set; } // BIZ_WORK_SN (Primary key). 사업식별자
        public string MentorId { get; set; } // MENTOR_ID (Primary key). 회원테이블의 LOGIN_ID
        public int? MngCompSn { get; set; } // MNG_COMP_SN
        public string Status { get; set; } // STATUS. 상태
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        // Foreign keys
        public virtual ScBizWork ScBizWork { get; set; } // FK_SC_BIZ_WORK_TO_SC_MENTOR_MAPPIING
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_SC_MENTOR_MAPPIING
        public virtual ScUsr ScUsr { get; set; } // FK_SC_USR_TO_SC_MENTOR_MAPPIING
    }

}
