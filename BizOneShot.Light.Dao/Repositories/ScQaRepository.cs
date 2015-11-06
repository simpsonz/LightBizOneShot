using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using PagedList;
using PagedList.EntityFramework;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IScQaRepository : IRepository<ScQa>
    {
        Task<IPagedList<ScQa>> GetpagedListQAsAsync(int page, int pageSize, string questionId, string expertType,
            DateTime? startDate = null, DateTime? endDate = null);

        Task<IList<ScQa>> GetQAsAsync(Expression<Func<ScQa, bool>> where);
        Task<ScQa> GetQAAsync(Expression<Func<ScQa, bool>> where);
        ScQa Insert(ScQa scQa);
    }


    public class ScQaRepository : RepositoryBase<ScQa>, IScQaRepository
    {
        public ScQaRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IPagedList<ScQa>> GetpagedListQAsAsync(int page, int pageSize, string questionId,
            string expertType, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await DbContext.ScQas
                .Include("ScUsr_QuestionId")
                .Include("ScUsr_QuestionId.ScCompInfo")
                .Include("ScUsr_AnswerId")
                .Include("ScUsr_AnswerId.ScCompInfo")
                .Where(
                    sq =>
                        sq.QuestionId == questionId && sq.ScUsr_AnswerId.UsrType == "P" &&
                        sq.ScUsr_AnswerId.UsrTypeDetail == expertType)
                .Where(sq => sq.AskDt >= startDate && sq.AskDt <= endDate)
                .OrderByDescending(sq => sq.AskDt).ToPagedListAsync(page, pageSize);
        }

        public async Task<IList<ScQa>> GetQAsAsync(Expression<Func<ScQa, bool>> where)
        {
            return
                await
                    DbContext.ScQas.Include("ScUsr_QuestionId")
                        .Include("ScUsr_QuestionId.ScCompInfo")
                        .Include("ScUsr_AnswerId")
                        .Include("ScUsr_AnswerId.ScCompInfo")
                        .Where(where)
                        .ToListAsync();
        }

        public async Task<ScQa> GetQAAsync(Expression<Func<ScQa, bool>> where)
        {
            return
                await
                    DbContext.ScQas.Include("ScUsr_QuestionId")
                        .Include("ScUsr_QuestionId.ScCompInfo")
                        .Include("ScUsr_AnswerId")
                        .Include("ScUsr_AnswerId.ScCompInfo")
                        .Where(where)
                        .SingleAsync();
        }

        public ScQa Insert(ScQa scQa)
        {
            return DbContext.ScQas.Add(scQa);
        }
    }
}