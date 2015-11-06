using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using PagedList;

namespace BizOneShot.Light.Services
{
    public interface IScReqDocService : IBaseService
    {
        Task<IPagedList<ScReqDoc>> GetPagedListSendDocs(int page, int pageSize, string senderId, string sendExpertType,
            DateTime? startDate = null, DateTime? endDate = null);

        Task<IPagedList<ScReqDoc>> GetPagedListReceiveDocs(int page, int pageSize, string receiverId,
            string sendExpertType, DateTime? startDate = null, DateTime? endDate = null);

        Task<IList<ScReqDoc>> GetReceiveDocs(string receiverId, string checkYN, DateTime startDate, DateTime endDate,
            string comName = null, string registrationNo = null);

        Task<IList<ScReqDoc>> GetReceiveDocs(string receiverId, string sendExpertType, DateTime? startDate = null,
            DateTime? endDate = null);

        Task<IList<ScReqDoc>> GetSendDocs(string senderId, string checkYN, DateTime startDate, DateTime endDate,
            string comName = null, string registrationNo = null);

        Task<IList<ScReqDoc>> GetSendDocs(string senderId, string sendExpertType, DateTime? startDate = null,
            DateTime? endDate = null);

        Task<ScReqDoc> GetReqDoc(int reqDocSn);
        Task<int> AddReqDocAsync(ScReqDoc scReqDoc);
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


        public async Task<IPagedList<ScReqDoc>> GetPagedListReceiveDocs(int page, int pageSize, string receiverId,
            string sendExpertType, DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = startDate ?? DateTime.Parse("1900-01-01");
            endDate = endDate ?? DateTime.Parse("2999-12-31");

            return
                await
                    scReqDocRepository.GetPagedListReqDocsAsync(page, pageSize, receiverId, sendExpertType, startDate,
                        endDate);
        }

        public async Task<IList<ScReqDoc>> GetReceiveDocs(string receiverId, string checkYN, DateTime startDate,
            DateTime endDate, string comName = null, string registrationNo = null)
        {
            var scReqDocs =
                await
                    scReqDocRepository.GetReqDocsAsync(
                        rd =>
                            rd.ReceiverId == receiverId && rd.Status == "N" && rd.ChkYn.Contains(checkYN) &&
                            rd.ReqDt >= startDate && rd.ReqDt <= endDate &&
                            rd.ScUsr_SenderId.ScCompInfo.CompNm.Contains(comName) &&
                            rd.ScUsr_SenderId.ScCompInfo.RegistrationNo.Contains(registrationNo));
            return scReqDocs.OrderByDescending(rd => rd.ReqDt).ToList();
        }


        public async Task<IList<ScReqDoc>> GetReceiveDocs(string receiverId, string sendExpertType,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = startDate ?? DateTime.Parse("1900-01-01");
            endDate = endDate ?? DateTime.Parse("2999-12-31");

            var scReqDocs = await scReqDocRepository.GetReqDocsAsync(
                rd => rd.ReceiverId == receiverId
                      && rd.Status == "N"
                      && rd.ScUsr_SenderId.UsrType == "P"
                      && rd.ScUsr_SenderId.UsrTypeDetail == sendExpertType && rd.ReqDt >= startDate &&
                      rd.ReqDt <= endDate
                );
            return scReqDocs.OrderByDescending(rd => rd.ReqDt).ToList();
        }


        public async Task<ScReqDoc> GetReqDoc(int reqDocSn)
        {
            var scReqDoc = await scReqDocRepository.GetReqDocAsync(rd => rd.ReqDocSn == reqDocSn);

            return scReqDoc;
        }

        public async Task<IList<ScReqDoc>> GetSendDocs(string senderId, string checkYN, DateTime startDate,
            DateTime endDate, string comName = null, string registrationNo = null)
        {
            var scReqDocs =
                await
                    scReqDocRepository.GetReqDocsAsync(
                        rd =>
                            rd.SenderId == senderId && rd.Status == "N" && rd.ChkYn.Contains(checkYN) &&
                            rd.ReqDt >= startDate && rd.ReqDt <= endDate &&
                            rd.ScUsr_ReceiverId.ScCompInfo.CompNm.Contains(comName) &&
                            rd.ScUsr_ReceiverId.ScCompInfo.RegistrationNo.Contains(registrationNo));
            return scReqDocs.OrderByDescending(rd => rd.ReqDt).ToList();
        }

        public async Task<IPagedList<ScReqDoc>> GetPagedListSendDocs(int page, int pageSize, string senderId,
            string sendExpertType, DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = startDate ?? DateTime.Parse("1900-01-01");
            endDate = endDate ?? DateTime.Parse("2999-12-31");

            return
                await
                    scReqDocRepository.GetPagedListSendDocsAsync(page, pageSize, senderId, sendExpertType, startDate,
                        endDate);
        }

        public async Task<IList<ScReqDoc>> GetSendDocs(string senderId, string sendExpertType,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = startDate ?? DateTime.Parse("1900-01-01");
            endDate = endDate ?? DateTime.Parse("2999-12-31");

            var scReqDocs = await scReqDocRepository.GetReqDocsAsync(
                rd => rd.SenderId == senderId
                      && rd.Status == "N"
                      && rd.ScUsr_ReceiverId.UsrType == "P"
                      && rd.ScUsr_ReceiverId.UsrTypeDetail == sendExpertType && rd.ReqDt >= startDate &&
                      rd.ReqDt <= endDate
                );
            return scReqDocs.OrderByDescending(rd => rd.ReqDt).ToList();
        }

        public async Task<int> AddReqDocAsync(ScReqDoc scReqDoc)
        {
            var rstScReqDoc = scReqDocRepository.Insert(scReqDoc);

            if (rstScReqDoc == null)
            {
                return -1;
            }
            return await SaveDbContextAsync();
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