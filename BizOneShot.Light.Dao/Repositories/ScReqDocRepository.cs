using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.WebConfiguration;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScReqDocRepository : IRepository<ScReqDoc>
    {
    }

    public class ScReqDocRepository : RepositoryBase<ScReqDoc>, IScReqDocRepository
    {
        public ScReqDocRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
