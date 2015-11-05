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
    // SC_NTC
    public class ScNtc
    {
        public int NoticeSn { get; set; } // NOTICE_SN (Primary key). 공지사항순번
        public string Subject { get; set; } // SUBJECT. 제목
        public string RmkTxt { get; set; } // RMK_TXT. 내용
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  D: 사용안함(Deleted)
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시
    }
}