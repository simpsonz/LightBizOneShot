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
    public interface IScFormFileRepository : IRepository<ScFormFile>
    {
    }

    public class ScFormFileRepository : RepositoryBase<ScFormFile>, IScFormFileRepository
    {
        public ScFormFileRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override async Task<IEnumerable<ScFormFile>> GetManyAsync(Expression<Func<ScFormFile, bool>> where)
        {
            return await DbContext.ScFormFiles.Include(i => i.ScFileInfo).Where(where).ToListAsync();
        }
    }
}