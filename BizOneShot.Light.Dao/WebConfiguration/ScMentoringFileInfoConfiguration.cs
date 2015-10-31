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
    // SC_MENTORING_FILE_INFO
    internal partial class ScMentoringFileInfoConfiguration : EntityTypeConfiguration<ScMentoringFileInfo>
    {
        public ScMentoringFileInfoConfiguration()
            : this("dbo")
        {
        }
 
        public ScMentoringFileInfoConfiguration(string schema)
        {
            ToTable(schema + ".SC_MENTORING_FILE_INFO");
            HasKey(x => x.FileSn);

            Property(x => x.FileSn).HasColumnName("FILE_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ReportSn).HasColumnName("REPORT_SN").IsRequired().HasColumnType("int");
            Property(x => x.Classify).HasColumnName("CLASSIFY").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);

            // Foreign keys
            HasRequired(a => a.ScFileInfo).WithOptional(b => b.ScMentoringFileInfo); // FK_SC_FILE_INFO_TO_SC_MENTORING_FILE_INFO
            HasRequired(a => a.ScMentoringReport).WithMany(b => b.ScMentoringFileInfoes).HasForeignKey(c => c.ReportSn); // FK_SC_MENTORING_REPORT_TO_SC_MENTORING_FILE_INFO
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
