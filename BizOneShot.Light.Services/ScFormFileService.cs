using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IScFormFileService : IBaseService
    {
        Task<IList<ScFormFile>> GetFormFilesByIdAsync(int? formSn = null);
    }


    public class ScFormFileService : IScFormFileService
    {
        private readonly IScFormFileRepository scFormFileRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScFormFileService(IScFormFileRepository scFormFileRepository, IUnitOfWork unitOfWork)
        {
            this.scFormFileRepository = scFormFileRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<ScFormFile>> GetFormFilesByIdAsync(int? formSn = null)
        {
            var listScFormFileTask =
                await scFormFileRepository.GetManyAsync(file => file.FormSn == formSn && file.ScFileInfo.Status == "N");

            return listScFormFileTask.OrderBy(file => file.FileSn).ToList();
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