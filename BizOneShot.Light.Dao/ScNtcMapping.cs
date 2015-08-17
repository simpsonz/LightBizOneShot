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
using BizOneShot.Light.Models;
using System.Threading;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace BizOneShot.Light.Dao
{
    // SC_NTC
    internal partial class ScNtcMapping : EntityTypeConfiguration<ScNtc>
    {
        public ScNtcMapping()
            : this("dbo")
        {
        }
 
        public ScNtcMapping(string schema)
        {
            ToTable(schema + ".SC_NTC");
            HasKey(x => x.NoticeSn);

            Property(x => x.NoticeSn).HasColumnName("NOTICE_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Subject).HasColumnName("SUBJECT").IsOptional().HasColumnType("nvarchar").HasMaxLength(1000);
            Property(x => x.RmkTxt).HasColumnName("RMK_TXT").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.Status).HasColumnName("STATUS").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasOptional(a => a.ScUsr_RegId).WithMany(b => b.ScNtcs_RegId).HasForeignKey(c => c.RegId); // FK_SC_USR_TO_SC_NTC
            HasOptional(a => a.ScUsr_UpdId).WithMany(b => b.ScNtcs_UpdId).HasForeignKey(c => c.UpdId); // FK_SC_USR_TO_SC_NTC2
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
