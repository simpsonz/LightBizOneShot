using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.WebConfiguration;
using System.Linq.Expressions;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScReqDocRepository : IRepository<ScReqDoc>
    {
        Task<IList<ScReqDoc>> GetReqDocsAsync(Expression<Func<ScReqDoc, bool>> where);
        Task<ScReqDoc> GetReqDocAsync(Expression<Func<ScReqDoc, bool>> where);
    }

    public class ScReqDocRepository : RepositoryBase<ScReqDoc>, IScReqDocRepository
    {
        public ScReqDocRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }


        public async Task<IList<ScReqDoc>> GetReqDocsAsync(Expression<Func<ScReqDoc, bool>> where)
        {
            return await this.DbContext.ScReqDocs.Include("ScUsr_SenderId.ScCompInfo").Include("ScUsr_ReceiverId.ScCompInfo").Where(where).ToListAsync();
        }



        public async Task<ScReqDoc> GetReqDocAsync(Expression<Func<ScReqDoc, bool>> where)
        {
            return await this.DbContext.ScReqDocs.Include("ScUsr_ReceiverId").Include("ScUsr_SenderId").Include("ScReqDocFiles").Include("ScUsr_SenderId.ScCompInfo").Include("ScUsr_ReceiverId.ScCompInfo").Where(where).SingleAsync();
        }
    }
}
