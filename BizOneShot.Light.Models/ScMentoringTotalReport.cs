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

namespace BizOneShot.Light.Models
{
    // SC_MENTORING_TOTAL_REPORT
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScMentoringTotalReport
    {
        public int TotalReportSn { get; set; } // TOTAL_REPORT_SN (Primary key). 맨토링종합보고서식별자
        public int BizWorkSn { get; set; } // BIZ_WORK_SN. 사업식별자
        public string LoginId { get; set; } // LOGIN_ID. 맨토식별자
        public int? CompSn { get; set; } // COMP_SN. 기업식별자
        public DateTime? SubmitDt { get; set; } // SUBMIT_DT. 제출일
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        // Reverse navigation
        public virtual ICollection<ScMentoringTrFileInfo> ScMentoringTrFileInfoes { get; set; } // SC_MENTORING_TR_FILE_INFO.FK_MENTORING_TOTAL_REPORT_TO_SC_MENTORING_TR_FILE_INFO

        // Foreign keys
        public virtual ScBizWork ScBizWork { get; set; } // FK_SC_BIZ_WORK_TO_MENTORING_TOTAL_REPORT
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_MENTORING_TOTAL_REPORT
        public virtual ScUsr ScUsr_LoginId { get; set; } // FK_SC_USR_TO_MENTORING_TOTAL_REPORT
        public virtual ScUsr ScUsr_RegId { get; set; } // FK_SC_USR_TO_MENTORING_TOTAL_REPORT2
        public virtual ScUsr ScUsr_UpdId { get; set; } // FK_SC_USR_TO_MENTORING_TOTAL_REPORT3
        
        public ScMentoringTotalReport()
        {
            ScMentoringTrFileInfoes = new List<ScMentoringTrFileInfo>();
        }
    }

}
