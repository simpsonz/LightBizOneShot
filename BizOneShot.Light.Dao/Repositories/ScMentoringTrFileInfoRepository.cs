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
    public interface IScMentoringTrFileInfoRepository : IRepository<ScMentoringTrFileInfo>
    {
        //Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId);
        Task<IList<ScMentoringTrFileInfo>> GetMentoringTrFileInfo(Expression<Func<ScMentoringTrFileInfo, bool>> where);
    }


    public class ScMentoringTrFileInfoRepository : RepositoryBase<ScMentoringTrFileInfo>,
        IScMentoringTrFileInfoRepository
    {
        public ScMentoringTrFileInfoRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<ScMentoringTrFileInfo>> GetMentoringTrFileInfo(
            Expression<Func<ScMentoringTrFileInfo, bool>> where)
        {
            return await DbContext.ScMentoringTrFileInfoes
                .Include(mtfi => mtfi.ScFileInfo)
                .Where(where).ToListAsync();
        }
    }
}