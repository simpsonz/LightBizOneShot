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
    // SC_QA
    public class ScQa
    {
        public int UsrQaSn { get; set; } // USR_QA_SN (Primary key). 회원문의답변순번
        public string QuestionId { get; set; } // QUESTION_ID. 질문자회원순번
        public string AnswerId { get; set; } // ANSWER_ID. 답변자회원순번
        public DateTime? AskDt { get; set; } // ASK_DT. 질문일시
        public string Subject { get; set; } // SUBJECT. 제목
        public string Question { get; set; } // QUESTION. 질문
        public DateTime? AnsDt { get; set; } // ANS_DT. 답변일시
        public string Answer { get; set; } // ANSWER. 답변
        public string AnsYn { get; set; } // ANS_YN. 답변완료여부  Y:답변완료함  N:답변완료안함(default)

        // Foreign keys
        public virtual ScUsr ScUsr_AnswerId { get; set; } // FK_SC_USR_TO_SC_QA2
        public virtual ScUsr ScUsr_QuestionId { get; set; } // FK_SC_USR_TO_SC_QA
    }

}
