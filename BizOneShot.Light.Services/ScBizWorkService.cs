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

    public interface IScBizWorkService : IBaseService
    {

        //IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);
        Task<IList<ScBizWork>> GetBizWorkList(int comSn);
    }


    public class ScBizWorkService : IScBizWorkService
    {
        private readonly IScBizWorkRepository scBizWorkRespository;
        private readonly IUnitOfWork unitOfWork;

        public ScBizWorkService(IScBizWorkRepository scBizWorkRespository, IUnitOfWork unitOfWork)
        {
            this.scBizWorkRespository = scBizWorkRespository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<ScBizWork>> GetBizWorkList(int comSn)
        {
            var scBizWorks = await scBizWorkRespository.GetManyAsync(bw => bw.CompSn == comSn);
            return scBizWorks.OrderByDescending(bw => bw.BizWorkSn).ToList();
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
