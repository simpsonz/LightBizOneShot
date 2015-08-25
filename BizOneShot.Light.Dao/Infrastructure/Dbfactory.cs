using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.WebConfiguration;
using BizOneShot.Light.Dao.DareConfiguration;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        WebDbContext dbContext;

        public WebDbContext Init()
        {
            return dbContext ?? (dbContext = new WebDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }

    public class DareDbFactory : Disposable, IDareDbFactory
    {
        DareDbContext dareDbContext;

        public DareDbContext Init()
        {
            return dareDbContext ?? (dareDbContext = new DareDbContext());
        }

        protected override void DisposeCore()
        {
            if (dareDbContext != null)
                dareDbContext.Dispose();
        }
    }
}
