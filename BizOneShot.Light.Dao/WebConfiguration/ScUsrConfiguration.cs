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
using BizOneShot.Light.Models.WebModels;
using System.Threading;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace BizOneShot.Light.Dao.WebConfiguration
{
    // SC_USR
    internal partial class ScUsrConfiguration : EntityTypeConfiguration<ScUsr>
    {
        public ScUsrConfiguration()
            : this("dbo")
        {
        }
 
        public ScUsrConfiguration(string schema)
        {
            ToTable(schema + ".SC_USR");
            HasKey(x => x.LoginId);

            Property(x => x.LoginId).HasColumnName("LOGIN_ID").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CompSn).HasColumnName("COMP_SN").IsRequired().HasColumnType("int");
            Property(x => x.LoginPw).HasColumnName("LOGIN_PW").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.Status).HasColumnName("STATUS").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.UsrType).HasColumnName("USR_TYPE").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.UsrTypeDetail).HasColumnName("USR_TYPE_DETAIL").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.DbType).HasColumnName("DB_TYPE").IsOptional().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.Name).HasColumnName("Name").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.FaxNo).HasColumnName("FAX_NO").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.TelNo).HasColumnName("TEL_NO").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.MbNo).HasColumnName("MB_NO").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.Email).HasColumnName("EMAIL").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(40);
            Property(x => x.PostNo).HasColumnName("POST_NO").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(5);
            Property(x => x.Addr1).HasColumnName("ADDR_1").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(75);
            Property(x => x.Addr2).HasColumnName("ADDR_2").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(75);
            Property(x => x.AccountNo).HasColumnName("ACCOUNT_NO").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.BankNm).HasColumnName("BANK_NM").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.DeptNm).HasColumnName("DEPT_NM").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasOptional(a => a.SyDareDbInfo).WithMany(b => b.ScUsrs).HasForeignKey(c => c.DbType); // FK_SY_DARE_DB_INFO_TO_SC_USR
            HasRequired(a => a.ScCompInfo).WithMany(b => b.ScUsrs).HasForeignKey(c => c.CompSn); // FK_SC_COMP_INFO_TO_SC_USR
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
