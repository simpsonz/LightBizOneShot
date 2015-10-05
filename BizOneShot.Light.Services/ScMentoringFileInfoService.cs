﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using System.Threading.Tasks;

using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;

namespace BizOneShot.Light.Services
{
    public interface IScMentoringFileInfoService : IBaseService
    {
        Task<IList<ScMentoringFileInfo>> GetMentoringFileInfo(int reportSn);   
    }


    public class ScMentoringFileInfoService : IScMentoringFileInfoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IScMentoringFileInfoRepository scMentoringFileInfoRepository;

        public ScMentoringFileInfoService(IUnitOfWork unitOfWork, IScMentoringFileInfoRepository scMentoringFileInfoRepository)
        {
            this.unitOfWork = unitOfWork;
            this.scMentoringFileInfoRepository = scMentoringFileInfoRepository;
        }


        public async Task<IList<ScMentoringFileInfo>> GetMentoringFileInfo(int reportSn)
        {
            return await scMentoringFileInfoRepository.GetMentoringFileInfo(mtfi => mtfi.ReportSn == reportSn && mtfi.ScFileInfo.Status == "N");
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
