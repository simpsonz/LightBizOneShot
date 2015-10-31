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
    // SC_FAQ
    internal partial class ScFaqConfiguration : EntityTypeConfiguration<ScFaq>
    {
        public ScFaqConfiguration()
            : this("dbo")
        {
        }
 
        public ScFaqConfiguration(string schema)
        {
            ToTable(schema + ".SC_FAQ");
            HasKey(x => x.FaqSn);

            Property(x => x.FaqSn).HasColumnName("FAQ_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.QclSn).HasColumnName("QCL_SN").IsOptional().HasColumnType("int");
            Property(x => x.QstTxt).HasColumnName("QST_TXT").IsOptional().HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.AnsTxt).HasColumnName("ANS_TXT").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.Stat).HasColumnName("STAT").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasOptional(a => a.ScQcl).WithMany(b => b.ScFaqs).HasForeignKey(c => c.QclSn); // FK_SC_QCL_TO_SC_FAQ
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
