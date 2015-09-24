using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizOneShot.Light.Models.ViewModels
{
    class BasicSurveyViewModels
    {
    }
    public class QuesMasterViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string RegistrationNo { get; set; } // REGISTRATION_NO
        public int? BasicYear { get; set; } // BASIC_YEAR
        public string SaveStatus { get; set; } // SAVE_STATUS
        public string Status { get; set; } // STATUS
        public string SubmitType { get; set; } // Submit 방식
        public QuesWriterViewModel QuesWriter { get; set; } // 작성자 정보

    }

    public class QuesWriterViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string CompNm { get; set; } // COMP_NM
        public string Name { get; set; } // Name
        public string DeptNm { get; set; } // DEPT_NM
        public string Position { get; set; } // POSITION
        public string TelNo { get; set; } // TEL_NO
        public string Email { get; set; } // EMAIL
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT
    }

    public class QuestionDropDownModel
    {
        public string SnStatus { get; set; } // QUESTION_SN (Primary key)
        public int? BasicYear { get; set; } // BASIC_YEAR
    }
}
