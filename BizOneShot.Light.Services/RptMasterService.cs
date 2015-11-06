using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using PagedList;

namespace BizOneShot.Light.Services
{
    public interface IRptMasterService : IBaseService
    {
        RptMaster Insert(RptMaster rptMaster);
        Task<int> AddRptMasterAsync(RptMaster rptMaster);

        Task<IList<RptMaster>> GetRptMasterListAsync(string mentorId, int year, int bizWorkSn, int compSn, string status);
        Task<IList<RptMaster>> GetRptMasterListAsync(int compSn, int year);
        Task<RptMaster> GetRptMasterAsync(int qustionSn, int compSn, int year);
        Task<RptMaster> GetRptMasterAsyncForCompany(int bizWorkSn, int compSn, int year);

        Task<IPagedList<RptMaster>> GetRptMasterList(int page, int pageSize, string mentorId, int basicYear,
            int bizWorkSn, int compSn, string status);

        Task<IPagedList<RptMaster>> GetRptMasterListForBizManager(int page, int pageSize, string executorId,
            int basicYear, int bizWorkSn, int compSn, string status);

        Task<IPagedList<RptMaster>> GetRptMasterListForSysManager(int page, int pageSize, int bizWorkSn, int mngCompSn,
            string status);

        Task<IPagedList<RptMaster>> GetRptMasterListForExpert(int page, int pageSize, string expertId, int bizWorkSn,
            int mngCompSn, string status);

        void ModifyRptMaster(RptMaster rptMaster);
    }


    public class RptMasterService : IRptMasterService
    {
        private readonly IRptMasterRepository rptMasterRepository;
        private readonly IUnitOfWork unitOfWork;

        public RptMasterService(IRptMasterRepository rptMasterRepository, IUnitOfWork unitOfWork)
        {
            this.rptMasterRepository = rptMasterRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<RptMaster>> GetRptMasterListAsync(string mentorId, int year, int bizWorkSn, int compSn,
            string status)
        {
            var rptMasters =
                await
                    rptMasterRepository.GetRptMastersAsync(
                        rm => rm.MentorId == mentorId && rm.BasicYear == year && rm.Status.Contains(status));
            rptMasters.Where(
                bw =>
                    bizWorkSn == 0
                        ? bw.BizWorkSn > 0
                        : bw.BizWorkSn == bizWorkSn && compSn == 0 ? bw.CompSn > 0 : bw.CompSn == compSn);

            return rptMasters.OrderByDescending(bw => bw.RegDt).ToList();
        }

        public async Task<IList<RptMaster>> GetRptMasterListAsync(int compSn, int year)
        {
            var rptMasters =
                await
                    rptMasterRepository.GetRptMastersAsync(
                        rm => rm.CompSn == compSn && rm.BasicYear == year && rm.Status == "C");
            return rptMasters;
        }

        public async Task<RptMaster> GetRptMasterAsync(int qustionSn, int compSn, int year)
        {
            var rptMaster =
                await
                    rptMasterRepository.GetRptMasterAsync(
                        rm => rm.QuestionSn == qustionSn && rm.CompSn == compSn && rm.BasicYear == year);
            return rptMaster;
        }

        public async Task<RptMaster> GetRptMasterAsyncForCompany(int bizWorkSn, int compSn, int year)
        {
            var rptMaster =
                await
                    rptMasterRepository.GetRptMasterAsync(
                        rm => rm.BizWorkSn == bizWorkSn && rm.CompSn == compSn && rm.BasicYear == year);
            return rptMaster;
        }

        public async Task<IPagedList<RptMaster>> GetRptMasterList(int page, int pageSize, string mentorId, int basicYear,
            int bizWorkSn, int compSn, string status)
        {
            return
                await rptMasterRepository.GetRptMasters(page, pageSize, mentorId, basicYear, bizWorkSn, compSn, status);
        }

        public async Task<IPagedList<RptMaster>> GetRptMasterListForBizManager(int page, int pageSize, string executorId,
            int basicYear, int bizWorkSn, int compSn, string status)
        {
            return
                await
                    rptMasterRepository.GetRptMastersForBizManager(page, pageSize, executorId, basicYear, bizWorkSn,
                        compSn, status);
        }

        public async Task<IPagedList<RptMaster>> GetRptMasterListForSysManager(int page, int pageSize, int bizWorkSn,
            int mngCompSn, string status)
        {
            return await rptMasterRepository.GetRptMastersForSysManager(page, pageSize, bizWorkSn, mngCompSn, status);
        }

        public async Task<IPagedList<RptMaster>> GetRptMasterListForExpert(int page, int pageSize, string expertId,
            int bizWorkSn, int mngCompSn, string status)
        {
            return
                await rptMasterRepository.GetRptMastersForExpert(page, pageSize, expertId, bizWorkSn, mngCompSn, status);
        }

        public RptMaster Insert(RptMaster rptMaster)
        {
            return rptMasterRepository.Insert(rptMaster);
        }


        public async Task<int> AddRptMasterAsync(RptMaster rptMaster)
        {
            var rstScUsr = rptMasterRepository.Insert(rptMaster);

            if (rstScUsr == null)
            {
                return -1;
            }
            return await SaveDbContextAsync();
        }


        public void ModifyRptMaster(RptMaster rptMaster)
        {
            rptMasterRepository.Update(rptMaster);
        }


        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await unitOfWork.CommitAsync();
        }
    }
}