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
    // SC_QA
    internal partial class ScQaConfiguration : EntityTypeConfiguration<ScQa>
    {
        public ScQaConfiguration()
            : this("dbo")
        {
        }
 
        public ScQaConfiguration(string schema)
        {
            ToTable(schema + ".SC_QA");
            HasKey(x => x.UsrQaSn);

            Property(x => x.UsrQaSn).HasColumnName("USR_QA_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.QuestionId).HasColumnName("QUESTION_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.AnswerId).HasColumnName("ANSWER_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.AskDt).HasColumnName("ASK_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.Subject).HasColumnName("SUBJECT").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Question).HasColumnName("QUESTION").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.AnsDt).HasColumnName("ANS_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.Answer).HasColumnName("ANSWER").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.AnsYn).HasColumnName("ANS_YN").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);

            // Foreign keys
            HasOptional(a => a.ScUsr_AnswerId).WithMany(b => b.ScQas_AnswerId).HasForeignKey(c => c.AnswerId); // FK_SC_USR_TO_SC_QA2
            HasOptional(a => a.ScUsr_QuestionId).WithMany(b => b.ScQas_QuestionId).HasForeignKey(c => c.QuestionId); // FK_SC_USR_TO_SC_QA
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
