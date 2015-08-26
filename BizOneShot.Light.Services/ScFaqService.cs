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
    public interface IScFaqService : IBaseService
    {

        //IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);

        IList<ScFaq> GetFaqs(string searchType = null, string keyword = null);
    }


    public class ScFaqService : IScFaqService
    {
        private readonly IScFaqRepository scFaqRespository;
        private readonly IUnitOfWork unitOfWork;

        public ScFaqService(IScFaqRepository scFaqRespository, IScQclRepository scQclRepository, IUnitOfWork unitOfWork)
        {
            this.scFaqRespository = scFaqRespository;
            this.unitOfWork = unitOfWork;
        }

        public IList<ScFaq> GetFaqs(string searchType = null, string keyword = null)
        {

            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(keyword))
            {
                return scFaqRespository.GetAll().ToList();
            }
            else if (searchType.Equals("0")) // 질문, 답변중 keyword가 포함된 faq 검색 
            {
                return scFaqRespository.GetMany(ci => ci.QstTxt.Contains(keyword) || ci.AnsTxt.Contains(keyword)).ToList();
            }
            else if (searchType.Equals("1")) // 질문중에 keyword가 포함된 faq 검색 
            {
                return scFaqRespository.GetMany(ci => ci.QstTxt.Contains(keyword)).ToList();
            }
            else if (searchType.Equals("2")) // 답변중에 keyword가 포함된 faq 검색 
            {
                return scFaqRespository.GetMany(ci => ci.AnsTxt.Contains(keyword)).ToList();
            }

            return scFaqRespository.GetAll().ToList();
        }


        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public void SaveDbContextAsync()
        {
            unitOfWork.CommitAsync();
        }
    }
}
