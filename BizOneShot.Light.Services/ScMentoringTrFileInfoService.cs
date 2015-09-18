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
    public interface IScMentoringTrFileInfoService : IBaseService
    {
        Task<IList<ScMentoringTrFileInfo>> GetMentoringTrFileInfo(int totalReportSn);   
    }


    public class ScMentoringTrFileInfoService : IScMentoringTrFileInfoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IScMentoringTrFileInfoRepository scMentoringTrFileInfoRepository;

        public ScMentoringTrFileInfoService(IUnitOfWork unitOfWork, IScMentoringTrFileInfoRepository scMentoringTrFileInfoRepository)
        {
            this.unitOfWork = unitOfWork;
            this.scMentoringTrFileInfoRepository = scMentoringTrFileInfoRepository;
        }


        public async Task<IList<ScMentoringTrFileInfo>> GetMentoringTrFileInfo(int totalReportSn)
        {
            return await scMentoringTrFileInfoRepository.GetMentoringTrFileInfo(mtfi => mtfi.TotalReportSn == totalReportSn && mtfi.ScFileInfo.Status == "N");
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
