using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScMentorMappingRepository : IRepository<ScMentorMappiing>
    {
        //IList<ScCompInfo> GetScCompInfoByName(string compNm);
    }


    public class ScMentorMappingRepository : RepositoryBase<ScMentorMappiing>, IScMentorMappingRepository
    {
        public ScMentorMappingRepository(IDbFactory dbFactory) : base(dbFactory) { }

    }
}
