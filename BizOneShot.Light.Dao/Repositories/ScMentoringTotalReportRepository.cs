using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using PagedList;
using PagedList.EntityFramework;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScMentoringTotalReportRepository : IRepository<ScMentoringTotalReport>
    {
        Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId);
        Task<ScMentoringTotalReport> GetMentoringTotalReportById(int totalReportSn);
        Task<IList<ScMentoringTotalReport>> GetMentoringTotalReport(Expression<Func<ScMentoringTotalReport, bool>> where);

        Task<IPagedList<ScMentoringTotalReport>> GetPagedListMentoringTotalReport(int page, int pageSize,
            string mentorId, int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0);

        Task<ScMentoringTotalReport> Insert(ScMentoringTotalReport scMentoringTotalReport);

        Task<IPagedList<ScMentoringTotalReport>> GetPagedListMentoringTotalReportMnByMngComp(int page, int pageSize,
            int mngComSn, string excutorId, int bizWorkYear, int bizWorkSn, int compSn, string mentorId);
    }


    public class ScMentoringTotalReportRepository : RepositoryBase<ScMentoringTotalReport>,
        IScMentoringTotalReportRepository
    {
        public ScMentoringTotalReportRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId)
        {
            return
                await
                    DbContext.ScMentoringTotalReports.Where(mtr => mtr.MentorId == mentorId && mtr.Status == "N")
                        .Select(mtr => mtr.SubmitDt.Value.Year)
                        .Distinct()
                        .OrderByDescending(dt => dt)
                        .ToListAsync();
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

        public async Task<IPagedList<ScMentoringTotalReport>> GetPagedListMentoringTotalReport(int page, int pageSize,
            string mentorId, int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0)
        {
            var date = DateTime.Now.Date;

            return await DbContext.ScMentoringTotalReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(mtr => mtr.MentorId == mentorId && mtr.Status == "N")
                .Where(mtr => mtr.ScBizWork.BizWorkEdDt.Value >= date)
                .Where(
                    mtr =>
                        bizWorkYear != 0
                            ? mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                              mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear
                            : mtr.ScBizWork.BizWorkStDt.Value.Year > 2000)
                .Where(mtr => bizWorkSn != 0 ? mtr.BizWorkSn == bizWorkSn : mtr.BizWorkSn > bizWorkSn)
                .Where(mtr => compSn != 0 ? mtr.CompSn == compSn : mtr.CompSn > compSn)
                .OrderByDescending(mtr => mtr.TotalReportSn).AsNoTracking()
                .AsNoTracking()
                .ToPagedListAsync(page, pageSize);
        }


        public async Task<IList<ScMentoringTotalReport>> GetMentoringTotalReport(
            Expression<Func<ScMentoringTotalReport, bool>> where)
        {
            return await DbContext.ScMentoringTotalReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(where)
                //.AsNoTracking()
                .ToListAsync();
        }


        public async Task<IPagedList<ScMentoringTotalReport>> GetPagedListMentoringTotalReportMnByMngComp(int page,
            int pageSize, int mngComSn, string excutorId, int bizWorkYear, int bizWorkSn, int compSn, string mentorId)
        {
            if (string.IsNullOrEmpty(excutorId))
            {
                return await DbContext.ScMentoringTotalReports
                    .Include(mtr => mtr.ScBizWork)
                    .Include(mtr => mtr.ScCompInfo)
                    .Include(mtr => mtr.ScUsr)
                    .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                    .Where(mtr => mtr.ScBizWork.MngCompSn == mngComSn && mtr.Status == "N")
                    .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                    .Where(mtr => compSn == 0 ? mtr.CompSn > compSn : mtr.CompSn == compSn)
                    .Where(mtr => mentorId == null ? mtr.MentorId != null : mtr.MentorId == mentorId)
                    .Where(
                        mtr =>
                            bizWorkYear == 0
                                ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0
                                : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                                  mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                    .OrderByDescending(mtr => mtr.TotalReportSn)
                    .AsNoTracking()
                    .ToPagedListAsync(page, pageSize);
            }
            return await DbContext.ScMentoringTotalReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(
                    mtr =>
                        mtr.ScBizWork.MngCompSn == mngComSn && mtr.ScBizWork.ExecutorId == excutorId &&
                        mtr.Status == "N")
                .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                .Where(mtr => compSn == 0 ? mtr.CompSn > compSn : mtr.CompSn == compSn)
                .Where(mtr => mentorId == null ? mtr.MentorId != null : mtr.MentorId == mentorId)
                .Where(
                    mtr =>
                        bizWorkYear == 0
                            ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0
                            : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                              mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                .OrderByDescending(mtr => mtr.TotalReportSn)
                .AsNoTracking()
                .ToPagedListAsync(page, pageSize);
        }

        public async Task<ScMentoringTotalReport> Insert(ScMentoringTotalReport scMentoringTotalReport)
        {
            return await Task.Run(() => DbContext.ScMentoringTotalReports.Add(scMentoringTotalReport));
        }
    }
}