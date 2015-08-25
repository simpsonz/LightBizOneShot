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

namespace BizOneShot.Light.Dao.Configuration
{
    // SC_QCL
    internal partial class ScQclConfiguration : EntityTypeConfiguration<ScQcl>
    {
        public ScQclConfiguration()
            : this("dbo")
        {
        }
 
        public ScQclConfiguration(string schema)
        {
            ToTable(schema + ".SC_QCL");
            HasKey(x => x.QclSn);

            Property(x => x.QclSn).HasColumnName("QCL_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.QclType).HasColumnName("QCL_TYPE").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.QclNm).HasColumnName("QCL_NM").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Status).HasColumnName("STATUS").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.DspOdr).HasColumnName("DSP_ODR").IsRequired().HasColumnType("int");
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasOptional(a => a.ScUsr_RegId).WithMany(b => b.ScQcls_RegId).HasForeignKey(c => c.RegId); // FK_SC_USR_TO_SC_QCL
            HasOptional(a => a.ScUsr_UpdId).WithMany(b => b.ScQcls_UpdId).HasForeignKey(c => c.UpdId); // FK_SC_USR_TO_SC_QCL2
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
