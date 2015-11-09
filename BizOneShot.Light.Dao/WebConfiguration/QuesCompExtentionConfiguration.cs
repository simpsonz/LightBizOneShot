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
    // QUES_COMP_EXTENTION
    internal partial class QuesCompExtentionConfiguration : EntityTypeConfiguration<QuesCompExtention>
    {
        public QuesCompExtentionConfiguration()
            : this("dbo")
        {
        }
 
        public QuesCompExtentionConfiguration(string schema)
        {
            ToTable(schema + ".QUES_COMP_EXTENTION");
            HasKey(x => x.QuestionSn);

            Property(x => x.QuestionSn).HasColumnName("QUESTION_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.PresidentNm).HasColumnName("PRESIDENT_NM").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.BirthDate).HasColumnName("BIRTH_DATE").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(13);
            Property(x => x.AcademicDegree).HasColumnName("ACADEMIC_DEGREE").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Major).HasColumnName("MAJOR").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CareerComp1).HasColumnName("CAREER_COMP1").IsOptional().HasColumnType("nvarchar").HasMaxLength(70);
            Property(x => x.Job1).HasColumnName("JOB1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.CareerComp2).HasColumnName("CAREER_COMP2").IsOptional().HasColumnType("nvarchar").HasMaxLength(70);
            Property(x => x.Job2).HasColumnName("JOB2").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.CareerComp3).HasColumnName("CAREER_COMP3").IsOptional().HasColumnType("nvarchar").HasMaxLength(70);
            Property(x => x.Job3).HasColumnName("JOB3").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.CareerBasicYear).HasColumnName("CAREER_BASIC_YEAR").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(4);
            Property(x => x.CareerBasicMonth).HasColumnName("CAREER_BASIC_MONTH").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(2);
            Property(x => x.TotalYear).HasColumnName("TOTAL_YEAR").IsOptional().HasColumnType("int");
            Property(x => x.TotalMonth).HasColumnName("TOTAL_MONTH").IsOptional().HasColumnType("int");
            Property(x => x.HistotyDate10).HasColumnName("HISTOTY_DATE10").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistotyDate9).HasColumnName("HISTOTY_DATE9").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistotyDate8).HasColumnName("HISTOTY_DATE8").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistotyDate7).HasColumnName("HISTOTY_DATE7").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistotyDate6).HasColumnName("HISTOTY_DATE6").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistotyDate5).HasColumnName("HISTOTY_DATE5").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistotyDate4).HasColumnName("HISTOTY_DATE4").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistotyDate3).HasColumnName("HISTOTY_DATE3").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistotyDate2).HasColumnName("HISTOTY_DATE2").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistotyDate1).HasColumnName("HISTOTY_DATE1").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistoryContent10).HasColumnName("HISTORY_CONTENT10").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HistoryContent9).HasColumnName("HISTORY_CONTENT9").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HistoryContent8).HasColumnName("HISTORY_CONTENT8").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HistoryContent7).HasColumnName("HISTORY_CONTENT7").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HistoryContent6).HasColumnName("HISTORY_CONTENT6").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HistoryContent5).HasColumnName("HISTORY_CONTENT5").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HistoryContent4).HasColumnName("HISTORY_CONTENT4").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HistoryContent3).HasColumnName("HISTORY_CONTENT3").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HistoryContent2).HasColumnName("HISTORY_CONTENT2").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.HistoryContent1).HasColumnName("HISTORY_CONTENT1").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasRequired(a => a.QuesMaster).WithOptional(b => b.QuesCompExtention); // FK_QUES_MASTER_TO_QUES_PRESIDENT_INFO
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
