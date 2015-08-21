using System.Collections.Generic;
using System.Linq;

using BizOneShot.Light.Models;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using System.Linq.Expressions;
using System;

namespace BizOneShot.Light.Services
{
    public interface IScFaqService
    {

        IEnumerable<ScFaq> GetFaqs(string searchType = null, string keyword = null);
    }


    public class ScFaqService : IScFaqService
    {
        private readonly IScFaqRepository scFaqRespository;
        private readonly IUnitOfWork unitOfWork;

        public ScFaqService(IScFaqRepository scFaqRespository, IUnitOfWork unitOfWork)
        {
            this.scFaqRespository = scFaqRespository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ScFaq> GetFaqs(string searchType = null, string keyword = null)
        {
            var result = scFaqRespository.GetAll();

            if (searchType.Equals("0")) // 질문, 답변중 keyword가 포함된 faq 검색 
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

            return result;
        }

        public void SaveScFaq()
        {
            unitOfWork.Commit();
        }

        /// <summary>
        /// Pages the specified query.
        /// </summary>
        /// <typeparam name="T">Generic Type Object</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The Object query where paging needs to be applied.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderByProperty">The order by property.</param>
        /// <param name="isAscendingOrder">if set to <c>true</c> [is ascending order].</param>
        /// <param name="rowsCount">The total rows count.</param>
        /// <returns></returns>
        private static IQueryable<T> PagedResult<T, TResult>(IQueryable<T> query, int pageNum, int pageSize,
                        Expression<Func<T, TResult>> orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            if (pageSize <= 0) pageSize = 20;

            //Total result count
            rowsCount = query.Count();

            //If page number should be > 0 else set to first page
            if (rowsCount <= pageSize || pageNum <= 0) pageNum = 1;

            //Calculate nunber of rows to skip on pagesize
            int excludedRows = (pageNum - 1) * pageSize;

            query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);

            //Skip the required rows for the current page and take the next records of pagesize count
            return query.Skip(excludedRows).Take(pageSize);
        }
    }
}
