using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScBizWorkRepository : IRepository<ScBizWork>
    {
        ScBizWork Insert(ScBizWork scBizWork);
        Task<IList<ScBizWork>> GetBizWorksAsync(Expression<Func<ScBizWork, bool>> where);
        Task<ScBizWork> GetBizWorkAsync(Expression<Func<ScBizWork, bool>> where);
        Task<ScBizWork> GetBizWorkByLoginIdAsync(Expression<Func<ScBizWork, bool>> where);


    }


    public class ScBizWorkRepository : RepositoryBase<ScBizWork>, IScBizWorkRepository
    {
        public ScBizWorkRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public ScBizWork Insert(ScBizWork scBizWork)
        {
            return this.DbContext.ScBizWorks.Add(scBizWork);
        }
        public async Task<IList<ScBizWork>> GetBizWorksAsync(Expression<Func<ScBizWork, bool>> where)
        {
            return await this.DbContext.ScBizWorks.Include("ScCompMappings").Include("ScUsr").Where(where).ToListAsync();
        }

        

        public async Task<ScBizWork> GetBizWorkAsync(Expression<Func<ScBizWork, bool>> where)
        {
            return await this.DbContext.ScBizWorks.Include("ScCompMappings").Include("ScUsr").Where(where).SingleOrDefaultAsync();
        }

        public async Task<ScBizWork> GetBizWorkByLoginIdAsync(Expression<Func<ScBizWork, bool>> where)
        {
            return await this.DbContext.ScBizWorks.Include("ScUsr").Where(where).SingleOrDefaultAsync();
        }

    }
}
