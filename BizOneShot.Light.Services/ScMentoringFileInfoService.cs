using System.Collections.Generic;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IScMentoringFileInfoService : IBaseService
    {
        Task<IList<ScMentoringFileInfo>> GetMentoringFileInfo(int reportSn);
    }


    public class ScMentoringFileInfoService : IScMentoringFileInfoService
    {
        private readonly IScMentoringFileInfoRepository scMentoringFileInfoRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScMentoringFileInfoService(IUnitOfWork unitOfWork,
            IScMentoringFileInfoRepository scMentoringFileInfoRepository)
        {
            this.unitOfWork = unitOfWork;
            this.scMentoringFileInfoRepository = scMentoringFileInfoRepository;
        }


        public async Task<IList<ScMentoringFileInfo>> GetMentoringFileInfo(int reportSn)
        {
            return
                await
                    scMentoringFileInfoRepository.GetMentoringFileInfo(
                        mtfi => mtfi.ReportSn == reportSn && mtfi.ScFileInfo.Status == "N");
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

        #endregion
    }
}