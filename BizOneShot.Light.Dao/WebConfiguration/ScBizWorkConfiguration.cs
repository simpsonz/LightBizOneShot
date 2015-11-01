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
    // SC_BIZ_WORK
    internal partial class ScBizWorkConfiguration : EntityTypeConfiguration<ScBizWork>
    {
        public ScBizWorkConfiguration()
            : this("dbo")
        {
        }
 
        public ScBizWorkConfiguration(string schema)
        {
            ToTable(schema + ".SC_BIZ_WORK");
            HasKey(x => x.BizWorkSn);

            Property(x => x.BizWorkSn).HasColumnName("BIZ_WORK_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ExecutorId).HasColumnName("EXECUTOR_ID").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.MngCompSn).HasColumnName("MNG_COMP_SN").IsRequired().HasColumnType("int");
            Property(x => x.MngDept).HasColumnName("MNG_DEPT").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.BizWorkNm).HasColumnName("BIZ_WORK_NM").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.BizWorkSummary).HasColumnName("BIZ_WORK_SUMMARY").IsOptional().HasColumnType("nvarchar").HasMaxLength(1000);
            Property(x => x.BizWorkStDt).HasColumnName("BIZ_WORK_ST_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.BizWorkEdDt).HasColumnName("BIZ_WORK_ED_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.Status).HasColumnName("STATUS").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasRequired(a => a.ScCompInfo).WithMany(b => b.ScBizWorks).HasForeignKey(c => c.MngCompSn); // FK_SC_COMP_INFO_TO_SC_BIZ_WORK
            HasRequired(a => a.ScUsr).WithMany(b => b.ScBizWorks).HasForeignKey(c => c.ExecutorId); // FK_SC_USR_TO_SC_BIZ_WORK
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
