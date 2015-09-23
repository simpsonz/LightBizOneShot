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
    // QUES_PRESIDENT_INFO
    public class QuesPresidentInfo
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string PresidentNm { get; set; } // PRESIDENT_NM
        public string BirthDate { get; set; } // BIRTH_DATE
        public string AcademicDegree { get; set; } // ACADEMIC_DEGREE
        public string Major { get; set; } // MAJOR
        public string CareerComp1 { get; set; } // CAREER_COMP1
        public string Job1 { get; set; } // JOB1
        public string CareerComp2 { get; set; } // CAREER_COMP2
        public string Job2 { get; set; } // JOB2
        public string CareerComp3 { get; set; } // CAREER_COMP3
        public string Job3 { get; set; } // JOB3
        public string CareerBasicYear { get; set; } // CAREER_BASIC_YEAR
        public string CareerBasicMonth { get; set; } // CAREER_BASIC_MONTH
        public int? TotalCareerPeriod { get; set; } // TOTAL_CAREER_PERIOD
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT

        // Foreign keys
        public virtual QuesMaster QuesMaster { get; set; } // FK_QUES_MASTER_TO_QUES_PRESIDENT_INFO
    }

}
