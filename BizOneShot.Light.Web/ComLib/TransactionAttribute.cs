using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Dao.WebConfiguration;
using BizOneShot.Light.Dao.Infrastructure;

namespace BizOneShot.Light.Web.ComLib
{
    public class WebDbTransactionAttribute : ActionFilterAttribute
    {
        protected readonly IDbFactory dbFactory;
        private WebDbContext dbContext;
        private System.Data.Entity.DbContextTransaction tran;

     
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