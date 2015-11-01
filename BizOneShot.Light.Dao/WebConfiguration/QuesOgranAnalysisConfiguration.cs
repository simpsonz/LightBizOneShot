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
    // QUES_OGRAN_ANALYSIS
    internal partial class QuesOgranAnalysisConfiguration : EntityTypeConfiguration<QuesOgranAnalysis>
    {
        public QuesOgranAnalysisConfiguration()
            : this("dbo")
        {
        }
 
        public QuesOgranAnalysisConfiguration(string schema)
        {
            ToTable(schema + ".QUES_OGRAN_ANALYSIS");
            HasKey(x => new { x.QuestionSn, x.DeptCd });

            Property(x => x.QuestionSn).HasColumnName("QUESTION_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.DeptCd).HasColumnName("DEPT_CD").IsRequired().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Dept1).HasColumnName("DEPT1").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Dept2).HasColumnName("DEPT2").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.OfficerCount).HasColumnName("OFFICER_COUNT").IsOptional().HasColumnType("int");
            Property(x => x.ChiefCount).HasColumnName("CHIEF_COUNT").IsOptional().HasColumnType("int");
            Property(x => x.StaffCount).HasColumnName("STAFF_COUNT").IsOptional().HasColumnType("int");
            Property(x => x.BeginnerCount).HasColumnName("BEGINNER_COUNT").IsOptional().HasColumnType("int");
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasRequired(a => a.QuesMaster).WithMany(b => b.QuesOgranAnalysis).HasForeignKey(c => c.QuestionSn); // FK_QUES_MASTER_TO_QUES_OGRAN_ANALYSIS
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
