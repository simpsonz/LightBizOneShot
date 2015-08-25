using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizOneShot.Light.Dao.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();

        void CommitAsync();
    }

    public interface IDareUnitOfWork
    {
        void Commit();

        void CommitAsync();
    }
}
