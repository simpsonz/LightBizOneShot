using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;


namespace BizOneShot.Light.Services
{
    public interface IScReqDocService : IBaseService
    {
        Task<IList<ScReqDoc>> GetReqDocsAsync(string searchType = null, string keyword = null);
    }
    public class ScReqDocService : IScReqDocService
    {
        private readonly IScReqDocRepository scReqDocRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScReqDocService(IScReqDocRepository scReqDocRepository, IUnitOfWork unitOfWork)
        {
            this.scReqDocRepository = scReqDocRepository;
            this.unitOfWork = unitOfWork;
        }
       
        public async Task<IList<ScReqDoc>> GetReqDocsAsync(string senderId, string receiverId)
        {
            IEnumerable<ScReqDoc> listScReqDocTask = null;
            listScReqDocTask = await scReqDocRepository.GetManyAsync(reqDoc => reqDoc.ReceiverId == receiverId && reqDoc.SenderId == receiverId && reqDoc.Status == "N");
            return listScReqDocTask.OrderByDescending(reqDoc => reqDoc.ReqDocSn).ToList();
        }

        #region SaveDbContext
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
