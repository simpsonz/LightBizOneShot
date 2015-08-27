using System.Collections.Generic;
using System.Linq;

using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;

using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace BizOneShot.Light.Services
{
    public interface IScFaqService : IBaseService
    {

        //IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);

        Task<IList<ScFaq>> GetFaqs(string searchType = null, string keyword = null);
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

        public async Task<IList<ScFaq>> GetFaqs(string searchType = null, string keyword = null)
        {
            IEnumerable<ScFaq> listScFaqTask = null;

            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(keyword))
            {
                listScFaqTask = await scFaqRespository.GetManyAsync(faq => faq.Stat == "N");
                return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToList();
            }
            else if (searchType.Equals("0")) // 질문, 답변중 keyword가 포함된 faq 검색 
            {
                listScFaqTask = await scFaqRespository.GetManyAsync(faq => faq.QstTxt.Contains(keyword) || faq.AnsTxt.Contains(keyword)& faq.Stat == "N");
                return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToList();
            }
            else if (searchType.Equals("1")) // 질문중에 keyword가 포함된 faq 검색 
            {
                listScFaqTask = await scFaqRespository.GetManyAsync(faq => faq.QstTxt.Contains(keyword) & faq.Stat == "N");
                return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToList();
            }
            else if (searchType.Equals("2")) // 답변중에 keyword가 포함된 faq 검색 
            {
                listScFaqTask = await scFaqRespository.GetManyAsync(faq => faq.AnsTxt.Contains(keyword) & faq.Stat == "N");
                return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToList();
            }

            listScFaqTask = await scFaqRespository.GetManyAsync(faq => faq.Stat == "N");
            return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToList();
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
