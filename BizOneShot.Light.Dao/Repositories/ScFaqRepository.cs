using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;





namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScFaqRepository : IRepository<ScFaq>
    {
    }


    public class ScFaqRepository : RepositoryBase<ScFaq>, IScFaqRepository
    {
        public ScFaqRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            //this.DbContext.Configuration.LazyLoadingEnabled = false;
        }

        public override async Task<IEnumerable<ScFaq>> GetManyAsync(Expression<Func<ScFaq, bool>> where)
        {
            return await this.DbContext.ScFaqs.Include("ScQcl").Where(where).ToListAsync();

        }

    }
}
