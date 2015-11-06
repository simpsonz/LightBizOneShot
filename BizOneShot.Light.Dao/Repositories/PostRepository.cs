using System.Collections.Generic;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IPostRepository : IRepository<UspSelectSidoForWebListReturnModel>
    {
        Task<IList<UspSelectSidoForWebListReturnModel>> GetSidoListAsync();
        Task<IList<UspSelectGunguForWebListReturnModel>> GetGunguListAsync(object[] parameters);

        Task<IList<UspSelectAddressByStreetSearchForWebListReturnModel>> GetAddressByStreetSearchListAsync(
            object[] parameters);

        Task<IList<UspSelectAddressByDongSearchForWebListReturnModel>> GetAddressByDongSearchListAsync(
            object[] parameters);

        Task<IList<UspSelectAddressByBuildingSearchForWebListReturnModel>> GetAddressByBuildingSearchListAsync(
            object[] parameters);
    }


    public class PostRepository : RepositoryBase<UspSelectSidoForWebListReturnModel>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<UspSelectSidoForWebListReturnModel>> GetSidoListAsync()
        {
            return
                await
                    DbContext.Database.SqlQuery<UspSelectSidoForWebListReturnModel>("USP_SelectSidoForWebList")
                        .ToListAsync();
        }

        public async Task<IList<UspSelectGunguForWebListReturnModel>> GetGunguListAsync(object[] parameters)
        {
            return
                await
                    DbContext.Database.SqlQuery<UspSelectGunguForWebListReturnModel>("USP_SelectGunguForWebList @SIDO",
                        parameters).ToListAsync();
        }


        public async Task<IList<UspSelectAddressByStreetSearchForWebListReturnModel>> GetAddressByStreetSearchListAsync(
            object[] parameters)
        {
            return
                await
                    DbContext.Database.SqlQuery<UspSelectAddressByStreetSearchForWebListReturnModel>(
                        "USP_SelectAddressByStreetSearchForWebList @SIDO, @GUNGU, @RD_NM, @MN_NO, @SUB_NO ", parameters)
                        .ToListAsync();
        }

        public async Task<IList<UspSelectAddressByDongSearchForWebListReturnModel>> GetAddressByDongSearchListAsync(
            object[] parameters)
        {
            return
                await
                    DbContext.Database.SqlQuery<UspSelectAddressByDongSearchForWebListReturnModel>(
                        "USP_SelectAddressByDongSearchForWebList @SIDO, @GUNGU, @DONG, @MN_NO, @SUB_NO", parameters)
                        .ToListAsync();
        }

        public async Task<IList<UspSelectAddressByBuildingSearchForWebListReturnModel>>
            GetAddressByBuildingSearchListAsync(object[] parameters)
        {
            return
                await
                    DbContext.Database.SqlQuery<UspSelectAddressByBuildingSearchForWebListReturnModel>(
                        "USP_SelectAddressByBuildingSearchForWebList @SIDO, @GUNGU, @BLD_NM", parameters).ToListAsync();
        }

        //}
        //    return await this.DbContext.ScQas.Include("ScUsr_QuestionId").Include("ScUsr_QuestionId.ScCompInfo").Include("ScUsr_AnswerId").Include("ScUsr_AnswerId.ScCompInfo").Where(where).SingleAsync();
        //{


        //public async Task<ScQa> GetQAAsync(Expression<Func<ScQa, bool>> where)
    }
}