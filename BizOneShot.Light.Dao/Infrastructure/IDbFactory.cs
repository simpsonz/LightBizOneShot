using BizOneShot.Light.Dao.DareConfiguration;
using BizOneShot.Light.Dao.WebConfiguration;

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