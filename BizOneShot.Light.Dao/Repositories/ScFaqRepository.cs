using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using PagedList;
using PagedList.EntityFramework;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScFaqRepository : IRepository<ScFaq>
    {
        Task<ScFaq> Insert(ScFaq scFaq);
        Task<IPagedList<ScFaq>> GetPagedListAsync(Expression<Func<ScFaq, bool>> where, int page, int pageSize);
    }


    public class ScFaqRepository : RepositoryBase<ScFaq>, IScFaqRepository
    {
        public ScFaqRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override async Task<IEnumerable<ScFaq>> GetManyAsync(Expression<Func<ScFaq, bool>> where)
        {
            return await DbContext.ScFaqs.Include("ScQcl").Where(where).ToListAsync();
        }


        public async Task<IPagedList<ScFaq>> GetPagedListAsync(Expression<Func<ScFaq, bool>> where, int page,
            int pageSize)
        {
            return
                await
                    DbContext.ScFaqs.Include("ScQcl")
                        .Where(where)
                        .OrderByDescending(sf => sf.FaqSn)
                        .ToPagedListAsync(page, pageSize);
            ;
        }


        public async Task<ScFaq> Insert(ScFaq scFaq)
        {
            return await Task.Run(() => DbContext.ScFaqs.Add(scFaq));
        }
    }
}