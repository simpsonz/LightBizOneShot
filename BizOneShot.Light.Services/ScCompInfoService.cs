using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;

namespace BizOneShot.Light.Services
{

    public interface IScCompInfoService : IBaseService
    {
        IList<ScCompInfo> GetScCompInfoByName(string compNm);

        IEnumerable<ScCompInfo> GetScCompInfos(string compNm = null);

        ScCompInfo GetScCompInfo(int id);

        void CreateScCompInfo(ScCompInfo scCompInfo);

        Task<IList<ScCompInfo>> GetScCompInfoByRegistationNo(string registationNo);


    }


    public class ScCompInfoService : IScCompInfoService
    {
        private readonly IScCompInfoRepository scCompInfoRespository;
        private readonly  IUnitOfWork unitOfWork;

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
            else
            {
                return scCompInfoRespository.GetAll().Where(ci => ci.CompNm == compNm);
            }
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
