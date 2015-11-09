using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizOneShot.Light.Services
{
    public interface IQuesResult1Service : IBaseService
    {
        Task<IList<QuesResult1>> GetQuesResult1sAsync(int questionSn, string code);
        Task<QuesResult1> GetQuesResult1Async(int questionSn, string detailCode);
    }


    public class QuesResult1Service : IQuesResult1Service
    {
        private readonly IQuesResult1Repository _quesResult1Repository;
        private readonly IUnitOfWork unitOfWork;

        public QuesResult1Service(IQuesResult1Repository _quesResult1Repository, IUnitOfWork unitOfWork)
        {
            this._quesResult1Repository = _quesResult1Repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<QuesResult1>> GetQuesResult1sAsync(int questionSn, string code)
        {
            var listQuesMaster =
                await
                    _quesResult1Repository.GetQuesResult1sAsync(
                        qr =>
                            qr.QuestionSn == questionSn && qr.QuesCheckList.CurrentUseYn == "Y" &&
                            qr.QuesCheckList.SmallClassCd == code);
            return listQuesMaster.OrderBy(qr => qr.QuesCheckList.SmallClassCd).ToList();
        }


        public async Task<QuesResult1> GetQuesResult1Async(int questionSn, string detailCode)
        {
            var quesMaster =
                await
                    _quesResult1Repository.GetQuesResult1Async(
                        qr =>
                            qr.QuestionSn == questionSn && qr.QuesCheckList.CurrentUseYn == "Y" &&
                            qr.QuesCheckList.DetailCd == detailCode);
            return quesMaster;
        }

        #region SaveContext

        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await unitOfWork.CommitAsync();
        }

        #endregion
    }
}