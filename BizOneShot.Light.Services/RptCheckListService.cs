using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizOneShot.Light.Services
{
    public interface IRptCheckListService : IBaseService
    {
        Task<IEnumerable<RptCheckList>> GetRptCheckListBySmallClassCd(string smallClassCd);
    }


    public class RptCheckListService : IRptCheckListService
    {
        private readonly IRptCheckListRepository _rptCheckListRepository;
        private readonly IUnitOfWork _unitOfWork;


        public RptCheckListService(IUnitOfWork unitOfWork, IRptCheckListRepository rptCheckListRepository)
        {
            _unitOfWork = unitOfWork;
            _rptCheckListRepository = rptCheckListRepository;
        }


        public async Task<IEnumerable<RptCheckList>> GetRptCheckListBySmallClassCd(string smallClassCd)
        {
            var rptCheckList = await _rptCheckListRepository.GetManyAsync(cl => cl.SmallClassCd == smallClassCd);
            return rptCheckList.OrderBy(cl => cl.DetailCd);
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