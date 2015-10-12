// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;

namespace BizOneShot.Light.Models.DareModels
{
    // SY_USER
    public class SHUSER_SyUser
    {
        public string IdUser { get; set; } // ID_USER (Primary key). 사용자아이디
        public string MembBusnpersNo { get; set; } // MEMB_BUSNPERS_NO. 회원사업자번호
        public string NmUser { get; set; } // NM_USER. 사용자이름
        public string Pwd { get; set; } // PWD. 암호
        public string SmartPwd { get; set; } // SMART_PWD
        public string UsrGbn { get; set; } // USR_GBN. 사용자구분
        public string Usercode { get; set; } // USERCODE. 사용자발주코드
        public string LangCd { get; set; } // LANG_CD. 언어코드
        public string EmpCode { get; set; } // EMP_CODE. 사원아이디
        public string StDocuapp { get; set; } // ST_DOCUAPP. 전표권한
        public string GrdUser { get; set; } // GRD_USER. 사용자그룹
        public string ShowModuleCd { get; set; } // SHOW_MODULE_CD. 시작 모듈코드
        public string ShowMenuCd { get; set; } // SHOW_MENU_CD. 시작 메뉴코드
        public string HangilPwd { get; set; } // HANGIL_PWD. 한길암호
        public string LastLoginDate { get; set; } // LAST_LOGIN_DATE. 마지막 로그인시간
        public string LastLogoutDate { get; set; } // LAST_LOGOUT_DATE. 마지막 로그아웃시간
        public string CheckingLogDt { get; set; } // CHECKING_LOG_DT. 체크_로그아웃시간
        public string PersData { get; set; } // PERS_DATA. 기타데이터
        public string UserStatus { get; set; } // USER_STATUS. 사용자 상태 0:접속불가
        public int? SmsChargeCnt { get; set; } // SMS_CHARGE_CNT. 문자충전갯수
        public string SmsSendCnt { get; set; } // SMS_SEND_CNT. 문자전송갯수
        public string DbAccessLog { get; set; } // DB_ACCESS_LOG. DB ACCESS LOG 유무
        public string LastProductType { get; set; } // LAST_PRODUCT_TYPE. 마지막로그인제품코드
        public string CreateIdDate { get; set; } // CREATE_ID_DATE. 아이디발주일시
        public string InsertId { get; set; } // INSERT_ID. 등록자
        public string InsertIp { get; set; } // INSERT_IP. 등록자IP
        public string InsertDt { get; set; } // INSERT_DT. 등록일시
        public string ModifyId { get; set; } // MODIFY_ID. 수정자
        public string ModifyIp { get; set; } // MODIFY_IP. 수정자IP
        public string ModifyDt { get; set; } // MODIFY_DT. 수정일시
        public string MenuAuthCd { get; set; } // MENU_AUTH_CD
        public string MainMenuView { get; set; } // MAIN_MENU_VIEW
        public string IsB2BUser { get; set; } // IS_B2B_USER
        public string Etc { get; set; } // ETC
        public string IsHrModuleYn { get; set; } // IS_HR_MODULE_YN
        public string MobileData { get; set; } // MOBILE_DATA
        public string WebUsrGbn { get; set; } // WEB_USR_GBN
    }

}
