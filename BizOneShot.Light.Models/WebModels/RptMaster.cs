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
    // RPT_MASTER
    public class RptMaster
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key). 문진표순번
        public int BizWorkSn { get; set; } // BIZ_WORK_SN (Primary key). 사업식별자
        public int BasicYear { get; set; } // BASIC_YEAR (Primary key). 기준년도
        public int CompSn { get; set; } // COMP_SN. 기업식별자
        public string MentorId { get; set; } // MENTOR_ID. 로그인식별자
        public int? SaveStatus { get; set; } // SAVE_STATUS. 저장단계를 저장    최초 : 0  작성자 : 1  기업정보 :2  ....
        public string Status { get; set; } // STATUS. 작성중 : P  작성완료 : C  삭제 : D
        public DateTime? RegDt { get; set; } // REG_DT
        public string RegId { get; set; } // REG_ID
        public DateTime? UpdDt { get; set; } // UPD_DT
        public string UpdId { get; set; } // UPD_ID

        // Foreign keys
        public virtual QuesMaster QuesMaster { get; set; } // FK_QUES_MASTER_TO_RPT_MASTER
        public virtual ScBizWork ScBizWork { get; set; } // FK_SC_BIZ_WORK_TO_RPT_MASTER
        public virtual ScCompInfo ScCompInfo { get; set; } // FK_SC_COMP_INFO_TO_RPT_MASTER
        public virtual ScUsr ScUsr { get; set; } // FK_SC_USR_TO_RPT_MASTER
    }

}
