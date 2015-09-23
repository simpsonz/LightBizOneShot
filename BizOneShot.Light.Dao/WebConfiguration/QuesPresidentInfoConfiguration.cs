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
    // QUES_PRESIDENT_INFO
    internal partial class QuesPresidentInfoConfiguration : EntityTypeConfiguration<QuesPresidentInfo>
    {
        public QuesPresidentInfoConfiguration()
            : this("dbo")
        {
        }
 
        public QuesPresidentInfoConfiguration(string schema)
        {
            ToTable(schema + ".QUES_PRESIDENT_INFO");
            HasKey(x => x.QuestionSn);

            Property(x => x.QuestionSn).HasColumnName("QUESTION_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.PresidentNm).HasColumnName("PRESIDENT_NM").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.BirthDate).HasColumnName("BIRTH_DATE").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(13);
            Property(x => x.AcademicDegree).HasColumnName("ACADEMIC_DEGREE").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.Major).HasColumnName("MAJOR").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CareerComp1).HasColumnName("CAREER_COMP1").IsOptional().HasColumnType("nvarchar").HasMaxLength(70);
            Property(x => x.Job1).HasColumnName("JOB1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.CareerComp2).HasColumnName("CAREER_COMP2").IsOptional().HasColumnType("nvarchar").HasMaxLength(70);
            Property(x => x.Job2).HasColumnName("JOB2").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.CareerComp3).HasColumnName("CAREER_COMP3").IsOptional().HasColumnType("nvarchar").HasMaxLength(70);
            Property(x => x.Job3).HasColumnName("JOB3").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.CareerBasicYear).HasColumnName("CAREER_BASIC_YEAR").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(4);
            Property(x => x.CareerBasicMonth).HasColumnName("CAREER_BASIC_MONTH").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(2);
            Property(x => x.TotalCareerPeriod).HasColumnName("TOTAL_CAREER_PERIOD").IsOptional().HasColumnType("int");
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasRequired(a => a.QuesMaster).WithOptional(b => b.QuesPresidentInfo); // FK_QUES_MASTER_TO_QUES_PRESIDENT_INFO
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
