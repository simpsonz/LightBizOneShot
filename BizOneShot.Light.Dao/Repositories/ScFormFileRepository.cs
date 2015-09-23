using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.WebConfiguration;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScFormFileRepository : IRepository<ScFormFile>
    {
    }

    public class ScFormFileRepository : RepositoryBase<ScFormFile>, IScFormFileRepository
    {
        public ScFormFileRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public override async Task<IEnumerable<ScFormFile>> GetManyAsync(Expression<Func<ScFormFile, bool>> where)
        {
            return await this.DbContext.ScFormFiles.Include(i => i.ScFileInfo).Where(where).ToListAsync();

        }

    }
}
