using BizOneShot.Light.Dao.DareConfiguration;
using BizOneShot.Light.Dao.WebConfiguration;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private WebDbContext dbContext;

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
        private DareDbContext dareDbContext;

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