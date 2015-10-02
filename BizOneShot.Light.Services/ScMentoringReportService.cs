using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using System.Threading.Tasks;

using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;

namespace BizOneShot.Light.Services
{
    public interface IScMentoringReportService : IBaseService
    {
        Task<IList<int>> GetMentoringReportMentoringDt(string mentorId);
        Task<ScMentoringReport> GetMentoringReportById(int reportSn);
        Task<IList<ScMentoringReport>> GetMentoringReportAsync(string mentorId, int submitDt = 0, int bizWorkSn = 0, int CompSn = 0);

        Task DeleteMentoringReport(IList<string> listReportSn);
        //Task ModifyMentoringTRStatusDelete(string totalReportSn);

        Task<int> AddScMentoringReportAsync(ScMentoringReport scMentoringReport);
        Task ModifyScMentoringReportAsync(ScMentoringReport scMentoringReport);
    }


    public class ScMentoringReportService : IScMentoringReportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IScMentoringReportRepository scMentoringReportRepository;

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

        public async Task<IList<ScMentoringReport>> GetMentoringReportAsync(string mentorId, int submitDt = 0, int bizWorkSn = 0, int compSn = 0)
        {


            var listScMentoringReport = await scMentoringReportRepository.GetMentoringReport
                (mtr => mtr.MentorId == mentorId && mtr.Status == "N");


            return listScMentoringReport.Where(mtr => submitDt != 0 ? mtr.MentoringDt.Value.Year == submitDt : mtr.MentoringDt.Value.Year > submitDt)
                .Where(mtr => bizWorkSn != 0 ? mtr.BizWorkSn == bizWorkSn : mtr.BizWorkSn > bizWorkSn)
                .Where(mtr => compSn != 0 ? mtr.CompSn == compSn : mtr.CompSn > compSn)
                .OrderByDescending(mtr => mtr.ReportSn)
                .ToList();
        }

        public async Task DeleteMentoringReport(IList<string> listReportSn)
        {
            foreach (var reportSn in listReportSn)
            {
                await Task.Run(() => ModifyMentoringReportStatusDelete(reportSn));
            }

            await SaveDbContextAsync();
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


        public async Task<int> AddScMentoringReportAsync(ScMentoringReport scMentoringReport)
        {
            var rstScMentoringReport = await scMentoringReportRepository.Insert(scMentoringReport);

            if (rstScMentoringReport == null)
            {
                return -1;
            }
            else
            {
                return await SaveDbContextAsync();
            }

        }

        public async Task ModifyScMentoringReportAsync(ScMentoringReport scMentoringReport)
        {
            await Task.Run(() => scMentoringReportRepository.Update(scMentoringReport));

          
            await SaveDbContextAsync();
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

        public Task<IList<ScMentoringTotalReport>> GetMentoringTotalReportAsync(string submitDate = null, int bizWorkSn = 0, int CompSn = 0)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
