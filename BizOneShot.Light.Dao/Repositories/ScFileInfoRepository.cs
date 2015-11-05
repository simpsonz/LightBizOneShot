using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScFileInfoRepository : IRepository<ScFileInfo>
    {
    }

    public class ScFileInfoRepository : RepositoryBase<ScFileInfo>, IScFileInfoRepository
    {
        public ScFileInfoRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}