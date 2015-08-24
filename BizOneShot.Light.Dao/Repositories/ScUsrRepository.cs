using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BizOneShot.Light.Models;
using BizOneShot.Light.Dao.Infrastructure;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScUsrRepository : IRepository<ScUsr>
    {
        //IList<ScCompInfo> GetScCompInfoByName(string compNm);
    }


    public class ScUsrRepository : RepositoryBase<ScUsr>, IScUsrRepository
    {
        public ScUsrRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
