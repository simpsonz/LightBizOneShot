using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using PagedList;
using PagedList.EntityFramework;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScMentoringReportRepository : IRepository<ScMentoringReport>
    {
        Task<IList<int>> GetMentoringReportMentoringDt(string mentorId);
        Task<ScMentoringReport> GetMentoringReportById(int reportSn);

        Task<IPagedList<ScMentoringReport>> GetPagedListMentoringReportByMngComp(int page, int pageSize, int mngComSn,
            string excutorId = null, int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0, string mentorId = null);

        Task<IList<ScMentoringReport>> GetMentoringReport(Expression<Func<ScMentoringReport, bool>> where);

        Task<IPagedList<ScMentoringReport>> GetPagedListMentoringReport(int page, int pageSize, string mentorId,
            int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0);

        Task<ScMentoringReport> Insert(ScMentoringReport scMentoringReport);

        Task<IList<MentoringStatsByCompanyGroupModel>> GetMentoringReportGroupBy(int bizWorkSn, int startYear,
            int startMonth, int endYear, int endMonth);

        Task<IList<MentoringStatsByMentorGroupModel>> GetMentoringReportGroupByMentor(int bizWorkSn, int startYear,
            int startMonth, int endYear, int endMonth);

        Task<IList<MentoringStatsByMentorCompGroupModel>> GetMentoringReportGroupByMentorComp(int bizWorkSn,
            int startYear, int startMonth, int endYear, int endMonth);

        Task<IList<MentoringStatsByAreaGroupModel>> GetMentoringReportGroupByArea(int bizWorkSn, int startYear,
            int startMonth, int endYear, int endMonth);
    }


    public class ScMentoringReportRepository : RepositoryBase<ScMentoringReport>, IScMentoringReportRepository
    {
        public ScMentoringReportRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<int>> GetMentoringReportMentoringDt(string mentorId)
        {
            return
                await
                    DbContext.ScMentoringReports.Where(mtr => mtr.MentorId == mentorId && mtr.Status == "N")
                        .Select(mtr => mtr.MentoringDt.Value.Year)
                        .Distinct()
                        .OrderByDescending(dt => dt)
                        .ToListAsync();
        }

        public async Task<ScMentoringReport> GetMentoringReportById(int reportSn)
        {
            return await DbContext.ScMentoringReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(mtr => mtr.ReportSn == reportSn)
                //.AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<IPagedList<ScMentoringReport>> GetPagedListMentoringReportByMngComp(int page, int pageSize,
            int mngComSn, string excutorId = null, int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0,
            string mentorId = null)
        {
            return await DbContext.ScMentoringReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(mtr => mtr.ScBizWork.MngCompSn == mngComSn && mtr.Status == "N")
                .Where(
                    mtr =>
                        string.IsNullOrEmpty(excutorId)
                            ? mtr.ScBizWork.ExecutorId != null
                            : mtr.ScBizWork.ExecutorId == excutorId)
                .Where(mtr => compSn == 0 ? mtr.CompSn > compSn : mtr.CompSn == compSn)
                .Where(mtr => mentorId == null ? mtr.MentorId != null : mtr.MentorId == mentorId)
                .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                .Where(
                    mtr =>
                        bizWorkYear == 0
                            ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0
                            : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                              mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                .OrderByDescending(mtr => mtr.ReportSn)
                .AsNoTracking()
                .ToPagedListAsync(page, pageSize);
        }

        //특정필드만 셀렉하는 예제
        //public async Task<IList<MentoringReportViewModel>> GetMentoringReport(Expression<Func<ScMentoringReport, bool>> where)
        //{
        //    return await DbContext.ScMentoringReports
        //        .Include(mtr => mtr.ScBizWork)
        //        .Include(mtr => mtr.ScCompInfo)
        //        .Include(mtr => mtr.ScUsr)
        //        .Include(mtr => mtr.ScMentoringFileInfoes)
        //        .Include(mtr => mtr.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
        //        .Where(where)
        //        .Select(mtr => new
        //            {
        //                mtr.BizWorkSn,
        //                mtr.CompSn,
        //                mtr.ScBizWork.BizWorkNm,
        //                mtr.ScBizWork.BizWorkStDt,
        //                mtr.ScBizWork.BizWorkEdDt,
        //                mtr.ScCompInfo.CompNm,
        //                mtr.ScUsr.Name

        //            }
        //        )
        //        .Select(mtr => new MentoringReportViewModel
        //            {
        //                 BizWorkSn = mtr.BizWorkSn  
        //            }
        //        )
        //        .ToListAsync();
        //}


        public async Task<IPagedList<ScMentoringReport>> GetPagedListMentoringReport(int page, int pageSize,
            string mentorId, int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0)
        {
            var date = DateTime.Now.Date;

            return await DbContext.ScMentoringReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(mtr => mtr.ScBizWork.BizWorkEdDt.Value >= date)
                .Where(mtr => mtr.MentorId == mentorId && mtr.Status == "N")
                .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                .Where(mtr => compSn == 0 ? mtr.CompSn > compSn : mtr.CompSn == compSn)
                .Where(
                    mtr =>
                        bizWorkYear == 0
                            ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0
                            : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                              mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                .OrderByDescending(mtr => mtr.ReportSn)
                .AsNoTracking()
                .ToPagedListAsync(page, pageSize);
        }

        public async Task<IList<ScMentoringReport>> GetMentoringReport(Expression<Func<ScMentoringReport, bool>> where)
        {
            return await DbContext.ScMentoringReports
                .Include(mtr => mtr.ScBizWork)
                .Include(mtr => mtr.ScCompInfo)
                .Include(mtr => mtr.ScUsr)
                .Include(mtr => mtr.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
                .Where(where)
                .AsNoTracking().ToListAsync();
        }


        public async Task<ScMentoringReport> Insert(ScMentoringReport scMentoringReport)
        {
            return await Task.Run(() => DbContext.ScMentoringReports.Add(scMentoringReport));
        }


        public async Task<IList<MentoringStatsByCompanyGroupModel>> GetMentoringReportGroupBy(int bizWorkSn,
            int startYear, int startMonth, int endYear, int endMonth)
        {
            var startDate = new DateTime(startYear, startMonth, 1);
            var endDate = new DateTime(endYear, endMonth, DateTime.DaysInMonth(endYear, endMonth));

            return await DbContext.ScMentoringReports
                .Where(mr => mr.Status == "N")
                .Where(mr => mr.BizWorkSn == bizWorkSn)
                .Where(mr => mr.MentoringDt.Value >= startDate)
                .Where(mr => mr.MentoringDt.Value <= endDate)
                .GroupBy(mr => new {mr.CompSn, mr.MentorAreaCd})
                .Select(g => new MentoringStatsByCompanyGroupModel
                {
                    CompSn = g.Key.CompSn.Value,
                    ComNm = DbContext.ScCompInfoes.Where(ci => ci.CompSn == g.Key.CompSn.Value).FirstOrDefault().CompNm,
                    MentoringAreaCd = g.Key.MentorAreaCd,
                    Count = g.Count()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<MentoringStatsByMentorGroupModel>> GetMentoringReportGroupByMentor(int bizWorkSn,
            int startYear, int startMonth, int endYear, int endMonth)
        {
            var startDate = new DateTime(startYear, startMonth, 1);
            var endDate = new DateTime(endYear, endMonth, DateTime.DaysInMonth(endYear, endMonth));

            var sql =
                @"SELECT MENTOR_ID LoginId, MENTORING_DT MentoringDt, COUNT(REPORT_SN) Count, SUM(DATEDIFF(hour, CONVERT(time,MENTORING_ST_HR),CONVERT(time,MENTORING_ED_HR))) SumMentoringHours " +
                @" FROM SC_MENTORING_REPORT " +
                @" Where BIZ_WORK_SN={0} AND MENTORING_DT BETWEEN {1} AND {2} " +
                @" GROUP BY MENTOR_ID, MENTORING_DT";

            var sqlParamsList = new List<object>();
            sqlParamsList.Add(bizWorkSn);
            sqlParamsList.Add(startDate);
            sqlParamsList.Add(endDate);

            return
                await
                    DbContext.Database.SqlQuery<MentoringStatsByMentorGroupModel>(sql, sqlParamsList.ToArray())
                        .ToListAsync();
        }

        public async Task<IList<MentoringStatsByMentorCompGroupModel>> GetMentoringReportGroupByMentorComp(
            int bizWorkSn, int startYear, int startMonth, int endYear, int endMonth)
        {
            var startDate = new DateTime(startYear, startMonth, 1);
            var endDate = new DateTime(endYear, endMonth, DateTime.DaysInMonth(endYear, endMonth));

            var sql = @"SELECT MENTOR_ID LoginId, COMP_SN CompSn, COUNT(REPORT_SN) Count " +
                      @" FROM SC_MENTORING_REPORT " +
                      @" Where BIZ_WORK_SN={0} AND MENTORING_DT BETWEEN {1} AND {2} " +
                      @" GROUP BY MENTOR_ID, COMP_SN";

            var sqlParamsList = new List<object>();
            sqlParamsList.Add(bizWorkSn);
            sqlParamsList.Add(startDate);
            sqlParamsList.Add(endDate);

            return
                await
                    DbContext.Database.SqlQuery<MentoringStatsByMentorCompGroupModel>(sql, sqlParamsList.ToArray())
                        .ToListAsync();
        }


        public async Task<IList<MentoringStatsByAreaGroupModel>> GetMentoringReportGroupByArea(int bizWorkSn,
            int startYear, int startMonth, int endYear, int endMonth)
        {
            var startDate = new DateTime(startYear, startMonth, 1);
            var endDate = new DateTime(endYear, endMonth, DateTime.DaysInMonth(endYear, endMonth));

            return await DbContext.ScMentoringReports
                .Where(mr => mr.Status == "N")
                .Where(mr => mr.BizWorkSn == bizWorkSn)
                .Where(mr => mr.MentoringDt.Value >= startDate)
                .Where(mr => mr.MentoringDt.Value <= endDate)
                .GroupBy(mr => new {mr.CompSn, mr.MentorAreaCd})
                .Select(g => new MentoringStatsByAreaGroupModel
                {
                    MentoringAreaCd = g.Key.MentorAreaCd,
                    Count = g.Count()
                }).AsNoTracking().ToListAsync();
        }


        //MentoringStatsByMentorCompGroupModel
    }
}