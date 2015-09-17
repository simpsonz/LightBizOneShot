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
    // SC_MENTORING_REPORT
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class ScMentoringReport
    {
        public int ReportSn { get; set; } // REPORT_SN (Primary key). 맨토링보고서식별자
        public int BizWorkSn { get; set; } // BIZ_WORK_SN. 사업식별자
        public string MentorId { get; set; } // MENTOR_ID. 맨토식별자
        public int? CompSn { get; set; } // COMP_SN. 기업식별자
        public DateTime? MentoringDt { get; set; } // MENTORING_DT. 맨토링일자
        public DateTime? MentoringStHr { get; set; } // MENTORING_ST_HR. 맨토링시작시간
        public DateTime? MentoringEdHr { get; set; } // MENTORING_ED_HR. 맨토링종료시간
        public string MentoringPlace { get; set; } // MENTORING_PLACE. 맨토링장소
        public string Attendee { get; set; } // ATTENDEE. 콤마로 구분하여 여러명이 들어감  예) 홍길동,임꺽정
        public string MentorAreaCd { get; set; } // MENTOR_AREA_CD. 코드정의해야 함함
        public string MentoringSubject { get; set; } // MENTORING_SUBJECT. 맨토링주제
        public string MentoringContents { get; set; } // MENTORING_CONTENTS. 맨토링내용
        public string Status { get; set; } // STATUS. 상태
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        // Reverse navigation
        public virtual ICollection<ScMentoringFileInfo> ScMentoringFileInfoes { get; set; } // SC_MENTORING_FILE_INFO.FK_SC_MENTORING_REPORT_TO_SC_MENTORING_FILE_INFO

        // Foreign keys
        public virtual ScBizWork ScBizWork { get; set; } // FK_SC_BIZ_WORK_TO_SC_MENTORING_REPORT
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_SC_MENTORING_REPORT
        public virtual ScUsr ScUsr { get; set; } // FK_SC_USR_TO_SC_MENTORING_REPORT
        
        public ScMentoringReport()
        {
            ScMentoringFileInfoes = new List<ScMentoringFileInfo>();
        }
    }

}
