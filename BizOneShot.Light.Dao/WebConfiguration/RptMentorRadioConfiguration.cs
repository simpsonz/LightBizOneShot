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
    // RPT_MENTOR_RADIO
    internal partial class RptMentorRadioConfiguration : EntityTypeConfiguration<RptMentorRadio>
    {
        public RptMentorRadioConfiguration()
            : this("dbo")
        {
        }
 
        public RptMentorRadioConfiguration(string schema)
        {
            ToTable(schema + ".RPT_MENTOR_RADIO");
            HasKey(x => new { x.QuestionSn, x.BizWorkSn, x.BasicYear, x.DetailCd });

            Property(x => x.QuestionSn).HasColumnName("QUESTION_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BizWorkSn).HasColumnName("BIZ_WORK_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BasicYear).HasColumnName("BASIC_YEAR").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.DetailCd).HasColumnName("DETAIL_CD").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(8).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RadioVal).HasColumnName("RADIO_VAL").IsOptional().HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.RptCheckList).WithMany(b => b.RptMentorRadios).HasForeignKey(c => c.DetailCd); // FK_RPT_CHECK_LIST_TO_RPT_MENTOR_RADIO
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
