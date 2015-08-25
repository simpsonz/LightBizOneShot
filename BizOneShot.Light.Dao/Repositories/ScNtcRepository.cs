using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Models;
using BizOneShot.Light.Dao.Infrastructure;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScNtcRepository : IRepository<ScNtc>
    {
    }

    public class ScNtcRepository : RepositoryBase<ScNtc>, IScNtcRepository
    {
        public ScNtcRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
