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
    // SC_MENTORING_FILE_INFO
    public class ScMentoringFileInfo
    {
        public int FileSn { get; set; } // FILE_SN (Primary key). 파일식별자
        public int ReportSn { get; set; } // REPORT_SN. 맨토링보고서식별자
        public string Classify { get; set; } // CLASSIFY. 구분값 정해야

        // Foreign keys
        public virtual ScFileInfo ScFileInfo { get; set; } // FK_SC_FILE_INFO_TO_SC_MENTORING_FILE_INFO
        public virtual ScMentoringReport ScMentoringReport { get; set; } // FK_SC_MENTORING_REPORT_TO_SC_MENTORING_FILE_INFO
    }

}
