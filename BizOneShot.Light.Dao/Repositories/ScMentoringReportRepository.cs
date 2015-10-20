using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScMentoringReportRepository : IRepository<ScMentoringReport>
    {
        Task<IList<int>> GetMentoringReportMentoringDt(string mentorId);
        Task<ScMentoringReport> GetMentoringReportById(int reportSn);
        Task<IList<ScMentoringReport>> GetMentoringReport(Expression<Func<ScMentoringReport, bool>> where);
        Task<ScMentoringReport> Insert(ScMentoringReport scMentoringReport);
        Task<IList<MentoringStatsByCompanyGroupModel>> GetMentoringReportGroupBy(int bizWorkSn, int startYear, int startMonth, int endYear, int endMonth);
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
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(mtr => mtr.ReportSn == reportSn)
                .SingleOrDefaultAsync();
        }

        public async Task<IList<ScMentoringReport>> GetMentoringReport(Expression<Func<ScMentoringReport, bool>> where)
        {
            return await DbContext.ScMentoringReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(where).ToListAsync();
        }

        public async Task<ScMentoringReport> Insert(ScMentoringReport scMentoringReport)
        {
            return await Task.Run(() => DbContext.ScMentoringReports.Add(scMentoringReport));
        }



        public async Task<IList<MentoringStatsByCompanyGroupModel>> GetMentoringReportGroupBy(int bizWorkSn, int startYear, int startMonth, int endYear, int endMonth)
        {
            var startDate = new DateTime(startYear, startMonth, 1);
            var endDate = new DateTime(endYear, endMonth, DateTime.DaysInMonth(endYear, endMonth));

            var a= await DbContext.ScMentoringReports
                .Where(mr => mr.Status == "N")
                .Where(mr => mr.BizWorkSn == bizWorkSn)
                .Where(mr => mr.SubmitDt.Value >= startDate)
                .Where(mr => mr.SubmitDt.Value <= endDate)
                .GroupBy(mr => new { mr.CompSn, mr.MentorAreaCd })
                .Select(g => new MentoringStatsByCompanyGroupModel
                {
                    CompSn = g.Key.CompSn.Value,
                    ComNm =  DbContext.ScCompInfoes.Where(ci => ci.CompSn == g.Key.CompSn.Value).FirstOrDefault().CompNm,
                    MentoringAreaCd = g.Key.MentorAreaCd,
                    Count = g.Count()
                }).ToListAsync();

            return a;
        }

    }
}
