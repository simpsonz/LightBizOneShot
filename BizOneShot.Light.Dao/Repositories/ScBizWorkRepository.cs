using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScBizWorkRepository : IRepository<ScBizWork>
    {
        //IList<ScCompInfo> GetScCompInfoByName(string compNm);
    }


    public class ScBizWorkRepository : RepositoryBase<ScBizWork>, IScBizWorkRepository
    {
        public ScBizWorkRepository(IDbFactory dbFactory) : base(dbFactory) { }

    }
}
