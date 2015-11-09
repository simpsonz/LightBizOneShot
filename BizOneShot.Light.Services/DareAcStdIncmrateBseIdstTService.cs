using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.DareModels;
using PagedList;

namespace BizOneShot.Light.Services
{
    public interface IDareAcStdIncmrateBseIdstTService : IBaseService
    {
        Task<IPagedList<SHUSER_AcStdIncmrateBseIdstT>> GetBizTypes(int page, int pageSize, string query = null);
    }


    public class DareAcStdIncmrateBseIdstTService : IDareAcStdIncmrateBseIdstTService
    {
        private readonly IDareAcStdIncmrateBseIdstTRepository _dareAcStdIncmrateBseIdstTRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DareAcStdIncmrateBseIdstTService(IUnitOfWork unitOfWork, IDareAcStdIncmrateBseIdstTRepository dareAcStdIncmrateBseIdstTRepository)
        {
            this._unitOfWork = unitOfWork;
            this._dareAcStdIncmrateBseIdstTRepository = dareAcStdIncmrateBseIdstTRepository;
        }

      
        //Async
        public async Task<IPagedList<SHUSER_AcStdIncmrateBseIdstT>> GetBizTypes(int page, int pageSize, string query = null)
        {
            return await _dareAcStdIncmrateBseIdstTRepository.GetBizTypes(page, pageSize, query);
        }

        

        #region SaveDbContext

        public void SaveDbContext()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await _unitOfWork.CommitAsync();
        }

        #endregion
    }
}