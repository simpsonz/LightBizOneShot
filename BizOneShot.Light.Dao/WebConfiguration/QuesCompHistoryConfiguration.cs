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
    // QUES_COMP_HISTORY
    internal partial class QuesCompHistoryConfiguration : EntityTypeConfiguration<QuesCompHistory>
    {
        public QuesCompHistoryConfiguration()
            : this("dbo")
        {
        }
 
        public QuesCompHistoryConfiguration(string schema)
        {
            ToTable(schema + ".QUES_COMP_HISTORY");
            HasKey(x => x.QuestionSn);

            Property(x => x.QuestionSn).HasColumnName("QUESTION_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.HistoryDate).HasColumnName("HISTORY_DATE").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.HistoryContent).HasColumnName("HISTORY_CONTENT").IsOptional().HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasRequired(a => a.QuesMaster).WithOptional(b => b.QuesCompHistory); // FK_QUES_MASTER_TO_QUES_COMP_HISTORY
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
