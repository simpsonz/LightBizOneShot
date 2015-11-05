using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Web.ComLib
{
    public class BaseController : Controller
    {
        //private ILogService _logService;


        //public BaseController()
        //{
        //    this._logService = new LogService();
        //}

        //public BaseController(ILogService logService)
        //{
        //    this._logService = logService;
        //}

        #region 에러처리 및 로깅

        /// <summary>
        ///     [기능] : Exception 처리 및 로깅
        ///     [작성] : 2014-10-23 김충기
        ///     [수정] :
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            //if (filterContext.ExceptionHandled)
            //    return;

            var actionName = filterContext.RouteData.Values["action"].ToString();
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var controllerType = filterContext.Controller.GetType();
            //var method = controllerType.GetMethod(actionName);   
            //var returnType = method.ReturnType;


            //통합관제 DB에 웹에러 로깅

            #region Insert WebLog

            //WebLogViewModel log = new WebLogViewModel
            //{
            //    SRV_CD = ConfigurationManager.AppSettings["ServiceCode"],
            //    SVR_IP = HttpContext.Request.UserHostAddress,
            //    RMK_TXT = filterContext.Exception.Message,
            //    LINE = filterContext.Exception.StackTrace,
            //    LOGIN_ID = (_LogOnUser != null) ? _LogOnUser.LOGIN_ID : "",
            //    USR_AGN = HttpContext.Request.UserAgent,
            //    FILE_NM = string.Format("/{0}/{1}", controllerName, actionName)
            //};
            //_logService.RegisterWeblog(log);

            #endregion

            var userComment = "일시적인 장애가 발생했습니다.잠시후 다시 시도해주시기 바랍니다."; //리소스에서 메시지 가져오기


            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.Result = new JsonResult {Data = userComment};
            }
            else
            {
                filterContext.Result = new RedirectResult("/Error/Error500");
            }

            filterContext.ExceptionHandled = true;
        }

        #endregion

        #region 메시지

        /// <summary>
        ///     [기능] : 삭제된(빈) 데이터 처리
        ///     [작성] : 2014-10-28 김충기
        ///     [수정] :
        /// </summary>
        protected void EmptyDataMessage()
        {
            Response.Write("<script>alert('삭제되었거나 존재하지 않는 게시물입니다.');</script>"); //메시지 리소스로 처리
            Response.Write("<script>history.back();</script>");
            Response.Flush();
            Response.End();
        }

        #endregion

        #region 세션처리

        /// <summary>
        ///     [기능] : 로그온 세션유무 확인
        ///     [작성] : 2014-10-23 김충기
        ///     [수정] :
        /// </summary>
        /// <returns></returns>
        private bool HasSession()
        {
            return Session[Global.LoginID] != null;
        }

        /// <summary>
        ///     [기능] : 로그온 세션생성
        ///     [작성] : 2014-10-23 김충기
        ///     [수정] :
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
            Session[Global.UserTypeVal] = GetUserTypeVal(user.UsrType); //권한체크용
            Session[Global.AgreeYn] = user.AgreeYn;
        }

        protected void SetLogo(string logo)
        {
            Session[Global.UserLogo] = logo;
        }

        /// <summary>
        ///     [기능] : 로그오프
        ///     [작성] : 2014-10-23 김충기
        ///     [수정] :
        /// </summary>
        protected void LogOff()
        {
            if (HasSession())
            {
                Session.Abandon();
            }
        }

        /// <summary>
        ///     [기능] : UserType enum value
        ///     [작성] : 2014-10-23 김충기
        ///     [수정] :
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        private int GetUserTypeVal(string userType)
        {
            switch (userType)
            {
                case Global.Company:
                    return (int) UserType.Company;
                case Global.Mentor:
                    return (int) UserType.Mentor;
                case Global.Expert:
                    return (int) UserType.Expert;
                case Global.BizManager:
                    return (int) UserType.BizManager;
                case Global.SysManager:
                    return (int) UserType.SysManager;
                default:
                    return 0;
            }
            ;
        }

        /// <summary>
        ///     [기능] : 로그온 회원정보 전역변수
        ///     [작성] : 2014-10-23 김충기
        ///     [수정] :
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
                return null;
            }
        }

        #endregion

        #region 검색조건유지 

        /// <summary>
        ///     [기능] : 검색조건 저장 함수
        ///     [작성] : 2014-10-23 김충기
        ///     [수정] :
        /// </summary>
        protected void SetSearchCookie()
        {
            var previousUrl = Request.ServerVariables["HTTP_REFERER"] != null
                ? Request.ServerVariables["HTTP_REFERER"].ToUpper()
                : "";
            var currentUrl = Request.ServerVariables["SCRIPT_NAME"].ToUpper();
            var searchInfo = "searchInfo=" + currentUrl;

            foreach (string key in Request.Form.Keys)
            {
                if (key != "__EVENTTARGET" && key != "__EVENTARGUMENT" && key != "__VIEWSTATE"
                    && key != "__PREVIOUSPAGE" && key != "__EVENTVALIDATION" && key != null)
                {
                    var values = Request.Form.GetValues(key);
                    for (var k = 0; k < values.Length; k++)
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
        ///     [기능] : 검색조건 반환 함수
        ///     [작성] : 2014-10-23 김충기
        ///     [수정] :
        /// </summary>
        /// <returns></returns>
        protected SortedList<string, string> GetSearchCookie()
        {
            var searchInfo = Request.Cookies[Global.ScpSearch] != null
                ? Server.UrlDecode(Request.Cookies[Global.ScpSearch].Value)
                : "";
            var conditions = Regex.Split(searchInfo, "@@", RegexOptions.IgnorePatternWhitespace);
            var sl = new SortedList<string, string>();
            for (var i = 0; i < conditions.Length; i++)
            {
                var keyValues = Regex.Split(conditions[i], "=", RegexOptions.IgnorePatternWhitespace);
                sl.Add(keyValues[0], keyValues[1]);
            }
            return sl;
        }

        /// <summary>
        ///     [기능] : 검색조건 유무확인
        ///     [작성] : 2014-10-23 김충기
        ///     [수정] :
        /// </summary>
        /// <returns></returns>
        protected bool HasSearchCookie()
        {
            var hasCookie = false;
            var searchInfo = Request.Cookies[Global.ScpSearch] != null
                ? Server.UrlDecode(Request.Cookies[Global.ScpSearch].Value)
                : "";
            var keyValues = Regex.Split(searchInfo, "@@", RegexOptions.IgnorePatternWhitespace);
            var previousUrl = Request.ServerVariables["HTTP_REFERER"] != null
                ? Request.ServerVariables["HTTP_REFERER"].ToUpper()
                : "";
            var currentUrl = Request.ServerVariables["SCRIPT_NAME"].ToUpper();
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

        #region 파일다운로드

        /// <summary>
        /// [기능] : 파일다운로드
        /// [작성] : 2014-11-24 김충기
        /// [수정] : 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileUrl"></param>
        /// <param name="folderType"></param>
        /// <returns></returns>
        //public FileResult FileDownload(string fileName, string fileUrl, FolderType folderType)
        //{
        //    string directory = "";
        //    switch (folderType)
        //    {
        //        case FolderType.Document:
        //            directory = ConfigurationManager.AppSettings["FilePath.Document"];
        //            break;
        //        case FolderType.Paper:
        //            directory = ConfigurationManager.AppSettings["FilePath.Paper"];
        //            break;
        //        case FolderType.Board:
        //            directory = ConfigurationManager.AppSettings["FilePath.Board"];
        //            break;
        //        case FolderType.Manual:
        //            directory = ConfigurationManager.AppSettings["FilePath.Manual"];
        //            break;
        //    }
        //    directory += fileUrl;
        //    return File(directory, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        //}

        #endregion
    }
}