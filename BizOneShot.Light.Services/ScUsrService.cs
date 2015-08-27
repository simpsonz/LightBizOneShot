using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.DareModels;

namespace BizOneShot.Light.Services
{
    public interface IScUsrService : IBaseService
    {

        //IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);

        Task<bool> ChkLoginId(string loginId);
        //bool AddCompanyUser(ScCompInfo scCompInfo, ScUsr scUsr, SHUSER_SyUser syUser);

        Task<int> AddCompanyUserAsync(ScCompInfo scCompInfo, ScUsr scUsr, SHUSER_SyUser syUser);
    }


    public class ScUsrService : IScUsrService
    {
        private readonly IScUsrRepository scUsrRespository;
        private readonly IScCompInfoRepository scCompInfoRespository;
        private readonly IUnitOfWork unitOfWork;

        private readonly ISyUserRepository syUserRespository;
        private readonly IDareUnitOfWork dareUnitOfWork;

        public ScUsrService(IScUsrRepository scUsrRespository, IUnitOfWork unitOfWork, ISyUserRepository syUserRespository, IDareUnitOfWork dareUnitOfWor, IScCompInfoRepository scCompInfoRespository)
        {
            this.scUsrRespository = scUsrRespository;
            this.unitOfWork = unitOfWork;
            this.syUserRespository = syUserRespository;
            this.dareUnitOfWork = dareUnitOfWor;
            this.scCompInfoRespository = scCompInfoRespository;
        }

        public async Task<bool> ChkLoginId(string loginId)
        {
            IEnumerable<ScUsr> listScUsrTask = null;
            listScUsrTask = await scUsrRespository.GetManyAsync(usr => usr.LoginId == loginId);

            if (listScUsrTask.Count() > 0)
                return false;

            IEnumerable<SHUSER_SyUser> listSyUserTask = null;
            listSyUserTask = await syUserRespository.GetManyAsync(usr => usr.IdUser == loginId);

            if (listSyUserTask.Count() > 0)
                return false;

            return true;
        }

        //public  bool AddCompanyUser(ScCompInfo scCompInfo, ScUsr scUsr, SHUSER_SyUser syUser)
        //{
        //    //var rstScUsr = scUsrRespository.Insert(scUsr);
        //    //scCompInfo.
        //    var rstScCompInfo = scCompInfoRespository.Insert(scCompInfo);
        //    var rstSyUser = syUserRespository.Insert(syUser);

        //    if (rstScCompInfo == null || rstSyUser != 1)
        //    { 
        //        return false;
        //    }
        //    else
        //    {
        //        await SaveDbContextAsync();
        //        return true;
        //    }

        //}


        public async Task<int> AddCompanyUserAsync(ScCompInfo scCompInfo, ScUsr scUsr, SHUSER_SyUser syUser)
        {
            //var rstScUsr = scUsrRespository.Insert(scUsr);
            //scCompInfo.
            var rstScCompInfo = scCompInfoRespository.Insert(scCompInfo);
            var rstSyUser = syUserRespository.Insert(syUser);

            if (rstScCompInfo == null || rstSyUser != 1)
            {
                return -1;
            }
            else
            {
                return await SaveDbContextAsync();
            }

        }




        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await unitOfWork.CommitAsync();
        }
    }
}
