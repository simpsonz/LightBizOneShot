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
    public interface IScMentoringTotalReportRepository : IRepository<ScMentoringTotalReport>
    {
        Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId);
        Task<ScMentoringTotalReport> GetMentoringTotalReportById(int totalReportSn);
        Task<IList<ScMentoringTotalReport>> GetMentoringTotalReport(Expression<Func<ScMentoringTotalReport, bool>> where);
        Task<ScMentoringTotalReport> Insert(ScMentoringTotalReport scMentoringTotalReport);

        PagedList<ScMentoringTotalReport> GetMentoringTotalReport(int page, int pageSize, int mngComSn, string excutorId, int bizWorkYear, int bizWorkSn, int compSn);

        //PagedList<ScMentoringTotalReport> GetMentoringTotalReport(Expression<Func<ScMentoringTotalReport, bool>> where, int page, int pageSize);

    }


    public class ScMentoringTotalReportRepository : RepositoryBase<ScMentoringTotalReport>, IScMentoringTotalReportRepository
    {
        public ScMentoringTotalReportRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public async Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId)
        {
            return await this.DbContext.ScMentoringTotalReports.Where(mtr => mtr.MentorId == mentorId && mtr.Status == "N").Select(mtr => mtr.SubmitDt.Value.Year).Distinct().OrderByDescending(dt => dt).ToListAsync();
        }

        public async Task<ScMentoringTotalReport> GetMentoringTotalReportById(int totalReportSn)
        {
            return await DbContext.ScMentoringTotalReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(mtr => mtr.TotalReportSn == totalReportSn)
                .SingleOrDefaultAsync();
        }

        public async Task<IList<ScMentoringTotalReport>> GetMentoringTotalReport(Expression<Func<ScMentoringTotalReport, bool>> where)
        {
            return await DbContext.ScMentoringTotalReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(where).ToListAsync();
        }

        //public PagedList<ScMentoringTotalReport> GetMentoringTotalReport(Expression<Func<ScMentoringTotalReport, bool>> where, int page, int pageSize)
        //{
        //    return DbContext.ScMentoringTotalReports
        //        .Include(mtr => mtr.ScBizWork)
        //        .Include(mtr => mtr.ScCompInfo)
        //        .Include(mtr => mtr.ScUsr)
        //        .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
        //        .Where(where).ToPagedList(page, pageSize);
        //}

        public PagedList<ScMentoringTotalReport> GetMentoringTotalReport(int page, int pageSize, int mngComSn, string excutorId, int bizWorkYear, int bizWorkSn, int compSn)
        {
            if (string.IsNullOrEmpty(excutorId))
            {
                return  DbContext.ScMentoringTotalReports
                    .Include(mtr => mtr.ScBizWork)
                    .Include(mtr => mtr.ScCompInfo)
                    .Include(mtr => mtr.ScUsr)
                    .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                    .Where(mtr => mtr.ScBizWork.MngCompSn == mngComSn && mtr.Status == "N")
                    .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                    .Where(mtr => compSn == 0 ? mtr.CompSn > compSn : mtr.CompSn == compSn)
                    .Where(mtr => bizWorkYear == 0 ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0 : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear && mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                    .OrderByDescending(mtr => mtr.TotalReportSn)
                    .ToPagedList(page, pageSize);
            }
            else
            {
                return  DbContext.ScMentoringTotalReports
                    .Include(mtr => mtr.ScBizWork)
                    .Include(mtr => mtr.ScCompInfo)
                    .Include(mtr => mtr.ScUsr)
                    .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                    .Where(mtr => mtr.ScBizWork.MngCompSn == mngComSn && mtr.ScBizWork.ExecutorId == excutorId && mtr.Status == "N")
                    .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                    .Where(mtr => compSn == 0 ? mtr.CompSn > compSn : mtr.CompSn == compSn)
                    .Where(mtr => bizWorkYear == 0 ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0 : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear && mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                    .OrderByDescending(mtr => mtr.TotalReportSn)
                    .ToPagedList(page, pageSize);
            }
        }

        public async Task<ScMentoringTotalReport> Insert(ScMentoringTotalReport scMentoringTotalReport)
        {
            return await Task.Run(() => DbContext.ScMentoringTotalReports.Add(scMentoringTotalReport));
        }
    }
}
