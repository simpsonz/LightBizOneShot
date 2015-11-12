using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface ICtWebLogRepository : IRepository<CtWebLog>
    {
        CtWebLog Insert(CtWebLog ctWebLog);
    }


    public class CtWebLogRepository : RepositoryBase<CtWebLog>, ICtWebLogRepository
    {
        public CtWebLogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public CtWebLog Insert(CtWebLog ctWebLog)
        {
            return DbContext.CtWebLogs.Add(ctWebLog);
        }
    }
}
