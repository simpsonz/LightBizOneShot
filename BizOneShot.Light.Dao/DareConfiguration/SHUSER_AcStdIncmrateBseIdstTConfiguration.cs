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
using BizOneShot.Light.Models.DareModels;
using System.Threading;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace BizOneShot.Light.Dao.DareConfiguration
{
    // AC_STD_INCMRATE_BSE_IDST_T
    internal partial class SHUSER_AcStdIncmrateBseIdstTConfiguration : EntityTypeConfiguration<SHUSER_AcStdIncmrateBseIdstT>
    {
        public SHUSER_AcStdIncmrateBseIdstTConfiguration()
            : this("SHUSER")
        {
        }
 
        public SHUSER_AcStdIncmrateBseIdstTConfiguration(string schema)
        {
            ToTable(schema + ".AC_STD_INCMRATE_BSE_IDST_T");
            HasKey(x => x.IdstCd);

            Property(x => x.IdstCd).HasColumnName("IDST_CD").IsRequired().HasColumnType("nvarchar").HasMaxLength(6).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.IdstLgClsfCd).HasColumnName("IDST_LG_CLSF_CD").IsOptional().HasColumnType("nvarchar").HasMaxLength(2);
            Property(x => x.IdstDtlNm).HasColumnName("IDST_DTL_NM").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(256);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
