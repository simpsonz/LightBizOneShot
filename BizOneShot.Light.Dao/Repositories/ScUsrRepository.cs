using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScUsrRepository : IRepository<ScUsr>
    {
        IList<ScUsr> GetScUsrById(string loginId);
        ScUsr Insert(ScUsr scUsr);
    }


    public class ScUsrRepository : RepositoryBase<ScUsr>, IScUsrRepository
    {
        public ScUsrRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public IList<ScUsr> GetScUsrById(string loginId)
        {
            var usrInfo = this.DbContext.ScUsrs.Where(ci => ci.LoginId == loginId).ToList();
            return usrInfo;
        }

        public ScUsr Insert(ScUsr scUsr)
        {
            return this.DbContext.ScUsrs.Add(scUsr);
        }
    }
}
