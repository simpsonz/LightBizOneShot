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
    // SC_BIZ_WORK
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScBizWork
    {
        public int BizWorkSn { get; set; } // BIZ_WORK_SN (Primary key). 사업식별자
        public string ExecutorId { get; set; } // EXECUTOR_ID. 담당자식별자
        public int MngCompSn { get; set; } // MNG_COMP_SN. 주관기관식별자
        public string MngDept { get; set; } // MNG_DEPT
        public string BizWorkNm { get; set; } // BIZ_WORK_NM. 사업명
        public string BizWorkSummary { get; set; } // BIZ_WORK_SUMMARY. 사업개요
        public DateTime? BizWorkStDt { get; set; } // BIZ_WORK_ST_DT. 사업시작일
        public DateTime? BizWorkEdDt { get; set; } // BIZ_WORK_ED_DT. 사업종료일
        public string Status { get; set; } // STATUS. 사업상태
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        // Reverse navigation
        public virtual ICollection<RptMaster> RptMasters { get; set; } // Many to many mapping
        public virtual ICollection<ScCompMapping> ScCompMappings { get; set; } // Many to many mapping
        public virtual ICollection<ScExpertMapping> ScExpertMappings { get; set; } // Many to many mapping
        public virtual ICollection<ScMentoringReport> ScMentoringReports { get; set; } // SC_MENTORING_REPORT.FK_SC_BIZ_WORK_TO_SC_MENTORING_REPORT
        public virtual ICollection<ScMentoringTotalReport> ScMentoringTotalReports { get; set; } // SC_MENTORING_TOTAL_REPORT.FK_SC_BIZ_WORK_TO_MENTORING_TOTAL_REPORT
        public virtual ICollection<ScMentorMappiing> ScMentorMappiings { get; set; } // Many to many mapping

        // Foreign keys
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_SC_BIZ_WORK
        public virtual ScUsr ScUsr { get; set; } // FK_SC_USR_TO_SC_BIZ_WORK
        
        public ScBizWork()
        {
            RptMasters = new List<RptMaster>();
            ScCompMappings = new List<ScCompMapping>();
            ScExpertMappings = new List<ScExpertMapping>();
            ScMentorMappiings = new List<ScMentorMappiing>();
            ScMentoringReports = new List<ScMentoringReport>();
            ScMentoringTotalReports = new List<ScMentoringTotalReport>();
        }
    }

}
