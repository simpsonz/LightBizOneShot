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
    public interface IScCompMappingRepository : IRepository<ScCompMapping>
    {
        Task<IPagedList<ScCompMapping>> GetPagedListCompMappingsAsync(int page, int pageSize, int compSn,
            int bizWorkSn = 0, string status = null, string compNm = null);

        Task<IList<ScCompMapping>> GetCompMappingsAsync(Expression<Func<ScCompMapping, bool>> where);
        Task<ScCompMapping> GetCompMappingAsync(Expression<Func<ScCompMapping, bool>> where);
        Task<IList<ScCompInfo>> GetCompanysAsync(Expression<Func<ScCompMapping, bool>> where);
        Task<IPagedList<ScCompInfo>> GetPagedListCompanysAsync(Expression<Func<ScCompMapping, bool>> where, int page, int pageSize);
        Task<IPagedList<ScCompMapping>> GetPagedListCompMappingsAsync(int page, int pageSize, int compSn, string excutorId = null, int bizWorkSn = 0);
        Task<IPagedList<ScCompMapping>> GetPagedListCompMappingsForBasicReportAsync(int page, int pageSize, int compSn, string excutorId = null, int bizWorkSn = 0);

        Task<IList<ScCompMapping>> GetExpertCompanysAsync(string loginId, string comName = null);
        Task<IList<ScCompMapping>> GetExpertCompanysAsync(Expression<Func<ScCompMapping, bool>> where);
        Task<IList<ScCompMapping>> GetExpertCompanysForPopupAsync(string expertId, string query);
    }


    public class ScCompMappingRepository : RepositoryBase<ScCompMapping>, IScCompMappingRepository
    {
        public ScCompMappingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
           
        public async Task<IPagedList<ScCompMapping>> GetPagedListCompMappingsAsync(int page, int pageSize, int compSn,
            int bizWorkSn = 0, string status = null, string compNm = null)
        {
            return await DbContext.ScCompMappings
                .Include("ScCompInfo")
                .Include("ScBizWork")
                .Include("ScUsr")
                .Include("ScBizWork.ScCompInfo")
                .Include("ScBizWork.ScUsr")
                .Where(scm => scm.ScBizWork.ScCompInfo.CompSn == compSn && scm.Status != "D")
                .Where(scm => bizWorkSn == 0 ? scm.BizWorkSn > bizWorkSn : scm.BizWorkSn == bizWorkSn)
                .Where(scm => string.IsNullOrEmpty(status) ? scm.Status != "D" : scm.Status == status)
                .Where(
                    scm =>
                        string.IsNullOrEmpty(compNm)
                            ? scm.ScCompInfo.CompNm != null
                            : scm.ScCompInfo.CompNm.Contains(compNm))
                .OrderByDescending(scm => scm.RegDt).ToPagedListAsync(page, pageSize);
        }

        public async Task<IPagedList<ScCompMapping>> GetPagedListCompMappingsAsync(int page, int pageSize, int compSn, string excutorId = null, int bizWorkSn = 0)
        {

            return await DbContext.ScCompMappings
                .Include("ScCompInfo")
                .Include("ScBizWork")
                .Where(scm => scm.Status != "D")
                .Where(scm => string.IsNullOrEmpty(excutorId) ? scm.ScBizWork.ExecutorId != null : scm.ScBizWork.ExecutorId == excutorId)
                .Where(scm => compSn == 0 ? scm.ScBizWork.MngCompSn > compSn : scm.ScBizWork.MngCompSn == compSn)
                .Where(scm => bizWorkSn == 0 ? scm.BizWorkSn > bizWorkSn : scm.BizWorkSn == bizWorkSn)
                .OrderByDescending(scm => scm.RegDt).ToPagedListAsync(page, pageSize);

        }

        public async Task<IPagedList<ScCompMapping>> GetPagedListCompMappingsForBasicReportAsync(int page, int pageSize, int compSn, string excutorId = null, int bizWorkSn = 0)
        {

            return await DbContext.ScCompMappings
                .Include("ScCompInfo")
                .Include("ScBizWork")
                .Where(scm => scm.Status != "D")
                .Where(scm => compSn == 0 ? scm.ScBizWork.MngCompSn > compSn : scm.ScBizWork.MngCompSn == compSn)
                .Where(scm => string.IsNullOrEmpty(excutorId) ? scm.ScBizWork.ExecutorId != null : scm.ScBizWork.ExecutorId == excutorId)
                .Where(scm => bizWorkSn == 0 ? scm.BizWorkSn > bizWorkSn : scm.BizWorkSn == bizWorkSn)
                .Where(scm => scm.ScCompInfo.RptMasters.Where(rm => rm.Status == "C" && scm.BizWorkSn == rm.BizWorkSn).Count() > 0)
                .OrderByDescending(scm => scm.RegDt).ToPagedListAsync(page, pageSize);

        }

        public async Task<IList<ScCompMapping>> GetCompMappingsAsync(Expression<Func<ScCompMapping, bool>> where)
        {
            return
                await
                    DbContext.ScCompMappings.Include("ScCompInfo")
                        .Include("ScBizWork")
                        .Include("ScUsr")
                        .Include("ScBizWork.ScCompInfo")
                        .Include("ScBizWork.ScUsr")
                        .Where(where)
                        .ToListAsync();
        }

        public async Task<ScCompMapping> GetCompMappingAsync(Expression<Func<ScCompMapping, bool>> where)
        {
            return
                await
                    DbContext.ScCompMappings.Include("ScCompInfo")
                        .Include("ScBizWork")
                        .Include("ScUsr")
                        .Include("ScBizWork.ScCompInfo")
                        .Include("ScBizWork.ScUsr")
                        .Where(where)
                        .SingleOrDefaultAsync();
        }

        public async Task<IList<ScCompInfo>> GetCompanysAsync(Expression<Func<ScCompMapping, bool>> where)
        {
            return
                await
                    DbContext.ScCompMappings.Include("ScCompMappings")
                        .Include("ScUsr")
                        .Where(where)
                        .Select(bw => bw.ScCompInfo)
                        .Include("ScUsrs")
                        .ToListAsync();
        }

        public async Task<IPagedList<ScCompInfo>> GetPagedListCompanysAsync(Expression<Func<ScCompMapping, bool>> where,
            int page, int pageSize)
        {
            return await DbContext.ScCompMappings
                .Include("ScCompMappings")
                .Include("ScUsr")
                .Where(where)
                .Select(bw => bw.ScCompInfo)
                .Include("ScUsrs")
                .OrderByDescending(sc => sc.CompNm)
                .ToPagedListAsync(page, pageSize);
        }

        public async Task<IList<ScCompMapping>> GetExpertCompanysAsync(Expression<Func<ScCompMapping, bool>> where)
        {
            return
                await
                    DbContext.ScCompMappings.Include("ScCompInfo")
                        .Include("ScCompInfo.ScUsrs")
                        .Where(where)
                        .ToListAsync();
        }

        public async Task<IList<ScCompMapping>> GetExpertCompanysAsync(string expertId, string comName = null)
        {
            if (string.IsNullOrEmpty(comName))
            { 
                var joinList = from a in DbContext.ScCompMappings
                    join c in DbContext.ScExpertMappings on a.BizWorkSn equals c.BizWorkSn
                    where c.ExpertId == expertId && a.Status == "A"
                               select a;

                return await joinList.Include("ScCompInfo").Include("ScCompInfo.ScUsrs").ToListAsync();
            }
            else
            {
                var joinList = from a in DbContext.ScCompMappings
                    join c in DbContext.ScExpertMappings on a.BizWorkSn equals c.BizWorkSn
                    where c.ExpertId == expertId && a.Status == "A" && a.ScCompInfo.CompNm.Contains(comName)
                               select a;

                return await joinList.Include("ScCompInfo").Include("ScCompInfo.ScUsrs").ToListAsync();
            }
        }

        public async Task<IList<ScCompMapping>> GetExpertCompanysForPopupAsync(string expertId, string query)
        {
            var joinList = from a in DbContext.ScCompMappings
                join c in DbContext.ScExpertMappings on a.BizWorkSn equals c.BizWorkSn
                where
                    c.ExpertId == expertId && a.Status == "A" &&
                    (a.ScCompInfo.CompNm.Contains(query) || a.ScCompInfo.RegistrationNo.Contains(query))
                           select a;

            return await joinList.Include("ScCompInfo").Include("ScCompInfo.ScUsrs").ToListAsync();
        }
    }
}
