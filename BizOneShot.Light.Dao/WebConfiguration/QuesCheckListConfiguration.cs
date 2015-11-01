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
    // QUES_CHECK_LIST
    internal partial class QuesCheckListConfiguration : EntityTypeConfiguration<QuesCheckList>
    {
        public QuesCheckListConfiguration()
            : this("dbo")
        {
        }
 
        public QuesCheckListConfiguration(string schema)
        {
            ToTable(schema + ".QUES_CHECK_LIST");
            HasKey(x => x.CheckListSn);

            Property(x => x.CheckListSn).HasColumnName("CHECK_LIST_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.LagreClassCd).HasColumnName("LAGRE_CLASS_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(2);
            Property(x => x.MidiumClassCd).HasColumnName("MIDIUM_CLASS_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(4);
            Property(x => x.SmallClassCd).HasColumnName("SMALL_CLASS_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(6);
            Property(x => x.DetailCd).HasColumnName("DETAIL_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(8);
            Property(x => x.ReportTitle).HasColumnName("REPORT_TITLE").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Title).HasColumnName("TITLE").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Content1).HasColumnName("CONTENT1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Content2).HasColumnName("CONTENT2").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.StartUpStep).HasColumnName("START_UP_STEP").IsOptional().HasColumnType("int");
            Property(x => x.GrowthStep).HasColumnName("GROWTH_STEP").IsOptional().HasColumnType("int");
            Property(x => x.IndependentStep).HasColumnName("INDEPENDENT_STEP").IsOptional().HasColumnType("int");
            Property(x => x.TotalStep).HasColumnName("TOTAL_STEP").IsOptional().HasColumnType("int");
            Property(x => x.CurrentUseYn).HasColumnName("CURRENT_USE_YN").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
