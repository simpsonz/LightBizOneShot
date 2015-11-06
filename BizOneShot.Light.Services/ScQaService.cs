using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Dao.Repositories;
using BizOneShot.Light.Models.WebModels;
using PagedList;

namespace BizOneShot.Light.Services
{
    public interface IScQaService : IBaseService
    {
        Task<IPagedList<ScQa>> GetPagedListReceiveQAsByQuestionId(int page, int pageSize, string questionId,
            string expertType, DateTime? startDate = null, DateTime? endDate = null);

        Task<IList<ScQa>> GetReceiveQAsAsync(string answerId, string checkYN, DateTime startDate, DateTime endDate,
            string comName = null, string registrationNo = null);

        Task<IList<ScQa>> GetReceiveQAsByQuestionId(string questionId, string expertType, DateTime? startDate = null,
            DateTime? endDate = null);

        Task<ScQa> GetQAAsync(int usrQaSn);
        Task<IDictionary<string, ScQa>> GetQADetailAsync(int usrQaSn, string questionId, string expertType);
        Task<int> AddQaAsync(ScQa scQa);
    }


    public class ScQaService : IScQaService
    {
        private readonly IScQaRepository _scQaRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScQaService(IScQaRepository _scQaRepository, IUnitOfWork unitOfWork)
        {
            this._scQaRepository = _scQaRepository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<ScQa> GetQAAsync(int usrQaSn)
        {
            var scQa = await _scQaRepository.GetQAAsync(sq => sq.UsrQaSn == usrQaSn);

            return scQa;
        }

        public async Task<IDictionary<string, ScQa>> GetQADetailAsync(int usrQaSn, string questionId, string expertType)
        {
            //이전
            var preScQaTask = await _scQaRepository.GetQAsAsync(
                sq => sq.QuestionId == questionId
                      && sq.ScUsr_AnswerId.UsrType == "P"
                      && sq.ScUsr_AnswerId.UsrTypeDetail == expertType
                      && sq.UsrQaSn < usrQaSn);
            var preScQa = preScQaTask.OrderBy(sq => sq.UsrQaSn).LastOrDefault();

            //현
            var scQa = await _scQaRepository.GetQAAsync(sq => sq.UsrQaSn == usrQaSn);

            //다음
            var nextScQaTask = await _scQaRepository.GetManyAsync(
                sq => sq.QuestionId == questionId
                      && sq.ScUsr_AnswerId.UsrType == "P"
                      && sq.ScUsr_AnswerId.UsrTypeDetail == expertType
                      && sq.UsrQaSn > usrQaSn);
            var nextScQa = nextScQaTask.OrderBy(sq => sq.UsrQaSn).FirstOrDefault();

            var dicScQa = new Dictionary<string, ScQa>();

            dicScQa.Add("preScQa", preScQa);
            dicScQa.Add("curScQa", scQa);
            dicScQa.Add("nextScQa", nextScQa);

            return dicScQa;
        }


        public async Task<IList<ScQa>> GetReceiveQAsAsync(string answerId, string checkYN, DateTime startDate,
            DateTime endDate, string comName = null, string registrationNo = null)
        {
            var listScQaTask =
                await
                    _scQaRepository.GetQAsAsync(
                        sq =>
                            sq.AnswerId == answerId && sq.AnsYn.Contains(checkYN) &&
                            sq.ScUsr_QuestionId.ScCompInfo.CompNm.Contains(comName) &&
                            sq.ScUsr_QuestionId.ScCompInfo.RegistrationNo.Contains(registrationNo) &&
                            sq.AskDt >= startDate && sq.AskDt <= endDate);
            return listScQaTask.OrderByDescending(sq => sq.AskDt).ToList();
        }

        public async Task<IPagedList<ScQa>> GetPagedListReceiveQAsByQuestionId(int page, int pageSize, string questionId,
            string expertType, DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = startDate ?? DateTime.Parse("1900-01-01");
            endDate = endDate ?? DateTime.Parse("2999-12-31");

            return
                await _scQaRepository.GetpagedListQAsAsync(page, pageSize, questionId, expertType, startDate, endDate);
        }


        public async Task<IList<ScQa>> GetReceiveQAsByQuestionId(string questionId, string expertType,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = startDate ?? DateTime.Parse("1900-01-01");
            endDate = endDate ?? DateTime.Parse("2999-12-31");

            var listScQaTask = await _scQaRepository.GetQAsAsync(
                sq => sq.QuestionId == questionId
                      && sq.ScUsr_AnswerId.UsrType == "P"
                      && sq.ScUsr_AnswerId.UsrTypeDetail == expertType && sq.AskDt >= startDate && sq.AskDt <= endDate);
            return listScQaTask.OrderByDescending(sq => sq.AskDt).ToList();
        }

        public async Task<int> AddQaAsync(ScQa scQa)
        {
            var rstScQa = _scQaRepository.Insert(scQa);

            if (rstScQa == null)
            {
                return -1;
            }
            return await SaveDbContextAsync();
        }

        #region SaveContext

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