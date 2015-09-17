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
    // SC_FORM
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScForm
    {
        public int FormSn { get; set; } // FORM_SN (Primary key). 서식식별자(순번)
        public string Subject { get; set; } // SUBJECT. 제목
        public string Contents { get; set; } // CONTENTS. 내용
        public string FormType { get; set; } // FORM_TYPE. 서식구분  M : 메뉴얼  N : 일반서식  S : SCP표준서식  P : 프로그램
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  D: 사용안함(Deleted)
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        // Reverse navigation
        public virtual ICollection<ScFormFile> ScFormFiles { get; set; } // SC_FORM_FILE.FK_SC_FORM_TO_SC_FORM_FILE
        
        public ScForm()
        {
            ScFormFiles = new List<ScFormFile>();
        }
    }

}
