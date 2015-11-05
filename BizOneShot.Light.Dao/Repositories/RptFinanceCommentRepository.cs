using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IRptFinanceCommentRepository : IRepository<RptFinanceComment>
    {
        RptFinanceComment Insert(RptFinanceComment rptFinanceComment);
        Task<IList<RptFinanceComment>> GetRptFinanceCommentsAsync(Expression<Func<RptFinanceComment, bool>> where);
        Task<RptFinanceComment> GetRptFinanceCommentAsync(Expression<Func<RptFinanceComment, bool>> where);
    }


    public class RptFinanceCommentRepository : RepositoryBase<RptFinanceComment>, IRptFinanceCommentRepository
    {
        public RptFinanceCommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public RptFinanceComment Insert(RptFinanceComment rptFinanceComment)
        {
            return DbContext.RptFinanceComments.Add(rptFinanceComment);
        }

        public async Task<IList<RptFinanceComment>> GetRptFinanceCommentsAsync(
            Expression<Func<RptFinanceComment, bool>> where)
        {
            return
                await DbContext.RptFinanceComments.Include("ScBizWork").Include("ScCompInfo").Where(where).ToListAsync();
        }

        public async Task<RptFinanceComment> GetRptFinanceCommentAsync(Expression<Func<RptFinanceComment, bool>> where)
        {
            return
                await
                    DbContext.RptFinanceComments.Include("ScBizWork")
                        .Include("ScCompInfo")
                        .Where(where)
                        .SingleOrDefaultAsync();
        }
    }
}