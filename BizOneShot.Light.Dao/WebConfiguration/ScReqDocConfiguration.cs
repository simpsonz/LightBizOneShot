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
    // SC_REQ_DOC
    internal partial class ScReqDocConfiguration : EntityTypeConfiguration<ScReqDoc>
    {
        public ScReqDocConfiguration()
            : this("dbo")
        {
        }
 
        public ScReqDocConfiguration(string schema)
        {
            ToTable(schema + ".SC_REQ_DOC");
            HasKey(x => x.ReqDocSn);

            Property(x => x.ReqDocSn).HasColumnName("REQ_DOC_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.SenderId).HasColumnName("SENDER_ID").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.ReceiverId).HasColumnName("RECEIVER_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.Status).HasColumnName("STATUS").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.ChkYn).HasColumnName("CHK_YN").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.ReqDt).HasColumnName("REQ_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.ReqSubject).HasColumnName("REQ_SUBJECT").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ReqContents).HasColumnName("REQ_CONTENTS").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.ResDt).HasColumnName("RES_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.ResContents).HasColumnName("RES_CONTENTS").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);

            // Foreign keys
            HasOptional(a => a.ScUsr_ReceiverId).WithMany(b => b.ScReqDocs_ReceiverId).HasForeignKey(c => c.ReceiverId); // FK_SC_USR_TO_SC_REQ_DOC2
            HasRequired(a => a.ScUsr_SenderId).WithMany(b => b.ScReqDocs_SenderId).HasForeignKey(c => c.SenderId); // FK_SC_USR_TO_SC_REQ_DOC
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
