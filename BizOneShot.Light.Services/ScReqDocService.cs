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
        Task<IList<ScReqDoc>> GetReceiveDocs(string receiverId, string checkYN, DateTime startDate, DateTime endDate, string comName = null, string registrationNo = null);

        Task<IList<ScReqDoc>> GetSendDocs(string senderId, string checkYN, DateTime startDate, DateTime endDate, string comName = null, string registrationNo = null);
        Task<ScReqDoc> GetReqDoc(int reqDocSn);
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


        public async Task<IList<ScReqDoc>> GetReceiveDocs(string receiverId, string checkYN, DateTime startDate, DateTime endDate, string comName = null, string registrationNo = null)
        {
            var scReqDocs = await scReqDocRepository.GetReqDocsAsync(rd => rd.ReceiverId == receiverId && rd.Status == "N" && rd.ChkYn.Contains(checkYN) && (rd.ReqDt >= startDate && rd.ReqDt <= endDate && rd.ScUsr_SenderId.ScCompInfo.CompNm.Contains(comName) && rd.ScUsr_SenderId.ScCompInfo.RegistrationNo.Contains(registrationNo)));
            return scReqDocs.OrderByDescending(rd => rd.ReqDt).ToList();
        }

        public async Task<ScReqDoc> GetReqDoc(int reqDocSn)
        {
            var scReqDoc = await scReqDocRepository.GetReqDocAsync(rd => rd.ReqDocSn == reqDocSn);

            return scReqDoc;
        }

        public async Task<IList<ScReqDoc>> GetSendDocs(string senderId, string checkYN, DateTime startDate, DateTime endDate, string comName = null, string registrationNo = null)
        {
            var scReqDocs = await scReqDocRepository.GetReqDocsAsync(rd => rd.SenderId == senderId && rd.Status == "N" && rd.ChkYn.Contains(checkYN) && (rd.ReqDt >= startDate && rd.ReqDt <= endDate && rd.ScUsr_ReceiverId.ScCompInfo.CompNm.Contains(comName) && rd.ScUsr_ReceiverId.ScCompInfo.RegistrationNo.Contains(registrationNo)));
            return scReqDocs.OrderByDescending(rd => rd.ReqDt).ToList();
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
