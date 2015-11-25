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

    public interface ICtWebLogService
    {
        void AddCtWebLogAsync(CtWebLog ctWebLog);
    }

    public class CtWebLogService : ICtWebLogService
    {
      
        public CtWebLogService()
        {
        }

        public void AddCtWebLogAsync(CtWebLog ctWebLog)
        {
            var ctWebLogRepository = new CtWebLogRepository(new DbFactory());

            ctWebLogRepository.Insert(ctWebLog);

        }


    }
}