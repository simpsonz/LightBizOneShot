using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.DareModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizOneShot.Light.Services
{
    public interface IFinenceReportService : IBaseService
    {
        Task<IList<SHUSER_SboBosLiteMonthlyCashReturnModel>> GetMonthlyCashListAsync(object[] parameters);
        Task<IList<SHUSER_SboMonthlySalesSelectReturnModel>> GetMonthlySalesAsync(object[] parameters);
        Task<SHUSER_SboMonthlyYearSalesSelectReturnModel> GetYearTotalSalesAsync(object[] parameters);
        Task<IList<SHUSER_SboMonthlyCostAnalysisSelectReturnModel>> GetCostAnalysisAsync(object[] parameters);
        Task<IList<SHUSER_SboMonthlyExpenseCostSelectReturnModel>> GetExpenseCostAsync(object[] parameters);
        Task<IList<SHUSER_SboMonthlyTaxSalesSelectReturnModel>> GetTaxSalesAsync(object[] parameters);
        Task<IList<SHUSER_SboMonthlyBankOutSelectReturnModel>> GetBankOutAsync(object[] parameters);

        Task<SHUSER_SboFinancialTab1SalesSelectReturnModel> GetCompanyMonthSalesAsync(object[] parameters);
        Task<SHUSER_SboFinancialTab2SalesSelectReturnModel> GetCompanyQuarterSalesAsync(object[] parameters);
        Task<SHUSER_SboFinancialTab3SalesSelectReturnModel> GetCompanyYearSalesAsync(object[] parameters);
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


        public async Task<IList<SHUSER_SboBosLiteMonthlyCashReturnModel>> GetMonthlyCashListAsync(object[] parameters)
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

        public async Task<IList<SHUSER_SboMonthlyCostAnalysisSelectReturnModel>> GetCostAnalysisAsync(
            object[] parameters)
        {
            var costAnalysisListTask = await financeReportRepository.GetCostAnalysisAsync(parameters);
            return costAnalysisListTask;
        }

        public async Task<IList<SHUSER_SboMonthlyExpenseCostSelectReturnModel>> GetExpenseCostAsync(object[] parameters)
        {
            var expenseCostTask = await financeReportRepository.GetExpenseCostAsync(parameters);
            return expenseCostTask;
        }

        public async Task<IList<SHUSER_SboMonthlyTaxSalesSelectReturnModel>> GetTaxSalesAsync(object[] parameters)
        {
            var taxSalesListTask = await financeReportRepository.GetTaxSalesAsync(parameters);
            return taxSalesListTask;
        }

        public async Task<IList<SHUSER_SboMonthlyBankOutSelectReturnModel>> GetBankOutAsync(object[] parameters)
        {
            var bankOutListTask = await financeReportRepository.GetBankOutAsync(parameters);
            return bankOutListTask;
        }

        public async Task<SHUSER_SboFinancialTab1SalesSelectReturnModel> GetCompanyMonthSalesAsync(object[] parameters)
        {
            var CompanyMonthSales = await financeReportRepository.GetCompanyMonthSalesAsync(parameters);
            return CompanyMonthSales;
        }

        public async Task<SHUSER_SboFinancialTab2SalesSelectReturnModel> GetCompanyQuarterSalesAsync(object[] parameters)
        {
            var CompanyQuarterSales = await financeReportRepository.GetCompanyQuarterSalesAsync(parameters);
            return CompanyQuarterSales;
        }

        public async Task<SHUSER_SboFinancialTab3SalesSelectReturnModel> GetCompanyYearSalesAsync(object[] parameters)
        {
            var CompanyYearSales = await financeReportRepository.GetCompanyYearSalesAsync(parameters);
            return CompanyYearSales;
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