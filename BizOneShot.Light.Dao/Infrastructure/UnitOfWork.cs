using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.WebConfiguration;
using BizOneShot.Light.Dao.DareConfiguration;
using System.Data.Entity.Validation;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private WebDbContext dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public WebDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            try { 
            return await DbContext.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        // get the error message 
                        string errorMessage = validationError.ErrorMessage;
                    }
                }
                throw ex;
            }
        }
    }

    public class DareUnitOfWork : IDareUnitOfWork
    {
        private readonly IDareDbFactory dareDbFactory;
        private DareDbContext dareDbContext;

        public DareUnitOfWork(IDareDbFactory dareDbFactory)
        {
            this.dareDbFactory = dareDbFactory;
        }

        public DareDbContext DareDbContext
        {
            get { return dareDbContext ?? (dareDbContext = dareDbFactory.Init()); }
        }

        public void Commit()
        {
            dareDbContext.SaveChanges();

        }

        public async Task<int> CommitAsync()
        {
            return await dareDbContext.SaveChangesAsync();
        }
    }
}
