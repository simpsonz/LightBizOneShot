using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Models.DareModels;
using PagedList;
using PagedList.EntityFramework;

namespace BizOneShot.Light.Dao.Repositories
{
    public interface IDareAcStdIncmrateBseIdstTRepository : IRepository<SHUSER_AcStdIncmrateBseIdstT>
    {
        Task<IPagedList<SHUSER_AcStdIncmrateBseIdstT>> GetBizTypes(int page, int pageSize, string query = null);
    }


    public class DareAcStdIncmrateBseIdstTRepository : DareRepositoryBase<SHUSER_AcStdIncmrateBseIdstT>, IDareAcStdIncmrateBseIdstTRepository
    {
        public DareAcStdIncmrateBseIdstTRepository(IDareDbFactory dareDbFactory) : base(dareDbFactory)
        {
        }

        public async Task<IPagedList<SHUSER_AcStdIncmrateBseIdstT>> GetBizTypes(int page, int pageSize, string query = null)
        {
           return await DareDbContext.SHUSER_AcStdIncmrateBseIdstTs
                .Where(bt => query == null ? bt.IdstCd != null : bt.IdstCd.Contains(query) || bt.IdstDtlNm.Contains(query))
                .OrderBy(bt => bt.IdstLgClsfCd)
                .ToPagedListAsync(page, pageSize);
        
        }

    }
}