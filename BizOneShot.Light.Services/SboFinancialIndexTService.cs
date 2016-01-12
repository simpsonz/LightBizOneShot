using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.DareModels;

namespace BizOneShot.Light.Services
{
    public interface ISboFinancialIndexTService : IBaseService
    {
        Task<SHUSER_SboFinancialIndexT> GetSHUSER_SboFinancialIndexT(string registrationNo, string corpCode,
            string bizCode, string year);
    }


    public class SboFinancialIndexTService : ISboFinancialIndexTService
    {
        private readonly IDareUnitOfWork dareUnitOfWork;
        private readonly ISboFinancialIndexTRepository sboFinancialIndexTRepository;

        public SboFinancialIndexTService(ISboFinancialIndexTRepository sboFinancialIndexTRepository,
            IDareUnitOfWork dareUnitOfWor)
        {
            this.sboFinancialIndexTRepository = sboFinancialIndexTRepository;
            dareUnitOfWork = dareUnitOfWor;
        }

        public async Task<SHUSER_SboFinancialIndexT> GetSHUSER_SboFinancialIndexT(string registrationNo, string corpCode,
            string bizCode, string year)
        {
            //보고서 재무정보는 전년도의 재무정보를 조회하여 계산한다.
            int queryYear = int.Parse(year) - 1;
            year = queryYear.ToString();
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