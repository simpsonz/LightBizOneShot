using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using System.Linq.Expressions;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface IScCompMappingRepository : IRepository<ScCompMapping>
    {
        Task<IList<ScCompMapping>> GetCompMappingsAsync(Expression<Func<ScCompMapping, bool>> where);
        Task<ScCompMapping> GetCompMappingAsync(Expression<Func<ScCompMapping, bool>> where);
        Task<IList<ScCompInfo>> GetCompanysAsync(Expression<Func<ScCompMapping, bool>> where);
    }


    public class ScCompMappingRepository : RepositoryBase<ScCompMapping>, IScCompMappingRepository
    {
        public ScCompMappingRepository(IDbFactory dbFactory) : base(dbFactory) { }


        public async Task<IList<ScCompMapping>> GetCompMappingsAsync(Expression<Func<ScCompMapping, bool>> where)
        {
            return await this.DbContext.ScCompMappings.Include("ScCompInfo").Include("ScBizWork").Include("ScUsr").Include("ScBizWork.ScCompInfo").Include("ScBizWork.ScUsr").Where(where).ToListAsync();
        }

        public async Task<ScCompMapping> GetCompMappingAsync(Expression<Func<ScCompMapping, bool>> where)
        {
            return await this.DbContext.ScCompMappings.Include("ScCompInfo").Include("ScBizWork").Include("ScUsr").Include("ScBizWork.ScCompInfo").Include("ScBizWork.ScUsr").Where(where).SingleAsync();
        }

        public async Task<IList<ScCompInfo>> GetCompanysAsync(Expression<Func<ScCompMapping, bool>> where)
        {
            return await this.DbContext.ScCompMappings.Include("ScCompMappings").Include("ScUsr").Where(where).Select(bw => bw.ScCompInfo).Include("ScUsrs").ToListAsync();
        }
    }
}
