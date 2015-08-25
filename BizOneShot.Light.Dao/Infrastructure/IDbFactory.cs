using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.WebConfiguration;
using BizOneShot.Light.Dao.DareConfiguration;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public interface IDbFactory
    {
        WebDbContext Init();
    }

    public interface IDareDbFactory
    {
        DareDbContext Init();
    }
}
