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
    // RPT_MNG_COMMENT
    internal partial class RptMngCommentConfiguration : EntityTypeConfiguration<RptMngComment>
    {
        public RptMngCommentConfiguration()
            : this("dbo")
        {
        }
 
        public RptMngCommentConfiguration(string schema)
        {
            ToTable(schema + ".RPT_MNG_COMMENT");
            HasKey(x => new { x.MngCompSn, x.BasicYear, x.DetailCd });

            Property(x => x.MngCompSn).HasColumnName("MNG_COMP_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BasicYear).HasColumnName("BASIC_YEAR").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.DetailCd).HasColumnName("DETAIL_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(8).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Comment).HasColumnName("COMMENT").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);

            // Foreign keys
            HasRequired(a => a.RptMngCode).WithMany(b => b.RptMngComments).HasForeignKey(c => c.DetailCd); // FK_RPT_MNG_CODE_TO_RPT_MNG_COMMENT
            HasRequired(a => a.ScCompInfo).WithMany(b => b.RptMngComments).HasForeignKey(c => c.MngCompSn); // FK_SC_COMP_INFO_TO_RPT_MNG_COMMENT
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
