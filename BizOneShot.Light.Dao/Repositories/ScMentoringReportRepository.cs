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
    public interface IScMentoringReportRepository : IRepository<ScMentoringReport>
    {
        Task<IList<int>> GetMentoringReportMentoringDt(string mentorId);
        Task<ScMentoringReport> GetMentoringReportById(int reportSn);
        Task<IList<ScMentoringReport>> GetMentoringReport(Expression<Func<ScMentoringReport, bool>> where);
        //Task<ScMentoringTotalReport> Insert(ScMentoringTotalReport scMentoringTotalReport);
    }


    public class ScMentoringReportRepository : RepositoryBase<ScMentoringReport>, IScMentoringReportRepository
    {
        public ScMentoringReportRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public async Task<IList<int>> GetMentoringReportMentoringDt(string mentorId)
        {
            return await this.DbContext.ScMentoringReports.Where(mtr => mtr.MentorId == mentorId && mtr.Status == "N").Select(mtr => mtr.MentoringDt.Value.Year).Distinct().OrderByDescending(dt => dt).ToListAsync();
        }

        public async Task<ScMentoringReport> GetMentoringReportById(int reportSn)
        {
            return await DbContext.ScMentoringReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                //.Include(mtr => mtr.ScMentoringTrFileInfoes)
                .Include(mtr => mtr.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(mtr => mtr.ReportSn == reportSn)
                .SingleOrDefaultAsync();
        }

        public async Task<IList<ScMentoringReport>> GetMentoringReport(Expression<Func<ScMentoringReport, bool>> where)
        {
            return await DbContext.ScMentoringReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                //.Include(mtr => mtr.ScMentoringTrFileInfoes)
                .Include(mtr => mtr.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(where).ToListAsync();
        }

        //public async Task<ScMentoringTotalReport> Insert(ScMentoringTotalReport scMentoringTotalReport)
        //{
        //    return await Task.Run(() => DbContext.ScMentoringTotalReports.Add(scMentoringTotalReport));
        //}
    }
}
