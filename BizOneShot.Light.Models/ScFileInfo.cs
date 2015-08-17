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
    // SC_FILE_INFO
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScFileInfo
    {
        public int FileSn { get; set; } // FILE_SN (Primary key). 파일식별자
        public string FileNm { get; set; } // FILE_NM. 파일명
        public string FilePath { get; set; } // FILE_PATH. 파일경로
        public string Status { get; set; } // STATUS. 상태  N: 정상  D: 삭제됨(Deleted)
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시

        // Reverse navigation
        public virtual ICollection<ScUsrResume> ScUsrResumes { get; set; } // SC_USR_RESUME.FK_SC_FILE_INFO_TO_SC_USR_RESUME
        public virtual ScFormFile ScFormFile { get; set; } // SC_FORM_FILE.FK_SC_FILE_INFO_TO_SC_FORM_FILE
        public virtual ScMentoringFileInfo ScMentoringFileInfo { get; set; } // SC_MENTORING_FILE_INFO.FK_SC_FILE_INFO_TO_SC_MENTORING_FILE_INFO
        public virtual ScMentoringTrFileInfo ScMentoringTrFileInfo { get; set; } // SC_MENTORING_TR_FILE_INFO.FK_SC_FILE_INFO_TO_SC_MENTORING_TR_FILE_INFO
        public virtual ScReqDocFile ScReqDocFile { get; set; } // SC_REQ_DOC_FILE.FK_SC_FILE_INFO_TO_SC_REQ_DOC_FILE

        // Foreign keys
        public virtual ScUsr ScUsr { get; set; } // FK_SC_USR_TO_SC_FILE_INFO
        
        public ScFileInfo()
        {
            ScUsrResumes = new List<ScUsrResume>();
        }
    }

}
