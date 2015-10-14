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
        Task<IList<SHUSER_SboMonthlySalesSelectReturnModel>> GetMonthlySalesAsync(object[] parameters);
        Task<SHUSER_SboMonthlyYearSalesSelectReturnModel> GetYearTotalSalesAsync(object[] parameters);
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

        public async Task<IList<SHUSER_SboMonthlySalesSelectReturnModel>> GetMonthlySalesAsync(object[] parameters)
        {
            var salesListTask = await financeReportRepository.GetMonthlySalesAsync(parameters);
            return salesListTask;
        }

        public async Task<SHUSER_SboMonthlyYearSalesSelectReturnModel> GetYearTotalSalesAsync(object[] parameters)
        {
            var yearTotalTask = await financeReportRepository.GetYearTotalSalesAsync(parameters);
            return yearTotalTask;
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
