using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizOneShot.Light.Services
{
    public interface IPostService : IBaseService
    {
        Task<IList<UspSelectSidoForWebListReturnModel>> GetSidoListAsync();
        Task<IList<UspSelectGunguForWebListReturnModel>> GetGunguListAsync(object[] parameters);

        Task<IList<UspSelectAddressByStreetSearchForWebListReturnModel>> GetAddressByStreetSearchListAsync(
            object[] parameters);

        Task<IList<UspSelectAddressByDongSearchForWebListReturnModel>> GetAddressByDongSearchListAsync(
            object[] parameters);

        Task<IList<UspSelectAddressByBuildingSearchForWebListReturnModel>> GetAddressByBuildingSearchListAsync(
            object[] parameters);
    }


    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork unitOfWork;

        public PostService(IPostRepository _postRepository, IUnitOfWork unitOfWork)
        {
            this._postRepository = _postRepository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<IList<UspSelectSidoForWebListReturnModel>> GetSidoListAsync()
        {
            var listSidoTask = await _postRepository.GetSidoListAsync();
            return listSidoTask.Distinct().OrderBy(sq => sq.SIDO).ToList();
        }

        public async Task<IList<UspSelectGunguForWebListReturnModel>> GetGunguListAsync(object[] parameters)
        {
            var listGunguTask = await _postRepository.GetGunguListAsync(parameters);
            return listGunguTask.Distinct().OrderBy(sq => sq.GUNGU).ToList();
        }

        public async Task<IList<UspSelectAddressByStreetSearchForWebListReturnModel>> GetAddressByStreetSearchListAsync(
            object[] parameters)
        {
            var listGunguTask = await _postRepository.GetAddressByStreetSearchListAsync(parameters);
            return listGunguTask.Distinct().OrderBy(sq => sq.ZIP_CD).ToList();
        }

        public async Task<IList<UspSelectAddressByDongSearchForWebListReturnModel>> GetAddressByDongSearchListAsync(
            object[] parameters)
        {
            var listGunguTask = await _postRepository.GetAddressByDongSearchListAsync(parameters);
            return listGunguTask.Distinct().OrderBy(sq => sq.ZIP_CD).ToList();
        }

        public async Task<IList<UspSelectAddressByBuildingSearchForWebListReturnModel>>
            GetAddressByBuildingSearchListAsync(object[] parameters)
        {
            var listGunguTask = await _postRepository.GetAddressByBuildingSearchListAsync(parameters);
            return listGunguTask.Distinct().OrderBy(sq => sq.ZIP_CD).ToList();
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