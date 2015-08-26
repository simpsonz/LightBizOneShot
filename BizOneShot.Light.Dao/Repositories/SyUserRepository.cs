using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BizOneShot.Light.Models.DareModels;
using BizOneShot.Light.Dao.Infrastructure;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface ISyUserRepository : IRepository<SHUSER_SyUser>
    {
        IList<SHUSER_SyUser> GetSyUserById(string loginId);
        SHUSER_SyUser Insert(SHUSER_SyUser syUser);
    }


    public class SyUserRepository : DareRepositoryBase<SHUSER_SyUser>, ISyUserRepository
    {
        public SyUserRepository(IDareDbFactory dareDbFactory) : base(dareDbFactory) { }

        public IList<SHUSER_SyUser> GetSyUserById(string loginId)
        {
            var usrInfo = this.DareDbContext.SHUSER_SyUsers.Where(ci => ci.IdUser == loginId).ToList();
            return usrInfo;
        }

        public SHUSER_SyUser Insert(SHUSER_SyUser syUser)
        {
            return this.DareDbContext.SHUSER_SyUsers.Add(syUser);
        }
    }
}
