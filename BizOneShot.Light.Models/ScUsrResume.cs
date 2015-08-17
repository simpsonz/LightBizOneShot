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
    // SC_USR_RESUME
    public class ScUsrResume
    {
        public string LoginId { get; set; } // LOGIN_ID (Primary key). 로그인식별자
        public int? FileSn { get; set; } // FILE_SN. 파일식별자

        // Foreign keys
        public virtual ScFileInfo ScFileInfo { get; set; } // FK_SC_FILE_INFO_TO_SC_USR_RESUME
        public virtual ScUsr ScUsr { get; set; } // FK_SC_USR_TO_SC_USR_RESUME
    }

}
