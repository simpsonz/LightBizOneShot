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
    public interface IRptMngCommentRepository : IRepository<RptMngComment>
    {
        RptMngComment Insert(RptMngComment rptMngComment);
        Task<IList<RptMngComment>> GetRptMngCommentsAsync(Expression<Func<RptMngComment, bool>> where);
        Task<RptMngComment> GetRptMngCommentAsync(Expression<Func<RptMngComment, bool>> where);
    }


    public class RptMngCommentRepository : RepositoryBase<RptMngComment>, IRptMngCommentRepository
    {
        public RptMngCommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public RptMngComment Insert(RptMngComment rptMngComment)
        {
            return DbContext.RptMngComments.Add(rptMngComment);
        }

        public async Task<IList<RptMngComment>> GetRptMngCommentsAsync(Expression<Func<RptMngComment, bool>> where)
        {
            return await DbContext.RptMngComments
                .Include(rmc => rmc.RptMngCode)
                .Where(where).ToListAsync();
        }

        public async Task<RptMngComment> GetRptMngCommentAsync(Expression<Func<RptMngComment, bool>> where)
        {
            return await DbContext.RptMngComments
                .Include(rmc => rmc.RptMngCode)
                .Where(where).SingleOrDefaultAsync();
        }
    }
}