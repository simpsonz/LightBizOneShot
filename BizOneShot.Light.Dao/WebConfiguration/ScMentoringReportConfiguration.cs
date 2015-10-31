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
    // SC_MENTORING_REPORT
    internal partial class ScMentoringReportConfiguration : EntityTypeConfiguration<ScMentoringReport>
    {
        public ScMentoringReportConfiguration()
            : this("dbo")
        {
        }
 
        public ScMentoringReportConfiguration(string schema)
        {
            ToTable(schema + ".SC_MENTORING_REPORT");
            HasKey(x => x.ReportSn);

            Property(x => x.ReportSn).HasColumnName("REPORT_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.BizWorkSn).HasColumnName("BIZ_WORK_SN").IsRequired().HasColumnType("int");
            Property(x => x.MentorId).HasColumnName("MENTOR_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.CompSn).HasColumnName("COMP_SN").IsOptional().HasColumnType("int");
            Property(x => x.MentoringDt).HasColumnName("MENTORING_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.MentoringStHr).HasColumnName("MENTORING_ST_HR").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(5);
            Property(x => x.MentoringEdHr).HasColumnName("MENTORING_ED_HR").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(5);
            Property(x => x.MentoringPlace).HasColumnName("MENTORING_PLACE").IsOptional().HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.Attendee).HasColumnName("ATTENDEE").IsOptional().HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.MentorAreaCd).HasColumnName("MENTOR_AREA_CD").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.MentoringSubject).HasColumnName("MENTORING_SUBJECT").IsOptional().HasColumnType("nvarchar").HasMaxLength(1000);
            Property(x => x.MentoringContents).HasColumnName("MENTORING_CONTENTS").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.SubmitDt).HasColumnName("SUBMIT_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.Status).HasColumnName("STATUS").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasOptional(a => a.ScCompInfo).WithMany(b => b.ScMentoringReports).HasForeignKey(c => c.CompSn); // FK_SC_COMP_INFO_TO_SC_MENTORING_REPORT
            HasOptional(a => a.ScUsr).WithMany(b => b.ScMentoringReports).HasForeignKey(c => c.MentorId); // FK_SC_USR_TO_SC_MENTORING_REPORT
            HasRequired(a => a.ScBizWork).WithMany(b => b.ScMentoringReports).HasForeignKey(c => c.BizWorkSn); // FK_SC_BIZ_WORK_TO_SC_MENTORING_REPORT
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
