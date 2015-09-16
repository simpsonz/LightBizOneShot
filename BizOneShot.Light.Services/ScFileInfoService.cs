using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using System.Linq.Expressions;
using System;


namespace BizOneShot.Light.Services
{
    public interface IScFileInfoService : IBaseService
    {
        void ModifyFileInfo(ScFileInfo scFileInfo);
    }


    public class ScFileInfoService : IScFileInfoService
    {
        private readonly IScFileInfoRepository scFileInfoRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScFileInfoService(IScFileInfoRepository scFileInfoRepository, IUnitOfWork unitOfWork)
        {
            this.scFileInfoRepository = scFileInfoRepository;
            this.unitOfWork = unitOfWork;
        }

        public void ModifyFileInfo(ScFileInfo scFileInfo)
        {
            scFileInfoRepository.Update(scFileInfo);
        }

        #region SaveDbContext
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
