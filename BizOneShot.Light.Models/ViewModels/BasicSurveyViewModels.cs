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

    public class QuesCompanyInfoViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string CompNm { get; set; } // COMP_NM
        public string EngCompNm { get; set; } // ENG_COMP_NM
        public string TelNo { get; set; } // TEL_NO
        public string FaxNo { get; set; } // FAX_NO
        public string Name { get; set; } // NAME
        public string Email { get; set; } // EMAIL
        public string RegistrationNo { get; set; } // REGISTRATION_NO
        public string CoRegistrationNo { get; set; } // CO_REGISTRATION_NO
        public string PublishDt { get; set; } // PUBLISH_DT
        public string HomeUrl { get; set; } // HOME_URL
        public string CompAddr { get; set; } // COMP_ADDR
        public string FactoryAddr { get; set; } // FACTORY_ADDR
        public string LabAddr { get; set; } // LAB_ADDR
        public string FacPossessYn { get; set; } // FAC_POSSESS_YN
        public string RndYn { get; set; } // RND_YN
        public string ProductNm1 { get; set; } // PRODUCT_NM1
        public string ProductNm2 { get; set; } // PRODUCT_NM2
        public string ProductNm3 { get; set; } // PRODUCT_NM3
        public bool MarketPublic { get; set; } // MARKET_PUBLIC
        public bool MarketCivil { get; set; } // MARKET_CIVIL
        public bool MarketConsumer { get; set; } // MARKET_CONSUMER
        public bool MarketForeing { get; set; } // MARKET_FOREING
        public bool MarketEtc { get; set; } // MARKET_ETC
        public string CompType { get; set; } // COMP_TYPE
        public string ResidentType { get; set; } // RESIDENT_TYPE
        public bool CertiVenture { get; set; } // CERTI_VENTURE
        public bool CertiRnd { get; set; } // CERTI_RND
        public bool CertiMainbiz { get; set; } // CERTI_MAINBIZ
        public bool CertiInnobiz { get; set; } // CERTI_INNOBIZ
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT
        public string SubmitType { get; set; } // Submit 방식
    }
}
