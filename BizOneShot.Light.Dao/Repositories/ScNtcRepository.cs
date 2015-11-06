using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using PagedList;
using PagedList.EntityFramework;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScNtcRepository : IRepository<ScNtc>
    {
        Task<ScNtc> Insert(ScNtc scNtc);
        Task<IPagedList<ScNtc>> GetPagedListAsync(Expression<Func<ScNtc, bool>> where, int page, int pageSize);
    }

    public class ScNtcRepository : RepositoryBase<ScNtc>, IScNtcRepository
    {
        public ScNtcRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<ScNtc> Insert(ScNtc scNtc)
        {
            return await Task.Run(() => DbContext.ScNtcs.Add(scNtc));
        }

        public async Task<IPagedList<ScNtc>> GetPagedListAsync(Expression<Func<ScNtc, bool>> where, int page,
            int pageSize)
        {
            return
                await
                    DbContext.ScNtcs.Where(where)
                        .OrderByDescending(ntc => ntc.NoticeSn)
                        .ToPagedListAsync(page, pageSize);
        }
    }
}