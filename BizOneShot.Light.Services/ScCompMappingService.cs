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
    public interface IScCompMappingService : IBaseService
    {
        Task<IList<ScCompMapping>> GetCompMappingListByMentorId(string mentorId = null, string status = null);
        Task<IList<ScCompMapping>> GetCompMappingListByMentorId(string mentorId, string status = "A", int bizWorkSn = 0, int bizWorkYear = 0);
        Task<IList<ScCompMapping>> GetCompMappingsAsync(int compSn, int bizWorkSn = 0, string status = null, string compNm = null);
        Task<ScCompMapping> GetCompMappingAsync(int bizWorkSn, int compSn);
        Task<ScCompMapping> GetCompMappingAsync(int compSn, string status = null);
        Task<IList<ScCompMapping>> GetExpertCompMappingsAsync(string expertId, int bizWorkSn = 0, string comName = null);
        Task<IList<ScCompMapping>> GetExpertCompMappingsForPopupAsync(string expertId, string query);
    }


    public class ScCompMappingService : IScCompMappingService
    {
        private readonly IScCompMappingRepository scCompMappingRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScCompMappingService(IScCompMappingRepository scCompMappingRepository, IUnitOfWork unitOfWork)
        {
            this.scCompMappingRepository = scCompMappingRepository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<IList<ScCompMapping>> GetExpertCompMappingsForPopupAsync(string expertId, string query)
        {
            IEnumerable<ScCompMapping> listScCompMappingTask = null;


            listScCompMappingTask = await scCompMappingRepository.GetExpertCompanysForPopupAsync(expertId, query);
            return listScCompMappingTask.OrderByDescending(scm => scm.RegDt).ToList();

        }

        public async Task<ScCompMapping> GetCompMappingAsync(int bizWorkSn, int compSn)
        {
            var scCompMapping = await scCompMappingRepository.GetCompMappingAsync(scm => scm.BizWorkSn == bizWorkSn && scm.CompSn == compSn);

            return scCompMapping;
        }


        //사업참여기업의 매핑정보를 가져옴
        public async Task<ScCompMapping> GetCompMappingAsync(int compSn, string status = null)
        {
            if (string.IsNullOrEmpty(status))
            {
                var scCompMapping = await scCompMappingRepository.GetCompMappingAsync(scm => scm.CompSn == compSn);

                return scCompMapping;
            }
            else
            {
                var scCompMapping = await scCompMappingRepository.GetCompMappingAsync(scm => scm.CompSn == compSn && scm.Status == status);

                return scCompMapping;
            }
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


            if (!string.IsNullOrEmpty(expertId) && bizWorkSn == 0 && string.IsNullOrEmpty(comName)) //100
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
        public async Task<IList<ScCompMapping>> GetCompMappingListByMentorId(string mentorId = null, string status = null)
        {

            if (!string.IsNullOrEmpty(mentorId) && !string.IsNullOrEmpty(status))
            {
                return await scCompMappingRepository.GetCompMappingsAsync(cmp => cmp.MentorId == mentorId && cmp.Status == status);
            }
            else if (!string.IsNullOrEmpty(mentorId) && string.IsNullOrEmpty(status))
            {
                return await scCompMappingRepository.GetCompMappingsAsync(cmp => cmp.MentorId == mentorId);
            }
            else if (string.IsNullOrEmpty(mentorId) && !string.IsNullOrEmpty(status))
            {
                return await scCompMappingRepository.GetCompMappingsAsync(cmp => cmp.Status == status);
            }
            else
            {
                return await scCompMappingRepository.GetCompMappingsAsync(cmp => cmp.BizWorkSn > 0);
            }
        }

        public async Task<IList<ScCompMapping>> GetCompMappingListByMentorId(string mentorId, string status = "A", int bizWorkSn = 0, int bizWorkYear = 0)
        {
            DateTime date = DateTime.Now.Date;
            if (bizWorkYear == 0)
            {
                //return await scCompMappingRepository.GetCompMappingsAsync(
                //cmp => cmp.MentorId == mentorId && cmp.Status == status
                //&& cmp.ScBizWork.BizWorkEdDt.Value > date
                //&& bizWorkSn == 0 ? cmp.ScBizWork.BizWorkSn > 0 : cmp.ScBizWork.BizWorkSn == bizWorkSn
                //);

                var listScCompMapping = await scCompMappingRepository.GetCompMappingsAsync(cmp => cmp.MentorId == mentorId && cmp.Status == status);

                return listScCompMapping.Where(cmp => cmp.ScBizWork.BizWorkEdDt.Value > date && bizWorkSn == 0 ? cmp.ScBizWork.BizWorkSn > 0 : cmp.ScBizWork.BizWorkSn == bizWorkSn).ToList();
            }
            else
            {
                //return await scCompMappingRepository.GetCompMappingsAsync(
                //cmp => cmp.MentorId == mentorId && cmp.Status == status
                //&& cmp.ScBizWork.BizWorkEdDt.Value > date
                //&& bizWorkSn == 0 ? cmp.ScBizWork.BizWorkSn > 0 : cmp.ScBizWork.BizWorkSn == bizWorkSn
                //);
                //return await scCompMappingRepository.GetCompMappingsAsync(
                //cmp => cmp.MentorId == mentorId && cmp.Status == status
                //&& cmp.ScBizWork.BizWorkEdDt.Value > date
                //&& bizWorkSn == 0 ? cmp.ScBizWork.BizWorkSn > 0 : cmp.ScBizWork.BizWorkSn == bizWorkSn
                //&& cmp.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear && cmp.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear
                //);

                var listScCompMapping = await scCompMappingRepository.GetCompMappingsAsync(cmp => cmp.MentorId == mentorId && cmp.Status == status);

                return listScCompMapping.Where(cmp => cmp.ScBizWork.BizWorkEdDt.Value > date
                && bizWorkSn == 0 ? cmp.ScBizWork.BizWorkSn > 0 : cmp.ScBizWork.BizWorkSn == bizWorkSn
                && cmp.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear && cmp.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear
                ).ToList();

            }
        }

        #region SaveContext
        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await unitOfWork.CommitAsync();
        }
        #endregion
    }
}
