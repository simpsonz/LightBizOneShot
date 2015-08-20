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
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using BizOneShot.Light.Models;
using System.Threading;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace BizOneShot.Light.Dao.Mappings
{
    // SC_REQ_DOC_FILE
    internal partial class ScReqDocFileMapping : EntityTypeConfiguration<ScReqDocFile>
    {
        public ScReqDocFileMapping()
            : this("dbo")
        {
        }
 
        public ScReqDocFileMapping(string schema)
        {
            ToTable(schema + ".SC_REQ_DOC_FILE");
            HasKey(x => x.FileSn2);

            Property(x => x.FileSn2).HasColumnName("FILE_SN2").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ReqDocSn).HasColumnName("REQ_DOC_SN").IsRequired().HasColumnType("int");
            Property(x => x.RegType).HasColumnName("REG_TYPE").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);

            // Foreign keys
            HasRequired(a => a.ScFileInfo).WithOptional(b => b.ScReqDocFile); // FK_SC_FILE_INFO_TO_SC_REQ_DOC_FILE
            HasRequired(a => a.ScReqDoc).WithMany(b => b.ScReqDocFiles).HasForeignKey(c => c.ReqDocSn); // FK_SC_REQ_DOC_TO_SC_REQ_DOC_FILE
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
