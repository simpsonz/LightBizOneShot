using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.ViewModels;

namespace BizOneShot.Light.Web.ComLib
{
    public class MenuAuthorizeAttribute : AuthorizeAttribute
    {
        //private string _preUrl = null; //rtnUrl 처리시 사용       
        public new UserType Roles; //어트리뷰트에서 참조하도록 Role 생성
        /// <summary>
        /// [기능] : 권한인증 처리
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

            //_preUrl = httpContext.Request.UrlReferrer.LocalPath;            
            UserType role = (UserType)Convert.ToInt32(httpContext.Session[Global.UserTypeVal]);
            if (Roles != 0 && ((Roles & role) != role))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// [기능] : 권한인증 결과 처리
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //rtnUrl을 이용해서 접근할 경우 각 회원유형의 메인페이지로 이동하게 처리
            filterContext.RequestContext.HttpContext.Response.Write("<script>alert('권한이 없습니다.');</script>");
            filterContext.RequestContext.HttpContext.Response.Write("<script>history.back();</script>");
            filterContext.RequestContext.HttpContext.Response.Flush();
            filterContext.RequestContext.HttpContext.Response.End();
        }
    }
}