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
    // CT_WEB_LOG
    internal partial class CtWebLogConfiguration : EntityTypeConfiguration<CtWebLog>
    {
        public CtWebLogConfiguration()
            : this("dbo")
        {
        }
 
        public CtWebLogConfiguration(string schema)
        {
            ToTable(schema + ".CT_WEB_LOG");
            HasKey(x => x.LogId);

            Property(x => x.LogId).HasColumnName("LOG_ID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.SrvCd).HasColumnName("SRV_CD").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(10);
            Property(x => x.SvrIp).HasColumnName("SVR_IP").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(255);
            Property(x => x.RmkTxt).HasColumnName("RMK_TXT").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(1000);
            Property(x => x.Line).HasColumnName("LINE").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(1000);
            Property(x => x.LoginId).HasColumnName("LOGIN_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(16);
            Property(x => x.UsrAgn).HasColumnName("USR_AGN").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.FileNm).HasColumnName("FILE_NM").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(200);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
