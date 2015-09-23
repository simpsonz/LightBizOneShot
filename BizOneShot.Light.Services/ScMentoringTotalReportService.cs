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
    public interface IScMentoringTotalReportService : IBaseService
    {
        //IList<int> GetMentoringTotalReportSubmitDt(string mentorId);
        Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId);
        Task<ScMentoringTotalReport> GetMentoringTotalReportById(int totalReportSn);
        Task<IList<ScMentoringTotalReport>> GetMentoringTotalReportAsync(string mentorId, int submitDt = 0, int bizWorkSn = 0, int CompSn = 0);

        Task DeleteMentoringTotalReport(IList<string> listTotalReportSn);
        Task ModifyMentoringTRStatusDelete(string totalReportSn);
    }


    public class ScMentoringTotalReportService : IScMentoringTotalReportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IScMentoringTotalReportRepository scMentoringTotalReportRepository;

        public ScMentoringTotalReportService(IUnitOfWork unitOfWork, IScMentoringTotalReportRepository scMentoringTotalReportRepository)
        {
            this.unitOfWork = unitOfWork;
            this.scMentoringTotalReportRepository = scMentoringTotalReportRepository;
        }


 
        public async Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId)
        {
            return await scMentoringTotalReportRepository.GetMentoringTotalReportSubmitDt(mentorId);
        }

        public async Task<ScMentoringTotalReport> GetMentoringTotalReportById(int totalReportSn)
        {
            return await scMentoringTotalReportRepository.GetMentoringTotalReportById(totalReportSn);
        }

        public async Task<IList<ScMentoringTotalReport>> GetMentoringTotalReportAsync(string mentorId, int submitDt = 0, int bizWorkSn = 0, int compSn = 0)
        {
            //return await scMentoringTotalReportRepository.GetMentoringTotalReport
            //    (mtr => mtr.MentorId == mentorId && mtr.Status == "N"
            //    && submitDt != 0 ? mtr.RegDt.Value.Year == submitDt : mtr.RegDt.Value.Year > submitDt
            //    && bizWorkSn != 0 ? mtr.BizWorkSn == bizWorkSn : mtr.BizWorkSn > bizWorkSn
            //    && compSn != 0 ? mtr.CompSn == compSn : mtr.CompSn > compSn
            //    );

            var listScMentoringTotalReport = await scMentoringTotalReportRepository.GetMentoringTotalReport
                (mtr => mtr.MentorId == mentorId && mtr.Status == "N");

            
            return listScMentoringTotalReport.Where(mtr => submitDt != 0 ? mtr.RegDt.Value.Year == submitDt : mtr.RegDt.Value.Year > submitDt)
                .Where(mtr => bizWorkSn != 0 ? mtr.BizWorkSn == bizWorkSn : mtr.BizWorkSn > bizWorkSn)
                .Where(mtr => compSn != 0 ? mtr.CompSn == compSn : mtr.CompSn > compSn)
                .ToList();
        }

        public async Task DeleteMentoringTotalReport(IList<string> listTotalReportSn)
        {
            foreach (var totalReportSn in listTotalReportSn)
            {
                await Task.Run(() => ModifyMentoringTRStatusDelete(totalReportSn));
            }

            await SaveDbContextAsync();
        }

        public async Task ModifyMentoringTRStatusDelete(string totalReportSn)
        {
            var scMentoringTotalReport = await Task.Run(() => scMentoringTotalReportRepository.GetById(int.Parse(totalReportSn)));

            scMentoringTotalReport.Status = "D";

            foreach (var scFileInfo in scMentoringTotalReport.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
            {
                await Task.Run(() => scFileInfo.Status = "D");
            }

            scMentoringTotalReportRepository.Update(scMentoringTotalReport);
        }

        //public void DeleteMentoringTotalReport(IList<string> listTotalReportSn)
        //{
        //    foreach (var totalReportSn in listTotalReportSn)
        //    {
        //        ModifyMentoringTRStatusDelete(totalReportSn);
        //    }

        //    SaveDbContext();
        //}

        //public void ModifyMentoringTRStatusDelete(string totalReportSn)
        //{
        //    var scMentoringTotalReport = scMentoringTotalReportRepository.GetById(int.Parse(totalReportSn));

        //    scMentoringTotalReport.Status = "D";

        //    foreach (var scFileInfo in scMentoringTotalReport.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
        //    {
        //        scFileInfo.Status = "D";
        //    }

        //    scMentoringTotalReportRepository.Update(scMentoringTotalReport);
        //}




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
