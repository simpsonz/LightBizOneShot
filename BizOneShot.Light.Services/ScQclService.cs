using System.Collections.Generic;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IScQclService : IBaseService
    {
        Task<IList<ScQcl>> GetScQclsAsync();
    }


    public class ScQclService : IScQclService
    {
        private readonly IScQclRepository _scQclRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScQclService(IScQclRepository _scQclRepository, IUnitOfWork unitOfWork)
        {
            this._scQclRepository = _scQclRepository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<IList<ScQcl>> GetScQclsAsync()
        {
            var scQcls = await _scQclRepository.GetScQclsAsync(sq => sq.Status == "N");

            return scQcls;
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