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
        Task<IList<ScCompMapping>> GetCompMappingsAsync(int compSn, int bizWorkSn = 0, string status = null, string compNm = null);
        Task<ScCompMapping> GetCompMappingAsync(int bizWorkSn, int compSn);
        Task<IList<ScCompMapping>> GetExpertCompMappingsAsync(string expertId, int bizWorkSn = 0, string comName = null);

    }


    public class ScCompInfoService : IScCompInfoService
    {
        private readonly IScCompInfoRepository scCompInfoRespository;
        private readonly IScCompMappingRepository scCompMappingRepository;
        private readonly  IUnitOfWork unitOfWork;

        public ScCompInfoService(IScCompInfoRepository scCompInfoRespository, IScCompMappingRepository scCompMappingRepository, IUnitOfWork unitOfWork)
        {
            this.scCompMappingRepository = scCompMappingRepository;
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

        public async Task<ScCompMapping> GetCompMappingAsync(int bizWorkSn, int compSn)
        {
            var scCompMapping = await scCompMappingRepository.GetCompMappingAsync(scm => scm.BizWorkSn == bizWorkSn && scm.CompSn == compSn);

            return scCompMapping;
        }

        public async Task<IList<ScCompMapping>> GetCompMappingsAsync(int compSn, int bizWorkSn = 0, string status = null, string compNm = null)
        {
            IEnumerable<ScCompMapping> listScCompMappingTask = null;


            if ((bizWorkSn == 0) && string.IsNullOrEmpty(status) && string.IsNullOrEmpty(compNm)) //000
            {
                listScCompMappingTask = await scCompMappingRepository.GetCompMappingsAsync(scm => scm.ScBizWork.ScCompInfo.CompSn == compSn && scm.Status != "D");
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else if ((bizWorkSn == 0) && string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(compNm)) //001
            {
                listScCompMappingTask = await scCompMappingRepository.GetCompMappingsAsync(scm => scm.ScBizWork.ScCompInfo.CompSn == compSn && scm.ScCompInfo.CompNm.Contains(compNm) && scm.Status != "D");
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else if ((bizWorkSn == 0) && !string.IsNullOrEmpty(status) && string.IsNullOrEmpty(compNm)) //010
            {
                listScCompMappingTask = await scCompMappingRepository.GetCompMappingsAsync(scm => scm.ScBizWork.ScCompInfo.CompSn == compSn && scm.Status == status);
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else if ((bizWorkSn == 0) && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(compNm)) //011
            {
                listScCompMappingTask = await scCompMappingRepository.GetCompMappingsAsync(scm => scm.ScBizWork.ScCompInfo.CompSn == compSn && scm.Status == status && scm.ScCompInfo.CompNm.Contains(compNm));
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else if ((bizWorkSn != 0) && string.IsNullOrEmpty(status) && string.IsNullOrEmpty(compNm)) //100
            {
                listScCompMappingTask = await scCompMappingRepository.GetCompMappingsAsync(scm => scm.ScBizWork.ScCompInfo.CompSn == compSn && scm.Status != "D" && scm.BizWorkSn == bizWorkSn);
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else if ((bizWorkSn != 0) && string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(compNm)) //101
            {
                listScCompMappingTask = await scCompMappingRepository.GetCompMappingsAsync(scm => scm.ScBizWork.ScCompInfo.CompSn == compSn && scm.Status != "D" && scm.BizWorkSn == bizWorkSn && scm.ScCompInfo.CompNm.Contains(compNm));
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else if ((bizWorkSn != 0) && !string.IsNullOrEmpty(status) && string.IsNullOrEmpty(compNm)) //110
            {
                listScCompMappingTask = await scCompMappingRepository.GetCompMappingsAsync(scm => scm.ScBizWork.ScCompInfo.CompSn == compSn && scm.Status == status && scm.BizWorkSn == bizWorkSn);
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else  //111
            {
                listScCompMappingTask = await scCompMappingRepository.GetCompMappingsAsync(scm => scm.ScBizWork.ScCompInfo.CompSn == compSn && scm.Status == status && scm.BizWorkSn == bizWorkSn && scm.ScCompInfo.CompNm.Contains(compNm));
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
        }


        public async Task<IList<ScCompMapping>> GetExpertCompMappingsAsync(string expertId, int bizWorkSn = 0, string comName = null)
        {
            IEnumerable<ScCompMapping> listScCompMappingTask = null;


            if (!string.IsNullOrEmpty(expertId) && bizWorkSn == 0  && string.IsNullOrEmpty(comName)) //100
            {
                listScCompMappingTask = await scCompMappingRepository.GetExpertCompanysAsync(expertId);
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else if (!string.IsNullOrEmpty(expertId) && (bizWorkSn == 0) && !string.IsNullOrEmpty(comName)) //001
            {
                listScCompMappingTask = await scCompMappingRepository.GetExpertCompanysAsync(expertId, comName);
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else if ((bizWorkSn != 0) && string.IsNullOrEmpty(comName)) //010
            {
                listScCompMappingTask = await scCompMappingRepository.GetExpertCompanysAsync(scm => scm.Status == "A" && scm.ScBizWork.BizWorkSn == bizWorkSn);
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else if ((bizWorkSn != 0) && !string.IsNullOrEmpty(comName)) //010
            {
                listScCompMappingTask = await scCompMappingRepository.GetExpertCompanysAsync(scm => scm.Status == "A" && scm.ScBizWork.BizWorkSn == bizWorkSn && scm.ScCompInfo.CompNm.Contains(comName));
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }
            else  //111
            {
                listScCompMappingTask = await scCompMappingRepository.GetExpertCompanysAsync(expertId);
                return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();
            }

                
        }

    }
}
