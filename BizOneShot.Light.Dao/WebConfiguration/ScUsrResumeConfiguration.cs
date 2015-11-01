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
    // SC_USR_RESUME
    internal partial class ScUsrResumeConfiguration : EntityTypeConfiguration<ScUsrResume>
    {
        public ScUsrResumeConfiguration()
            : this("dbo")
        {
        }
 
        public ScUsrResumeConfiguration(string schema)
        {
            ToTable(schema + ".SC_USR_RESUME");
            HasKey(x => x.LoginId);

            Property(x => x.LoginId).HasColumnName("LOGIN_ID").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FileSn).HasColumnName("FILE_SN").IsOptional().HasColumnType("int");

            // Foreign keys
            HasOptional(a => a.ScFileInfo).WithMany(b => b.ScUsrResumes).HasForeignKey(c => c.FileSn); // FK_SC_FILE_INFO_TO_SC_USR_RESUME
            HasRequired(a => a.ScUsr).WithOptional(b => b.ScUsrResume); // FK_SC_USR_TO_SC_USR_RESUME
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
