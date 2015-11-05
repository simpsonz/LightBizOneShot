using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using PagedList;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IExpertMappingRepository : IRepository<ScExpertMapping>
    {
        Task<IList<ScExpertMapping>> GetExperetMappingsAsync(Expression<Func<ScExpertMapping, bool>> where);
        Task<ScExpertMapping> GetExpertMappingAsync(Expression<Func<ScExpertMapping, bool>> where);

        IPagedList<ScCompMapping> GetExpertMappingsAsync(int page, int pageSize, string expertId, int bizWorkSn,
            int mngCompSn, string status);
    }


    public class ScExpertMappingRepository : RepositoryBase<ScExpertMapping>, IExpertMappingRepository
    {
        public ScExpertMappingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<ScExpertMapping>> GetExperetMappingsAsync(Expression<Func<ScExpertMapping, bool>> where)
        {
            return
                await
                    DbContext.ScExpertMappings.Include("ScBizWork")
                        .Include("ScUsr")
                        .Include("ScUsr.ScUsrResume.ScFileInfo")
                        .Where(where)
                        .ToListAsync();
        }

        public async Task<ScExpertMapping> GetExpertMappingAsync(Expression<Func<ScExpertMapping, bool>> where)
        {
            return
                await
                    DbContext.ScExpertMappings.Include("ScBizWork")
                        .Include("ScBizWork.ScCompInfo")
                        .Include("ScUsr")
                        .Include("ScUsr.ScCompInfo")
                        .Include("ScUsr.ScUsrResume.ScFileInfo")
                        .Where(where)
                        .SingleOrDefaultAsync();
        }


        public IPagedList<ScCompMapping> GetExpertMappingsAsync(int page, int pageSize, string expertId, int bizWorkSn,
            int mngCompSn, string status)
        {
            var listScCompMappings = DbContext.ScExpertMappings
                .Where(em => em.ExpertId == expertId)
                .Select(sm => sm.ScBizWork)
                .Select(tt => tt.ScCompMappings).ToList();

            IList<ScCompMapping> scCompMappings = new List<ScCompMapping>();
            foreach (var listScCompMapping in listScCompMappings)
            {
                foreach (var scCompMapping in listScCompMapping)
                {
                    scCompMappings.Add(scCompMapping);
                }
            }

            var rstScCompMappings = scCompMappings.Where(em => em.Status == status)
                .Where(rm => mngCompSn == 0 ? rm.ScBizWork.MngCompSn > 0 : rm.ScBizWork.MngCompSn == mngCompSn)
                .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                .OrderByDescending(rm => rm.RegDt)
                .ToPagedList(page, pageSize);

            return rstScCompMappings;
        }
    }
}