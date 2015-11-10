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
    // SC_BIZ_TYPE
    internal partial class ScBizTypeConfiguration : EntityTypeConfiguration<ScBizType>
    {
        public ScBizTypeConfiguration()
            : this("dbo")
        {
        }
 
        public ScBizTypeConfiguration(string schema)
        {
            ToTable(schema + ".SC_BIZ_TYPE");
            HasKey(x => new { x.CompSn, x.BizTypeCd });

            Property(x => x.CompSn).HasColumnName("COMP_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BizTypeCd).HasColumnName("BIZ_TYPE_CD").IsRequired().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(6).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BizTypeNm).HasColumnName("BIZ_TYPE_NM").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(256);

            // Foreign keys
            HasRequired(a => a.ScCompInfo).WithMany(b => b.ScBizTypes).HasForeignKey(c => c.CompSn); // FK_SC_COMP_INFO_TO_SC_BIZ_TYPE
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
