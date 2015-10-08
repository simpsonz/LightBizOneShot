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
    // SC_USR
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScUsr
    {
        public string LoginId { get; set; } // LOGIN_ID (Primary key). 로그인식별자
        public int CompSn { get; set; } // COMP_SN. 기업식별자
        public string LoginPw { get; set; } // LOGIN_PW. 로그인비밀번호
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  R: 탈퇴 요청(Retired)  D: 탈퇴 완료(Deleted)
        public string AgreeYn { get; set; } // AGREE_YN. 동의여부  R: 거부(Refuse)  A: 동의(Agree)
        public string UsrType { get; set; } // USR_TYPE. 회원유형  C: 기업회원(Company)  M:멘토(Mentor)  B: 사업회원(Business)  P:전문가회원(Professonal)  S: SCP (SCP)
        public string UsrTypeDetail { get; set; } // USR_TYPE_DETAIL. A :  관리자(Admi)  O: 담당자(Operator)    T: 세무/회계사(Tax Accountant)  W: 노무  L:법무  P:특허  M:마케팅  F:자금  D:기술개발  Z:기타
        public string DbType { get; set; } // DB_TYPE. DB유형
        public string Name { get; set; } // Name. 이름
        public string FaxNo { get; set; } // FAX_NO. Fax
        public string TelNo { get; set; } // TEL_NO. 전화번호
        public string MbNo { get; set; } // MB_NO. 휴대폰
        public string Email { get; set; } // EMAIL. 이메일
        public string PostNo { get; set; } // POST_NO. 우편번호
        public string Addr1 { get; set; } // ADDR_1. 주소1
        public string Addr2 { get; set; } // ADDR_2. 주소2
        public string AccountNo { get; set; } // ACCOUNT_NO. 계좌번호
        public string BankNm { get; set; } // BANK_NM. 계좌은행
        public string DeptNm { get; set; } // DEPT_NM. 소속부서명
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        // Reverse navigation
        public virtual ICollection<ScBizWork> ScBizWorks { get; set; } // SC_BIZ_WORK.FK_SC_USR_TO_SC_BIZ_WORK
        public virtual ICollection<ScCompMapping> ScCompMappings { get; set; } // SC_COMP_MAPPING.FK_SC_MENTOR_MAPPIING_TO_SC_COMP_MAPPING
        public virtual ICollection<ScExpertMapping> ScExpertMappings { get; set; } // Many to many mapping
        public virtual ICollection<ScMentoringReport> ScMentoringReports { get; set; } // SC_MENTORING_REPORT.FK_SC_USR_TO_SC_MENTORING_REPORT
        public virtual ICollection<ScMentoringTotalReport> ScMentoringTotalReports { get; set; } // SC_MENTORING_TOTAL_REPORT.FK_SC_USR_TO_MENTORING_TOTAL_REPORT
        public virtual ICollection<ScMentorMappiing> ScMentorMappiings { get; set; } // Many to many mapping
        public virtual ICollection<ScQa> ScQas_AnswerId { get; set; } // SC_QA.FK_SC_USR_TO_SC_QA2
        public virtual ICollection<ScQa> ScQas_QuestionId { get; set; } // SC_QA.FK_SC_USR_TO_SC_QA
        public virtual ICollection<ScReqDoc> ScReqDocs_ReceiverId { get; set; } // SC_REQ_DOC.FK_SC_USR_TO_SC_REQ_DOC2
        public virtual ICollection<ScReqDoc> ScReqDocs_SenderId { get; set; } // SC_REQ_DOC.FK_SC_USR_TO_SC_REQ_DOC
        public virtual ScUsrResume ScUsrResume { get; set; } // SC_USR_RESUME.FK_SC_USR_TO_SC_USR_RESUME

        // Foreign keys
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_SC_USR
        public virtual SyDareDbInfo SyDareDbInfo { get; set; } // FK_SY_DARE_DB_INFO_TO_SC_USR
        
        public ScUsr()
        {
            AgreeYn = "R";
            ScBizWorks = new List<ScBizWork>();
            ScCompMappings = new List<ScCompMapping>();
            ScExpertMappings = new List<ScExpertMapping>();
            ScMentorMappiings = new List<ScMentorMappiing>();
            ScMentoringReports = new List<ScMentoringReport>();
            ScMentoringTotalReports = new List<ScMentoringTotalReport>();
            ScQas_AnswerId = new List<ScQa>();
            ScQas_QuestionId = new List<ScQa>();
            ScReqDocs_ReceiverId = new List<ScReqDoc>();
            ScReqDocs_SenderId = new List<ScReqDoc>();
        }
    }

}
