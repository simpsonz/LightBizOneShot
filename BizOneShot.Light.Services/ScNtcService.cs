using System.Collections.Generic;
using System.Linq;

using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;

using System.Linq.Expressions;
using System;

namespace BizOneShot.Light.Services
{
    public interface IScNtcService : IBaseService
    {
        IList<ScNtc> GetNotices(string searchType = null, string keyword = null);
    }


    public class ScNtcService : IScNtcService
    {
        private readonly IScNtcRepository scNtcRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScNtcService(IScNtcRepository scNtcRepository, IUnitOfWork unitOfWork)
        {
            this.scNtcRepository = scNtcRepository;
            this.unitOfWork = unitOfWork;
        }

        public IList<ScNtc> GetNotices(string searchType = null, string keyword = null)
        {
            
            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(keyword))
            {
                return scNtcRepository.GetAll().ToList();
            }
            else if (searchType.Equals("0")) // 제목, 내용중 keyword가 포함된 faq 검색 
            {
                return scNtcRepository.GetMany(ntc => ntc.Subject.Contains(keyword) || ntc.RmkTxt.Contains(keyword)).ToList();
            }
            else if (searchType.Equals("1")) // 제목중에 keyword가 포함된 faq 검색 
            {
                return scNtcRepository.GetMany(ntc => ntc.Subject.Contains(keyword)).ToList();
            }
            else if (searchType.Equals("2")) // 내용중에 keyword가 포함된 faq 검색 
            {
                return scNtcRepository.GetMany(ntc => ntc.RmkTxt.Contains(keyword)).ToList();
            }

            return scNtcRepository.GetAll().ToList();
        }

        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        
    }
}
