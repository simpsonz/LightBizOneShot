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
    // QUES_MASTER
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class QuesMaster
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string RegistrationNo { get; set; } // REGISTRATION_NO
        public int? BasicYear { get; set; } // BASIC_YEAR
        public int? SaveStatus { get; set; } // SAVE_STATUS
        public string Status { get; set; } // STATUS

        // Reverse navigation
        public virtual ICollection<QuesOgranAnalysis> QuesOgranAnalysis { get; set; } // Many to many mapping
        public virtual ICollection<QuesResult1> QuesResult1 { get; set; } // Many to many mapping
        public virtual ICollection<QuesResult2> QuesResult2 { get; set; } // Many to many mapping
        public virtual ICollection<RptMaster> RptMasters { get; set; } // Many to many mapping
        public virtual QuesCompExtention QuesCompExtention { get; set; } // QUES_COMP_EXTENTION.FK_QUES_MASTER_TO_QUES_PRESIDENT_INFO
        public virtual QuesCompHistory QuesCompHistory { get; set; } // QUES_COMP_HISTORY.FK_QUES_MASTER_TO_QUES_COMP_HISTORY
        public virtual QuesCompInfo QuesCompInfo { get; set; } // QUES_COMP_INFO.FK_QUES_MASTER_TO_QUES_COMP_INFO
        public virtual QuesWriter QuesWriter { get; set; } // QUES_WRITER.FK_QUES_MASTER_TO_QUES_WRITER
        
        public QuesMaster()
        {
            QuesOgranAnalysis = new List<QuesOgranAnalysis>();
            QuesResult1 = new List<QuesResult1>();
            QuesResult2 = new List<QuesResult2>();
            RptMasters = new List<RptMaster>();
        }
    }

}
