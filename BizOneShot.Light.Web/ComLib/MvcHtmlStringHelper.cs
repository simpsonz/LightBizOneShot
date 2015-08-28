using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Resources;

using System.Text;
using System.Text.RegularExpressions;

namespace BizOneShot.Light.Web.ComLib
{
    public static class MvcHtmlStringHelper
    {
        /// <summary>
        /// [기능] : 페이징 처리
        /// [작성] : 2014-10-23 김충기
        /// [수정] :  
        /// </summary>
        /// <param name="html"></param>
        /// <param name="javascript"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageCount"></param>
        /// <param name="hasPreviousPage"></param>
        /// <param name="hasNextPage"></param>
        /// <returns></returns>
        public static MvcHtmlString Pagination(this HtmlHelper html, string javascript, int pageNumber, int pageCount, bool hasPreviousPage, bool hasNextPage)
        {
            var sb = new StringBuilder();
            sb.Append("<div class=\"page_num_wrap\"><div class=\"page_num\"><ul>");
            if (hasPreviousPage == true)
            {
                sb.AppendFormat("<li><input type=\"button\" class=\"first\" value=\"첫페이지\" onclick=\"" + javascript + "('{0}');\"></li>", 1);
                sb.AppendFormat("<li><input type=\"button\" class=\"pre\" value=\"이전 페이지\" onclick=\"" + javascript + "('{0}');\"></li>", pageNumber - 1);
            }
            else
            {
                sb.Append("<li><input type=\"button\" class=\"first\" value=\"첫페이지\" ");
                sb.Append("<li><input type=\"button\" class=\"pre\" value=\"이전 페이지\" ></li>");
            }

            int setSize = 10;   //페이지 내비사이즈

            int startPage = (pageNumber / setSize) * setSize + 1;       //시작페이지

            for (int i = startPage; i < setSize + startPage; i++)
            {
                if (i > pageCount)
                    break;

                if (pageNumber == i)
                {
                    sb.AppendFormat("<li class=\"on\">{0}", i);
                }
                else
                {
                    sb.AppendFormat("<li><a href=\"javascript:" + javascript + "('{0}');\">{1}</a>", i, i);
                }

                if (i != pageCount && i != setSize + startPage - 1)
                {
                    sb.Append("<span>l</span>");
                }
                sb.Append("</li>");
            }

            if (hasNextPage == true)
            {
                sb.AppendFormat("<li><input type=\"button\" class=\"next\" value=\"다음 페이지\" onclick=\"" + javascript + "('{0}');\"></li>", pageNumber + 1);
                sb.AppendFormat("<li><input type=\"button\" class=\"last\" value=\"끝 페이지\" onclick=\"" + javascript + "('{0}');\"></li>", pageCount);
            }
            else
            {
                sb.Append("<li><input type=\"button\" class=\"next\" value=\"다음 페이지\"></li>");
                sb.Append("<li><input type=\"button\" class=\"last\" value=\"끝 페이지\" ></li>"); ;
            }

            sb.Append("</ul></div></div>");
            return MvcHtmlString.Create(sb.ToString());
        }


        /// <summary>
        /// HTML 금지문자 변경
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IHtmlString RepladeHtmlText(this HtmlHelper helper, string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return new HtmlString(""); 
            }

           return helper.Raw(text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>").Replace(" ", "&nbsp;"));

        }
    }
}