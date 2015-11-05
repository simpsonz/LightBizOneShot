using System.Data.Entity;
using System.Web.Mvc;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.WebConfiguration;

namespace BizOneShot.Light.Web.ComLib
{
    public class WebDbTransactionAttribute : ActionFilterAttribute
    {
        protected readonly IDbFactory dbFactory;
        private WebDbContext dbContext;
        private DbContextTransaction tran;


        public WebDbTransactionAttribute(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public WebDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            tran = DbContext.Database.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                tran.Commit();
            }
            else
            {
                tran.Rollback();
            }
        }
    }
}