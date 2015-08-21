using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BizOneShot.Light.Models;
using BizOneShot.Light.Dao.Infrastructure;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScFaqRepository : IRepository<ScFaq>
    {
        //IList<ScCompInfo> GetScCompInfoByName(string compNm);
    }


    public class ScFaqRepository : RepositoryBase<ScFaq>, IScFaqRepository
    {
        public ScFaqRepository(IDbFactory dbFactory) : base(dbFactory) { }

    }
}
