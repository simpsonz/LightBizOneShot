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

namespace BizOneShot.Light.Models.DareModels
{
    // SBO_FINANCIAL_INDEX_T
    public class SHUSER_SboFinancialIndexT
    {
        public string MembBusnpersNo { get; set; } // MEMB_BUSNPERS_NO (Primary key)
        public string CorpCode { get; set; } // CORP_CODE (Primary key)
        public string BizCd { get; set; } // BIZ_CD (Primary key)
        public string Year { get; set; } // YEAR (Primary key)
        public string Prdno { get; set; } // PRDNO
        public string PrdnoYm { get; set; } // PRDNO_YM
        public decimal? ReserchAmt { get; set; } // RESERCH_AMT
        public decimal? CurrentSale { get; set; } // CURRENT_SALE
        public decimal? PrevSale { get; set; } // PREV_SALE
        public decimal? CurrentEarning { get; set; } // CURRENT_EARNING
        public decimal? PrevEarning { get; set; } // PREV_EARNING
        public decimal? OperatingEarning { get; set; } // OPERATING_EARNING
        public decimal? TotalCapital { get; set; } // TOTAL_CAPITAL
        public decimal? CurrentAsset { get; set; } // CURRENT_ASSET
        public decimal? InventoryAsset { get; set; } // INVENTORY_ASSET
        public decimal? CurrentLiability { get; set; } // CURRENT_LIABILITY
        public decimal? TotalLiability { get; set; } // TOTAL_LIABILITY
        public decimal? TotalAsset { get; set; } // TOTAL_ASSET
        public decimal? NonOperEar { get; set; } // NON_OPER_EAR
        public decimal? InterstCost { get; set; } // INTERST_COST
        public decimal? SalesCredit { get; set; } // SALES_CREDIT
        public decimal? ValueAdded { get; set; } // VALUE_ADDED
        public decimal? MaterialCost { get; set; } // MATERIAL_COST
        public decimal? QtEmp { get; set; } // QT_EMP
        public string InsertDts { get; set; } // INSERT_DTS
        public string InsertId { get; set; } // INSERT_ID
        public string ModifyDts { get; set; } // MODIFY_DTS
        public string ModifyId { get; set; } // MODIFY_ID
    }

}
