using System.Collections.Generic;
using System.Linq;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.DareModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface ISyUserRepository : IRepository<SHUSER_SyUser>
    {
        IList<SHUSER_SyUser> GetSyUserById(string loginId);
        int Insert(SHUSER_SyUser syUser);
        int UpdatePassword(SHUSER_SyUser syUser);
    }


    public class SyUserRepository : DareRepositoryBase<SHUSER_SyUser>, ISyUserRepository
    {
        public SyUserRepository(IDareDbFactory dareDbFactory) : base(dareDbFactory)
        {
        }

        public IList<SHUSER_SyUser> GetSyUserById(string loginId)
        {
            var usrInfo = DareDbContext.SHUSER_SyUsers.Where(ci => ci.IdUser == loginId).ToList();
            return usrInfo;
        }

        public int Insert(SHUSER_SyUser syUser)
        {
            // 다래 DB가 운영이기 때문에 개발단계에서는 실제 저장안함. 운영 반영시 적용 필요
            // WEB_USR_GBN : 무료 사용자와 유료사용자 구분값 '1' or '' - 유료, '3' - 무료
            // USR_GBN : 기업사용자, 세무회계사 구분값 '1' - 기업사용자, '2' - 세무회계사
            // 스타트업 버젼에서는 유료사용자, 세무회계사는 없음.
            var commandString =
                string.Format(
                    "INSERT INTO SHUSER.SY_USER (ID_USER, MEMB_BUSNPERS_NO, NM_USER, PWD, USR_GBN, USER_STATUS, INSERT_ID, INSERT_DT, WEB_USR_GBN) VALUES('{0}','{1}','{2}',PWDENCRYPT('{3}'),'{4}','{5}', '{6}', CONVERT(VARCHAR, GETDATE(), 112), '{7}')",
                    syUser.IdUser, syUser.MembBusnpersNo, syUser.NmUser, syUser.SmartPwd, syUser.UsrGbn,
                    syUser.UserStatus, syUser.IdUser, syUser.WebUsrGbn);

            return DareDbContext.Database.ExecuteSqlCommand(commandString);
            //return this.DareDbContext.SHUSER_SyUsers.Add(syUser);
        }


        public int UpdatePassword(SHUSER_SyUser syUser)
        {
            // 다래 DB가 운영이기 때문에 개발단계에서는 실제 저장안함. 운영 반영시 적용 필요
            // WEB_USR_GBN : 무료 사용자와 유료사용자 구분값 '1' or '' - 유료, '3' - 무료
            // USR_GBN : 기업사용자, 세무회계사 구분값 '1' - 기업사용자, '2' - 세무회계사
            // 스타트업 버젼에서는 유료사용자, 세무회계사는 없음.
            var commandString =
                string.Format(
                    "UPDATE SHUSER.SY_USER SET PWD = PWDENCRYPT('{0}'), MODIFY_ID = '{1}', MODIFY_DT = CONVERT(VARCHAR, GETDATE(), 112) WHERE ID_USER = '{1}' AND MEMB_BUSNPERS_NO = '{2}'",
                    syUser.SmartPwd, syUser.IdUser, syUser.MembBusnpersNo);

            return DareDbContext.Database.ExecuteSqlCommand(commandString);
            //return this.DareDbContext.SHUSER_SyUsers.Add(syUser);
        }
    }
}