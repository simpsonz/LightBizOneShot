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
    public interface IScReqDocFileRepository : IRepository<ScReqDocFile>
    {
        Task<IEnumerable<ScReqDocFile>> GetFilesAsync(Expression<Func<ScReqDocFile, bool>> where);
    }

    public class ScReqDocFileRepository : RepositoryBase<ScReqDocFile>, IScReqDocFileRepository
    {
        public ScReqDocFileRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IEnumerable<ScReqDocFile>> GetFilesAsync(Expression<Func<ScReqDocFile, bool>> where)
        {
            return await DbContext.ScReqDocFiles.Include("ScFileInfo").Where(where).ToListAsync();
        }
    }
}