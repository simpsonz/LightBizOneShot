using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IRptMngCommentService : IBaseService
    {
        RptMngComment Insert(RptMngComment rptMngComment);
        Task<int> AddRptMngCommentAsync(RptMngComment rptMngComment);

        Task<IList<RptMngComment>> GetRptMngCommentListAsync(int basicYear, string pageCode);
        Task<RptMngComment> GetRptMngCommentAsync(int basicYear, string detailCode);
    }


    public class RptMngCommentService : IRptMngCommentService
    {
        private readonly IRptMngCommentRepository rptMngCommentRepository;
        private readonly IUnitOfWork unitOfWork;

        public RptMngCommentService(IUnitOfWork unitOfWork, IRptMngCommentRepository rptMngCommentRepository)
        {
            this.unitOfWork = unitOfWork;
            this.rptMngCommentRepository = rptMngCommentRepository;
        }

        public async Task<IList<RptMngComment>> GetRptMngCommentListAsync(int basicYear, string pageCode)
        {
            var rptMngComments =
                await
                    rptMngCommentRepository.GetRptMngCommentsAsync(
                        rm => rm.BasicYear == basicYear && rm.RptMngCode.SmallClassCd == pageCode);

            return rptMngComments.OrderBy(rm => rm.DetailCd).ToList();
        }

        public async Task<RptMngComment> GetRptMngCommentAsync(int basicYear, string detailCode)
        {
            var rptMngComment =
                await
                    rptMngCommentRepository.GetRptMngCommentAsync(
                        rm => rm.BasicYear == basicYear && rm.DetailCd == detailCode);
            return rptMngComment;
        }

        public RptMngComment Insert(RptMngComment rptMngComment)
        {
            return rptMngCommentRepository.Insert(rptMngComment);
        }


        public async Task<int> AddRptMngCommentAsync(RptMngComment rptMngComment)
        {
            var result = rptMngCommentRepository.Insert(rptMngComment);

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