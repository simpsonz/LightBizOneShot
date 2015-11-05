using System.Threading.Tasks;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();

        Task<int> CommitAsync();
    }

    public interface IDareUnitOfWork
    {
        void Commit();

        Task<int> CommitAsync();
    }
}