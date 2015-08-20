﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Configuration;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public interface IDbFactory
    {
        WebDbContext Init();
    }
}