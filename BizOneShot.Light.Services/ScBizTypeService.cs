using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IScBizTypeService : IBaseService
    {
        Task<IEnumerable<ScBizType>> GetScBizTypeByCompSn(int compSn);
        void DeleteScBizTypeByCompSn(int compSn);
        void AddScBizType(ScBizType scBizType);
    }


    public class ScBizTypeService : IScBizTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IScBizTypeRepository _scBizTypeRepository;



        public ScBizTypeService(IUnitOfWork unitOfWork, IScBizTypeRepository scBizTypeRepository)
        {
            _unitOfWork = unitOfWork;
            _scBizTypeRepository = scBizTypeRepository;
        }


        public async Task<IEnumerable<ScBizType>> GetScBizTypeByCompSn(int compSn)
        {
            var listScBizType = await _scBizTypeRepository.GetManyAsync(bt => bt.CompSn == compSn);
            return listScBizType.OrderBy(bt => bt.BizTypeCd);
        }

        public void DeleteScBizTypeByCompSn(int compSn)
        {
            _scBizTypeRepository.Delete(bt => bt.CompSn == compSn);
        }

        public void AddScBizType(ScBizType scBizType)
        {
            _scBizTypeRepository.Add(scBizType);
        }


        public void SaveDbContext()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await _unitOfWork.CommitAsync();
        }
    }
}