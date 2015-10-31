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
    // SC_MENTOR_MAPPIING
    internal partial class ScMentorMappiingConfiguration : EntityTypeConfiguration<ScMentorMappiing>
    {
        public ScMentorMappiingConfiguration()
            : this("dbo")
        {
        }
 
        public ScMentorMappiingConfiguration(string schema)
        {
            ToTable(schema + ".SC_MENTOR_MAPPIING");
            HasKey(x => new { x.BizWorkSn, x.MentorId });

            Property(x => x.BizWorkSn).HasColumnName("BIZ_WORK_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.MentorId).HasColumnName("MENTOR_ID").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.MngCompSn).HasColumnName("MNG_COMP_SN").IsOptional().HasColumnType("int");
            Property(x => x.Status).HasColumnName("STATUS").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasOptional(a => a.ScCompInfo).WithMany(b => b.ScMentorMappiings).HasForeignKey(c => c.MngCompSn); // FK_SC_COMP_INFO_TO_SC_MENTOR_MAPPIING
            HasRequired(a => a.ScBizWork).WithMany(b => b.ScMentorMappiings).HasForeignKey(c => c.BizWorkSn); // FK_SC_BIZ_WORK_TO_SC_MENTOR_MAPPIING
            HasRequired(a => a.ScUsr).WithMany(b => b.ScMentorMappiings).HasForeignKey(c => c.MentorId); // FK_SC_USR_TO_SC_MENTOR_MAPPIING
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
