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
using System.Threading;

namespace BizOneShot.Light.Models.WebModels
{
    // SC_REQ_DOC
    [GeneratedCode("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScReqDoc
    {
        public ScReqDoc()
        {
            ScReqDocFiles = new List<ScReqDocFile>();
        }

        public int ReqDocSn { get; set; } // REQ_DOC_SN (Primary key). 자료요청식별자(순번)
        public string SenderId { get; set; } // SENDER_ID. 발신자식별자
        public string ReceiverId { get; set; } // RECEIVER_ID. 수신자식별자
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  D: 삭제됨(Deleted)
        public string ChkYn { get; set; } // CHK_YN. 수신확인여부  Y: 수신함  N 수신안함
        public DateTime? ReqDt { get; set; } // REQ_DT. 요청일시
        public string ReqSubject { get; set; } // REQ_SUBJECT. 송신제목
        public string ReqContents { get; set; } // REQ_CONTENTS. 송신내용
        public DateTime? ResDt { get; set; } // RES_DT. 답변일시
        public string ResContents { get; set; } // RES_CONTENTS. 답변내용

        // Reverse navigation
        public virtual ICollection<ScReqDocFile> ScReqDocFiles { get; set; }
        // SC_REQ_DOC_FILE.FK_SC_REQ_DOC_TO_SC_REQ_DOC_FILE

        // Foreign keys
        public virtual ScUsr ScUsr_ReceiverId { get; set; } // FK_SC_USR_TO_SC_REQ_DOC2
        public virtual ScUsr ScUsr_SenderId { get; set; } // FK_SC_USR_TO_SC_REQ_DOC
    }
}