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
    // SC_USR
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScUsr
    {
        public string LoginId { get; set; } // LOGIN_ID (Primary key). 로그인식별자
        public int CompSn { get; set; } // COMP_SN. 기업식별자
        public string LoginPw { get; set; } // LOGIN_PW. 로그인비밀번호
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  R: 탈퇴 요청(Retired)  D: 탈퇴 완료(Deleted)
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
        public virtual ICollection<ScBizWork> ScBizWorks_ExecutorId { get; set; } // SC_BIZ_WORK.FK_SC_USR_TO_SC_BIZ_WORK
        public virtual ICollection<ScBizWork> ScBizWorks_RegId { get; set; } // SC_BIZ_WORK.FK_SC_USR_TO_SC_BIZ_WORK2
        public virtual ICollection<ScBizWork> ScBizWorks_UpdId { get; set; } // SC_BIZ_WORK.FK_SC_USR_TO_SC_BIZ_WORK3
        public virtual ICollection<ScCompInfo> ScCompInfoes_RegId { get; set; } // SC_COMP_INFO.FK_SC_USR_TO_SC_COMP_INFO
        public virtual ICollection<ScCompInfo> ScCompInfoes_UpdId { get; set; } // SC_COMP_INFO.FK_SC_USR_TO_SC_COMP_INFO2
        public virtual ICollection<ScCompMapping> ScCompMappings_RegId { get; set; } // SC_COMP_MAPPING.FK_SC_USR_TO_SC_COMP_MAPPING
        public virtual ICollection<ScCompMapping> ScCompMappings_UpdId { get; set; } // SC_COMP_MAPPING.FK_SC_USR_TO_SC_COMP_MAPPING2
        public virtual ICollection<ScExpertMapping> ScExpertMappings_ExpertId { get; set; } // Many to many mapping
        public virtual ICollection<ScExpertMapping> ScExpertMappings_RegId { get; set; } // SC_EXPERT_MAPPING.FK_SC_USR_TO_SC_EXPERT_MAPPING
        public virtual ICollection<ScExpertMapping> ScExpertMappings_UpdId { get; set; } // SC_EXPERT_MAPPING.FK_SC_USR_TO_SC_EXPERT_MAPPING2
        public virtual ICollection<ScFaq> ScFaqs_RegId { get; set; } // SC_FAQ.FK_SC_USR_TO_SC_FAQ
        public virtual ICollection<ScFaq> ScFaqs_UpdId { get; set; } // SC_FAQ.FK_SC_USR_TO_SC_FAQ2
        public virtual ICollection<ScFileInfo> ScFileInfoes { get; set; } // SC_FILE_INFO.FK_SC_USR_TO_SC_FILE_INFO
        public virtual ICollection<ScForm> ScForms_RegId { get; set; } // SC_FORM.FK_SC_USR_TO_SC_FORM
        public virtual ICollection<ScForm> ScForms_UpdId { get; set; } // SC_FORM.FK_SC_USR_TO_SC_FORM2
        public virtual ICollection<ScMentoringReport> ScMentoringReports_MentorId { get; set; } // SC_MENTORING_REPORT.FK_SC_USR_TO_SC_MENTORING_REPORT
        public virtual ICollection<ScMentoringReport> ScMentoringReports_RegId { get; set; } // SC_MENTORING_REPORT.FK_SC_USR_TO_SC_MENTORING_REPORT2
        public virtual ICollection<ScMentoringReport> ScMentoringReports_UpdId { get; set; } // SC_MENTORING_REPORT.FK_SC_USR_TO_SC_MENTORING_REPORT3
        public virtual ICollection<ScMentoringTotalReport> ScMentoringTotalReports_LoginId { get; set; } // SC_MENTORING_TOTAL_REPORT.FK_SC_USR_TO_MENTORING_TOTAL_REPORT
        public virtual ICollection<ScMentoringTotalReport> ScMentoringTotalReports_RegId { get; set; } // SC_MENTORING_TOTAL_REPORT.FK_SC_USR_TO_MENTORING_TOTAL_REPORT2
        public virtual ICollection<ScMentoringTotalReport> ScMentoringTotalReports_UpdId { get; set; } // SC_MENTORING_TOTAL_REPORT.FK_SC_USR_TO_MENTORING_TOTAL_REPORT3
        public virtual ICollection<ScMentorMappiing> ScMentorMappiings_MentorId { get; set; } // Many to many mapping
        public virtual ICollection<ScMentorMappiing> ScMentorMappiings_RegId { get; set; } // SC_MENTOR_MAPPIING.FK_SC_USR_TO_SC_MENTOR_MAPPIING2
        public virtual ICollection<ScMentorMappiing> ScMentorMappiings_UpdId { get; set; } // SC_MENTOR_MAPPIING.FK_SC_USR_TO_SC_MENTOR_MAPPIING3
        public virtual ICollection<ScNtc> ScNtcs_RegId { get; set; } // SC_NTC.FK_SC_USR_TO_SC_NTC
        public virtual ICollection<ScNtc> ScNtcs_UpdId { get; set; } // SC_NTC.FK_SC_USR_TO_SC_NTC2
        public virtual ICollection<ScQa> ScQas_AnswerId { get; set; } // SC_QA.FK_SC_USR_TO_SC_QA2
        public virtual ICollection<ScQa> ScQas_QuestionId { get; set; } // SC_QA.FK_SC_USR_TO_SC_QA
        public virtual ICollection<ScQcl> ScQcls_RegId { get; set; } // SC_QCL.FK_SC_USR_TO_SC_QCL
        public virtual ICollection<ScQcl> ScQcls_UpdId { get; set; } // SC_QCL.FK_SC_USR_TO_SC_QCL2
        public virtual ICollection<ScReqDoc> ScReqDocs_ReceiverId { get; set; } // SC_REQ_DOC.FK_SC_USR_TO_SC_REQ_DOC2
        public virtual ICollection<ScReqDoc> ScReqDocs_SenderId { get; set; } // SC_REQ_DOC.FK_SC_USR_TO_SC_REQ_DOC
        public virtual ICollection<ScUsr> ScUsrs_RegId { get; set; } // SC_USR.FK_SC_USR_TO_SC_USR
        public virtual ICollection<ScUsr> ScUsrs_UpdId { get; set; } // SC_USR.FK_SC_USR_TO_SC_USR2
        public virtual ScUsrResume ScUsrResume { get; set; } // SC_USR_RESUME.FK_SC_USR_TO_SC_USR_RESUME

        // Foreign keys
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_SC_USR
        public virtual ScUsr ScUsr_RegId { get; set; } // FK_SC_USR_TO_SC_USR
        public virtual ScUsr ScUsr_UpdId { get; set; } // FK_SC_USR_TO_SC_USR2
        public virtual SyDareDbInfo SyDareDbInfo { get; set; } // FK_SY_DARE_DB_INFO_TO_SC_USR
        
        public ScUsr()
        {
            ScBizWorks_ExecutorId = new List<ScBizWork>();
            ScBizWorks_RegId = new List<ScBizWork>();
            ScBizWorks_UpdId = new List<ScBizWork>();
            ScCompInfoes_RegId = new List<ScCompInfo>();
            ScCompInfoes_UpdId = new List<ScCompInfo>();
            ScCompMappings_RegId = new List<ScCompMapping>();
            ScCompMappings_UpdId = new List<ScCompMapping>();
            ScExpertMappings_ExpertId = new List<ScExpertMapping>();
            ScExpertMappings_RegId = new List<ScExpertMapping>();
            ScExpertMappings_UpdId = new List<ScExpertMapping>();
            ScFaqs_RegId = new List<ScFaq>();
            ScFaqs_UpdId = new List<ScFaq>();
            ScFileInfoes = new List<ScFileInfo>();
            ScForms_RegId = new List<ScForm>();
            ScForms_UpdId = new List<ScForm>();
            ScMentorMappiings_MentorId = new List<ScMentorMappiing>();
            ScMentorMappiings_RegId = new List<ScMentorMappiing>();
            ScMentorMappiings_UpdId = new List<ScMentorMappiing>();
            ScMentoringReports_MentorId = new List<ScMentoringReport>();
            ScMentoringReports_RegId = new List<ScMentoringReport>();
            ScMentoringReports_UpdId = new List<ScMentoringReport>();
            ScMentoringTotalReports_LoginId = new List<ScMentoringTotalReport>();
            ScMentoringTotalReports_RegId = new List<ScMentoringTotalReport>();
            ScMentoringTotalReports_UpdId = new List<ScMentoringTotalReport>();
            ScNtcs_RegId = new List<ScNtc>();
            ScNtcs_UpdId = new List<ScNtc>();
            ScQas_AnswerId = new List<ScQa>();
            ScQas_QuestionId = new List<ScQa>();
            ScQcls_RegId = new List<ScQcl>();
            ScQcls_UpdId = new List<ScQcl>();
            ScReqDocs_ReceiverId = new List<ScReqDoc>();
            ScReqDocs_SenderId = new List<ScReqDoc>();
            ScUsrs_RegId = new List<ScUsr>();
            ScUsrs_UpdId = new List<ScUsr>();
        }
    }

}
