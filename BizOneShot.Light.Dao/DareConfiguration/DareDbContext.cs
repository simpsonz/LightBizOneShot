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
    public partial class DareDbContext : DbContext, IDareDbContext
    {
        public DbSet<SHUSER_AcStdIncmrateBseIdstT> SHUSER_AcStdIncmrateBseIdstTs { get; set; } // AC_STD_INCMRATE_BSE_IDST_T
        public DbSet<SHUSER_SboFinancialIndexT> SHUSER_SboFinancialIndexTs { get; set; } // SBO_FINANCIAL_INDEX_T
        public DbSet<SHUSER_SyUser> SHUSER_SyUsers { get; set; } // SY_USER
        
        static DareDbContext()
        {
            System.Data.Entity.Database.SetInitializer<DareDbContext>(null);
        }

        public DareDbContext()
            : base("Name=DareDbContext")
        {
            InitializePartial();
        }

        public DareDbContext(string connectionString) : base(connectionString)
        {
            InitializePartial();
        }

        public DareDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
            InitializePartial();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new SHUSER_AcStdIncmrateBseIdstTConfiguration());
            modelBuilder.Configurations.Add(new SHUSER_SboFinancialIndexTConfiguration());
            modelBuilder.Configurations.Add(new SHUSER_SyUserConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new SHUSER_AcStdIncmrateBseIdstTConfiguration(schema));
            modelBuilder.Configurations.Add(new SHUSER_SboFinancialIndexTConfiguration(schema));
            modelBuilder.Configurations.Add(new SHUSER_SyUserConfiguration(schema));
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
    }
}
