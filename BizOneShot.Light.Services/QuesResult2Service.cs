using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizOneShot.Light.Services
{
    public interface IQuesResult2Service : IBaseService
    {
        Task<IList<QuesResult2>> GetQuesResult2sAsync(int questionSn, string code);
    }


    public class QuesResult2Service : IQuesResult2Service
    {
        private readonly IQuesResult2Repository _quesResult2Repository;
        private readonly IUnitOfWork unitOfWork;

        public QuesResult2Service(IQuesResult2Repository _quesResult2Repository, IUnitOfWork unitOfWork)
        {
            this._quesResult2Repository = _quesResult2Repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<QuesResult2>> GetQuesResult2sAsync(int questionSn, string code)
        {
            var listQuesResult2 =
                await
                    _quesResult2Repository.GetQuesResult2sAsync(
                        qr =>
                            qr.QuestionSn == questionSn && qr.QuesCheckList.CurrentUseYn == "Y" &&
                            qr.QuesCheckList.SmallClassCd == code);
            return listQuesResult2.OrderBy(qr => qr.QuesCheckList.SmallClassCd).ToList();
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