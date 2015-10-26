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

    public interface IScBizWorkService : IBaseService
    {

        //IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);
        //Task<IList<ScBizWork>> GetBizWorkList(int comSn, string excutorId = null);
        Task<IList<ScBizWork>> GetBizWorkList(int mngComSn, string excutorId = null, int bizWorkYear = 0);
        Task<IList<ScBizWork>> GetEndBizWorkList(DateTime endDateTiem);
        Task<IList<ScBizWork>> GetBizWorkListByBizWorkNm(int comSn, string query);
        Task<ScBizWork> GetBizWorkByBizWorkSn(int bizWorkSn);
        ScBizWork Insert(ScBizWork scBizWork);
        Task<IList<ScCompInfo>> GetBizWorkComList(int bizWorkSn);
        Task<int> AddBizWorkAsync(ScBizWork scBizWork);
        Task<ScBizWork> GetBizWorkByloginId(string loginId);
    }


    public class ScBizWorkService : IScBizWorkService
    {
        private readonly IScBizWorkRepository scBizWorkRespository;
        private readonly IScCompMappingRepository scCompMappingRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScBizWorkService(IScBizWorkRepository scBizWorkRespository, IScCompMappingRepository scCompMappingRepository, IUnitOfWork unitOfWork)
        {
            this.scBizWorkRespository = scBizWorkRespository;
            this.scCompMappingRepository = scCompMappingRepository;
            this.unitOfWork = unitOfWork;
        }

        //public async Task<IList<ScBizWork>> GetBizWorkList(int mngComSn, string excutorId = null)
        //{
        //    if(string.IsNullOrEmpty(excutorId))
        //    { 
        //        var scBizWorks = await scBizWorkRespository.GetBizWorksAsync(bw => bw.MngCompSn == mngComSn && bw.Status =="N");
        //        return scBizWorks.OrderByDescending(bw => bw.BizWorkSn).ToList();
        //    }
        //    else
        //    {
        //        var scBizWorks = await scBizWorkRespository.GetBizWorksAsync(bw => bw.MngCompSn == mngComSn && bw.Status == "N" && bw.ExecutorId == excutorId);
        //        return scBizWorks.OrderByDescending(bw => bw.BizWorkSn).ToList();
        //    }
        //}

        public async Task<IList<ScBizWork>> GetBizWorkList(int mngComSn, string excutorId = null, int bizWorkYear = 0)
        {
            if (string.IsNullOrEmpty(excutorId))
            {
                var scBizWorks = await scBizWorkRespository.GetBizWorksAsync(bw => bw.MngCompSn == mngComSn && bw.Status == "N");
                scBizWorks.Where(bw => bizWorkYear == 0 ? bw.BizWorkStDt.Value.Year > 0 : bw.BizWorkStDt.Value.Year <= bizWorkYear && bw.BizWorkEdDt.Value.Year >= bizWorkYear);

                return scBizWorks.OrderByDescending(bw => bw.BizWorkSn).ToList();
            }
            else
            {
                var scBizWorks = await scBizWorkRespository.GetBizWorksAsync(bw => bw.MngCompSn == mngComSn && bw.Status == "N" && bw.ExecutorId == excutorId);
                scBizWorks.Where(bw => bizWorkYear == 0 ? bw.BizWorkStDt.Value.Year > 0 : bw.BizWorkStDt.Value.Year <= bizWorkYear && bw.BizWorkEdDt.Value.Year >= bizWorkYear);

                return scBizWorks.OrderByDescending(bw => bw.BizWorkSn).ToList();
            }
        }

        public async Task<IList<ScBizWork>> GetEndBizWorkList(DateTime endDateTiem)
        {
            var scBizWorks = await scBizWorkRespository.GetBizWorksAsync(bw => bw.BizWorkEdDt < endDateTiem && bw.Status == "N");
            return scBizWorks.OrderByDescending(bw => bw.BizWorkSn).ToList();
        }

        public async Task<IList<ScCompInfo>> GetBizWorkComList(int bizWorkSn)
        {
            var scBizWorks = await scCompMappingRepository.GetCompanysAsync(bw => bw.BizWorkSn == bizWorkSn);
            return  scBizWorks.OrderByDescending(sc => sc.CompNm).ToList();
        }

        public async Task<IList<ScBizWork>> GetBizWorkListByBizWorkNm(int mngComSn, string query)
        {
            var scBizWorks = await scBizWorkRespository.GetBizWorksAsync(bw => bw.MngCompSn == mngComSn && bw.BizWorkNm.Contains(query));
            return scBizWorks.OrderByDescending(bw => bw.BizWorkSn).ToList();
        }

        public async Task<ScBizWork> GetBizWorkByBizWorkSn(int bizWorkSn)
        {
            var scBizWork = await scBizWorkRespository.GetBizWorkAsync(bw => bw.BizWorkSn == bizWorkSn);

            return scBizWork;
        }

        public async Task<ScBizWork> GetBizWorkByloginId(string loginId)
        {
            var scBizWork = await scBizWorkRespository.GetBizWorkByLoginIdAsync(bw => bw.ExecutorId == loginId);

            return scBizWork;
        }

        public ScBizWork Insert(ScBizWork scBizWork)
        {
            return scBizWorkRespository.Insert(scBizWork);
        }


        public async Task<int> AddBizWorkAsync(ScBizWork scBizWork)
        {
            var rstScUsr = scBizWorkRespository.Insert(scBizWork);

            if (rstScUsr == null)
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
