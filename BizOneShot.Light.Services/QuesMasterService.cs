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
    public interface IQuesMasterService : IBaseService
    {
        Task<IList<QuesMaster>> GetQuesMastersAsync(string registrationNo);
        Task<QuesMaster> GetQuesMasterAsync(int questionSn);
        Task<QuesMaster> GetQuesMasterAsync(string registrationNo, int basicYear);
        Task<QuesMaster> AddQuesMasterAsync(QuesMaster quesMaster);
        Task<QuesMaster> GetQuesCompInfoAsync(int questionSn);
        Task<QuesMaster> GetQuesCompExtentionAsync(int questionSn);
        Task<QuesMaster> GetQuesResult1Async(int questionSn);
        Task<QuesMaster> GetQuesResult2Async(int questionSn);
        Task<QuesMaster> GetQuesOgranAnalysisAsync(int questionSn);
    }


    public class QuesMasterService : IQuesMasterService
    {
        private readonly IQuesMasterRepository _quesMasterRepository;
        private readonly IUnitOfWork unitOfWork;

        public QuesMasterService(IQuesMasterRepository _quesMasterRepository, IUnitOfWork unitOfWork)
        {
            this._quesMasterRepository = _quesMasterRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<QuesMaster>> GetQuesMastersAsync(string registrationNo)
        {
            var listQuesMaster = await _quesMasterRepository.GetQuesMastersAsync(qm => qm.RegistrationNo == registrationNo && qm.Status != "D");
            return listQuesMaster.OrderByDescending(qm => qm.BasicYear).ToList();
        }
        
        public async Task<QuesMaster> GetQuesMasterAsync(int questionSn)
        {
            var quesMaster = await _quesMasterRepository.GetQuesMasterAsync(qm => qm.QuestionSn == questionSn && qm.Status != "D");
            return quesMaster;
        }

        public async Task<QuesMaster> GetQuesMasterAsync(string registrationNo, int basicYear)
        {
            var quesMaster = await _quesMasterRepository.GetQuesMasterAsync(qm => qm.RegistrationNo == registrationNo && qm.BasicYear == basicYear && qm.Status == "C");
            return quesMaster;
        }

        public async Task<QuesMaster> GetQuesCompInfoAsync(int questionSn)
        {
            var quesMaster = await _quesMasterRepository.GetQuesCompInfoAsync(qm => qm.QuestionSn == questionSn && qm.Status != "D");
            return quesMaster;
        }

        public async Task<QuesMaster> GetQuesCompExtentionAsync(int questionSn)
        {
            var quesMaster = await _quesMasterRepository.GetQuesCompExtentionAsync(qm => qm.QuestionSn == questionSn && qm.Status != "D");
            return quesMaster;
        }

        public async Task<QuesMaster> GetQuesResult1Async(int questionSn)
        {
            var quesMaster = await _quesMasterRepository.GetQuesResult1Async(qm => qm.QuestionSn == questionSn && qm.Status != "D");
            return quesMaster;
        }

        public async Task<QuesMaster> GetQuesResult2Async(int questionSn)
        {
            var quesMaster = await _quesMasterRepository.GetQuesResult2Async(qm => qm.QuestionSn == questionSn && qm.Status != "D");
            return quesMaster;
        }

        public async Task<QuesMaster> GetQuesOgranAnalysisAsync(int questionSn)
        {
            var quesMaster = await _quesMasterRepository.GetQuesOgranAnalysisAsync(qm => qm.QuestionSn == questionSn && qm.Status != "D");
            return quesMaster;
        }

        public async Task<QuesMaster> AddQuesMasterAsync(QuesMaster quesMaster)
        {
            //var rstScUsr = scUsrRespository.Insert(scUsr);
            //scCompInfo.
            var rstQuesMaster = _quesMasterRepository.Insert(quesMaster);

            if (rstQuesMaster != null)
            {
                await SaveDbContextAsync();
            }

            return rstQuesMaster;

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
