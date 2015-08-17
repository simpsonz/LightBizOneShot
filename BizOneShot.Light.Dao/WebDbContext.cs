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
using BizOneShot.Light.Models;
using System.Threading;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace BizOneShot.Light.Dao
{
    public partial class WebDbContext : DbContext, IWebDbContext
    {
        public DbSet<ScBizType> ScBizTypes { get; set; } // SC_BIZ_TYPE
        public DbSet<ScBizWork> ScBizWorks { get; set; } // SC_BIZ_WORK
        public DbSet<ScCompInfo> ScCompInfoes { get; set; } // SC_COMP_INFO
        public DbSet<ScCompMapping> ScCompMappings { get; set; } // SC_COMP_MAPPING
        public DbSet<ScExpertMapping> ScExpertMappings { get; set; } // SC_EXPERT_MAPPING
        public DbSet<ScFaq> ScFaqs { get; set; } // SC_FAQ
        public DbSet<ScFileInfo> ScFileInfoes { get; set; } // SC_FILE_INFO
        public DbSet<ScForm> ScForms { get; set; } // SC_FORM
        public DbSet<ScFormFile> ScFormFiles { get; set; } // SC_FORM_FILE
        public DbSet<ScMentoringFileInfo> ScMentoringFileInfoes { get; set; } // SC_MENTORING_FILE_INFO
        public DbSet<ScMentoringReport> ScMentoringReports { get; set; } // SC_MENTORING_REPORT
        public DbSet<ScMentoringTotalReport> ScMentoringTotalReports { get; set; } // SC_MENTORING_TOTAL_REPORT
        public DbSet<ScMentoringTrFileInfo> ScMentoringTrFileInfoes { get; set; } // SC_MENTORING_TR_FILE_INFO
        public DbSet<ScMentorMappiing> ScMentorMappiings { get; set; } // SC_MENTOR_MAPPIING
        public DbSet<ScNtc> ScNtcs { get; set; } // SC_NTC
        public DbSet<ScQa> ScQas { get; set; } // SC_QA
        public DbSet<ScQcl> ScQcls { get; set; } // SC_QCL
        public DbSet<ScReqDoc> ScReqDocs { get; set; } // SC_REQ_DOC
        public DbSet<ScReqDocFile> ScReqDocFiles { get; set; } // SC_REQ_DOC_FILE
        public DbSet<ScUsr> ScUsrs { get; set; } // SC_USR
        public DbSet<ScUsrResume> ScUsrResumes { get; set; } // SC_USR_RESUME
        public DbSet<SyDareDbInfo> SyDareDbInfoes { get; set; } // SY_DARE_DB_INFO
        
        static WebDbContext()
        {
            System.Data.Entity.Database.SetInitializer<WebDbContext>(null);
        }

        public WebDbContext()
            : base("Name=WebDbContext")
        {
            InitializePartial();
        }

        public WebDbContext(string connectionString) : base(connectionString)
        {
            InitializePartial();
        }

        public WebDbContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
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

            modelBuilder.Configurations.Add(new ScBizTypeMapping());
            modelBuilder.Configurations.Add(new ScBizWorkMapping());
            modelBuilder.Configurations.Add(new ScCompInfoMapping());
            modelBuilder.Configurations.Add(new ScCompMappingMapping());
            modelBuilder.Configurations.Add(new ScExpertMappingMapping());
            modelBuilder.Configurations.Add(new ScFaqMapping());
            modelBuilder.Configurations.Add(new ScFileInfoMapping());
            modelBuilder.Configurations.Add(new ScFormMapping());
            modelBuilder.Configurations.Add(new ScFormFileMapping());
            modelBuilder.Configurations.Add(new ScMentoringFileInfoMapping());
            modelBuilder.Configurations.Add(new ScMentoringReportMapping());
            modelBuilder.Configurations.Add(new ScMentoringTotalReportMapping());
            modelBuilder.Configurations.Add(new ScMentoringTrFileInfoMapping());
            modelBuilder.Configurations.Add(new ScMentorMappiingMapping());
            modelBuilder.Configurations.Add(new ScNtcMapping());
            modelBuilder.Configurations.Add(new ScQaMapping());
            modelBuilder.Configurations.Add(new ScQclMapping());
            modelBuilder.Configurations.Add(new ScReqDocMapping());
            modelBuilder.Configurations.Add(new ScReqDocFileMapping());
            modelBuilder.Configurations.Add(new ScUsrMapping());
            modelBuilder.Configurations.Add(new ScUsrResumeMapping());
            modelBuilder.Configurations.Add(new SyDareDbInfoMapping());

            OnModelCreatingPartial(modelBuilder);
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new ScBizTypeMapping(schema));
            modelBuilder.Configurations.Add(new ScBizWorkMapping(schema));
            modelBuilder.Configurations.Add(new ScCompInfoMapping(schema));
            modelBuilder.Configurations.Add(new ScCompMappingMapping(schema));
            modelBuilder.Configurations.Add(new ScExpertMappingMapping(schema));
            modelBuilder.Configurations.Add(new ScFaqMapping(schema));
            modelBuilder.Configurations.Add(new ScFileInfoMapping(schema));
            modelBuilder.Configurations.Add(new ScFormMapping(schema));
            modelBuilder.Configurations.Add(new ScFormFileMapping(schema));
            modelBuilder.Configurations.Add(new ScMentoringFileInfoMapping(schema));
            modelBuilder.Configurations.Add(new ScMentoringReportMapping(schema));
            modelBuilder.Configurations.Add(new ScMentoringTotalReportMapping(schema));
            modelBuilder.Configurations.Add(new ScMentoringTrFileInfoMapping(schema));
            modelBuilder.Configurations.Add(new ScMentorMappiingMapping(schema));
            modelBuilder.Configurations.Add(new ScNtcMapping(schema));
            modelBuilder.Configurations.Add(new ScQaMapping(schema));
            modelBuilder.Configurations.Add(new ScQclMapping(schema));
            modelBuilder.Configurations.Add(new ScReqDocMapping(schema));
            modelBuilder.Configurations.Add(new ScReqDocFileMapping(schema));
            modelBuilder.Configurations.Add(new ScUsrMapping(schema));
            modelBuilder.Configurations.Add(new ScUsrResumeMapping(schema));
            modelBuilder.Configurations.Add(new SyDareDbInfoMapping(schema));
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
    }
}
