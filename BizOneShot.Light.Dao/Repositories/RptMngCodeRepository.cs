using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IRptMngCodeRepository : IRepository<RptMngCode>
    {
    }


    public class RptMngCodeRepository : RepositoryBase<RptMngCode>, IRptMngCodeRepository
    {
        public RptMngCodeRepository(IDbFactory dbFactory) : base(dbFactory) { }
 
    }


}
