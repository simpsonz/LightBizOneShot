using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IScReqDocFileService : IBaseService
    {
        Task<IList<ScReqDocFile>> GetReqFilesAsync(int reqDocSn, string regType);
    }


    public class ScReqDocFileService : IScReqDocFileService
    {
        private readonly IScReqDocFileRepository _scReqDocFileRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScReqDocFileService(IScReqDocFileRepository _scReqDocFileRepository, IUnitOfWork unitOfWork)
        {
            this._scReqDocFileRepository = _scReqDocFileRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<ScReqDocFile>> GetReqFilesAsync(int reqDocSn, string regType)
        {
            var listScReqDocFileTask =
                await
                    _scReqDocFileRepository.GetFilesAsync(
                        file => file.ReqDocSn == reqDocSn && file.ScFileInfo.Status == "N" && file.RegType == regType);

            return listScReqDocFileTask.OrderBy(file => file.FileSn).ToList();
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