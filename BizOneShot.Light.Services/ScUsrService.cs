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

        bool ChkLoginId(string loginId);
        bool AddCompanyUser(ScCompInfo scCompInfo, ScUsr scUsr, SHUSER_SyUser syUser);
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

        public bool ChkLoginId(string loginId)
        {
            if (scUsrRespository.GetScUsrById(loginId).Count() > 0)
                return false;

            if (syUserRespository.GetSyUserById(loginId).Count() > 0)
                return false;

            return true;
        }

        public bool AddCompanyUser(ScCompInfo scCompInfo, ScUsr scUsr, SHUSER_SyUser syUser)
        {
            //var rstScUsr = scUsrRespository.Insert(scUsr);
            //scCompInfo.
            var rstScCompInfo = scCompInfoRespository.Insert(scCompInfo);
            var rstSyUser = syUserRespository.Insert(syUser);

            if (rstScCompInfo == null || rstSyUser == null)
            { 
                return false;
            }
            else
            {
                SaveDbContext();
                return true;
            }
            
        }




        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }
    }
}
