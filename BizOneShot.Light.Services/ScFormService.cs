using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;

using System.Linq.Expressions;
using System;


namespace BizOneShot.Light.Services
{
    public interface IScFormService : IBaseService
    {
        Task<IList<ScForm>> GetManualsAsync(string searchType = null, string keyword = null);

        Task<IDictionary<string, ScForm>> GetManualDetailByIdAsync(int formSn);

        Task<ScForm> GetScFormAsync(int formSn);
        Task<int> AddScFormAsync(ScForm scForm);
        Task ModifyScFormAsync(ScForm scForm);
    }


    public class ScFormService : IScFormService
    {
        private readonly IScFormRepository scFormRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScFormService(IScFormRepository scFormRepository, IUnitOfWork unitOfWork)
        {
            this.scFormRepository = scFormRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<ScForm>> GetManualsAsync(string searchType = null, string keyword = null)
        {
            IEnumerable<ScForm> listScFormTask = null;
            if (string.IsNullOrEmpty(searchType) || string.IsNullOrEmpty(keyword))
            {
                listScFormTask = await scFormRepository.GetManyAsync(manual => manual.Status == "N");
                return listScFormTask.OrderByDescending(manual => manual.FormSn)
                    .ToList();
            }
            else if (searchType.Equals("0")) // 제목, 내용중 keyword가 포함된 Notice 검색 
            {
                listScFormTask = await scFormRepository.GetManyAsync(manual => manual.Subject.Contains(keyword) || manual.Contents.Contains(keyword) && manual.Status == "N");
                return listScFormTask.OrderByDescending(manual => manual.FormSn)
                    .ToList();
            }
            else if (searchType.Equals("1")) // 제목중에 keyword가 포함된 Notice 검색 
            {
                listScFormTask = await scFormRepository.GetManyAsync(manual => manual.Subject.Contains(keyword) && manual.Status == "N");
                return listScFormTask.OrderByDescending(manual => manual.FormSn)
                    .ToList();
            }
            else if (searchType.Equals("2")) // 내용중에 keyword가 포함된 Notice 검색 
            {
                listScFormTask = await scFormRepository.GetManyAsync(manual => manual.Contents.Contains(keyword) && manual.Status == "N");
                return listScFormTask.OrderByDescending(manual => manual.FormSn)
                    .ToList();
            }

            listScFormTask = await scFormRepository.GetManyAsync(manual => manual.Status == "N");
            return listScFormTask.OrderByDescending(manual => manual.FormSn)
                .ToList();
        }

        public async Task<IDictionary<string, ScForm>> GetManualDetailByIdAsync(int formSn)
        {
            var preFormTask = await scFormRepository.GetManyAsync(manual => manual.FormSn < formSn && manual.Status == "N");
            var preForm = preFormTask.OrderBy(manual => manual.FormSn).LastOrDefault();

            var curForm = await scFormRepository.GetAsync(manual => manual.FormSn == formSn);

            var nextFormTask = await scFormRepository.GetManyAsync(manual => manual.FormSn > formSn && manual.Status == "N");
            var nextForm = nextFormTask.OrderBy(manual => manual.FormSn).FirstOrDefault();

            var dicScForms = new Dictionary<string, ScForm>();

            dicScForms.Add("preForm", preForm);
            dicScForms.Add("curForm", curForm);
            dicScForms.Add("nextForm", nextForm);

            return dicScForms;
        }

        public async Task<ScForm> GetScFormAsync(int formSn)
        {
            return await scFormRepository.GetAsync(manual => manual.FormSn == formSn && manual.Status == "N");
        }

        public async Task<int> AddScFormAsync(ScForm scForm)
        {
            var rstScForm = await scFormRepository.Insert(scForm);

            if (rstScForm == null)
            {
                return -1;
            }
            else
            {
                return await SaveDbContextAsync();
            }

        }

        public async Task ModifyScFormAsync(ScForm scForm)
        {
            await Task.Run(() => scFormRepository.Update(scForm));


            await SaveDbContextAsync();
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
