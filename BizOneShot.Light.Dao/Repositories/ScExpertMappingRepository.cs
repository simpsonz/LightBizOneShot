using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface IExpertMappingRepository : IRepository<ScExpertMapping>
    {
        //IList<ScCompInfo> GetScCompInfoByName(string compNm);
    }


    public class ScExpertMappingRepository : RepositoryBase<ScExpertMapping>, IExpertMappingRepository
    {
        public ScExpertMappingRepository(IDbFactory dbFactory) : base(dbFactory) { }

    }
}
