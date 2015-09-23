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
    public interface IScReqDocFileRepository : Infrastructure.IRepository<ScReqDocFile>
    {
        Task<IEnumerable<ScReqDocFile>> GetFilesAsync(Expression<Func<ScReqDocFile, bool>> where);
    }

    public class ScReqDocFileRepository : RepositoryBase<ScReqDocFile>, IScReqDocFileRepository
    {
        public ScReqDocFileRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public  async Task<IEnumerable<ScReqDocFile>> GetFilesAsync(Expression<Func<ScReqDocFile, bool>> where)
        {
            return await this.DbContext.ScReqDocFiles.Include("ScFileInfo").Where(where).ToListAsync();

        }

    }
}
