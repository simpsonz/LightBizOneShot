using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IQuesCheckListRepository : IRepository<QuesCheckList>
    {
        Task<IList<QuesCheckList>> GetQuesCheckListsAsync(Expression<Func<QuesCheckList, bool>> where);
    }


    public class QuesCheckListRepository : RepositoryBase<QuesCheckList>, IQuesCheckListRepository
    {
        public QuesCheckListRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<QuesCheckList>> GetQuesCheckListsAsync(Expression<Func<QuesCheckList, bool>> where)
        {
            return await DbContext.QuesCheckLists.Where(where)
                //.AsNoTracking()
                .ToListAsync();
        }
    }
}