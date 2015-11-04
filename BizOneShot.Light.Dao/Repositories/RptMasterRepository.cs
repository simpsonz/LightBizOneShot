using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using PagedList;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface IRptMasterRepository : IRepository<RptMaster>
    {
        RptMaster Insert(RptMaster rptMaster);
        Task<IList<RptMaster>> GetRptMastersAsync(Expression<Func<RptMaster, bool>> where);
        IPagedList<RptMaster> GetRptMasters(int page, int pageSize, string mentorID, int basicYear, int bizWorkSn, int compSn, string status);
        IPagedList<RptMaster> GetRptMastersForBizManager(int page, int pageSize, string executorId, int basicYear, int bizWorkSn, int compSn, string status);
        IPagedList<RptMaster> GetRptMastersForSysManager(int page, int pageSize, int bizWorkSn, int mngCompSn, string status);
        Task<RptMaster> GetRptMasterAsync(Expression<Func<RptMaster, bool>> where);
    }


    public class RptMasterRepository : RepositoryBase<RptMaster>, IRptMasterRepository
    {
        public RptMasterRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public RptMaster Insert(RptMaster rptMaster)
        {
            return this.DbContext.RptMasters.Add(rptMaster);
        }

        public async Task<IList<RptMaster>> GetRptMastersAsync(Expression<Func<RptMaster, bool>> where)
        {
            return await this.DbContext.RptMasters.Include("ScBizWork").Include("ScCompInfo").Where(where).ToListAsync();
        }

        public async Task<RptMaster> GetRptMasterAsync(Expression<Func<RptMaster, bool>> where)
        {
            return await this.DbContext.RptMasters.Include("ScBizWork").Include("ScCompInfo").Where(where).SingleOrDefaultAsync();
        }

        public IPagedList<RptMaster> GetRptMasters(int page, int pageSize, string mentorID, int basicYear, int bizWorkSn, int compSn, string status)
        {
            return DbContext.RptMasters
                    .Include(rm => rm.ScBizWork)
                    .Include(rm => rm.ScCompInfo)
                    .Where(rm => rm.MentorId == mentorID && rm.BasicYear == basicYear && rm.Status.Contains(status))
                    .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                    .Where(rm => compSn == 0 ? rm.CompSn > 0 : rm.CompSn == compSn)
                    .OrderByDescending(rm => rm.RegDt)
                    .ToPagedList(page, pageSize);
        }

        public IPagedList<RptMaster> GetRptMastersForBizManager(int page, int pageSize, string executorId, int basicYear, int bizWorkSn, int compSn, string status)
        {
            if(executorId == null)
            {
                return DbContext.RptMasters
                    .Include(rm => rm.ScBizWork)
                    .Include(rm => rm.ScCompInfo)
                    .Where(rm => rm.ScBizWork.MngCompSn == compSn && rm.Status == status)
                    .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                    .Where(rm => basicYear == 0 ? rm.BasicYear > 0 : rm.BasicYear == basicYear)
                    .OrderByDescending(rm => rm.RegDt)
                    .ToPagedList(page, pageSize);
            }
            else
            {
                return DbContext.RptMasters
                    .Include(rm => rm.ScBizWork)
                    .Include(rm => rm.ScCompInfo)
                    .Where(rm => rm.ScBizWork.MngCompSn == compSn && rm.ScBizWork.ExecutorId == executorId && rm.Status == status)
                    .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                    .Where(rm => basicYear == 0 ? rm.BasicYear > 0 : rm.BasicYear == basicYear)
                    .OrderByDescending(rm => rm.RegDt)
                    .ToPagedList(page, pageSize);
            }
            
        }


        public IPagedList<RptMaster> GetRptMastersForSysManager(int page, int pageSize, int bizWorkSn, int mngCompSn, string status)
        {
            return DbContext.RptMasters
                    .Include(rm => rm.ScBizWork)
                    .Include(rm => rm.ScCompInfo)
                    .Where(rm => rm.Status == status)
                    .Where(rm => mngCompSn == 0 ? rm.ScBizWork.MngCompSn > 0 : rm.ScBizWork.MngCompSn == mngCompSn)
                    .Where(rm => bizWorkSn == 0 ? rm.BizWorkSn > 0 : rm.BizWorkSn == bizWorkSn)
                    .OrderByDescending(rm => rm.RegDt)
                    .ToPagedList(page, pageSize);

        }
    }
}
