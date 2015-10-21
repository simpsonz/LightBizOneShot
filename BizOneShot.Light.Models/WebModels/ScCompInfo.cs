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
    // SC_COMP_INFO
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScCompInfo
    {
        public int CompSn { get; set; } // COMP_SN (Primary key). 기업식별자
        public string Status { get; set; } // STATUS. 상태  N: 정상회원(Normal)  P: 승인대기(Pending) - 사용안함  C: 거절됨(Canceled) - 사용안함  R: 탈퇴 요청(Retired)  D: 탈퇴 완료(Deleted)
        public string CompType { get; set; } // COMP_TYPE. 사업자유형  I: 개인사업자(Individual Company)  C: 법인사업자(Corporate Company)    다시정의해야함
        public string RegistrationNo { get; set; } // REGISTRATION_NO. 사업자등록번호
        public string CompNm { get; set; } // COMP_NM. 회사명
        public string Email { get; set; } // EMAIL. 대표이메일주소
        public string TelNo { get; set; } // TEL_NO. 대표전화번호
        public string PostNo { get; set; } // POST_NO. 우편번호
        public string Addr1 { get; set; } // ADDR_1. 주소1
        public string Addr2 { get; set; } // ADDR_2. 주소2
        public string OwnNm { get; set; } // OWN_NM. 대표자명
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        // Reverse navigation
        public virtual ICollection<RptMaster> RptMasters { get; set; } // RPT_MASTER.FK_SC_COMP_INFO_TO_RPT_MASTER
        public virtual ICollection<ScBizWork> ScBizWorks { get; set; } // SC_BIZ_WORK.FK_SC_COMP_INFO_TO_SC_BIZ_WORK
        public virtual ICollection<ScCompMapping> ScCompMappings { get; set; } // Many to many mapping
        public virtual ICollection<ScMentoringReport> ScMentoringReports { get; set; } // SC_MENTORING_REPORT.FK_SC_COMP_INFO_TO_SC_MENTORING_REPORT
        public virtual ICollection<ScMentoringTotalReport> ScMentoringTotalReports { get; set; } // SC_MENTORING_TOTAL_REPORT.FK_SC_COMP_INFO_TO_MENTORING_TOTAL_REPORT
        public virtual ICollection<ScMentorMappiing> ScMentorMappiings { get; set; } // SC_MENTOR_MAPPIING.FK_SC_COMP_INFO_TO_SC_MENTOR_MAPPIING
        public virtual ICollection<ScUsr> ScUsrs { get; set; } // SC_USR.FK_SC_COMP_INFO_TO_SC_USR
        public virtual ScBizType ScBizType { get; set; } // SC_BIZ_TYPE.FK_SC_COMP_INFO_TO_SC_BIZ_TYPE
        
        public ScCompInfo()
        {
            RptMasters = new List<RptMaster>();
            ScBizWorks = new List<ScBizWork>();
            ScCompMappings = new List<ScCompMapping>();
            ScMentorMappiings = new List<ScMentorMappiing>();
            ScMentoringReports = new List<ScMentoringReport>();
            ScMentoringTotalReports = new List<ScMentoringTotalReport>();
            ScUsrs = new List<ScUsr>();
        }
    }

}
