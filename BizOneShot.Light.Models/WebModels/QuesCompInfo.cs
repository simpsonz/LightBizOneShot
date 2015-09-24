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
    // QUES_COMP_INFO
    public class QuesCompInfo
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
        public DateTime? PublishDt { get; set; } // PUBLISH_DT
        public string HomeUrl { get; set; } // HOME_URL
        public string CompAddr { get; set; } // COMP_ADDR
        public string FactoryAddr { get; set; } // FACTORY_ADDR
        public string LabAddr { get; set; } // LAB_ADDR
        public string FacPossessYn { get; set; } // FAC_POSSESS_YN
        public string RndYn { get; set; } // RND_YN
        public string ProductNm1 { get; set; } // PRODUCT_NM1
        public string ProductNm2 { get; set; } // PRODUCT_NM2
        public string ProductNm3 { get; set; } // PRODUCT_NM3
        public string MarketList { get; set; } // MARKET_LIST
        public string CompType { get; set; } // COMP_TYPE
        public string ResidentType { get; set; } // RESIDENT_TYPE
        public string CertiType { get; set; } // CERTI_TYPE
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT

        // Foreign keys
        public virtual QuesMaster QuesMaster { get; set; } // FK_QUES_MASTER_TO_QUES_COMP_INFO
    }

}
