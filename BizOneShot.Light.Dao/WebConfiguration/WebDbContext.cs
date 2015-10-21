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
    public partial class WebDbContext : DbContext, IWebDbContext
    {
        public DbSet<QuesCheckList> QuesCheckLists { get; set; } // QUES_CHECK_LIST
        public DbSet<QuesCompExtention> QuesCompExtentions { get; set; } // QUES_COMP_EXTENTION
        public DbSet<QuesCompHistory> QuesCompHistories { get; set; } // QUES_COMP_HISTORY
        public DbSet<QuesCompInfo> QuesCompInfoes { get; set; } // QUES_COMP_INFO
        public DbSet<QuesMaster> QuesMasters { get; set; } // QUES_MASTER
        public DbSet<QuesOgranAnalysis> QuesOgranAnalysis { get; set; } // QUES_OGRAN_ANALYSIS
        public DbSet<QuesResult1> QuesResult1 { get; set; } // QUES_RESULT1
        public DbSet<QuesResult2> QuesResult2 { get; set; } // QUES_RESULT2
        public DbSet<QuesWriter> QuesWriters { get; set; } // QUES_WRITER
        public DbSet<RptCheckList> RptCheckLists { get; set; } // RPT_CHECK_LIST
        public DbSet<RptMaster> RptMasters { get; set; } // RPT_MASTER
        public DbSet<RptMentorCheck> RptMentorChecks { get; set; } // RPT_MENTOR_CHECK
        public DbSet<RptMentorComment> RptMentorComments { get; set; } // RPT_MENTOR_COMMENT
        public DbSet<RptMentorRadio> RptMentorRadios { get; set; } // RPT_MENTOR_RADIO
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

            modelBuilder.Configurations.Add(new QuesCheckListConfiguration());
            modelBuilder.Configurations.Add(new QuesCompExtentionConfiguration());
            modelBuilder.Configurations.Add(new QuesCompHistoryConfiguration());
            modelBuilder.Configurations.Add(new QuesCompInfoConfiguration());
            modelBuilder.Configurations.Add(new QuesMasterConfiguration());
            modelBuilder.Configurations.Add(new QuesOgranAnalysisConfiguration());
            modelBuilder.Configurations.Add(new QuesResult1Configuration());
            modelBuilder.Configurations.Add(new QuesResult2Configuration());
            modelBuilder.Configurations.Add(new QuesWriterConfiguration());
            modelBuilder.Configurations.Add(new RptCheckListConfiguration());
            modelBuilder.Configurations.Add(new RptMasterConfiguration());
            modelBuilder.Configurations.Add(new RptMentorCheckConfiguration());
            modelBuilder.Configurations.Add(new RptMentorCommentConfiguration());
            modelBuilder.Configurations.Add(new RptMentorRadioConfiguration());
            modelBuilder.Configurations.Add(new ScBizTypeConfiguration());
            modelBuilder.Configurations.Add(new ScBizWorkConfiguration());
            modelBuilder.Configurations.Add(new ScCompInfoConfiguration());
            modelBuilder.Configurations.Add(new ScCompMappingConfiguration());
            modelBuilder.Configurations.Add(new ScExpertMappingConfiguration());
            modelBuilder.Configurations.Add(new ScFaqConfiguration());
            modelBuilder.Configurations.Add(new ScFileInfoConfiguration());
            modelBuilder.Configurations.Add(new ScFormConfiguration());
            modelBuilder.Configurations.Add(new ScFormFileConfiguration());
            modelBuilder.Configurations.Add(new ScMentoringFileInfoConfiguration());
            modelBuilder.Configurations.Add(new ScMentoringReportConfiguration());
            modelBuilder.Configurations.Add(new ScMentoringTotalReportConfiguration());
            modelBuilder.Configurations.Add(new ScMentoringTrFileInfoConfiguration());
            modelBuilder.Configurations.Add(new ScMentorMappiingConfiguration());
            modelBuilder.Configurations.Add(new ScNtcConfiguration());
            modelBuilder.Configurations.Add(new ScQaConfiguration());
            modelBuilder.Configurations.Add(new ScQclConfiguration());
            modelBuilder.Configurations.Add(new ScReqDocConfiguration());
            modelBuilder.Configurations.Add(new ScReqDocFileConfiguration());
            modelBuilder.Configurations.Add(new ScUsrConfiguration());
            modelBuilder.Configurations.Add(new ScUsrResumeConfiguration());
            modelBuilder.Configurations.Add(new SyDareDbInfoConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new QuesCheckListConfiguration(schema));
            modelBuilder.Configurations.Add(new QuesCompExtentionConfiguration(schema));
            modelBuilder.Configurations.Add(new QuesCompHistoryConfiguration(schema));
            modelBuilder.Configurations.Add(new QuesCompInfoConfiguration(schema));
            modelBuilder.Configurations.Add(new QuesMasterConfiguration(schema));
            modelBuilder.Configurations.Add(new QuesOgranAnalysisConfiguration(schema));
            modelBuilder.Configurations.Add(new QuesResult1Configuration(schema));
            modelBuilder.Configurations.Add(new QuesResult2Configuration(schema));
            modelBuilder.Configurations.Add(new QuesWriterConfiguration(schema));
            modelBuilder.Configurations.Add(new RptCheckListConfiguration(schema));
            modelBuilder.Configurations.Add(new RptMasterConfiguration(schema));
            modelBuilder.Configurations.Add(new RptMentorCheckConfiguration(schema));
            modelBuilder.Configurations.Add(new RptMentorCommentConfiguration(schema));
            modelBuilder.Configurations.Add(new RptMentorRadioConfiguration(schema));
            modelBuilder.Configurations.Add(new ScBizTypeConfiguration(schema));
            modelBuilder.Configurations.Add(new ScBizWorkConfiguration(schema));
            modelBuilder.Configurations.Add(new ScCompInfoConfiguration(schema));
            modelBuilder.Configurations.Add(new ScCompMappingConfiguration(schema));
            modelBuilder.Configurations.Add(new ScExpertMappingConfiguration(schema));
            modelBuilder.Configurations.Add(new ScFaqConfiguration(schema));
            modelBuilder.Configurations.Add(new ScFileInfoConfiguration(schema));
            modelBuilder.Configurations.Add(new ScFormConfiguration(schema));
            modelBuilder.Configurations.Add(new ScFormFileConfiguration(schema));
            modelBuilder.Configurations.Add(new ScMentoringFileInfoConfiguration(schema));
            modelBuilder.Configurations.Add(new ScMentoringReportConfiguration(schema));
            modelBuilder.Configurations.Add(new ScMentoringTotalReportConfiguration(schema));
            modelBuilder.Configurations.Add(new ScMentoringTrFileInfoConfiguration(schema));
            modelBuilder.Configurations.Add(new ScMentorMappiingConfiguration(schema));
            modelBuilder.Configurations.Add(new ScNtcConfiguration(schema));
            modelBuilder.Configurations.Add(new ScQaConfiguration(schema));
            modelBuilder.Configurations.Add(new ScQclConfiguration(schema));
            modelBuilder.Configurations.Add(new ScReqDocConfiguration(schema));
            modelBuilder.Configurations.Add(new ScReqDocFileConfiguration(schema));
            modelBuilder.Configurations.Add(new ScUsrConfiguration(schema));
            modelBuilder.Configurations.Add(new ScUsrResumeConfiguration(schema));
            modelBuilder.Configurations.Add(new SyDareDbInfoConfiguration(schema));
            return modelBuilder;
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
    }
}
