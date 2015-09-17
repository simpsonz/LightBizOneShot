using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IPostService : IBaseService
    {
        Task<IList<UspSelectSidoForWebListReturnModel>> GetSidosAsync();
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


        public async Task<IList<UspSelectSidoForWebListReturnModel>> GetSidosAsync()
        {
            var listSidoTask = await _postRepository.GetSidosAsync();
            return listSidoTask.Distinct().OrderByDescending(sq => sq.SIDO).ToList();
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
