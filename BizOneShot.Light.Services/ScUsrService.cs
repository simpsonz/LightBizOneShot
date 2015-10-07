﻿using System;
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
        Task<ScUsr> SelectScUsr(string loginId);

        Task<ScUsr> SelectMentorInfo(string loginId);
        void ModifyScUsr(ScUsr scUsr);

        Task<int> AddCompanyUserAsync(ScCompInfo scCompInfo, ScUsr scUsr, SHUSER_SyUser syUser);
        Task<int> AddBizManagerAsync(ScCompInfo scCompInfo);
        Task<int> AddBizManagerMemberAsync(ScUsr scUsr);
        Task<IList<ScUsr>> GetBizManagerAsync();
        Task<IList<ScUsr>> GetBizManagerByComNameAsync(string keyword = null);
        Task<IList<ScUsr>> GetExpertManagerAsync();
        Task<IList<ScUsr>> GetExpertManagerAsync(string bizMngSn = null, string expertType = null);
        //Task<IList<ScUsr>> GetMentorListAsync(int mngCompSn);
        Task<ScUsr> SelectScUsr(string loginId, string registrationNo);

    }


    public class ScUsrService : IScUsrService
    {
        private readonly IScUsrRepository scUsrRespository;
        private readonly IScCompInfoRepository scCompInfoRespository;
        private readonly IScMentorMappingRepository scMentorMappingRepository;
        private readonly IUnitOfWork unitOfWork;

        private readonly ISyUserRepository syUserRespository;
        private readonly IDareUnitOfWork dareUnitOfWork;

        public ScUsrService(IScUsrRepository scUsrRespository, IUnitOfWork unitOfWork, ISyUserRepository syUserRespository, IDareUnitOfWork dareUnitOfWor, IScCompInfoRepository scCompInfoRespository, IScMentorMappingRepository scMentorMappingRepository)
        {
            this.scUsrRespository = scUsrRespository;
            this.unitOfWork = unitOfWork;
            this.syUserRespository = syUserRespository;
            this.dareUnitOfWork = dareUnitOfWor;
            this.scCompInfoRespository = scCompInfoRespository;
            this.scMentorMappingRepository = scMentorMappingRepository;
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


        public async Task<ScUsr> SelectScUsr(string loginId)
        {
            var scUsr = await scUsrRespository.GetAsync(sc => sc.LoginId == loginId && sc.Status == "N");

            return scUsr;
        }

        public async Task<ScUsr> SelectScUsr(string loginId, string registrationNo)
        {
            var scUsr = await scUsrRespository.GetAsync(sc => sc.LoginId == loginId && sc.Status == "N" && sc.ScCompInfo.RegistrationNo == registrationNo);

            return scUsr;
        }


        public async Task<ScUsr> SelectMentorInfo(string loginId)
        {
            var scUsr = await scUsrRespository.GetMentorInfoById(sc => sc.LoginId == loginId && sc.Status == "N");

            return scUsr;
        }

        public void ModifyScUsr(ScUsr scUsr)
        {
            scUsrRespository.Update(scUsr);
        }


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
                int rst =  await SaveDbContextAsync();
                if (rst != 1)
                    return rst;

                return await SaveDareDbContextAsync();
            }

        }

        public async Task<int> AddBizManagerMemberAsync(ScUsr scUsr)
        {
            var rstScUsr = scUsrRespository.Insert(scUsr);

            if (rstScUsr == null)
            {
                return -1;
            }
            else
            {
                return await SaveDbContextAsync();
            }

        }

        public async Task<int> AddBizManagerAsync(ScCompInfo scCompInfo)
        {
            //var rstScUsr = scUsrRespository.Insert(scUsr);
            //scCompInfo.
            var rstScCompInfo = scCompInfoRespository.Insert(scCompInfo);

            if (rstScCompInfo == null )
            {
                return -1;
            }
            else
            {
                return await SaveDbContextAsync();
            }

        }


        public async Task<IList<ScUsr>> GetBizManagerAsync()
        {
            IEnumerable<ScUsr> listScUsrTask = null;

            listScUsrTask = await scUsrRespository.GetManyAsync(usr => usr.Status == "N" && usr.UsrType == "B" && usr.UsrTypeDetail == "A");
            return listScUsrTask.OrderByDescending(usr => usr.RegDt).ToList();
        }


        public async Task<IList<ScUsr>> GetBizManagerByComNameAsync(string keyword = null)
        {
            IEnumerable<ScUsr> listScUsrTask = null;

            listScUsrTask = await scUsrRespository.GetManyAsync(usr => usr.Status == "N" && usr.UsrType == "B" && usr.UsrTypeDetail == "A" && usr.ScCompInfo.CompNm.Contains(keyword));
            return listScUsrTask.OrderByDescending(usr => usr.RegDt).ToList();

        }


        public async Task<IList<ScUsr>> GetExpertManagerAsync()
        {
            IEnumerable<ScUsr> listScUsrTask = null;

            listScUsrTask = await scUsrRespository.GetManyAsync(usr => usr.Status == "N" && usr.UsrType == "P");
            return listScUsrTask.OrderByDescending(usr => usr.RegDt).ToList();
        }

        
        public async Task<IList<ScUsr>> GetExpertManagerAsync(string bizMngSn = null, string expertType = null)
        {
            IEnumerable<ScUsr> listScUsrTask = null;


            if ((string.IsNullOrEmpty(bizMngSn) && string.IsNullOrEmpty(expertType)) || ((bizMngSn == "0") && string.IsNullOrEmpty(expertType)))
            {
                listScUsrTask = await scUsrRespository.GetManyAsync(usr => usr.Status == "N" && usr.UsrType == "P");
                return listScUsrTask.OrderByDescending(usr => usr.RegDt).ToList();
            }
            else if ((bizMngSn != "0") && string.IsNullOrEmpty(expertType))
            {
                listScUsrTask = await scUsrRespository.GetManyAsync(usr => usr.Status == "N" && usr.UsrType == "P" && usr.ScCompInfo.CompSn == int.Parse(bizMngSn));
                return listScUsrTask.OrderByDescending(usr => usr.RegDt).ToList();
            }
            else if ((bizMngSn == "0") && !string.IsNullOrEmpty(expertType))
            {
                listScUsrTask = await scUsrRespository.GetManyAsync(usr => usr.Status == "N" && usr.UsrType == "P" && usr.UsrTypeDetail == expertType);
                return listScUsrTask.OrderByDescending(usr => usr.RegDt).ToList();
            }
            else 
            {
                listScUsrTask = await scUsrRespository.GetManyAsync(usr => usr.Status == "N" && usr.UsrType == "P" && usr.UsrTypeDetail == expertType && usr.ScCompInfo.CompSn == int.Parse(bizMngSn));
                return listScUsrTask.OrderByDescending(usr => usr.RegDt).ToList();
            }


        }

        //public async Task<IList<ScUsr>> GetMentorListAsync(int mngCompSn)
        //{
        //    IEnumerable<ScMentorMappiing> listScUsrTask = null;

        //    listScUsrTask = await scMentorMappingRepository.GetManyAsync(mmp => mmp.Status == "N" && mmp.ScUsr.UsrType == "M" && mmp.ScCompInfo.CompSn == mngCompSn);
        //    return listScUsrTask.Select(mmp => mmp.ScUsr).OrderByDescending(usr => usr.RegDt).ToList();
        //}
        
        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await unitOfWork.CommitAsync();
        }

        public async Task<int> SaveDareDbContextAsync()
        {
            return await dareUnitOfWork.CommitAsync();
        }
    }
}
