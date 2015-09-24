﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IQuesCompInfoRepository : IRepository<QuesCompInfo>
    {
        Task<QuesCompInfo> GetQuesCompInfoAsync(Expression<Func<QuesCompInfo, bool>> where);
        QuesCompInfo Insert(QuesCompInfo quesCompInfo);
    }


    public class QuesCompInfoRepository : RepositoryBase<QuesCompInfo>, IQuesCompInfoRepository
    {
        public QuesCompInfoRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public async Task<QuesCompInfo> GetQuesCompInfoAsync(Expression<Func<QuesCompInfo, bool>> where)
        {
            return await this.DbContext.QuesCompInfoes.Where(where).SingleAsync();
        }

        public QuesCompInfo Insert(QuesCompInfo quesCompInfo)
        {
            return this.DbContext.QuesCompInfoes.Add(quesCompInfo);
        }

    }
}
