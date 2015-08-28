using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;

namespace BizOneShot.Light.Web.ComLib
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        private string _rawUrl = null;  //current url
        /// <summary>
        /// [기능] : 권한 검증(검증실패 false 반환)
        /// [작성] : 2014-10-23 김충기
        /// [수정] : 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            _rawUrl = httpContext.Request.RawUrl;
            return httpContext.Session[Global.LoginID] != null;
        }

        /// <summary>
        /// [기능] : 인증실패 처리, 로그인 실패시 로그인 화면으로 이동
        /// [작성] : 2014-10-23 김충기
        /// [수정] :                  
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(string.Format("{0}?rtnUrl={1}", ConfigurationManager.AppSettings["LoginUrl"], _rawUrl), false);
        }


    }
}