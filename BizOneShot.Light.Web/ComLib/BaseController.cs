using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Text.RegularExpressions;
using System.Reflection;
using System.Configuration;

using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Services;
using System.Threading.Tasks;

namespace BizOneShot.Light.Web.ComLib
{
    public class BaseController : Controller
    {

        #region 에러처리 및 로깅

        /// <summary>
        /// [기능] : Exception 처리 및 로깅
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            var ctWebLogService = Resolver.GetService<ICtWebLogService>();

            if (filterContext.ExceptionHandled)
                return ;

            string actionName = filterContext.RouteData.Values["action"].ToString();
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            Type controllerType = filterContext.Controller.GetType();

            //통합관제 DB에 웹에러 로깅
            #region Insert WebLog
            CtWebLog log = new CtWebLog
            {
                SrvCd = ConfigurationManager.AppSettings["ServiceCode"],
                SvrIp = HttpContext.Request.UserHostAddress,
                RmkTxt = filterContext.Exception.Message,
                Line = filterContext.Exception.StackTrace,
                LoginId = (_LogOnUser != null) ? _LogOnUser.LoginId : "",
                UsrAgn = HttpContext.Request.UserAgent,
                FileNm = string.Format("/{0}/{1}", controllerName, actionName),
                RegDt = DateTime.Now
            };

            if(log.Line.Length > 500)
            {
                log.Line = log.Line.Substring(0, 500);
            }

            ctWebLogService.AddCtWebLogAsync(log);
            #endregion

            string userComment = "일시적인 장애가 발생했습니다.잠시후 다시 시도해주시기 바랍니다."; //리소스에서 메시지 가져오기


            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.Result = new JsonResult() { Data = userComment };
            }
            else
            {
                filterContext.Result = new RedirectResult("/Error/Error500");
            }

            filterContext.ExceptionHandled = true;
        }
        #endregion

        #region 세션처리
        /// <summary>
        /// [기능] : 로그온 세션유무 확인
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        /// <returns></returns>
        private bool HasSession()
        {
            return Session[Global.LoginID] != null;
        }

        /// <summary>
        /// [기능] : 로그온 세션생성
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        /// <param name="user"></param>
        protected void LogOn(ScUsr user)
        {
            Session[Global.LoginID] = user.LoginId;
            Session[Global.CompSN] = user.CompSn;
            Session[Global.CompRegistrationNo] = user.ScCompInfo.RegistrationNo;
            Session[Global.UserNM] = user.Name;
            Session[Global.UserType] = user.UsrType;
            Session[Global.UserDetailType] = user.UsrTypeDetail;
            Session[Global.UserTypeVal] = GetUserTypeVal(user.UsrType);    //권한체크용
            Session[Global.AgreeYn] = user.AgreeYn;
        }

        protected void SetLogo(string logo, string method)
        {
            Session[Global.UserLogo] = logo;
            Session[Global.StartMethod] = method;
        }

        /// <summary>
        /// [기능] : 로그오프
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        protected string LogOff()
        {
            if (HasSession())
            {
                var method = Session[Global.StartMethod].ToString();
                Session.Abandon();
                return method;
            }

            return "";
        }

        /// <summary>
        /// [기능] : UserType enum value
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        private int GetUserTypeVal(string userType)
        {
            switch (userType)
            {
                case Global.Company:
                    return (int)UserType.Company;
                case Global.Mentor:
                    return (int)UserType.Mentor;
                case Global.Expert:
                    return (int)UserType.Expert;
                case Global.BizManager:
                    return (int)UserType.BizManager;
                case Global.SysManager:
                    return (int)UserType.SysManager;
                default:
                    return 0;
            };
        }

        /// <summary>
        /// [기능] : 로그온 회원정보 전역변수


        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        public ScUsr _LogOnUser
        {
            get
            {
                if (HasSession())
                {
                    return new ScUsr
                    {
                        LoginId = Session[Global.LoginID].ToString(),
                        Name = Session[Global.UserNM].ToString(),
                        UsrType = Session[Global.UserType].ToString(),
                        UsrTypeDetail = Session[Global.UserDetailType].ToString()
                    };
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region 검색조건유지 
        /// <summary>
        /// [기능] : 검색조건 저장 함수
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        protected void SetSearchCookie()
        {
            string previousUrl = (Request.ServerVariables["HTTP_REFERER"] != null ? Request.ServerVariables["HTTP_REFERER"].ToUpper() : "");
            string currentUrl = Request.ServerVariables["SCRIPT_NAME"].ToUpper();
            string searchInfo = "searchInfo=" + currentUrl;

            foreach (string key in Request.Form.Keys)
            {
                if (key != "__EVENTTARGET" && key != "__EVENTARGUMENT" && key != "__VIEWSTATE"
                    && key != "__PREVIOUSPAGE" && key != "__EVENTVALIDATION" && key != null)
                {
                    string[] values = Request.Form.GetValues(key);
                    for (int k = 0; k < values.Length; k++)
                    {
                        if (k == 0)
                        {
                            searchInfo += "@@" + key + "=" + values[k];
                        }
                        else
                        {
                            searchInfo += "," + values[k];
                        }
                    }
                }
            }

            if ((previousUrl != "") && (previousUrl.IndexOf(currentUrl) > 0))
            {
                Response.Cookies[Global.ScpSearch].Value = Server.UrlEncode(searchInfo);
            }
        }

        /// <summary>
        /// [기능] : 검색조건 반환 함수
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        /// <returns></returns>
        protected SortedList<string, string> GetSearchCookie()
        {
            string searchInfo = (Request.Cookies[Global.ScpSearch] != null ? Server.UrlDecode(Request.Cookies[Global.ScpSearch].Value) : "");
            string[] conditions = Regex.Split(searchInfo, "@@", RegexOptions.IgnorePatternWhitespace);
            SortedList<string, string> sl = new SortedList<string, string>();
            for (int i = 0; i < conditions.Length; i++)
            {
                string[] keyValues = Regex.Split(conditions[i], "=", RegexOptions.IgnorePatternWhitespace);
                sl.Add(keyValues[0], keyValues[1]);
            }
            return sl;
        }

        /// <summary>
        /// [기능] : 검색조건 유무확인 
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        /// <returns></returns>
        protected bool HasSearchCookie()
        {
            bool hasCookie = false;
            string searchInfo = (Request.Cookies[Global.ScpSearch] != null ? Server.UrlDecode(Request.Cookies[Global.ScpSearch].Value) : "");
            string[] keyValues = Regex.Split(searchInfo, "@@", RegexOptions.IgnorePatternWhitespace);
            string previousUrl = (Request.ServerVariables["HTTP_REFERER"] != null ? Request.ServerVariables["HTTP_REFERER"].ToUpper() : "");
            string currentUrl = Request.ServerVariables["SCRIPT_NAME"].ToUpper();
            //if ((previousUrl.IndexOf(currentUrl) == -1) && (keyValues.Length > 1))
            if (keyValues.Length > 1)
            {
                if (searchInfo.IndexOf(currentUrl) > 0)
                {
                    hasCookie = true;
                }
            }
            return hasCookie;
        }
        #endregion

        #region 메시지
        /// <summary>
        /// [기능] : 삭제된(빈) 데이터 처리
        /// [작성] : 2014-10-28 김충기
        /// [수정] : 
        /// </summary>
        protected void EmptyDataMessage()
        {
            Response.Write("<script>alert('삭제되었거나 존재하지 않는 게시물입니다.');</script>");    //메시지 리소스로 처리
            Response.Write("<script>history.back();</script>");
            Response.Flush();
            Response.End();
        }
        #endregion
    }
}