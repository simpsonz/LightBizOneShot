﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{

    public interface IRptMngCodeService : IBaseService
    {

        Task<IEnumerable<RptMngCode>> GetRptMngCodeBySmallClassCd(string smallClassCd);
    }


    public class RptMngCodeService : IRptMngCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRptMngCodeRepository _rptMngCodeRepository;
        

        public RptMngCodeService(IUnitOfWork unitOfWork, IRptMngCodeRepository rptMngCodeRepository)
        {
            this._unitOfWork = unitOfWork;
            this._rptMngCodeRepository = rptMngCodeRepository;
            
        }


        public async Task<IEnumerable<RptMngCode>> GetRptMngCodeBySmallClassCd(string smallClassCd)
        {
            var rptCheckList = await _rptMngCodeRepository.GetManyAsync(cl => cl.SmallClassCd == smallClassCd);
            return rptCheckList.OrderBy(cl => cl.DetailCd);
        }


        public void SaveDbContext()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await _unitOfWork.CommitAsync();
        }
    }
}
