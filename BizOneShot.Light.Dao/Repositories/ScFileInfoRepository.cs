using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.WebConfiguration;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScFileInfoRepository : IRepository<ScFileInfo>
    {
    }

    public class ScFileInfoRepository : RepositoryBase<ScFileInfo>, IScFileInfoRepository
    {
        public ScFileInfoRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
