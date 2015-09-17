using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface IPostRepository : IRepository<UspSelectSidoForWebListReturnModel>
    {
        Task<IList<UspSelectSidoForWebListReturnModel>> GetSidosAsync();
        //Task<UspSelectSidoForWebListReturnModel> GetSidoAsync(Expression<Func<UspSelectSidoForWebListReturnModel, bool>> where);
    }


    public class PostRepository : RepositoryBase<UspSelectSidoForWebListReturnModel>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public async Task<IList<UspSelectSidoForWebListReturnModel>> GetSidosAsync()
        {
            return await this.DbContext.Database.SqlQuery<UspSelectSidoForWebListReturnModel>("USP_SelectSidoForWebList").ToListAsync();
        }


        //public async Task<ScQa> GetQAAsync(Expression<Func<ScQa, bool>> where)
        //{
        //    return await this.DbContext.ScQas.Include("ScUsr_QuestionId").Include("ScUsr_QuestionId.ScCompInfo").Include("ScUsr_AnswerId").Include("ScUsr_AnswerId.ScCompInfo").Where(where).SingleAsync();
        //}

    }
}
