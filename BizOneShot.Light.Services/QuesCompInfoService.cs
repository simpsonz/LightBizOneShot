using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IQuesCompInfoService : IBaseService
    {
        Task<QuesCompInfo> GetQuesCompInfoAsync(int questionSn);
        Task<int> AddQuesCompInfoAsync(QuesCompInfo quesCompInfo);
    }

    public class QuesCompInfoService : IQuesCompInfoService
    {
        private readonly IQuesCompInfoRepository _quesCompInfoRepository;
        private readonly IUnitOfWork unitOfWork;

        public QuesCompInfoService(IQuesCompInfoRepository _quesCompInfoRepository, IUnitOfWork unitOfWork)
        {
            this._quesCompInfoRepository = _quesCompInfoRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<QuesCompInfo> GetQuesCompInfoAsync(int questionSn)
        {
            var quesCompInfo = await _quesCompInfoRepository.GetAsync(qci => qci.QuestionSn == questionSn);
            return quesCompInfo;
        }

        public async Task<int> AddQuesCompInfoAsync(QuesCompInfo quesCompInfo)
        {
            var rstQuesCompInfo = _quesCompInfoRepository.Insert(quesCompInfo);

            if (rstQuesCompInfo == null)
            {
                return -1;
            }
            return await SaveDbContextAsync();
        }

        #region SaveContext

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