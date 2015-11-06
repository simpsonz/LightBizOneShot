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
    public interface IScUsrRepository : IRepository<ScUsr>
    {
        Task<IList<ScUsr>> GetScUsrById(string loginId);
        ScUsr Insert(ScUsr scUsr);
        Task<ScUsr> GetMentorInfoById(Expression<Func<ScUsr, bool>> where);
    }


    public class ScUsrRepository : RepositoryBase<ScUsr>, IScUsrRepository
    {
        public ScUsrRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IList<ScUsr>> GetScUsrById(string loginId)
        {
            var usrInfo = await DbContext.ScUsrs.Where(ci => ci.LoginId == loginId).ToListAsync();
            return usrInfo;
        }

        public ScUsr Insert(ScUsr scUsr)
        {
            return DbContext.ScUsrs.Add(scUsr);
        }

        //public async Task<ScUsr> GetMentorInfoById(string loginId)
        //{
        //    var scusr = await this.DbContext.ScUsrs
        //        .Include(i => i.ScUsrResume)
        //        //.Include(i => i.ScUsrResume.ScFileInfo)
        //        .Include(i => i.ScMentorMappiings.Select(s => s.ScBizWork))
        //        .Where(ci => ci.LoginId == loginId && ci.Status == "N").FirstOrDefaultAsync();
        //    return scusr;
        //}

        /// <summary>
        ///     맨토정보 가져오기(Eager 로딩, include ScUsrResume, ScMentorMappiings.Select(s => s.ScBizWork)
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<ScUsr> GetMentorInfoById(Expression<Func<ScUsr, bool>> where)
        {
            var scusr = await DbContext.ScUsrs
                .Include(i => i.ScUsrResume)
                //.Include(i => i.ScUsrResume.ScFileInfo)
                .Include(i => i.ScMentorMappiings.Select(s => s.ScBizWork))
                .Where(where).FirstOrDefaultAsync();
            return scusr;
        }
    }
}