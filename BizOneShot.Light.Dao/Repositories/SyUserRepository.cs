using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizOneShot.Light.Models.DareModels;
using BizOneShot.Light.Dao.Infrastructure;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface ISyUserRepository : IRepository<SHUSER_SyUser>
    {
        IList<SHUSER_SyUser> GetSyUserById(string loginId);
        int Insert(SHUSER_SyUser syUser);
    }


    public class SyUserRepository : DareRepositoryBase<SHUSER_SyUser>, ISyUserRepository
    {
        public SyUserRepository(IDareDbFactory dareDbFactory) : base(dareDbFactory) { }

        public IList<SHUSER_SyUser> GetSyUserById(string loginId)
        {
            var usrInfo = this.DareDbContext.SHUSER_SyUsers.Where(ci => ci.IdUser == loginId).ToList();
            return usrInfo;
        }

        public int Insert(SHUSER_SyUser syUser)
        {
            // 다래 DB가 운영이기 때문에 개발단계에서는 실제 저장안함. 운영 반영시 적용 필요
            string commandString = string.Format("INSERT INTO SHUSER.SY_USER (ID_USER, MEMB_BUSNPERS_NO, NM_USER, PWD, USR_GBN, USER_STATUS, INSERT_ID, INSERT_DT) VALUES('{0}','{1}','{2}',PWDENCRYPT('{3}'),'{4}','{5}', '{6}' CONVERT(VARCHAR, GETDATE(), 112))",
                                                      syUser.IdUser, syUser.MembBusnpersNo, syUser.NmUser, syUser.Pwd, syUser.UsrGbn, syUser.UserStatus, syUser.IdUser);

            return  this.DareDbContext.Database.ExecuteSqlCommand(commandString);
            //return this.DareDbContext.SHUSER_SyUsers.Add(syUser);
        }
    }
}
