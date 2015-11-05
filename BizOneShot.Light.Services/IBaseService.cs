using System.Threading.Tasks;

namespace BizOneShot.Light.Services
{
    public interface IBaseService
    {
        void SaveDbContext();

        Task<int> SaveDbContextAsync();
    }
}