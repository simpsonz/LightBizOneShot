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
    public interface IScMentoringFileInfoRepository : IRepository<ScMentoringFileInfo>
    {
        Task<IList<ScMentoringFileInfo>> GetMentoringFileInfo(Expression<Func<ScMentoringFileInfo, bool>> where);
    }


    public class ScMentoringFileInfoRepository : RepositoryBase<ScMentoringFileInfo>, IScMentoringFileInfoRepository
    {
        public ScMentoringFileInfoRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<ScMentoringFileInfo>> GetMentoringFileInfo(
            Expression<Func<ScMentoringFileInfo, bool>> where)
        {
            return await DbContext.ScMentoringFileInfoes
                .Include(mtfi => mtfi.ScFileInfo)
                .Where(where).ToListAsync();
        }
    }
}