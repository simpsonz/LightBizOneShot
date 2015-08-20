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

namespace BizOneShot.Light.Dao.Configuration
{
    public interface IWebDbContext : IDisposable
    {
        DbSet<ScBizType> ScBizTypes { get; set; } // SC_BIZ_TYPE
        DbSet<ScBizWork> ScBizWorks { get; set; } // SC_BIZ_WORK
        DbSet<ScCompInfo> ScCompInfoes { get; set; } // SC_COMP_INFO
        DbSet<ScCompMapping> ScCompMappings { get; set; } // SC_COMP_MAPPING
        DbSet<ScExpertMapping> ScExpertMappings { get; set; } // SC_EXPERT_MAPPING
        DbSet<ScFaq> ScFaqs { get; set; } // SC_FAQ
        DbSet<ScFileInfo> ScFileInfoes { get; set; } // SC_FILE_INFO
        DbSet<ScForm> ScForms { get; set; } // SC_FORM
        DbSet<ScFormFile> ScFormFiles { get; set; } // SC_FORM_FILE
        DbSet<ScMentoringFileInfo> ScMentoringFileInfoes { get; set; } // SC_MENTORING_FILE_INFO
        DbSet<ScMentoringReport> ScMentoringReports { get; set; } // SC_MENTORING_REPORT
        DbSet<ScMentoringTotalReport> ScMentoringTotalReports { get; set; } // SC_MENTORING_TOTAL_REPORT
        DbSet<ScMentoringTrFileInfo> ScMentoringTrFileInfoes { get; set; } // SC_MENTORING_TR_FILE_INFO
        DbSet<ScMentorMappiing> ScMentorMappiings { get; set; } // SC_MENTOR_MAPPIING
        DbSet<ScNtc> ScNtcs { get; set; } // SC_NTC
        DbSet<ScQa> ScQas { get; set; } // SC_QA
        DbSet<ScQcl> ScQcls { get; set; } // SC_QCL
        DbSet<ScReqDoc> ScReqDocs { get; set; } // SC_REQ_DOC
        DbSet<ScReqDocFile> ScReqDocFiles { get; set; } // SC_REQ_DOC_FILE
        DbSet<ScUsr> ScUsrs { get; set; } // SC_USR
        DbSet<ScUsrResume> ScUsrResumes { get; set; } // SC_USR_RESUME
        DbSet<SyDareDbInfo> SyDareDbInfoes { get; set; } // SY_DARE_DB_INFO

        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
        System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
