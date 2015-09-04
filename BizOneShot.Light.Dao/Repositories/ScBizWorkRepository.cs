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
        ScBizWork Insert(ScBizWork scBizWork);
    }


    public class ScBizWorkRepository : RepositoryBase<ScBizWork>, IScBizWorkRepository
    {
        public ScBizWorkRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public ScBizWork Insert(ScBizWork scBizWork)
        {
            return this.DbContext.ScBizWorks.Add(scBizWork);
        }

    }
}
