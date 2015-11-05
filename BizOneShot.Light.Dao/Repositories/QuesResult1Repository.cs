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
    public interface IQuesResult1Repository : IRepository<QuesResult1>
    {
        Task<IList<QuesResult1>> GetQuesResult1sAsync(Expression<Func<QuesResult1, bool>> where);
        Task<QuesResult1> GetQuesResult1Async(Expression<Func<QuesResult1, bool>> where);
    }


    public class QuesResult1Repository : RepositoryBase<QuesResult1>, IQuesResult1Repository
    {
        public QuesResult1Repository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<QuesResult1>> GetQuesResult1sAsync(Expression<Func<QuesResult1, bool>> where)
        {
            return await DbContext.QuesResult1.Include("QuesCheckList").Where(where).ToListAsync();
        }

        public async Task<QuesResult1> GetQuesResult1Async(Expression<Func<QuesResult1, bool>> where)
        {
            return await DbContext.QuesResult1.Include("QuesCheckList").Where(where).SingleOrDefaultAsync();
        }
    }
}