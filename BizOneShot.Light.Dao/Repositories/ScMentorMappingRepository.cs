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
    public interface IScMentorMappingRepository : IRepository<ScMentorMappiing>
    {
        Task<IList<ScMentorMappiing>> GetMentorMappingsInScUsrAsync(Expression<Func<ScMentorMappiing, bool>> where);
        Task<IList<ScMentorMappiing>> GetMentorMappingsAsync(Expression<Func<ScMentorMappiing, bool>> where);
        Task<ScMentorMappiing> GetMentorMappingAsync(Expression<Func<ScMentorMappiing, bool>> where);
    }


    public class ScMentorMappingRepository : RepositoryBase<ScMentorMappiing>, IScMentorMappingRepository
    {
        public ScMentorMappingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }


        public async Task<IList<ScMentorMappiing>> GetMentorMappingsInScUsrAsync(
            Expression<Func<ScMentorMappiing, bool>> where)
        {
            return await DbContext.ScMentorMappiings
                .Include(mm => mm.ScUsr)
                .Where(where).ToListAsync();
        }

        public async Task<IList<ScMentorMappiing>> GetMentorMappingsAsync(Expression<Func<ScMentorMappiing, bool>> where)
        {
            return await DbContext.ScMentorMappiings
                .Include("ScBizWork")
                .Include("ScUsr")
                .Include("ScUsr.ScUsrResume.ScFileInfo")
                .Where(where).ToListAsync();
        }

        public async Task<ScMentorMappiing> GetMentorMappingAsync(Expression<Func<ScMentorMappiing, bool>> where)
        {
            return
                await
                    DbContext.ScMentorMappiings.Include("ScBizWork")
                        .Include("ScUsr")
                        .Include("ScUsr.ScUsrResume.ScFileInfo")
                        .Where(where)
                        .SingleAsync();
        }
    }
}