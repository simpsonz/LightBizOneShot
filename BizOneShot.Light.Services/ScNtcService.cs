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
        IDictionary<string, ScNtc> GetNoticeById(int noticeSn);
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
                return scNtcRepository.GetAll().OrderByDescending(ntc => ntc.NoticeSn).ToList();
            }
            else if (searchType.Equals("0")) // 제목, 내용중 keyword가 포함된 Notice 검색 
            {
                return scNtcRepository.GetMany(ntc => ntc.Subject.Contains(keyword) || ntc.RmkTxt.Contains(keyword))
                    .Where(ntc=> ntc.Status == "N")
                    .OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }
            else if (searchType.Equals("1")) // 제목중에 keyword가 포함된 Notice 검색 
            {
                return scNtcRepository.GetMany(ntc => ntc.Subject.Contains(keyword))
                    .Where(ntc => ntc.Status == "N")
                    .OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }
            else if (searchType.Equals("2")) // 내용중에 keyword가 포함된 Notice 검색 
            {
                return scNtcRepository.GetMany(ntc => ntc.RmkTxt.Contains(keyword))
                    .Where(ntc => ntc.Status == "N")
                    .OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }

            return scNtcRepository.GetAll()
                .Where(ntc => ntc.Status == "N")
                .OrderByDescending(ntc => ntc.NoticeSn)
                .ToList();
        }

        public IDictionary<string,ScNtc> GetNoticeById(int noticeSn)
        {
            //return scNtcRepository.GetMany(ntc => ntc.NoticeSn >= noticeSn - 1 && ntc.NoticeSn <= noticeSn + 1).OrderByDescending(ntc => ntc.NoticeSn).ToList();
            var preNotice = scNtcRepository.GetMany(ntc => ntc.NoticeSn < noticeSn)
                .Where(ntc => ntc.Status == "N")
                .Max<ScNtc>();

            var curNotice = scNtcRepository.Get(ntc => ntc.NoticeSn == noticeSn);

            var nextNotice = scNtcRepository.GetMany(ntc => ntc.NoticeSn > noticeSn)
                .Where(ntc => ntc.Status == "N")
                .Min<ScNtc>();

            IDictionary<string, ScNtc> dicScNtcs = new Dictionary<string, ScNtc>();

            dicScNtcs.Add("preNotice", preNotice);
            dicScNtcs.Add("curNotice", curNotice);
            dicScNtcs.Add("nextNotice", nextNotice);

            return dicScNtcs;
        }

        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        
    }
}
