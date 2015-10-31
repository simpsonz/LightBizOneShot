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
    // SC_FORM_FILE
    internal partial class ScFormFileConfiguration : EntityTypeConfiguration<ScFormFile>
    {
        public ScFormFileConfiguration()
            : this("dbo")
        {
        }
 
        public ScFormFileConfiguration(string schema)
        {
            ToTable(schema + ".SC_FORM_FILE");
            HasKey(x => x.FileSn);

            Property(x => x.FileSn).HasColumnName("FILE_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FormSn).HasColumnName("FORM_SN").IsRequired().HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.ScFileInfo).WithOptional(b => b.ScFormFile); // FK_SC_FILE_INFO_TO_SC_FORM_FILE
            HasRequired(a => a.ScForm).WithMany(b => b.ScFormFiles).HasForeignKey(c => c.FormSn); // FK_SC_FORM_TO_SC_FORM_FILE
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
