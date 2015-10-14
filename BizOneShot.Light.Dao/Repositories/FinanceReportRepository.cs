using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.DareModels;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IFinanceReportRepository : IRepository<SHUSER_SboMonthlyCashSelectReturnModel>
    {
        Task<IList<SHUSER_SboMonthlyCashSelectReturnModel>> GetMonthlyCashListAsync(object[] parameters);
    }


    public class FinanceReportRepository : DareRepositoryBase<SHUSER_SboMonthlyCashSelectReturnModel>, IFinanceReportRepository
    {
        public FinanceReportRepository(IDareDbFactory dbFactory) : base(dbFactory) { }

        public async Task<IList<SHUSER_SboMonthlyCashSelectReturnModel>> GetMonthlyCashListAsync(object[] parameters)
        {
            return await this.DareDbContext.Database.SqlQuery<SHUSER_SboMonthlyCashSelectReturnModel>("SBO_MONTHLY_CASH_SELECT @MEMB_BUSNPERS_NO, @CORP_CODE, @BIZ_CD, @SET_YEAR, @SET_MONTH ", parameters).ToListAsync();
        }


    }
}
