using System.Collections.Generic;
using System.Linq;

using BizOneShot.Light.Models;
using BizOneShot.Light.ViewModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;

using System.Linq.Expressions;
using System;

namespace BizOneShot.Light.Services
{
    public interface IScFaqService : IBaseService
    {

        //IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);

        IList<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);
    }


    public class ScFaqService : IScFaqService
    {
        private readonly IScFaqRepository scFaqRespository;
        private readonly IScQclRepository scQclRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScFaqService(IScFaqRepository scFaqRespository, IScQclRepository scQclRepository, IUnitOfWork unitOfWork)
        {
            this.scFaqRespository = scFaqRespository;
            this.scQclRepository = scQclRepository;
            this.unitOfWork = unitOfWork;
        }

        public IList<FaqViewModel> GetFaqs(string searchType = null, string keyword = null)
        {
            var result = from tFaq in scFaqRespository.GetAll()
                         join tQcl in scQclRepository.GetAll() on tFaq.QclSn equals tQcl.QclSn
                         where tFaq.Stat == "N"
                         select new FaqViewModel
                         {
                             FaqSn = tFaq.FaqSn,
                             QclSn = tFaq.QclSn,
                             QstTxt = tFaq.QstTxt,
                             AnsTxt = tFaq.AnsTxt,
                             Stat = tFaq.Stat,
                             RegId = tFaq.RegId,
                             RegDt = tFaq.RegDt,
                             UpdId = tFaq.UpdId,
                             UpdDt = tFaq.UpdDt,
                             QclNm = tQcl.QclNm
                         };



            //var result = scFaqRespository.GetAll();

            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(keyword))
            {
                return result.ToList();
            }
            else if (searchType.Equals("0")) // 질문, 답변중 keyword가 포함된 faq 검색 
            {
                result = result.Where(ci => ci.QstTxt.Contains(keyword) || ci.AnsTxt.Contains(keyword));
            }
            else if (searchType.Equals("1")) // 질문중에 keyword가 포함된 faq 검색 
            {
                result = result.Where(ci => ci.QstTxt.Contains(keyword));
            }
            else if (searchType.Equals("2")) // 답변중에 keyword가 포함된 faq 검색 
            {
                result = result.Where(ci => ci.AnsTxt.Contains(keyword));
            }

            return result.ToList();
        }

        //public IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null)
        //{
        //    var result = from tFaq in scFaqRespository.GetAll()
        //                 join tQcl in scQclRepository.GetAll() on tFaq.QclSn equals tQcl.QclSn
        //                 where tFaq.Stat == "N"
        //                 select new FaqViewModel
        //                 {
        //                     FaqSn = tFaq.FaqSn,
        //                     QclSn = tFaq.QclSn,
        //                     QstTxt = tFaq.QstTxt,
        //                     AnsTxt = tFaq.AnsTxt,
        //                     Stat = tFaq.Stat,
        //                     RegId = tFaq.RegId,
        //                     RegDt = tFaq.RegDt,
        //                     UpdId = tFaq.UpdId,
        //                     UpdDt = tFaq.UpdDt,
        //                     QclNm = tQcl.QclNm
        //                 };



        //    //var result = scFaqRespository.GetAll();

        //    if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(keyword))
        //    {
        //        return result;
        //    }
        //    else if (searchType.Equals("0")) // 질문, 답변중 keyword가 포함된 faq 검색 
        //    {
        //        result = result.Where(ci => ci.QstTxt.Contains(keyword) || ci.AnsTxt.Contains(keyword));
        //    }
        //    else if (searchType.Equals("1")) // 질문중에 keyword가 포함된 faq 검색 
        //    {
        //        result = result.Where(ci => ci.QstTxt.Contains(keyword));
        //    }
        //    else if (searchType.Equals("2")) // 답변중에 keyword가 포함된 faq 검색 
        //    {
        //        result = result.Where(ci => ci.AnsTxt.Contains(keyword));
        //    }

        //    return result;
        //}


        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }
    }
}
