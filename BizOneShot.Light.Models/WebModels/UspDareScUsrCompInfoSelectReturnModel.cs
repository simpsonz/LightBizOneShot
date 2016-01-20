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
    public class UspDareScUsrCompInfoSelectReturnModel
    {
        public String LOGIN_ID { get; set; }
        public Int32 COMP_SN { get; set; }
        public String STATUS { get; set; }
        public String AGREE_YN { get; set; }
        public String USR_TYPE { get; set; }
        public String USR_TYPE_DETAIL { get; set; }
        public String DB_TYPE { get; set; }
        public String NAME { get; set; }
        public String FAX_NO { get; set; }
        public String TEL_NO { get; set; }
        public String MB_NO { get; set; }
        public String EMAIL { get; set; }
        public String POST_NO { get; set; }
        public String ADDR_1 { get; set; }
        public String ADDR_2 { get; set; }
        public String ACCOUNT_NO { get; set; }
        public String BANK_NM { get; set; }
        public String DEPT_NM { get; set; }
        public Int32 COMP_SN1 { get; set; }
        public String STATUS1 { get; set; }
        public String COMP_TYPE { get; set; }
        public String REGISTRATION_NO { get; set; }
        public String COMP_NM { get; set; }
        public String EMAIL1 { get; set; }
        public String TEL_NO1 { get; set; }
        public String POST_NO1 { get; set; }
        public String ADDR_11 { get; set; }
        public String ADDR_21 { get; set; }
        public String OWN_NM { get; set; }
        public String REG_ID { get; set; }
        public DateTime? REG_DT { get; set; }
        public String UPD_ID { get; set; }
        public DateTime? UPD_DT { get; set; }
    }

}
