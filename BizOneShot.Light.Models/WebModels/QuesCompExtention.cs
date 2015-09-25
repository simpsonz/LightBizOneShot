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
    // QUES_COMP_EXTENTION
    public class QuesCompExtention
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
        public int? TotalYear { get; set; } // TOTAL_YEAR
        public int? TotalMonth { get; set; } // TOTAL_MONTH
        public string HistotyDate10 { get; set; } // HISTOTY_DATE10
        public string HistotyDate9 { get; set; } // HISTOTY_DATE9
        public string HistotyDate8 { get; set; } // HISTOTY_DATE8
        public string HistotyDate7 { get; set; } // HISTOTY_DATE7
        public string HistotyDate6 { get; set; } // HISTOTY_DATE6
        public string HistotyDate5 { get; set; } // HISTOTY_DATE5
        public string HistotyDate4 { get; set; } // HISTOTY_DATE4
        public string HistotyDate3 { get; set; } // HISTOTY_DATE3
        public string HistotyDate2 { get; set; } // HISTOTY_DATE2
        public string HistotyDate1 { get; set; } // HISTOTY_DATE1
        public string HistoryContent10 { get; set; } // HISTORY_CONTENT10
        public string HistoryContent9 { get; set; } // HISTORY_CONTENT9
        public string HistoryContent8 { get; set; } // HISTORY_CONTENT8
        public string HistoryContent7 { get; set; } // HISTORY_CONTENT7
        public string HistoryContent6 { get; set; } // HISTORY_CONTENT6
        public string HistoryContent5 { get; set; } // HISTORY_CONTENT5
        public string HistoryContent4 { get; set; } // HISTORY_CONTENT4
        public string HistoryContent3 { get; set; } // HISTORY_CONTENT3
        public string HistoryContent2 { get; set; } // HISTORY_CONTENT2
        public string HistoryContent1 { get; set; } // HISTORY_CONTENT1
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT

        // Foreign keys
        public virtual QuesMaster QuesMaster { get; set; } // FK_QUES_MASTER_TO_QUES_PRESIDENT_INFO
    }

}
