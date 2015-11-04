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

    public interface IExpertMappingRepository : IRepository<ScExpertMapping>
    {
        Task<IList<ScExpertMapping>> GetExperetMappingsAsync(Expression<Func<ScExpertMapping, bool>> where);
        Task<ScExpertMapping> GetExpertMappingAsync(Expression<Func<ScExpertMapping, bool>> where);
    }


    public class ScExpertMappingRepository : RepositoryBase<ScExpertMapping>, IExpertMappingRepository
    {
        public ScExpertMappingRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public async Task<IList<ScExpertMapping>> GetExperetMappingsAsync(Expression<Func<ScExpertMapping, bool>> where)
        {
            
            return await this.DbContext.ScExpertMappings.Include("ScBizWork").Include("ScUsr").Include("ScUsr.ScUsrResume.ScFileInfo").Where(where).ToListAsync();
        }

        public async Task<ScExpertMapping> GetExpertMappingAsync(Expression<Func<ScExpertMapping, bool>> where)
        {
            return await this.DbContext.ScExpertMappings.Include("ScBizWork").Include("ScBizWork.ScCompInfo").Include("ScUsr").Include("ScUsr.ScCompInfo").Include("ScUsr.ScUsrResume.ScFileInfo").Where(where).SingleOrDefaultAsync();
        }
    }
}
