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

    public interface IScExpertMappingService : IBaseService
    {

        //IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);
        Task<IList<ScExpertMapping>> GetExpertManagerAsync(string bizMngSn = null, string expertType = null);
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
                listScUsrTask = await scExpertMappingRespository.GetManyAsync(em => em.ScBizWork.ScCompInfo.CompSn.ToString() == bizMngSn);
                return listScUsrTask.OrderByDescending(em => em.RegDt).ToList();
            }
            else
            {
                listScUsrTask = await scExpertMappingRespository.GetManyAsync(em => em.ScBizWork.ScCompInfo.CompSn.ToString() == bizMngSn && em.ScUsr.UsrTypeDetail == expertType);
                return listScUsrTask.OrderByDescending(usr => usr.RegDt).ToList();
            }
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
