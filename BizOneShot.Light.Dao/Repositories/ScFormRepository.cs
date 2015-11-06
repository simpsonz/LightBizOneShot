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
    public interface IScFormRepository : IRepository<ScForm>
    {
        Task<ScForm> Insert(ScForm scForm);

        Task<IPagedList<ScForm>> GetPagedListAsync(Expression<Func<ScForm, bool>> where, int page, int pageSize);
    }

    public class ScFormRepository : RepositoryBase<ScForm>, IScFormRepository
    {
        public ScFormRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<ScForm> Insert(ScForm scForm)
        {
            return await Task.Run(() => DbContext.ScForms.Add(scForm));
        }

        public async Task<IPagedList<ScForm>> GetPagedListAsync(Expression<Func<ScForm, bool>> where, int page,
            int pageSize)
        {
            return
                await
                    DbContext.ScForms.Where(where)
                        .OrderByDescending(manual => manual.FormSn)
                        .ToPagedListAsync(page, pageSize);
        }
    }
}