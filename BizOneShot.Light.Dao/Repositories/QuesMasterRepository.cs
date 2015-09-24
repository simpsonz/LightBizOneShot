using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Dao.Repositories
{

    public interface IQuesMasterRepository : IRepository<QuesMaster>
    {
        Task<IList<QuesMaster>> GetQuesMastersAsync(Expression<Func<QuesMaster, bool>> where);
        Task<QuesMaster> GetQuesMasterAsync(Expression<Func<QuesMaster, bool>> where);
        QuesMaster Insert(QuesMaster quesMaster);
    }


    public class QuesMasterRepository : RepositoryBase<QuesMaster>, IQuesMasterRepository
    {
        public QuesMasterRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public async Task<IList<QuesMaster>> GetQuesMastersAsync(Expression<Func<QuesMaster, bool>> where)
        {
            return await this.DbContext.QuesMasters.Include("QuesWriter").Where(where).ToListAsync();
        }


        public async Task<QuesMaster> GetQuesMasterAsync(Expression<Func<QuesMaster, bool>> where)
        {
            return await this.DbContext.QuesMasters.Include("QuesWriter").Where(where).SingleAsync();
        }

        public QuesMaster Insert(QuesMaster quesMaster)
        {
            return this.DbContext.QuesMasters.Add(quesMaster);
        }

    }
}
