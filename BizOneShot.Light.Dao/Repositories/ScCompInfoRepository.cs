﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.WebConfiguration;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScCompInfoRepository : IRepository<ScCompInfo>
    {
        IList<ScCompInfo> GetScCompInfoByName(string compNm);
        ScCompInfo Insert(ScCompInfo compInfo);
    }


    public class ScCompInfoRepository : RepositoryBase<ScCompInfo>, IScCompInfoRepository
    {
        public ScCompInfoRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public IList<ScCompInfo> GetScCompInfoByName(string compNm)
        {
            var compInfos = this.DbContext.ScCompInfoes.Where(ci => ci.CompNm == compNm).ToList();

            return compInfos;
        }

        public override void Update(ScCompInfo compInfo)
        {
            compInfo.Email = "test@test.com";

            base.Update(compInfo);
        }

        public ScCompInfo Insert(ScCompInfo compInfo)
        {
            return this.DbContext.ScCompInfoes.Add(compInfo);
        }
    }
}
