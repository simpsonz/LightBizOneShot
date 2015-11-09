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
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class FakeDareDbContext : IDareDbContext
    {
        public DbSet<SHUSER_AcStdIncmrateBseIdstT> SHUSER_AcStdIncmrateBseIdstTs { get; set; }
        public DbSet<SHUSER_SboFinancialIndexT> SHUSER_SboFinancialIndexTs { get; set; }
        public DbSet<SHUSER_SyUser> SHUSER_SyUsers { get; set; }

        public FakeDareDbContext()
        {
            SHUSER_AcStdIncmrateBseIdstTs = new FakeDbSet<SHUSER_AcStdIncmrateBseIdstT>();
            SHUSER_SboFinancialIndexTs = new FakeDbSet<SHUSER_SboFinancialIndexT>();
            SHUSER_SyUsers = new FakeDbSet<SHUSER_SyUser>();
        }
        
        public int SaveChangesCount { get; private set; } 
        public int SaveChanges()
        {
            ++SaveChangesCount;
            return 1;
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
