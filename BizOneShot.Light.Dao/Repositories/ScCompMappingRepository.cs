using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface IScCompMappingRepository : IRepository<ScCompMapping>
    {
        //IList<ScCompInfo> GetScCompInfoByName(string compNm);
    }


    public class ScCompMappingRepository : RepositoryBase<ScCompMapping>, IScCompMappingRepository
    {
        public ScCompMappingRepository(IDbFactory dbFactory) : base(dbFactory) { }

    }
}
