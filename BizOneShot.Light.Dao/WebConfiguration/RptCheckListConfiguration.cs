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
    // RPT_CHECK_LIST
    internal partial class RptCheckListConfiguration : EntityTypeConfiguration<RptCheckList>
    {
        public RptCheckListConfiguration()
            : this("dbo")
        {
        }
 
        public RptCheckListConfiguration(string schema)
        {
            ToTable(schema + ".RPT_CHECK_LIST");
            HasKey(x => x.DetailCd);

            Property(x => x.DetailCd).HasColumnName("DETAIL_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(8).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.LagreClassCd).HasColumnName("LAGRE_CLASS_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(6);
            Property(x => x.MidiumClassCd).HasColumnName("MIDIUM_CLASS_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(2);
            Property(x => x.SmallClassCd).HasColumnName("SMALL_CLASS_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(4);
            Property(x => x.Title).HasColumnName("TITLE").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Content1).HasColumnName("CONTENT1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Content2).HasColumnName("CONTENT2").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Type).HasColumnName("TYPE").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.Status).HasColumnName("STATUS").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
