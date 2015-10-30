using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.DareModels;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface ISboFinancialIndexTRepository : IRepository<SHUSER_SboFinancialIndexT>
    {
        Task<SHUSER_SboFinancialIndexT>  GetSHUSER_SboFinancialIndexT(Expression<Func<SHUSER_SboFinancialIndexT, bool>> where);
    }


    public class SboFinancialIndexTRepository : DareRepositoryBase<SHUSER_SboFinancialIndexT>, ISboFinancialIndexTRepository
    {
        public SboFinancialIndexTRepository(IDareDbFactory dareDbFactory) : base(dareDbFactory) { }

        public async Task<SHUSER_SboFinancialIndexT> GetSHUSER_SboFinancialIndexT(Expression<Func<SHUSER_SboFinancialIndexT, bool>> where)
        {
            var sboFinancialIndexT = await this.DareDbContext.SHUSER_SboFinancialIndexTs.Where(where).SingleOrDefaultAsync();
            return sboFinancialIndexT;
        }
    }
}
