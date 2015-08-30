using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizOneShot.Light.Web.ComLib
{
    public static class Global
    {
        //세션상수
        public static readonly string LoginID = "LoginID";
        public static readonly string CompSN = "CompSN";
        public static readonly string UserNM = "UserNM";
        public static readonly string UserType = "UserType";
        public static readonly string UserTypeVal = "UserTypeEnum";
        public static readonly string UserDetailType = "UserDetailType";
        public static readonly string UserLogo = "UserLogo";

        //쿠키상수
        public static readonly string ScpSearch = "ScpSearch";

        //UserType
        public const string Company = "C";
        public const string Mentor = "M";
        public const string Expert = "P";
        public const string BizManager = "B";
        public const string SysManager = "S";

    }
}