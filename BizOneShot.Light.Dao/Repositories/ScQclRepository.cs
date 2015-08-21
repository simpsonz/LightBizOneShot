using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BizOneShot.Light.Models;
using BizOneShot.Light.Dao.Infrastructure;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface IScQclRepository : IRepository<ScQcl>
    {
        //IList<ScCompInfo> GetScCompInfoByName(string compNm);
    }


    public class ScQclRepository : RepositoryBase<ScQcl>, IScQclRepository
    {
        public ScQclRepository(IDbFactory dbFactory) : base(dbFactory) { }

    }
}
