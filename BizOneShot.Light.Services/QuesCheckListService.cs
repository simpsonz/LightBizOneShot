using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizOneShot.Light.Services
{
    public interface IQuesCheckListService : IBaseService
    {
        Task<IList<QuesCheckList>> GetQuesCheckListAsync(string code);
    }


    public class QuesCheckListService : IQuesCheckListService
    {
        private readonly IQuesCheckListRepository _quesCheckListRepository;
        private readonly IUnitOfWork unitOfWork;

        public QuesCheckListService(IQuesCheckListRepository _quesCheckListRepository, IUnitOfWork unitOfWork)
        {
            this._quesCheckListRepository = _quesCheckListRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<QuesCheckList>> GetQuesCheckListAsync(string code)
        {
            var listQuesCheckList =
                await
                    _quesCheckListRepository.GetQuesCheckListsAsync(
                        qcl => qcl.SmallClassCd == code && qcl.CurrentUseYn == "Y");
            return listQuesCheckList.OrderBy(qr => qr.SmallClassCd).ToList();
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