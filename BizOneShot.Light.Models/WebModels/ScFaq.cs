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
    // SC_FAQ
    public class ScFaq
    {
        public int FaqSn { get; set; } // FAQ_SN (Primary key). 자주하는질문순번
        public int? QclSn { get; set; } // QCL_SN. 질문분류코드(순번)
        public string QstTxt { get; set; } // QST_TXT. 질문
        public string AnsTxt { get; set; } // ANS_TXT. 답변
        public string Stat { get; set; } // STAT. 상태  N: 정상(Normal)  D: 사용안함(Deleted)
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        // Foreign keys
        public virtual ScQcl ScQcl { get; set; } // FK_SC_QCL_TO_SC_FAQ
        public virtual ScUsr ScUsr_RegId { get; set; } // FK_SC_USR_TO_SC_FAQ
        public virtual ScUsr ScUsr_UpdId { get; set; } // FK_SC_USR_TO_SC_FAQ2
    }

}
