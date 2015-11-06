using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{
    public interface IScCompInfoService : IBaseService
    {
        IList<ScCompInfo> GetScCompInfoByName(string compNm);

        IEnumerable<ScCompInfo> GetScCompInfos(string compNm = null);

        ScCompInfo GetScCompInfo(int id);
        Task<ScCompInfo> GetScCompInfoByCompSn(int compSn);

        void CreateScCompInfo(ScCompInfo scCompInfo);

        Task<IList<ScCompInfo>> GetScCompInfoByRegistationNo(string registationNo);
        Task<IList<ScUsr>> GetScCompInfoForSearchId(string registationNo);
    }


    public class ScCompInfoService : IScCompInfoService
    {
        private readonly IScCompInfoRepository scCompInfoRespository;
        private readonly IUnitOfWork unitOfWork;

        public ScCompInfoService(IScCompInfoRepository scCompInfoRespository, IUnitOfWork unitOfWork)
        {
            this.scCompInfoRespository = scCompInfoRespository;
            this.unitOfWork = unitOfWork;
        }


        public IList<ScCompInfo> GetScCompInfoByName(string compNm)
        {
            return scCompInfoRespository.GetScCompInfoByName(compNm);
        }

        public IEnumerable<ScCompInfo> GetScCompInfos(string compNm = null)
        {
            if (string.IsNullOrEmpty(compNm))
            {
                return scCompInfoRespository.GetAll();
            }
            return scCompInfoRespository.GetAll().Where(ci => ci.CompNm == compNm);
        }

        public ScCompInfo GetScCompInfo(int id)
        {
            var scCompInfo = scCompInfoRespository.GetById(id);
            return scCompInfo;
        }

        public async Task<IList<ScCompInfo>> GetScCompInfoByRegistationNo(string registationNo)
        {
            var scCompInfos = await scCompInfoRespository.GetManyAsync(scr => scr.RegistrationNo == registationNo);
            return scCompInfos.ToList();
        }

        public async Task<ScCompInfo> GetScCompInfoByCompSn(int compSn)
        {
            var scCompInfo = await scCompInfoRespository.GetCompInfoAsync(scr => scr.CompSn == compSn);
            return scCompInfo;
        }


        public async Task<IList<ScUsr>> GetScCompInfoForSearchId(string registationNo)
        {
            var scCompInfo = await scCompInfoRespository.GetAsync(scr => scr.RegistrationNo == registationNo);

            if (scCompInfo == null)
                return new List<ScUsr>();
            return scCompInfo.ScUsrs.ToList();
        }

        public void CreateScCompInfo(ScCompInfo scCompInfo)
        {
            scCompInfoRespository.Add(scCompInfo);
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