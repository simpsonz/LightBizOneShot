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
    // RPT_FINANCE_COMMENT
    internal partial class RptFinanceCommentConfiguration : EntityTypeConfiguration<RptFinanceComment>
    {
        public RptFinanceCommentConfiguration()
            : this("dbo")
        {
        }
 
        public RptFinanceCommentConfiguration(string schema)
        {
            ToTable(schema + ".RPT_FINANCE_COMMENT");
            HasKey(x => new { x.CompSn, x.ExpertId, x.BasicYear, x.BasicMonth });

            Property(x => x.CompSn).HasColumnName("COMP_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ExpertId).HasColumnName("EXPERT_ID").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BizWorkSn).HasColumnName("BIZ_WORK_SN").IsRequired().HasColumnType("int");
            Property(x => x.BasicYear).HasColumnName("BASIC_YEAR").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BasicMonth).HasColumnName("BASIC_MONTH").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Comment).HasColumnName("COMMENT").IsOptional().HasColumnType("nvarchar").HasMaxLength(500);
            Property(x => x.WriteDt).HasColumnName("WRITE_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasRequired(a => a.ScBizWork).WithMany(b => b.RptFinanceComments).HasForeignKey(c => c.BizWorkSn); // FK_SC_BIZ_WORK_TO_RPT_FINANCE_COMMENT
            HasRequired(a => a.ScCompInfo).WithMany(b => b.RptFinanceComments).HasForeignKey(c => c.CompSn); // FK_SC_COMP_INFO_TO_RPT_FINANCE_COMMENT
            HasRequired(a => a.ScUsr).WithMany(b => b.RptFinanceComments).HasForeignKey(c => c.ExpertId); // FK_SC_USR_TO_RPT_FINANCE_COMMENT
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
