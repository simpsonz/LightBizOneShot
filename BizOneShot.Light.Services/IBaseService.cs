using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BizOneShot.Light.Services
{
    public interface IBaseService
    {
        void SaveDbContext();

        Task<int> SaveDbContextAsync();
    }
}
