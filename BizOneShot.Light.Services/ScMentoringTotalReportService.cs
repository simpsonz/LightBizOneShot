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
        Task<IList<ScMentoringTotalReport>> GetMentoringTotalReportAsync(string mentorId, int submitDt = 0, int bizWorkSn = 0, int CompSn = 0);

        Task ModifyMentoringTRStatusDelete(IList<string> listTotalReportSn);
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


        //public IList<int> GetMentoringTotalReportSubmitDt(string mentorId)
        //{
        //    return scMentoringTotalReportRepository.GetMany(mtr => mtr.MentorId == mentorId).Select(mtr => mtr.SubmitDt.Value.Year).Distinct().ToList(); 
        //}
        public Task<IList<int>> GetMentoringTotalReportSubmitDt(string mentorId)
        {
            return scMentoringTotalReportRepository.GetMentoringTotalReportSubmitDt(mentorId);
        }

        public async Task<IList<ScMentoringTotalReport>> GetMentoringTotalReportAsync(string mentorId, int submitDt = 0, int bizWorkSn = 0, int compSn = 0)
        {
            //IEnumerable<ScMentoringTotalReport> listTotalReport = null;

            return await scMentoringTotalReportRepository.GetMentoringTotalReport
                (mtr => mtr.MentorId == mentorId && mtr.Status == "N"
                && submitDt != 0 ? mtr.RegDt.Value.Year == submitDt : mtr.RegDt.Value.Year > submitDt
                && bizWorkSn != 0 ? mtr.BizWorkSn == bizWorkSn : mtr.BizWorkSn > bizWorkSn
                && compSn != 0 ? mtr.CompSn == compSn : mtr.CompSn > compSn
                );

            //if (submitYear == 0 && bizWorkSn == 0 && compSn == 0)
            //{
            //    return await scMentoringTotalReportRepository.GetMentoringTotalReport(mtr => mtr.MentorId == mentorId);
            //}
            //else if(submitYear != 0 && bizWorkSn == 0 && compSn == 0)
            //{
            //    return await scMentoringTotalReportRepository.GetMentoringTotalReport(mtr => mtr.MentorId == mentorId && mtr.RegDt.Value.Year == submitYear);
            //}
            //else if(submitYear != 0 && bizWorkSn == 0 && compSn == 0)
            //{

            //}



            //    return listTotalReport.ToList();
        }

        public async Task ModifyMentoringTRStatusDelete(IList<string> listTotalReportSn)
        {  
            foreach(var totalReportSn in listTotalReportSn)
            {
                await Task.Run(() => ModifyMentoringTRStatusDelete(totalReportSn));
            }
        }

        public async Task ModifyMentoringTRStatusDelete(string totalReportSn)
        {
            var scMentoringTotalReport =  await Task.Run(() => scMentoringTotalReportRepository.GetById(int.Parse(totalReportSn)));

            scMentoringTotalReport.Status = "N";

            foreach(var scFileInfo in scMentoringTotalReport.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo))
            {
                await Task.Run(() => scFileInfo.Status = "N");
            }
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
