﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.WebConfiguration;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        WebDbContext dbContext;

        public WebDbContext Init()
        {
            return dbContext ?? (dbContext = new WebDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }

}
