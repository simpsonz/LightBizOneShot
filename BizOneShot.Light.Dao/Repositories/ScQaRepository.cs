using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface IScQaRepository : IRepository<ScQa>
    {
        Task<IList<ScQa>> GetQAsAsync(Expression<Func<ScQa, bool>> where);
        Task<ScQa> GetQAAsync(Expression<Func<ScQa, bool>> where);
        ScQa Insert(ScQa scQa);
    }


    public class ScQaRepository : RepositoryBase<ScQa>, IScQaRepository
    {
        public ScQaRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public async Task<IList<ScQa>> GetQAsAsync(Expression<Func<ScQa, bool>> where)
        {
            return await this.DbContext.ScQas.Include("ScUsr_QuestionId").Include("ScUsr_QuestionId.ScCompInfo").Include("ScUsr_AnswerId").Include("ScUsr_AnswerId.ScCompInfo").Where(where).ToListAsync();
        }


        public async Task<ScQa> GetQAAsync(Expression<Func<ScQa, bool>> where)
        {
            return await this.DbContext.ScQas.Include("ScUsr_QuestionId").Include("ScUsr_QuestionId.ScCompInfo").Include("ScUsr_AnswerId").Include("ScUsr_AnswerId.ScCompInfo").Where(where).SingleAsync();
        }

        public ScQa Insert(ScQa scQa)
        {
            return this.DbContext.ScQas.Add(scQa);
        }

    }
}
