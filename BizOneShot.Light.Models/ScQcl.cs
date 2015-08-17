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
    // SC_QCL
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScQcl
    {
        public int QclSn { get; set; } // QCL_SN (Primary key). 질문분류코드(순번)
        public string QclType { get; set; } // QCL_TYPE. 질문분류유형  A: 1:1문의와 FAQ 모두적용되는 질문분류(All)  Q: 1:1문의에만 적용되는 질문분류(QA)  F: FAQ에만 적용되는 질문분류(FAQ)
        public string QclNm { get; set; } // QCL_NM. 질문분류명
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  D: 사용안함(Deleted)
        public int DspOdr { get; set; } // DSP_ODR. 표시순서
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        // Reverse navigation
        public virtual ICollection<ScFaq> ScFaqs { get; set; } // SC_FAQ.FK_SC_QCL_TO_SC_FAQ

        // Foreign keys
        public virtual ScUsr ScUsr_RegId { get; set; } // FK_SC_USR_TO_SC_QCL
        public virtual ScUsr ScUsr_UpdId { get; set; } // FK_SC_USR_TO_SC_QCL2
        
        public ScQcl()
        {
            ScFaqs = new List<ScFaq>();
        }
    }

}
