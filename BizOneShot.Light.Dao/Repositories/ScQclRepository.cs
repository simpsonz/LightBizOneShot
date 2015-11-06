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
    public interface IScQclRepository : IRepository<ScQcl>
    {
        Task<IList<ScQcl>> GetScQclsAsync(Expression<Func<ScQcl, bool>> where);
    }


    public class ScQclRepository : RepositoryBase<ScQcl>, IScQclRepository
    {
        public ScQclRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<ScQcl>> GetScQclsAsync(Expression<Func<ScQcl, bool>> where)
        {
            return await DbContext.ScQcls.Where(where).ToListAsync();
        }
    }
}