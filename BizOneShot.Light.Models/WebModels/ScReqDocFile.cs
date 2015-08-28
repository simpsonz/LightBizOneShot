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
    // SC_REQ_DOC_FILE
    public class ScReqDocFile
    {
        public int FileSn { get; set; } // FILE_SN (Primary key). 파일식별자
        public int ReqDocSn { get; set; } // REQ_DOC_SN. 자료요청식별자(순번)
        public string RegType { get; set; } // REG_TYPE. 작성자 유형  S: 요청자가 작성한 첨부파일(Sender)  R: 답변자가 작성한 첨부파일(Receiver)

        // Foreign keys
        public virtual ScFileInfo ScFileInfo { get; set; } // FK_SC_FILE_INFO_TO_SC_REQ_DOC_FILE
        public virtual ScReqDoc ScReqDoc { get; set; } // FK_SC_REQ_DOC_TO_SC_REQ_DOC_FILE
    }

}
