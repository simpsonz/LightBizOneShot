using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IRptCheckListRepository : IRepository<RptCheckList>
    {
        //Task<IList<RptCheckList>> GetRptCheckList(Expression<Func<RptCheckList, bool>> where);
    }


    public class RptCheckListRepository : RepositoryBase<RptCheckList>, IRptCheckListRepository
    {
        public RptCheckListRepository(IDbFactory dbFactory) : base(dbFactory) { }

     
        //public async Task<IList<RptCheckList>> GetRptCheckList(Expression<Func<RptCheckList, bool>> where)
        //{
        //    return await DbContext.RptCheckLists.Where(where).ToListAsync();
        //}

       
    }


}
