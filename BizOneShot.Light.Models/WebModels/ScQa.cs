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
    // SC_QA
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScQa
    {
        public int UsrQaSn { get; set; } // USR_QA_SN (Primary key). 회원문의답변순번
        public int? RelQaSn { get; set; } // REL_QA_SN. 관련문의답변순번
        public string QuestionId { get; set; } // QUESTION_ID. 질문자회원순번
        public string AnswerId { get; set; } // ANSWER_ID. 답변자회원순번
        public DateTime? AskDt { get; set; } // ASK_DT. 질문일시
        public string Subject { get; set; } // SUBJECT. 제목
        public string Question { get; set; } // QUESTION. 질문
        public DateTime? AnsDt { get; set; } // ANS_DT. 답변일시
        public string Answer { get; set; } // ANSWER. 답변
        public string AnsYn { get; set; } // ANS_YN. 답변완료여부  Y:답변완료함  N:답변완료안함(default)
        public string ChkYn { get; set; } // CHK_YN. 답변확인여부  Y:답변확인함  N:답변확인안함(default)
        public int? AnsPnt { get; set; } // ANS_PNT. 답변평가
        public string ReQst { get; set; } // RE_QST. 재질문여부  Y:재질문  N:재질문아님(default)
        public string Email { get; set; } // EMAIL. 이메일주소
        public string TelNo { get; set; } // TEL_NO. 전화번호
        public string SmsYn { get; set; } // SMS_YN. SMS수신여부  Y:수신함  N:수신안함(default)

        // Reverse navigation
        public virtual ICollection<ScQa> ScQas { get; set; } // SC_QA.FK_SC_QA_TO_SC_QA

        // Foreign keys
        public virtual ScQa ScQa_RelQaSn { get; set; } // FK_SC_QA_TO_SC_QA
        public virtual ScUsr ScUsr_AnswerId { get; set; } // FK_SC_USR_TO_SC_QA2
        public virtual ScUsr ScUsr_QuestionId { get; set; } // FK_SC_USR_TO_SC_QA
        
        public ScQa()
        {
            ScQas = new List<ScQa>();
        }
    }

}
