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
        Task<IList<ScQa>> GetReceiveQAsAsync(string loginId);
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

        public async Task<IList<ScQa>> GetReceiveQAsAsync(string loginId)
        {
            IEnumerable<ScQa> listScQaTask = null;

            listScQaTask = await _scQaRepository.GetQAsAsync(sq => sq.AnswerId == loginId);
            return listScQaTask.OrderByDescending(sq => sq.AnsDt).ToList();
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
