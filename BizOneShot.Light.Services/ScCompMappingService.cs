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
        //Task<IList<ScCompMapping>> GetCompMappingsAsync(string mentorId = null, string status = null);
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


        //public async Task<IList<ScCompMapping>> GetCompMappingsAsync(string mentorId = null, string status = null)
        //{
        //}

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
