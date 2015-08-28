using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.WebConfiguration;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScFormRepository : IRepository<ScForm>
    {
    }

    public class ScFormRepository : RepositoryBase<ScForm>, IScFormRepository
    {
        public ScFormRepository(IDbFactory dbFactory) : base(dbFactory) { }

    }
}
