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
    // QUES_RESULT2
    internal partial class QuesResult2Configuration : EntityTypeConfiguration<QuesResult2>
    {
        public QuesResult2Configuration()
            : this("dbo")
        {
        }

        public QuesResult2Configuration(string schema)
        {
            ToTable(schema + ".QUES_RESULT2");
            HasKey(x => new {x.QuestionSn, x.CheckListSn});

            Property(x => x.QuestionSn)
                .HasColumnName("QUESTION_SN")
                .IsRequired()
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CheckListSn)
                .HasColumnName("CHECK_LIST_SN")
                .IsRequired()
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BasicYear).HasColumnName("BASIC_YEAR").IsOptional().HasColumnType("int");
            Property(x => x.D)
                .HasColumnName("D")
                .IsOptional()
                .IsUnicode(false)
                .HasColumnType("varchar")
                .HasMaxLength(15);
            Property(x => x.D451)
                .HasColumnName("D-1")
                .IsOptional()
                .IsUnicode(false)
                .HasColumnType("varchar")
                .HasMaxLength(15);
            Property(x => x.D452)
                .HasColumnName("D-2")
                .IsOptional()
                .IsUnicode(false)
                .HasColumnType("varchar")
                .HasMaxLength(15);
            Property(x => x.D453)
                .HasColumnName("D-3")
                .IsOptional()
                .IsUnicode(false)
                .HasColumnType("varchar")
                .HasMaxLength(15);
            Property(x => x.RegId)
                .HasColumnName("REG_ID")
                .IsOptional()
                .IsUnicode(false)
                .HasColumnType("varchar")
                .HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId)
                .HasColumnName("UPD_ID")
                .IsOptional()
                .IsUnicode(false)
                .HasColumnType("varchar")
                .HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasRequired(a => a.QuesCheckList).WithMany(b => b.QuesResult2).HasForeignKey(c => c.CheckListSn);
                // FK_QUES_CHECK_LIST_TO_QUES_RESULT2
            HasRequired(a => a.QuesMaster).WithMany(b => b.QuesResult2).HasForeignKey(c => c.QuestionSn);
                // FK_QUES_MASTER_TO_QUES_RESULT2
            InitializePartial();
        }

        partial void InitializePartial();
    }
}