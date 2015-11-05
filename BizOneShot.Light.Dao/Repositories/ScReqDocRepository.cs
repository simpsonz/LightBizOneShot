using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using PagedList;
using PagedList.EntityFramework;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScReqDocRepository : IRepository<ScReqDoc>
    {
        Task<IPagedList<ScReqDoc>> GetPagedListSendDocsAsync(int page, int pageSize, string senderId,
            string sendExpertType, DateTime? startDate = null, DateTime? endDate = null);

        Task<IPagedList<ScReqDoc>> GetPagedListReqDocsAsync(int page, int pageSize, string receiverId,
            string sendExpertType, DateTime? startDate = null, DateTime? endDate = null);

        Task<IList<ScReqDoc>> GetReqDocsAsync(Expression<Func<ScReqDoc, bool>> where);
        Task<ScReqDoc> GetReqDocAsync(Expression<Func<ScReqDoc, bool>> where);
        ScReqDoc Insert(ScReqDoc scReqDoc);
    }

    public class ScReqDocRepository : RepositoryBase<ScReqDoc>, IScReqDocRepository
    {
        public ScReqDocRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IPagedList<ScReqDoc>> GetPagedListSendDocsAsync(int page, int pageSize, string senderId,
            string sendExpertType, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await DbContext.ScReqDocs
                .Include("ScUsr_ReceiverId")
                .Include("ScUsr_SenderId")
                .Include("ScUsr_SenderId.ScCompInfo")
                .Include("ScUsr_ReceiverId.ScCompInfo")
                .Where(
                    rd =>
                        rd.SenderId == senderId && rd.Status == "N" && rd.ScUsr_ReceiverId.UsrType == "P" &&
                        rd.ScUsr_ReceiverId.UsrTypeDetail == sendExpertType)
                .Where(rd => rd.ReqDt >= startDate && rd.ReqDt <= endDate)
                .OrderByDescending(rd => rd.ReqDt).ToPagedListAsync(page, pageSize);
        }


        public async Task<IPagedList<ScReqDoc>> GetPagedListReqDocsAsync(int page, int pageSize, string receiverId,
            string sendExpertType, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await DbContext.ScReqDocs
                .Include("ScUsr_ReceiverId")
                .Include("ScUsr_SenderId")
                .Include("ScUsr_SenderId.ScCompInfo")
                .Include("ScUsr_ReceiverId.ScCompInfo")
                .Where(
                    rd =>
                        rd.ReceiverId == receiverId && rd.Status == "N" && rd.ScUsr_SenderId.UsrType == "P" &&
                        rd.ScUsr_SenderId.UsrTypeDetail == sendExpertType)
                .Where(rd => rd.ReqDt >= startDate && rd.ReqDt <= endDate)
                .OrderByDescending(rd => rd.ReqDt).ToPagedListAsync(page, pageSize);
        }

        public async Task<IList<ScReqDoc>> GetReqDocsAsync(Expression<Func<ScReqDoc, bool>> where)
        {
            return
                await
                    DbContext.ScReqDocs.Include("ScUsr_ReceiverId")
                        .Include("ScUsr_SenderId")
                        .Include("ScUsr_SenderId.ScCompInfo")
                        .Include("ScUsr_ReceiverId.ScCompInfo")
                        .Where(where)
                        .ToListAsync();
        }

        public async Task<ScReqDoc> GetReqDocAsync(Expression<Func<ScReqDoc, bool>> where)
        {
            return
                await
                    DbContext.ScReqDocs.Include("ScUsr_ReceiverId")
                        .Include("ScUsr_SenderId")
                        .Include("ScReqDocFiles")
                        .Include("ScUsr_SenderId.ScCompInfo")
                        .Include("ScUsr_ReceiverId.ScCompInfo")
                        .Where(where)
                        .SingleAsync();
        }

        public ScReqDoc Insert(ScReqDoc scReqDoc)
        {
            return DbContext.ScReqDocs.Add(scReqDoc);
        }
    }
}