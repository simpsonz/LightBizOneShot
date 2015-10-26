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

    public interface IRptCheckListService : IBaseService
    {

        Task<IEnumerable<RptCheckList>> GetRptCheckListBySmallClassCd(string smallClassCd);
    }


    public class RptCheckListService : IRptCheckListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRptCheckListRepository _rptCheckListRepository;
        

        public RptCheckListService(IUnitOfWork unitOfWork, IRptCheckListRepository rptCheckListRepository)
        {
            this._unitOfWork = unitOfWork;
            this._rptCheckListRepository = rptCheckListRepository;
            
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
