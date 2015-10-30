using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.DareModels;

namespace BizOneShot.Light.Services
{
    public interface ISboFinancialIndexTService : IBaseService
    {
        Task<SHUSER_SboFinancialIndexT> GetSHUSER_SboFinancialIndexT(string registrationNo, string corpCode, string bizCode, string year);

    }


    public class SboFinancialIndexTService : ISboFinancialIndexTService
    {
        private readonly ISboFinancialIndexTRepository sboFinancialIndexTRepository;
        private readonly IDareUnitOfWork dareUnitOfWork;

        public SboFinancialIndexTService(ISboFinancialIndexTRepository sboFinancialIndexTRepository, IDareUnitOfWork dareUnitOfWor)
        {
            this.sboFinancialIndexTRepository = sboFinancialIndexTRepository;
            this.dareUnitOfWork = dareUnitOfWor;
        }

        public async Task<SHUSER_SboFinancialIndexT> GetSHUSER_SboFinancialIndexT(string registrationNo, string corpCode, string bizCode, string year)
        {
            var sboFinancialIndexT = await sboFinancialIndexTRepository.GetSHUSER_SboFinancialIndexT(i => i.MembBusnpersNo == registrationNo && i.CorpCode == corpCode && i.BizCd == bizCode && i.Year == year);

            return sboFinancialIndexT;
        }

        public void SaveDbContext()
        {
            dareUnitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await dareUnitOfWork.CommitAsync();
        }

    }
}
