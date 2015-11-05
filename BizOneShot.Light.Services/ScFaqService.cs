using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using PagedList;

namespace BizOneShot.Light.Services
{
    public interface IScFaqService : IBaseService
    {
        //IEnumerable<FaqViewModel> GetFaqs(string searchType = null, string keyword = null);

        Task<IPagedList<ScFaq>> GetFaqsAsync(int page, int pageSize, string searchType = null, string keyword = null);

        Task<IPagedList<ScFaq>> GetPagedListFaqsAsync(int page, int pageSize, string searchType = null,
            string keyword = null);

        Task<ScFaq> GetFaqAsync(int faqSn);
        Task<int> AddFaqAsync(ScFaq scFaq);
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


        public async Task<IPagedList<ScFaq>> GetFaqsAsync(int page, int pageSize, string searchType = null,
            string keyword = null)
        {
            IEnumerable<ScFaq> listScFaqTask = null;

            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(keyword))
            {
                listScFaqTask = await scFaqRespository.GetManyAsync(faq => faq.Stat == "N");
                return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToPagedList(page, pageSize);
            }
            if (searchType.Equals("0")) // 질문, 답변중 keyword가 포함된 faq 검색 
            {
                listScFaqTask =
                    await
                        scFaqRespository.GetManyAsync(
                            faq => faq.QstTxt.Contains(keyword) || faq.AnsTxt.Contains(keyword) & faq.Stat == "N");
                return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToPagedList(page, pageSize);
            }
            if (searchType.Equals("1")) // 질문중에 keyword가 포함된 faq 검색 
            {
                listScFaqTask =
                    await scFaqRespository.GetManyAsync(faq => faq.QstTxt.Contains(keyword) & faq.Stat == "N");
                return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToPagedList(page, pageSize);
            }
            if (searchType.Equals("2")) // 답변중에 keyword가 포함된 faq 검색 
            {
                listScFaqTask =
                    await scFaqRespository.GetManyAsync(faq => faq.AnsTxt.Contains(keyword) & faq.Stat == "N");
                return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToPagedList(page, pageSize);
            }

            listScFaqTask = await scFaqRespository.GetManyAsync(faq => faq.Stat == "N");
            return listScFaqTask.OrderByDescending(faq => faq.FaqSn).ToPagedList(page, pageSize);
        }

        public async Task<IPagedList<ScFaq>> GetPagedListFaqsAsync(int page, int pageSize, string searchType = null,
            string keyword = null)
        {
            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(keyword))
            {
                return await scFaqRespository.GetPagedListAsync(faq => faq.Stat == "N", page, pageSize);
            }
            if (searchType.Equals("0")) // 질문, 답변중 keyword가 포함된 faq 검색 
            {
                return
                    await
                        scFaqRespository.GetPagedListAsync(
                            faq => faq.QstTxt.Contains(keyword) || faq.AnsTxt.Contains(keyword) & faq.Stat == "N", page,
                            pageSize);
            }
            if (searchType.Equals("1")) // 질문중에 keyword가 포함된 faq 검색 
            {
                return
                    await
                        scFaqRespository.GetPagedListAsync(faq => faq.QstTxt.Contains(keyword) & faq.Stat == "N", page,
                            pageSize);
            }
            if (searchType.Equals("2")) // 답변중에 keyword가 포함된 faq 검색 
            {
                return
                    await
                        scFaqRespository.GetPagedListAsync(faq => faq.AnsTxt.Contains(keyword) & faq.Stat == "N", page,
                            pageSize);
            }

            return await scFaqRespository.GetPagedListAsync(faq => faq.Stat == "N", page, pageSize);
        }


        public async Task<ScFaq> GetFaqAsync(int faqSn)
        {
            return await scFaqRespository.GetAsync(faq => faq.Stat == "N" && faq.FaqSn == faqSn);
        }

        public async Task<int> AddFaqAsync(ScFaq scFaq)
        {
            var rstScFaq = await scFaqRespository.Insert(scFaq);

            if (rstScFaq == null)
            {
                return -1;
            }
            return await SaveDbContextAsync();
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