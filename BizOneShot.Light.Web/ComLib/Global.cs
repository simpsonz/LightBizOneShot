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
        public static readonly string UserSN = "UserSN";
        public static readonly string UserNM = "UserNM";
        public static readonly string UserType = "UserType";
        public static readonly string UserTypeVal = "UserTypeEnum";

        //쿠키상수
        public static readonly string ScpSearch = "ScpSearch";

        //UserType
        public const string CompanyUser = "C";
        public const string Bookkeeper = "B";
        public const string TaxAccountant = "T";
        public const string TaxOperator = "O";
        public const string SCPAdministrator = "S";

    }
}