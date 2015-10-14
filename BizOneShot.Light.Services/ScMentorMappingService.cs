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
    public interface IScMentorMappingService : IBaseService
    {
        Task<IList<ScMentorMappiing>> GetMentorListAsync(int mngCompSn, int bizWorkSn = 0, string usrTypeDetail = null);
        Task<ScMentorMappiing> GetMentor(int bizWorkSn, string mentorId);
        //Task<IList<ScMentorMappiing>> GetMentorMappingListByMentorId(string mentorId = null);
        Task<IList<ScMentorMappiing>> GetMentorMappingListByMentorId(string mentorId = null, int bizWorkYear = 0);
        Task<int> AddMentorAsync(ScCompInfo scCompInfo);
    }


    public class ScMentorMappingService : IScMentorMappingService
    {
        private readonly IScMentorMappingRepository scMentorMappingRepository;
        private readonly IScCompInfoRepository scCompInfoRespository;
        private readonly IUnitOfWork unitOfWork;

        public ScMentorMappingService(IScMentorMappingRepository scMentorMappingRepository, IScCompInfoRepository scCompInfoRespository, IUnitOfWork unitOfWork)
        {
            this.scMentorMappingRepository = scMentorMappingRepository;
            this.scCompInfoRespository = scCompInfoRespository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<ScMentorMappiing>> GetMentorListAsync(int mngCompSn, int bizWorkSn = 0, string usrTypeDetail = null)
        {
            IEnumerable<ScMentorMappiing> listMentorTask = null;

            if (bizWorkSn == 0 && string.IsNullOrEmpty(usrTypeDetail))
            {
                listMentorTask = await scMentorMappingRepository.GetMentorMappingsAsync(mmp => mmp.Status == "N" && mmp.ScUsr.UsrType == "M" && mmp.MngCompSn == mngCompSn);
            }
            else if (bizWorkSn != 0 && string.IsNullOrEmpty(usrTypeDetail))
            {
                listMentorTask = await scMentorMappingRepository.GetMentorMappingsAsync(mmp => mmp.Status == "N" && mmp.ScUsr.UsrType == "M" && mmp.MngCompSn == mngCompSn && mmp.BizWorkSn == bizWorkSn);
            }
            else if (bizWorkSn == 0 && !string.IsNullOrEmpty(usrTypeDetail))
            {
                listMentorTask = await scMentorMappingRepository.GetMentorMappingsAsync(mmp => mmp.Status == "N" && mmp.ScUsr.UsrType == "M" && mmp.MngCompSn == mngCompSn && mmp.ScUsr.UsrTypeDetail == usrTypeDetail);
            }
            else
            {
                listMentorTask = await scMentorMappingRepository.GetMentorMappingsAsync(mmp => mmp.Status == "N" && mmp.ScUsr.UsrType == "M" && mmp.MngCompSn == mngCompSn && mmp.BizWorkSn == bizWorkSn && mmp.ScUsr.UsrTypeDetail == usrTypeDetail);
            }
            return listMentorTask.OrderByDescending(mmp => mmp.RegDt).ToList();
        }

        public async Task<ScMentorMappiing> GetMentor(int bizWorkSn, string mentorId)
        {
            var scMentorMapping = await scMentorMappingRepository.GetMentorMappingAsync(smm => smm.BizWorkSn == bizWorkSn && smm.MentorId == mentorId);

            return scMentorMapping;
        }



        public async Task<IList<ScMentorMappiing>> GetMentorMappingListByMentorId(string mentorId = null, int bizWorkYear = 0)
        {
            if (mentorId == null)
            {
                return await scMentorMappingRepository.GetMentorMappingsAsync(mmp => mmp.Status == "N");
            }
            else if (mentorId != null && bizWorkYear == 0)
            {
                DateTime date = DateTime.Now.Date;
                //return await scMentorMappingRepository.GetMentorMappingsAsync
                //    (
                //    mmp => mmp.MentorId == mentorId && mmp.Status == "N" 
                //    && mmp.ScBizWork.BizWorkEdDt.Value >= date
                //    );

                var listScMentorMapping =  await scMentorMappingRepository.GetMentorMappingsAsync(mmp => mmp.MentorId == mentorId && mmp.Status == "N");

                return listScMentorMapping.Where(mmp => mmp.ScBizWork.BizWorkEdDt.Value >= date).ToList();

            }
            else 
            {
                //return await scMentorMappingRepository.GetMentorMappingsAsync
                //    (
                //    mmp => mmp.MentorId == mentorId && mmp.Status == "N"
                //    && mmp.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear && mmp.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear
                //    );

                var listScMentorMapping = await scMentorMappingRepository.GetMentorMappingsAsync(mmp => mmp.MentorId == mentorId && mmp.Status == "N");

                return listScMentorMapping.Where(mmp => mmp.ScBizWork.BizWorkStDt.Value.Year <= bizWorkYear && mmp.ScBizWork.BizWorkEdDt.Value.Year >= bizWorkYear).ToList();
            }


        }

        public async Task<int> AddMentorAsync(ScCompInfo scCompInfo)
        {
            //var rstScUsr = scUsrRespository.Insert(scUsr);
            //scCompInfo.
            var rstScCompInfo = scCompInfoRespository.Insert(scCompInfo);

            if (rstScCompInfo == null)
            {
                return -1;
            }
            else
            {
                return await SaveDbContextAsync();
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
