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
    public interface IRptMasterRepository : IRepository<RptMaster>
    {
        RptMaster Insert(RptMaster rptMaster);
        Task<IList<RptMaster>> GetRptMastersAsync(Expression<Func<RptMaster, bool>> where);

        Task<IPagedList<RptMaster>> GetRptMasters(int page, int pageSize, string mentorID, int basicYear, int bizWorkSn,
            int compSn, string status);

        Task<IPagedList<RptMaster>> GetRptMastersForBizManager(int page, int pageSize, string executorId, int basicYear,
            int bizWorkSn, int compSn, string status);

        Task<IPagedList<RptMaster>> GetRptMastersForSysManager(int page, int pageSize, int bizWorkSn, int mngCompSn,
            string status);

        Task<IPagedList<RptMaster>> GetRptMastersForExpert(int page, int pageSize, string expertId, int bizWorkSn,
            int mngCompSn, string status);

        Task<RptMaster> GetRptMasterAsync(Expression<Func<RptMaster, bool>> where);
    }


    public class RptMasterRepository : RepositoryBase<RptMaster>, IRptMasterRepository
    {
        public RptMasterRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public RptMaster Insert(RptMaster rptMaster)
        {
            return DbContext.RptMasters.Add(rptMaster);
        }

        public async Task<IList<RptMaster>> GetRptMastersAsync(Expression<Func<RptMaster, bool>> where)
        {
            return await DbContext.RptMasters.Include("ScBizWork").Include("ScCompInfo").Where(where).ToListAsync();
        }

        public async Task<RptMaster> GetRptMasterAsync(Expression<Func<RptMaster, bool>> where)
        {
            return
                await
                    DbContext.RptMasters.Include("ScBizWork").Include("ScCompInfo").Where(where).SingleOrDefaultAsync();
        }

        public async Task<IPagedList<RptMaster>> GetRptMasters(int page, int pageSize, string mentorID, int basicYear,
            int bizWorkSn, int compSn, string status)
        {
            //return await DbContext.RptMasters
            //    .Include(rm => rm.ScBizWork)
            //    .Include(rm => rm.ScCompInfo)
            //    .Where(rm => rm.MentorId == mentorID && rm.BasicYear == basicYear && rm.Status.Contains(status) && rm.Status != "D")
            //    .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
            //    .Where(rm => compSn == 0 ? rm.CompSn > 0 : rm.CompSn == compSn)
            //    .OrderByDescending(rm => rm.RegDt)
            //    .AsNoTracking().ToPagedListAsync(page, pageSize);

            var rptMasters =  DbContext.RptMasters
                             .Where(rm => rm.MentorId == mentorID && rm.BasicYear == basicYear && rm.Status.Contains(status) && rm.Status != "D")
                             .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                             .Where(rm => compSn == 0 ? rm.CompSn > 0 : rm.CompSn == compSn);

            var joinList = from a in rptMasters
                           join c in DbContext.ScCompMappings on new { a.BizWorkSn, a.CompSn } equals new { c.BizWorkSn, c.CompSn }
                           where c.Status == "A"
                           select a;


            return await joinList.Include("ScBizWork").Include("ScCompInfo")
                   .OrderByDescending(rm => rm.RegDt)
                   .AsNoTracking().ToPagedListAsync(page, pageSize);
        }

        public async Task<IPagedList<RptMaster>> GetRptMastersForBizManager(int page, int pageSize, string executorId,
            int basicYear, int bizWorkSn, int compSn, string status)
        {
            if (executorId == null)
            {
                return await DbContext.RptMasters
                    .Include(rm => rm.ScBizWork)
                    .Include(rm => rm.ScCompInfo)
                    .Where(rm => rm.ScBizWork.MngCompSn == compSn && rm.Status == status)
                    .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                    .Where(rm => basicYear == 0 ? rm.BasicYear > 0 : rm.BasicYear == basicYear)
                    .OrderByDescending(rm => rm.RegDt)
                    .AsNoTracking().ToPagedListAsync(page, pageSize);
            }
            return await DbContext.RptMasters
                .Include(rm => rm.ScBizWork)
                .Include(rm => rm.ScCompInfo)
                .Where(
                    rm =>
                        rm.ScBizWork.MngCompSn == compSn && rm.ScBizWork.ExecutorId == executorId && rm.Status == status)
                .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                .Where(rm => basicYear == 0 ? rm.BasicYear > 0 : rm.BasicYear == basicYear)
                .OrderByDescending(rm => rm.RegDt)
                .AsNoTracking().ToPagedListAsync(page, pageSize);
        }


        public async Task<IPagedList<RptMaster>> GetRptMastersForSysManager(int page, int pageSize, int bizWorkSn,
            int mngCompSn, string status)
        {
            return await DbContext.RptMasters
                .Include(rm => rm.ScBizWork)
                .Include(rm => rm.ScCompInfo)
                .Where(rm => rm.Status == status)
                .Where(rm => mngCompSn == 0 ? rm.ScBizWork.MngCompSn > 0 : rm.ScBizWork.MngCompSn == mngCompSn)
                .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                .OrderByDescending(rm => rm.RegDt)
                .AsNoTracking().ToPagedListAsync(page, pageSize);
        }

        public async Task<IPagedList<RptMaster>> GetRptMastersForExpert(int page, int pageSize, string expertId,
            int bizWorkSn, int mngCompSn, string status)
        {
            var listRptMatsers = await DbContext.ScExpertMappings
                .Where(em => em.ExpertId == expertId)
                .Select(sm => sm.ScBizWork)
                .Select(tt => tt.RptMasters).AsNoTracking().ToListAsync();

            IList<RptMaster> rptMaterView = new List<RptMaster>();
            foreach (var rptMasters in listRptMatsers)
            {
                foreach (var rptMaster in rptMasters)
                {
                    rptMaterView.Add(rptMaster);
                }
            }

            var rptMaters = rptMaterView.Where(em => em.Status == status)
                .Where(rm => mngCompSn == 0 ? rm.ScBizWork.MngCompSn > 0 : rm.ScBizWork.MngCompSn == mngCompSn)
                .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                .OrderByDescending(rm => rm.RegDt)
                .ToPagedList(page, pageSize);

            return rptMaters;
        }
    }
}