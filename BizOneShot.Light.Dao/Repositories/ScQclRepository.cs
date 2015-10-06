using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.WebConfiguration;
using System.Linq.Expressions;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface IScQclRepository : IRepository<ScQcl>
    {
        Task<IList<ScQcl>> GetScQclsAsync(Expression<Func<ScQcl, bool>> where);
    }


    public class ScQclRepository : RepositoryBase<ScQcl>, IScQclRepository
    {
        public ScQclRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public async Task<IList<ScQcl>> GetScQclsAsync(Expression<Func<ScQcl, bool>> where)
        {
            return await this.DbContext.ScQcls.Where(where).ToListAsync();
        }

    }
}
