using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IRptMentorCommentService : IBaseService
    {
        RptMentorComment Insert(RptMentorComment rptMentorComment);
        Task<int> AddRptMentorCommentAsync(RptMentorComment rptMentorComment);

        Task<IList<RptMentorComment>> GetRptMentorCommentListAsync(int qustionSn, int bizWorkSn, int basicYear,
            string pageCode);

        Task<RptMentorComment> GetRptMentorCommentAsync(int qustionSn, int bizWorkSn, int basicYear, string detailCode);
    }


    public class RptMentorCommentService : IRptMentorCommentService
    {
        private readonly IRptMentorCommentRepository rptMentorCommentRepository;
        private readonly IUnitOfWork unitOfWork;

        public RptMentorCommentService(IRptMentorCommentRepository rptMentorCommentRepository, IUnitOfWork unitOfWork)
        {
            this.rptMentorCommentRepository = rptMentorCommentRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<RptMentorComment>> GetRptMentorCommentListAsync(int qustionSn, int bizWorkSn,
            int basicYear, string pageCode)
        {
            var rptMentorComments =
                await
                    rptMentorCommentRepository.GetRptMentorCommentsAsync(
                        rm =>
                            rm.QuestionSn == qustionSn && rm.BizWorkSn == bizWorkSn && rm.BasicYear == basicYear &&
                            rm.RptCheckList.SmallClassCd == pageCode);

            return rptMentorComments.OrderBy(rm => rm.DetailCd).ToList();
        }

        public async Task<RptMentorComment> GetRptMentorCommentAsync(int qustionSn, int bizWorkSn, int basicYear,
            string detailCode)
        {
            var rptMentorComment =
                await
                    rptMentorCommentRepository.GetRptMentorCommentAsync(
                        rm =>
                            rm.QuestionSn == qustionSn && rm.BizWorkSn == bizWorkSn && rm.BasicYear == basicYear &&
                            rm.DetailCd == detailCode);
            return rptMentorComment;
        }

        public RptMentorComment Insert(RptMentorComment rptMentorComment)
        {
            return rptMentorCommentRepository.Insert(rptMentorComment);
        }


        public async Task<int> AddRptMentorCommentAsync(RptMentorComment rptMentorComment)
        {
            var result = rptMentorCommentRepository.Insert(rptMentorComment);

            if (result == null)
            {
                return -1;
            }
            return await SaveDbContextAsync();
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