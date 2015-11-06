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
    public interface IQuesResult2Repository : IRepository<QuesResult2>
    {
        Task<IList<QuesResult2>> GetQuesResult2sAsync(Expression<Func<QuesResult2, bool>> where);
    }


    public class QuesResult2Repository : RepositoryBase<QuesResult2>, IQuesResult2Repository
    {
        public QuesResult2Repository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<QuesResult2>> GetQuesResult2sAsync(Expression<Func<QuesResult2, bool>> where)
        {
            return await DbContext.QuesResult2.Include("QuesCheckList").Where(where).ToListAsync();
        }
    }
}