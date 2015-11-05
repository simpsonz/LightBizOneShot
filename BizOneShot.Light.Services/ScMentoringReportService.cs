using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using PagedList;

namespace BizOneShot.Light.Services
{
    public interface IScMentoringReportService : IBaseService
    {
        Task<IList<int>> GetMentoringReportMentoringDt(string mentorId);
        Task<ScMentoringReport> GetMentoringReportById(int reportSn);

        Task<IPagedList<ScMentoringReport>> GetPagedListMentoringReportByMngComp(int page, int pageSize, int mngComSn,
            string excutorId = null, int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0, string mentorId = null);

        Task<IList<ScMentoringReport>> GetMentoringReportAsync(string mentorId, int bizWorkYear = 0, int bizWorkSn = 0,
            int compSn = 0);

        Task<IPagedList<ScMentoringReport>> GetPagedListMentoringReportAsync(int page, int pageSize, string mentorId,
            int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0);

        Task<IList<ScMentoringReport>> GetMentoringReportAsync(int mngComSn, string excutorId = null,
            int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0);

        Task<IList<ScMentoringReport>> GetMentoringReportAsync(int mngComSn, string excutorId = null,
            int bizWorkYear = 0, int bizWorkSn = 0, string mentorId = null);


        Task DeleteMentoringReport(IList<string> listReportSn);
        //Task ModifyMentoringTRStatusDelete(string totalReportSn);

        Task<int> AddScMentoringReportAsync(ScMentoringReport scMentoringReport);
        Task ModifyScMentoringReportAsync(ScMentoringReport scMentoringReport);

        Task<IList<MentoringStatsByCompanyGroupModel>> GetMentoringReportGroupBy(int bizWorkSn, int startYear,
            int startMonth, int endYear, int endMonth);

        Task<IList<MentoringStatsByMentorGroupModel>> GetMentoringReportGroupByMentor(int bizWorkSn, int startYear,
            int startMonth, int endYear, int endMonth);

        Task<IList<MentoringStatsByMentorCompGroupModel>> GetMentoringReportGroupByMentorComp(int bizWorkSn,
            int startYear, int startMonth, int endYear, int endMonth);

        Task<IList<MentoringStatsByAreaGroupModel>> GetMentoringReportGroupByArea(int bizWorkSn, int startYear,
            int startMonth, int endYear, int endMonth);
    }

    public class ScMentoringReportService : IScMentoringReportService
    {
        private readonly IScMentoringReportRepository scMentoringReportRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScMentoringReportService(IUnitOfWork unitOfWork, IScMentoringReportRepository scMentoringReportRepository)
        {
            this.unitOfWork = unitOfWork;
            this.scMentoringReportRepository = scMentoringReportRepository;
        }


        public async Task<IList<int>> GetMentoringReportMentoringDt(string mentorId)
        {
            return await scMentoringReportRepository.GetMentoringReportMentoringDt(mentorId);
        }

        public async Task<ScMentoringReport> GetMentoringReportById(int reportSn)
        {
            return await scMentoringReportRepository.GetMentoringReportById(reportSn);
        }

        public async Task<IPagedList<ScMentoringReport>> GetPagedListMentoringReportByMngComp(int page, int pageSize,
            int mngComSn, string excutorId = null, int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0,
            string mentorId = null)
        {
            return
                await
                    scMentoringReportRepository.GetPagedListMentoringReportByMngComp(page, pageSize, mngComSn, excutorId,
                        bizWorkYear, bizWorkSn, compSn, mentorId);
        }

        public async Task<IList<ScMentoringReport>> GetMentoringReportAsync(string mentorId, int bizWorkYear = 0,
            int bizWorkSn = 0, int compSn = 0)
        {
            var date = DateTime.Now.Date;

            var listScMentoringReport = await scMentoringReportRepository.GetMentoringReport
                (mtr => mtr.MentorId == mentorId && mtr.Status == "N");

            return listScMentoringReport.Where(mtr => mtr.ScBizWork.BizWorkEdDt.Value >= date)
                .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                .Where(mtr => compSn == 0 ? mtr.CompSn > compSn : mtr.CompSn == compSn)
                .Where(
                    mtr =>
                        bizWorkYear == 0
                            ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0
                            : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                              mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                .OrderByDescending(mtr => mtr.ReportSn)
                .ToList();
        }


        public async Task<IPagedList<ScMentoringReport>> GetPagedListMentoringReportAsync(int page, int pageSize,
            string mentorId, int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0)
        {
            return
                await
                    scMentoringReportRepository.GetPagedListMentoringReport(page, pageSize, mentorId, bizWorkYear,
                        bizWorkSn, compSn);
        }

        public async Task<IList<ScMentoringReport>> GetMentoringReportAsync(int mngComSn, string excutorId = null,
            int bizWorkYear = 0, int bizWorkSn = 0, int compSn = 0)
        {
            if (string.IsNullOrEmpty(excutorId))
            {
                var listScMentoringReport =
                    await
                        scMentoringReportRepository.GetMentoringReport(
                            mtr => mtr.ScBizWork.MngCompSn == mngComSn && mtr.Status == "N");

                return listScMentoringReport
                    .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                    .Where(mtr => compSn == 0 ? mtr.CompSn > compSn : mtr.CompSn == compSn)
                    .Where(
                        mtr =>
                            bizWorkYear == 0
                                ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0
                                : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                                  mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                    .OrderByDescending(mtr => mtr.ReportSn)
                    .ToList();
            }
            else
            {
                var listScMentoringReport =
                    await
                        scMentoringReportRepository.GetMentoringReport(
                            mtr =>
                                mtr.ScBizWork.MngCompSn == mngComSn && mtr.ScBizWork.ExecutorId == excutorId &&
                                mtr.Status == "N");

                return listScMentoringReport
                    .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                    .Where(mtr => compSn == 0 ? mtr.CompSn > compSn : mtr.CompSn == compSn)
                    .Where(
                        mtr =>
                            bizWorkYear == 0
                                ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0
                                : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                                  mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                    .OrderByDescending(mtr => mtr.ReportSn)
                    .ToList();
            }
        }

        public async Task<IList<ScMentoringReport>> GetMentoringReportAsync(int mngComSn, string excutorId = null,
            int bizWorkYear = 0, int bizWorkSn = 0, string mentorId = null)
        {
            if (string.IsNullOrEmpty(excutorId))
            {
                var listScMentoringReport =
                    await
                        scMentoringReportRepository.GetMentoringReport(
                            mtr => mtr.ScBizWork.MngCompSn == mngComSn && mtr.Status == "N");

                return listScMentoringReport
                    .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                    .Where(mtr => mentorId == null ? mtr.MentorId != null : mtr.MentorId == mentorId)
                    .Where(
                        mtr =>
                            bizWorkYear == 0
                                ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0
                                : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                                  mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                    .OrderByDescending(mtr => mtr.ReportSn)
                    .ToList();
            }
            else
            {
                var listScMentoringReport =
                    await
                        scMentoringReportRepository.GetMentoringReport(
                            mtr =>
                                mtr.ScBizWork.MngCompSn == mngComSn && mtr.ScBizWork.ExecutorId == excutorId &&
                                mtr.Status == "N");

                return listScMentoringReport
                    .Where(mtr => bizWorkSn == 0 ? mtr.BizWorkSn > bizWorkSn : mtr.BizWorkSn == bizWorkSn)
                    .Where(mtr => mentorId == null ? mtr.MentorId != null : mtr.MentorId == mentorId)
                    .Where(
                        mtr =>
                            bizWorkYear == 0
                                ? mtr.ScBizWork.BizWorkStDt.Value.Year > 0
                                : mtr.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear &&
                                  mtr.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear)
                    .OrderByDescending(mtr => mtr.ReportSn)
                    .ToList();
            }
        }

        public async Task DeleteMentoringReport(IList<string> listReportSn)
        {
            foreach (var reportSn in listReportSn)
            {
                await Task.Run(() => ModifyMentoringReportStatusDelete(reportSn));
            }

            await SaveDbContextAsync();
        }


        public async Task<int> AddScMentoringReportAsync(ScMentoringReport scMentoringReport)
        {
            var rstScMentoringReport = await scMentoringReportRepository.Insert(scMentoringReport);

            if (rstScMentoringReport == null)
            {
                return -1;
            }
            return await SaveDbContextAsync();
        }

        public async Task ModifyScMentoringReportAsync(ScMentoringReport scMentoringReport)
        {
            await Task.Run(() => scMentoringReportRepository.Update(scMentoringReport));


            await SaveDbContextAsync();
        }


        //기업지원 통계 (기업별)
        public async Task<IList<MentoringStatsByCompanyGroupModel>> GetMentoringReportGroupBy(int bizWorkSn,
            int startYear, int startMonth, int endYear, int endMonth)
        {
            return
                await
                    scMentoringReportRepository.GetMentoringReportGroupBy(bizWorkSn, startYear, startMonth, endYear,
                        endMonth);
        }

        //기업지원 통계 (멘토별)
        public async Task<IList<MentoringStatsByMentorGroupModel>> GetMentoringReportGroupByMentor(int bizWorkSn,
            int startYear, int startMonth, int endYear, int endMonth)
        {
            return
                await
                    scMentoringReportRepository.GetMentoringReportGroupByMentor(bizWorkSn, startYear, startMonth,
                        endYear, endMonth);
        }

        //기업지원 통계 (멘토별) 에서 기업수를 가져오기 위한
        public async Task<IList<MentoringStatsByMentorCompGroupModel>> GetMentoringReportGroupByMentorComp(
            int bizWorkSn, int startYear, int startMonth, int endYear, int endMonth)
        {
            return
                await
                    scMentoringReportRepository.GetMentoringReportGroupByMentorComp(bizWorkSn, startYear, startMonth,
                        endYear, endMonth);
        }

        //기업지원 통계 (분야별)
        public async Task<IList<MentoringStatsByAreaGroupModel>> GetMentoringReportGroupByArea(int bizWorkSn,
            int startYear, int startMonth, int endYear, int endMonth)
        {
            return
                await
                    scMentoringReportRepository.GetMentoringReportGroupByArea(bizWorkSn, startYear, startMonth, endYear,
                        endMonth);
        }

        public async Task ModifyMentoringReportStatusDelete(string reportSn)
        {
            var scMentoringReport = await scMentoringReportRepository.GetMentoringReportById(int.Parse(reportSn));

            scMentoringReport.Status = "D";

            foreach (var scFileInfo in scMentoringReport.ScMentoringFileInfoes.Select(mtfi => mtfi.ScFileInfo))
            {
                await Task.Run(() => scFileInfo.Status = "D");
            }

            scMentoringReportRepository.Update(scMentoringReport);
        }

        #region SaveDbContext

        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await unitOfWork.CommitAsync();
        }

        //public Task<IList<ScMentoringTotalReport>> GetMentoringTotalReportAsync(string submitDate = null, int bizWorkSn = 0, int CompSn = 0)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}