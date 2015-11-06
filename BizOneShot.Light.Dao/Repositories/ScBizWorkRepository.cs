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
    public interface IScBizWorkRepository : IRepository<ScBizWork>
    {
        ScBizWork Insert(ScBizWork scBizWork);
        Task<IList<ScBizWork>> GetBizWorksAsync(Expression<Func<ScBizWork, bool>> where);

        Task<IPagedList<ScBizWork>> GetPagedListBizWorksAsync(int page, int pageSize, int mngComSn,
            string excutorId = null, int bizWorkYear = 0);

        Task<ScBizWork> GetBizWorkAsync(Expression<Func<ScBizWork, bool>> where);
        Task<ScBizWork> GetBizWorkByLoginIdAsync(Expression<Func<ScBizWork, bool>> where);
    }


    public class ScBizWorkRepository : RepositoryBase<ScBizWork>, IScBizWorkRepository
    {
        public ScBizWorkRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public ScBizWork Insert(ScBizWork scBizWork)
        {
            return DbContext.ScBizWorks.Add(scBizWork);
        }

        public async Task<IList<ScBizWork>> GetBizWorksAsync(Expression<Func<ScBizWork, bool>> where)
        {
            return await DbContext.ScBizWorks.Include("ScCompMappings").Include("ScUsr").Where(where).ToListAsync();
        }

        public async Task<IPagedList<ScBizWork>> GetPagedListBizWorksAsync(int page, int pageSize, int mngComSn,
            string excutorId = null, int bizWorkYear = 0)
        {
            if (string.IsNullOrEmpty(excutorId))
            {
                return
                    await
                        DbContext.ScBizWorks.Include("ScCompMappings")
                            .Include("ScUsr")
                            .Where(bw => bw.MngCompSn == mngComSn && bw.Status == "N")
                            .Where(
                                bw =>
                                    bizWorkYear == 0
                                        ? bw.BizWorkStDt.Value.Year > 0
                                        : bw.BizWorkStDt.Value.Year <= bizWorkYear &&
                                          bw.BizWorkEdDt.Value.Year >= bizWorkYear)
                            .OrderByDescending(bw => bw.BizWorkSn).ToPagedListAsync(page, pageSize);
            }
            return
                await
                    DbContext.ScBizWorks.Include("ScCompMappings")
                        .Include("ScUsr")
                        .Where(bw => bw.MngCompSn == mngComSn && bw.Status == "N" && bw.ExecutorId == excutorId)
                        .Where(
                            bw =>
                                bizWorkYear == 0
                                    ? bw.BizWorkStDt.Value.Year > 0
                                    : bw.BizWorkStDt.Value.Year <= bizWorkYear &&
                                      bw.BizWorkEdDt.Value.Year >= bizWorkYear)
                        .OrderByDescending(bw => bw.BizWorkSn).ToPagedListAsync(page, pageSize);
        }

        public async Task<ScBizWork> GetBizWorkAsync(Expression<Func<ScBizWork, bool>> where)
        {
            return
                await
                    DbContext.ScBizWorks.Include("ScCompMappings").Include("ScUsr").Where(where).SingleOrDefaultAsync();
        }

        public async Task<ScBizWork> GetBizWorkByLoginIdAsync(Expression<Func<ScBizWork, bool>> where)
        {
            return await DbContext.ScBizWorks.Include("ScUsr").Where(where).SingleOrDefaultAsync();
        }
    }
}