using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Services
{

    public interface ICtWebLogService : IBaseService
    {
        void AddCtWebLogAsync(CtWebLog ctWebLog);
    }

    public class CtWebLogService : ICtWebLogService
    {
        private readonly ICtWebLogRepository ctWebLogRepository;
        private readonly IUnitOfWork unitOfWork;

        public CtWebLogService(IUnitOfWork unitOfWork,
            ICtWebLogRepository ctWebLogRepository)
        {
            this.unitOfWork = unitOfWork;
            this.ctWebLogRepository = ctWebLogRepository;
        }

        public void AddCtWebLogAsync(CtWebLog ctWebLog)
        {
            var rstCtWebLog = ctWebLogRepository.Insert(ctWebLog);

            if (rstCtWebLog != null)
            {
                SaveDbContext();
            }
        }

        #region SaveDbContext

        public void SaveDbContext()
        {
            unitOfWork.Commit();
        }

        public async Task<int> SaveDbContextAsync()
        {
            return await unitOfWork.CommitAsync();
        }

        #endregion
    }
}