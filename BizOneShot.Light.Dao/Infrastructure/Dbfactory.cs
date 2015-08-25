using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.WebConfiguration;
using BizOneShot.Light.Dao.DareConfiguration;

namespace BizOneShot.Light.Dao.Infrastructure
{
    //public class DbFactory : Disposable, IDbFactory
    //{
    //    WebDbContext dbContext;

    //    public WebDbContext Init()
    //    {
    //        return dbContext ?? (dbContext = new WebDbContext());
    //    }

    //    protected override void DisposeCore()
    //    {
    //        if (dbContext != null)
    //            dbContext.Dispose();
    //    }
    //}


    public class DbFactory<TDbConext> : Disposable, IDbFactory<TDbConext>
    {
        DbContext dbContext;

        public DbContext Init()
        {
            if (typeof(TDbConext) == typeof(WebDbContext))
            {
                if (dbContext == null || dbContext.GetType() == typeof(WebDbContext))
                {
                    dbContext = new WebDbContext();
                }
                 
            }
            else
            {
                if (dbContext == null || dbContext.GetType() == typeof(DareDbContext))
                {
                    dbContext = new DareDbContext();
                }
            }

            return dbContext;
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        
    }

}
