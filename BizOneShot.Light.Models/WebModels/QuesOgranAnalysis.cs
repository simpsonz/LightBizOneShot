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
    // QUES_OGRAN_ANALYSIS
    public class QuesOgranAnalysis
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string DeptCd { get; set; } // DEPT_CD (Primary key)
        public string Dept1 { get; set; } // DEPT1
        public string Dept2 { get; set; } // DEPT2
        public int? OfficerCount { get; set; } // OFFICER_COUNT
        public int? ChiefCount { get; set; } // CHIEF_COUNT
        public int? StaffCount { get; set; } // STAFF_COUNT
        public int? BeginnerCount { get; set; } // BEGINNER_COUNT
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT

        // Foreign keys
        public virtual QuesMaster QuesMaster { get; set; } // FK_QUES_MASTER_TO_QUES_OGRAN_ANALYSIS
    }

}
