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
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class FakeWebDbContext : IWebDbContext
    {
        public DbSet<ScBizType> ScBizTypes { get; set; }
        public DbSet<ScBizWork> ScBizWorks { get; set; }
        public DbSet<ScCompInfo> ScCompInfoes { get; set; }
        public DbSet<ScCompMapping> ScCompMappings { get; set; }
        public DbSet<ScExpertMapping> ScExpertMappings { get; set; }
        public DbSet<ScFaq> ScFaqs { get; set; }
        public DbSet<ScFileInfo> ScFileInfoes { get; set; }
        public DbSet<ScForm> ScForms { get; set; }
        public DbSet<ScFormFile> ScFormFiles { get; set; }
        public DbSet<ScMentoringFileInfo> ScMentoringFileInfoes { get; set; }
        public DbSet<ScMentoringReport> ScMentoringReports { get; set; }
        public DbSet<ScMentoringTotalReport> ScMentoringTotalReports { get; set; }
        public DbSet<ScMentoringTrFileInfo> ScMentoringTrFileInfoes { get; set; }
        public DbSet<ScMentorMappiing> ScMentorMappiings { get; set; }
        public DbSet<ScNtc> ScNtcs { get; set; }
        public DbSet<ScQa> ScQas { get; set; }
        public DbSet<ScQcl> ScQcls { get; set; }
        public DbSet<ScReqDoc> ScReqDocs { get; set; }
        public DbSet<ScReqDocFile> ScReqDocFiles { get; set; }
        public DbSet<ScUsr> ScUsrs { get; set; }
        public DbSet<ScUsrResume> ScUsrResumes { get; set; }
        public DbSet<SyDareDbInfo> SyDareDbInfoes { get; set; }

        public FakeWebDbContext()
        {
            ScBizTypes = new FakeDbSet<ScBizType>();
            ScBizWorks = new FakeDbSet<ScBizWork>();
            ScCompInfoes = new FakeDbSet<ScCompInfo>();
            ScCompMappings = new FakeDbSet<ScCompMapping>();
            ScExpertMappings = new FakeDbSet<ScExpertMapping>();
            ScFaqs = new FakeDbSet<ScFaq>();
            ScFileInfoes = new FakeDbSet<ScFileInfo>();
            ScForms = new FakeDbSet<ScForm>();
            ScFormFiles = new FakeDbSet<ScFormFile>();
            ScMentoringFileInfoes = new FakeDbSet<ScMentoringFileInfo>();
            ScMentoringReports = new FakeDbSet<ScMentoringReport>();
            ScMentoringTotalReports = new FakeDbSet<ScMentoringTotalReport>();
            ScMentoringTrFileInfoes = new FakeDbSet<ScMentoringTrFileInfo>();
            ScMentorMappiings = new FakeDbSet<ScMentorMappiing>();
            ScNtcs = new FakeDbSet<ScNtc>();
            ScQas = new FakeDbSet<ScQa>();
            ScQcls = new FakeDbSet<ScQcl>();
            ScReqDocs = new FakeDbSet<ScReqDoc>();
            ScReqDocFiles = new FakeDbSet<ScReqDocFile>();
            ScUsrs = new FakeDbSet<ScUsr>();
            ScUsrResumes = new FakeDbSet<ScUsrResume>();
            SyDareDbInfoes = new FakeDbSet<SyDareDbInfo>();
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
