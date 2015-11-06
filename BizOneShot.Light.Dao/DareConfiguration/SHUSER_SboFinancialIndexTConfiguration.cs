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
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using BizOneShot.Light.Models.DareModels;
using System.Threading;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace BizOneShot.Light.Dao.DareConfiguration
{
    // SBO_FINANCIAL_INDEX_T
    internal partial class SHUSER_SboFinancialIndexTConfiguration : EntityTypeConfiguration<SHUSER_SboFinancialIndexT>
    {
        public SHUSER_SboFinancialIndexTConfiguration()
            : this("SHUSER")
        {
        }
 
        public SHUSER_SboFinancialIndexTConfiguration(string schema)
        {
            ToTable(schema + ".SBO_FINANCIAL_INDEX_T");
            HasKey(x => new { x.MembBusnpersNo, x.CorpCode, x.BizCd, x.Year });

            Property(x => x.MembBusnpersNo).HasColumnName("MEMB_BUSNPERS_NO").IsRequired().HasColumnType("nvarchar").HasMaxLength(13).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CorpCode).HasColumnName("CORP_CODE").IsRequired().HasColumnType("nvarchar").HasMaxLength(4).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BizCd).HasColumnName("BIZ_CD").IsRequired().HasColumnType("nvarchar").HasMaxLength(8).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Year).HasColumnName("YEAR").IsRequired().HasColumnType("nvarchar").HasMaxLength(4).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Prdno).HasColumnName("PRDNO").IsOptional().HasColumnType("nvarchar").HasMaxLength(2);
            Property(x => x.PrdnoYm).HasColumnName("PRDNO_YM").IsOptional().HasColumnType("nvarchar").HasMaxLength(12);
            Property(x => x.ReserchAmt).HasColumnName("RESERCH_AMT").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.CurrentSale).HasColumnName("CURRENT_SALE").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.PrevSale).HasColumnName("PREV_SALE").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.CurrentEarning).HasColumnName("CURRENT_EARNING").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.PrevEarning).HasColumnName("PREV_EARNING").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.OperatingEarning).HasColumnName("OPERATING_EARNING").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.TotalCapital).HasColumnName("TOTAL_CAPITAL").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.CurrentAsset).HasColumnName("CURRENT_ASSET").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.InventoryAsset).HasColumnName("INVENTORY_ASSET").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.CurrentLiability).HasColumnName("CURRENT_LIABILITY").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.TotalLiability).HasColumnName("TOTAL_LIABILITY").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.TotalAsset).HasColumnName("TOTAL_ASSET").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.NonOperEar).HasColumnName("NON_OPER_EAR").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.InterstCost).HasColumnName("INTERST_COST").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.SalesCredit).HasColumnName("SALES_CREDIT").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.ValueAdded).HasColumnName("VALUE_ADDED").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.MaterialCost).HasColumnName("MATERIAL_COST").IsOptional().HasColumnType("decimal").HasPrecision(17,4);
            Property(x => x.QtEmp).HasColumnName("QT_EMP").IsOptional().HasColumnType("decimal");
            Property(x => x.InsertDts).HasColumnName("INSERT_DTS").IsOptional().HasColumnType("nvarchar").HasMaxLength(14);
            Property(x => x.InsertId).HasColumnName("INSERT_ID").IsOptional().HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.ModifyDts).HasColumnName("MODIFY_DTS").IsOptional().HasColumnType("nvarchar").HasMaxLength(14);
            Property(x => x.ModifyId).HasColumnName("MODIFY_ID").IsOptional().HasColumnType("nvarchar").HasMaxLength(20);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
