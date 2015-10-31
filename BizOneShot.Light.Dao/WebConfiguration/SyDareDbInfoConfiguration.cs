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
    // SY_DARE_DB_INFO
    internal partial class SyDareDbInfoConfiguration : EntityTypeConfiguration<SyDareDbInfo>
    {
        public SyDareDbInfoConfiguration()
            : this("dbo")
        {
        }
 
        public SyDareDbInfoConfiguration(string schema)
        {
            ToTable(schema + ".SY_DARE_DB_INFO");
            HasKey(x => x.DbType);

            Property(x => x.DbType).HasColumnName("DB_TYPE").IsRequired().HasColumnType("nvarchar").HasMaxLength(10).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.DbIp).HasColumnName("DB_IP").IsOptional().HasColumnType("nvarchar").HasMaxLength(30);
            Property(x => x.DbName).HasColumnName("DB_NAME").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.DbState).HasColumnName("DB_STATE").IsOptional().HasColumnType("nvarchar").HasMaxLength(1);
            Property(x => x.NotifyMsg).HasColumnName("NOTIFY_MSG").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.DefaultType).HasColumnName("DEFAULT_TYPE").IsOptional().HasColumnType("nvarchar").HasMaxLength(1);
            Property(x => x.Ect).HasColumnName("ECT").IsOptional().HasColumnType("nvarchar").HasMaxLength(300);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
