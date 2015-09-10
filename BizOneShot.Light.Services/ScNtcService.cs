using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        Task<IList<ScNtc>> GetNoticesAsync(string searchType = null, string keyword = null);
        IDictionary<string, ScNtc> GetNoticeDetailById(int noticeSn);
        Task<IDictionary<string, ScNtc>> GetNoticeDetailByIdAsync(int noticeSn);
        Task<IList<ScNtc>> GetNoticesForMainAsync();
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
                return scNtcRepository.GetMany(ntc => ntc.Status == "N")
                    .OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }
            else if (searchType.Equals("0")) // 제목, 내용중 keyword가 포함된 Notice 검색 
            {
                return scNtcRepository.GetMany(ntc => ntc.Subject.Contains(keyword) || ntc.RmkTxt.Contains(keyword) && ntc.Status == "N")
                    .OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }
            else if (searchType.Equals("1")) // 제목중에 keyword가 포함된 Notice 검색 
            {
                return scNtcRepository.GetMany(ntc => ntc.Subject.Contains(keyword) && ntc.Status == "N")
                    .OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }
            else if (searchType.Equals("2")) // 내용중에 keyword가 포함된 Notice 검색 
            {
                return scNtcRepository.GetMany(ntc => ntc.RmkTxt.Contains(keyword) && ntc.Status == "N")
                    .OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }

            return scNtcRepository.GetMany(ntc => ntc.Status == "N")
                .OrderByDescending(ntc => ntc.NoticeSn)
                .ToList();
        }
        //Async
        public async Task<IList<ScNtc>> GetNoticesAsync(string searchType = null, string keyword = null)
        {
            IEnumerable<ScNtc> listScNtcTask = null;
            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(keyword))
            {
                listScNtcTask = await scNtcRepository.GetManyAsync(ntc => ntc.Status == "N");
                return listScNtcTask.OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }
            else if (searchType.Equals("0")) // 제목, 내용중 keyword가 포함된 Notice 검색 
            {
                listScNtcTask = await scNtcRepository.GetManyAsync(ntc => ntc.Subject.Contains(keyword) || ntc.RmkTxt.Contains(keyword) && ntc.Status == "N");
                return listScNtcTask.OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }
            else if (searchType.Equals("1")) // 제목중에 keyword가 포함된 Notice 검색 
            {
                listScNtcTask = await scNtcRepository.GetManyAsync(ntc => ntc.Subject.Contains(keyword) && ntc.Status == "N");
                return listScNtcTask.OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }
            else if (searchType.Equals("2")) // 내용중에 keyword가 포함된 Notice 검색 
            {
                listScNtcTask = await scNtcRepository.GetManyAsync(ntc => ntc.RmkTxt.Contains(keyword) && ntc.Status == "N");
                return listScNtcTask.OrderByDescending(ntc => ntc.NoticeSn)
                    .ToList();
            }

            listScNtcTask = await scNtcRepository.GetManyAsync(ntc => ntc.Status == "N");
            return listScNtcTask.OrderByDescending(ntc => ntc.NoticeSn)
                .ToList();
        }

        public async Task<IList<ScNtc>> GetNoticesForMainAsync()
        {
            var listScNtcTask = await scNtcRepository.GetManyAsync(ntc => ntc.Status == "N");
            return listScNtcTask.OrderByDescending(ntc => ntc.NoticeSn).Take(5)
                .ToList();
        }

        public IDictionary<string, ScNtc> GetNoticeDetailById(int noticeSn)
        {
            var preNotice = scNtcRepository.GetMany(ntc => ntc.NoticeSn < noticeSn && ntc.Status == "N")
                .OrderBy(ntc => ntc.NoticeSn)
                .LastOrDefault();

            var curNotice = scNtcRepository.Get(ntc => ntc.NoticeSn == noticeSn);

            var nextNotice = scNtcRepository.GetMany(ntc => ntc.NoticeSn > noticeSn && ntc.Status == "N")
                .OrderBy(ntc => ntc.NoticeSn)
                .FirstOrDefault();

            var dicScNtcs = new Dictionary<string, ScNtc>();

            dicScNtcs.Add("preNotice", preNotice);
            dicScNtcs.Add("curNotice", curNotice);
            dicScNtcs.Add("nextNotice", nextNotice);


            return dicScNtcs;
        }

        //Async
        public async Task<IDictionary<string, ScNtc>> GetNoticeDetailByIdAsync(int noticeSn)
        {

            var preNoticeTask = await scNtcRepository.GetManyAsync(ntc => ntc.NoticeSn < noticeSn && ntc.Status == "N");
            var preNotice = preNoticeTask.OrderBy(ntc => ntc.NoticeSn).LastOrDefault();


            var curNotice = await scNtcRepository.GetAsync(ntc => ntc.NoticeSn == noticeSn);

            var nextNoticeTask = await scNtcRepository.GetManyAsync(ntc => ntc.NoticeSn > noticeSn && ntc.Status == "N");
            var nextNotice = nextNoticeTask.OrderBy(ntc => ntc.NoticeSn).FirstOrDefault();

            var dicScNtcs = new Dictionary<string, ScNtc>();

            dicScNtcs.Add("preNotice", preNotice);
            dicScNtcs.Add("curNotice", curNotice);
            dicScNtcs.Add("nextNotice", nextNotice);


            return  dicScNtcs;
        }

        #region SaveDbContext
        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await unitOfWork.CommitAsync();
        }
        #endregion

    }
}
