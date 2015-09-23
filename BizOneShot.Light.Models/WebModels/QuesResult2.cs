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
    // QUES_RESULT2
    public class QuesResult2
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public int CheckListSn { get; set; } // CHECK_LIST_SN (Primary key)
        public int? BasicYear { get; set; } // BASIC_YEAR
        public string D { get; set; } // D
        public string D451 { get; set; } // D-1
        public string D452 { get; set; } // D-2
        public string D453 { get; set; } // D-3
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT

        // Foreign keys
        public virtual QuesCheckList QuesCheckList { get; set; } // FK_QUES_CHECK_LIST_TO_QUES_RESULT2
        public virtual QuesMaster QuesMaster { get; set; } // FK_QUES_MASTER_TO_QUES_RESULT2
    }

}
