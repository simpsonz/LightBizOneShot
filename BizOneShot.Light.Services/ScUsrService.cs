using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;

namespace BizOneShot.Light.Services
{
    public interface IScUsrService : IBaseService
    {

        //IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);

        bool ChkLoginId(string loginId);
    }


    public class ScUsrService : IScUsrService
    {
        private readonly IScUsrRepository scUsrRespository;
        private readonly IUnitOfWork unitOfWork;

        private readonly ISyUserRepository syUserRespository;
        private readonly IDareUnitOfWork dareUnitOfWork;

        public ScUsrService(IScUsrRepository scUsrRespository, IUnitOfWork unitOfWork, ISyUserRepository syUserRespository, IDareUnitOfWork dareUnitOfWor)
        {
            this.scUsrRespository = scUsrRespository;
            this.unitOfWork = unitOfWork;
            this.syUserRespository = syUserRespository;
            this.dareUnitOfWork = dareUnitOfWor;
        }

        public bool ChkLoginId(string loginId)
        {
            if (scUsrRespository.GetScUsrById(loginId).Count() > 0)
                return false;

            if (syUserRespository.GetSyUserById(loginId).Count() > 0)
                return false;

            return true;
        }


        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }
    }
}
