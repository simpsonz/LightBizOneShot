using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IRptMngCodeRepository : IRepository<RptMngCode>
    {
    }


    public class RptMngCodeRepository : RepositoryBase<RptMngCode>, IRptMngCodeRepository
    {
        public RptMngCodeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}