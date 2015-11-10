using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScBizTypeRepository : IRepository<ScBizType>
    {
    }


    public class ScBizTypeRepository : RepositoryBase<ScBizType>, IScBizTypeRepository
    {
        public ScBizTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}