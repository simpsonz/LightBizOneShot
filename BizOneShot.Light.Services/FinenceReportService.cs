using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.DareModels;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{

    public interface IFinenceReportService : IBaseService
    {
        Task<IList<SHUSER_SboMonthlyCashSelectReturnModel>> GetMonthlyCashListAsync(object[] parameters);
    }


    public class FinenceReportService : IFinenceReportService
    {
        private readonly IFinanceReportRepository financeReportRepository;
        private readonly IUnitOfWork unitOfWork;

        public FinenceReportService(IFinanceReportRepository financeReportRepository, IUnitOfWork unitOfWork)
        {
            this.financeReportRepository = financeReportRepository;
            this.unitOfWork = unitOfWork;
        }



        public async Task<IList<SHUSER_SboMonthlyCashSelectReturnModel>> GetMonthlyCashListAsync(object[] parameters)
        {
            var cashListTask = await financeReportRepository.GetMonthlyCashListAsync(parameters);
            return cashListTask;
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
