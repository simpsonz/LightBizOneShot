using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{

    public interface IScQaService : IBaseService
    {
        Task<IList<ScQa>> GetReceiveQAsAsync(string loginId, string checkYN, DateTime startDate, DateTime endDate, string comName = null, string registrationNo = null);
        Task<ScQa> GetQAAsync(int usrQaSn);
    }


    public class ScQaService : IScQaService
    {
        private readonly IScQaRepository _scQaRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScQaService(IScQaRepository _scQaRepository, IUnitOfWork unitOfWork)
        {
            this._scQaRepository = _scQaRepository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<ScQa> GetQAAsync(int usrQaSn)
        {
            var scQa = await _scQaRepository.GetQAAsync(sq => sq.UsrQaSn == usrQaSn);

            return scQa;
        }

        public async Task<IList<ScQa>> GetReceiveQAsAsync(string loginId, string checkYN, DateTime startDate, DateTime endDate, string comName = null, string registrationNo = null)
        {
            var listScQaTask = await _scQaRepository.GetQAsAsync(sq => sq.AnswerId == loginId && sq.AnsYn.Contains(checkYN) && sq.ScUsr_QuestionId.ScCompInfo.CompNm.Contains(comName) && sq.ScUsr_QuestionId.ScCompInfo.RegistrationNo.Contains(registrationNo) && (sq.AskDt >= startDate && sq.AskDt <= endDate));
            return listScQaTask.OrderByDescending(sq => sq.AskDt).ToList();
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
