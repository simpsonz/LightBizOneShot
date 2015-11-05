using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using PagedList;

namespace BizOneShot.Light.Services
{
    public interface IScExpertMappingService : IBaseService
    {
        Task<IList<ScExpertMapping>> GetExpertManagerAsync(string bizMngSn = null, string expertType = null);
        Task<ScExpertMapping> GetExpertAsync(string ExpertId);
        Task<ScExpertMapping> GetExpertAsync(int bizWorkSn, string expertType);
        Task<IList<ScExpertMapping>> GetExpertsAsync(string ExpertId);
        Task<ScExpertMapping> GetSameDetailTypeExpertAsync(int bizWorkSn, string userTypeDetail);

        IPagedList<ScCompMapping> GetScCompMappings(int page, int pageSize, string expertId, int bizWorkSn,
            int mngCompSn, string status);
    }


    public class ScExpertMappingService : IScExpertMappingService
    {
        private readonly IExpertMappingRepository scExpertMappingRespository;
        private readonly IUnitOfWork unitOfWork;

        public ScExpertMappingService(IExpertMappingRepository scExpertMappingRespository, IUnitOfWork unitOfWork)
        {
            this.scExpertMappingRespository = scExpertMappingRespository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<ScExpertMapping>> GetExpertManagerAsync(string bizMngSn = null, string expertType = null)
        {
            IEnumerable<ScExpertMapping> listScUsrTask = null;


            if ((bizMngSn != "0") && string.IsNullOrEmpty(expertType))
            {
                listScUsrTask =
                    await
                        scExpertMappingRespository.GetManyAsync(
                            em => em.ScBizWork.ScCompInfo.CompSn.ToString() == bizMngSn);
                return listScUsrTask.OrderByDescending(em => em.RegDt).ToList();
            }
            listScUsrTask =
                await
                    scExpertMappingRespository.GetManyAsync(
                        em =>
                            em.ScBizWork.ScCompInfo.CompSn.ToString() == bizMngSn &&
                            em.ScUsr.UsrTypeDetail == expertType);
            return listScUsrTask.OrderByDescending(usr => usr.RegDt).ToList();
        }

        public async Task<ScExpertMapping> GetExpertAsync(string ExpertId)
        {
            var scExpertMapping =
                await scExpertMappingRespository.GetExpertMappingAsync(sem => sem.ExpertId == ExpertId);
            return scExpertMapping;
        }

        public async Task<ScExpertMapping> GetSameDetailTypeExpertAsync(int bizWorkSn, string userTypeDetail)
        {
            var scExpertMapping =
                await
                    scExpertMappingRespository.GetExpertMappingAsync(
                        sem =>
                            sem.BizWorkSn == bizWorkSn && sem.ScUsr.UsrTypeDetail == userTypeDetail && sem.Status == "N");
            return scExpertMapping;
        }

        public async Task<IList<ScExpertMapping>> GetExpertsAsync(string ExpertId)
        {
            var scExpertsMapping =
                await scExpertMappingRespository.GetExperetMappingsAsync(sem => sem.ExpertId == ExpertId);

            return scExpertsMapping;
        }


        public async Task<ScExpertMapping> GetExpertAsync(int bizWorkSn, string expertType)
        {
            var scExpertMapping =
                await
                    scExpertMappingRespository.GetExpertMappingAsync(
                        sem =>
                            sem.BizWorkSn == bizWorkSn && sem.ScUsr.UsrType == "P" &&
                            sem.ScUsr.UsrTypeDetail == expertType);

            return scExpertMapping;
        }

        public IPagedList<ScCompMapping> GetScCompMappings(int page, int pageSize, string expertId, int bizWorkSn,
            int mngCompSn, string status)
        {
            return scExpertMappingRespository.GetExpertMappingsAsync(page, pageSize, expertId, bizWorkSn, mngCompSn,
                status);
        }


        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await unitOfWork.CommitAsync();
        }
    }
}