using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizOneShot.Light.Services
{
    public interface IRptFinanceCommentService : IBaseService
    {
        RptFinanceComment Insert(RptFinanceComment rptFinanceComment);
        Task<int> AddRptFinanceCommentAsync(RptFinanceComment rptFinanceComment);

        Task<IList<RptFinanceComment>> GetRptFinanceCommentsAsync(int compSn, int basicYear, int basicMonth);
        Task<RptFinanceComment> GetRptFinanceCommentAsync(int bizWorkSn, int compSn, int basicYear, int basicMonth);
        void ModifyRptFinanceComment(RptFinanceComment rptFinanceComment);
    }


    public class RptFinanceCommentService : IRptFinanceCommentService
    {
        private readonly IRptFinanceCommentRepository rptFinanceCommentRepository;
        private readonly IUnitOfWork unitOfWork;

        public RptFinanceCommentService(IRptFinanceCommentRepository rptFinanceCommentRepository, IUnitOfWork unitOfWork)
        {
            this.rptFinanceCommentRepository = rptFinanceCommentRepository;
            this.unitOfWork = unitOfWork;
        }

        public RptFinanceComment Insert(RptFinanceComment rptFinanceComment)
        {
            return rptFinanceCommentRepository.Insert(rptFinanceComment);
        }


        public async Task<IList<RptFinanceComment>> GetRptFinanceCommentsAsync(int compSn, int basicYear, int basicMonth)
        {
            var rptFinanceComments =
                await
                    rptFinanceCommentRepository.GetRptFinanceCommentsAsync(
                        rm => rm.CompSn == compSn && rm.BasicYear == basicYear && rm.BasicMonth == basicMonth);

            return rptFinanceComments.OrderByDescending(bw => bw.WriteDt).ToList();
        }

        public async Task<RptFinanceComment> GetRptFinanceCommentAsync(int bizWorkSn, int compSn, int basicYear,
            int basicMonth)
        {
            var rptFinanceComment =
                await
                    rptFinanceCommentRepository.GetRptFinanceCommentAsync(
                        rm =>
                            rm.BizWorkSn == bizWorkSn && rm.CompSn == compSn && rm.BasicYear == basicYear &&
                            rm.BasicMonth == basicMonth);
            return rptFinanceComment;
        }

        public async Task<int> AddRptFinanceCommentAsync(RptFinanceComment rptFinanceComment)
        {
            var result = rptFinanceCommentRepository.Insert(rptFinanceComment);

            if (result == null)
            {
                return -1;
            }
            return await SaveDbContextAsync();
        }


        public void ModifyRptFinanceComment(RptFinanceComment rptFinanceComment)
        {
            rptFinanceCommentRepository.Update(rptFinanceComment);
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