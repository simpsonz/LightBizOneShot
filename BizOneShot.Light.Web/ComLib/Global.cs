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

        //UserDetailType
        public const string Admin = "A";
        public const string Member = "M";

        //LeftMenu
        //공통
        public const string Cs = "Cs";                          //고객센터
        public const string Report = "Report";                  //보고서
        public const string MyInfo = "MyInfo";                  //내정보관리

        //기업회원
        public const string ExpertService = "ExpertService";    //전문가서비스
       
        //SysManager   
        public const string ExpertMng = "ExpertMng";            //전문가 관리
        public const string BizMng = "BizMng";                  //사업관리자 관리

        //BizManager
        public const string MentorMng = "MentorMng";            //멘토 관리
        public const string BizWorkMng = "BizWorkMng";          //사업 관리
        public const string ComMng = "ComMng";                  //기업 관리

    }
}