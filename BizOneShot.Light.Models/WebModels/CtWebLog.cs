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
    // CT_WEB_LOG
    public class CtWebLog
    {
        public int LogId { get; set; } // LOG_ID (Primary key). 로그식별자
        public string SrvCd { get; set; } // SRV_CD. 서비스코드
        public string SvrIp { get; set; } // SVR_IP. 서버IP
        public string RmkTxt { get; set; } // RMK_TXT. 오류내용
        public string Line { get; set; } // LINE. 발생라인
        public string LoginId { get; set; } // LOGIN_ID. 사용자아이디
        public string UsrAgn { get; set; } // USR_AGN. 사용자브라우저
        public string FileNm { get; set; } // FILE_NM. 파일명
        public DateTime? RegDt { get; set; } // REG_DT. 발생일시
    }

}
