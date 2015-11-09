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
    public interface IDareDbContext : IDisposable
    {
        DbSet<SHUSER_AcStdIncmrateBseIdstT> SHUSER_AcStdIncmrateBseIdstTs { get; set; } // AC_STD_INCMRATE_BSE_IDST_T
        DbSet<SHUSER_SboFinancialIndexT> SHUSER_SboFinancialIndexTs { get; set; } // SBO_FINANCIAL_INDEX_T
        DbSet<SHUSER_SyUser> SHUSER_SyUsers { get; set; } // SY_USER

        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
        System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
