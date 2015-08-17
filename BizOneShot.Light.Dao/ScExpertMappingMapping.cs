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
    // SC_EXPERT_MAPPING
    internal partial class ScExpertMappingMapping : EntityTypeConfiguration<ScExpertMapping>
    {
        public ScExpertMappingMapping()
            : this("dbo")
        {
        }
 
        public ScExpertMappingMapping(string schema)
        {
            ToTable(schema + ".SC_EXPERT_MAPPING");
            HasKey(x => new { x.BizWorkSn, x.ExpertId });

            Property(x => x.BizWorkSn).HasColumnName("BIZ_WORK_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ExpertId).HasColumnName("EXPERT_ID").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Status).HasColumnName("STATUS").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasOptional(a => a.ScUsr_RegId).WithMany(b => b.ScExpertMappings_RegId).HasForeignKey(c => c.RegId); // FK_SC_USR_TO_SC_EXPERT_MAPPING
            HasOptional(a => a.ScUsr_UpdId).WithMany(b => b.ScExpertMappings_UpdId).HasForeignKey(c => c.UpdId); // FK_SC_USR_TO_SC_EXPERT_MAPPING2
            HasRequired(a => a.ScBizWork).WithMany(b => b.ScExpertMappings).HasForeignKey(c => c.BizWorkSn); // FK_SC_BIZ_WORK_TO_SC_EXPERT_MAPPING
            HasRequired(a => a.ScUsr_ExpertId).WithMany(b => b.ScExpertMappings_ExpertId).HasForeignKey(c => c.ExpertId); // FK_SC_USR_TO_SC_EXPERT_MAPPING3
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
