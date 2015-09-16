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
    public interface IScMentoringTotalReportRepository : IRepository<ScMentoringTotalReport>
    {
        Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId);
        Task<IList<ScMentoringTotalReport>> GetMentoringTotalReport(Expression<Func<ScMentoringTotalReport, bool>> where); 
    }


    public class ScMentoringTotalReportRepository : RepositoryBase<ScMentoringTotalReport>, IScMentoringTotalReportRepository
    {
        public ScMentoringTotalReportRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public async Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId)
        {
            return await this.DbContext.ScMentoringTotalReports.Where(mtr => mtr.MentorId == mentorId).Select(mtr => mtr.SubmitDt.Value.Year).Distinct().OrderByDescending(dt => dt).ToListAsync();
        }

        public async Task<IList<ScMentoringTotalReport>> GetMentoringTotalReport(Expression<Func<ScMentoringTotalReport, bool>> where)
        {
            return await this.DbContext.ScMentoringTotalReports
                .Include(mtr => mtr.ScBizWork).Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScMentoringTrFileInfoes)
                .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(where).ToListAsync();
        }
    }
}
