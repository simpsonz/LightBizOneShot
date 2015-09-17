using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizOneShot.Light.Models.ViewModels
{
    class MentorViewModel
    {
    }

    public class JoinMentorViewModel
    {
        //회원정보
        [Required]
        [Display(Name = "아이디")]
        [MaxLength(12, ErrorMessage = "{0}는 최대 {1}자 입니다..")]
        [MinLength(6, ErrorMessage = "{0}는 {1}자 이상이어야 합니다.")]
        public string LoginId { get; set; } // LOGIN_ID (Primary key). 로그인식별자
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        [MaxLength(12, ErrorMessage = "{0}는 최대 {1}자 입니다..")]
        [MinLength(8, ErrorMessage = "{0}는 {1}자 이상이어야 합니다.")]
        public string LoginPw { get; set; } // LOGIN_PW. 로그인비밀번호
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        [MaxLength(12, ErrorMessage = "{0}는 최대 {1}자 입니다..")]
        [MinLength(8, ErrorMessage = "{0}는 {1}자 이상이어야 합니다.")]
        [Compare("LoginPw", ErrorMessage = "비밀번호와 확인 비밀번호가 일치하지 않습니다.")]
        public string ConfirmLoginPw { get; set; } // LOGIN_PW. 로그인비밀번호 확인
        [Required]
        [Display(Name = "담당자명")]
        [MaxLength(40, ErrorMessage = "{0}은 최대 {1}자 입니다..")]
        public string Name { get; set; } // Name. 이름
        [Required]
        [Display(Name = "이메일")]
        public string Email1 { get; set; } // EMAIL. 이메일
        [Required]
        [Display(Name = "이메일")]
        public string Email2 { get; set; } // EMAIL. 이메일
        public string Email { get; set; } // EMAIL. 이메일
        //담당사업
        [Required]
        [Display(Name = "사업식별자")]
        public int BizWorkSn { get; set; } //담당 사업
        [Display(Name = "사업명")]
        public string BizWorkNm { get; set; } //사업명
        public string UsrTypeDetail { get; set; }

        public string TelNo1 { get; set; } // TEL_NO. 전화번호
        public string TelNo2 { get; set; } // TEL_NO. 전화번호
        public string TelNo3 { get; set; } // TEL_NO. 전화번호
        public string TelNo { get; set; } // TEL_NO. 전화번호
        public string MbNo { get; set; } // MB_NO. 휴대폰
        public string FaxNo { get; set; } // FAX_NO. Fax
        public string PostNo { get; set; } // POST_NO. 우편번호
        public string Addr1 { get; set; } // ADDR_1. 주소1
        public string Addr2 { get; set; } // ADDR_2. 주소2
        public string BankNm { get; set; } // BANK_NM. 계좌은행
        public string AccountNo { get; set; } // ACCOUNT_NO. 계좌번호
        //이력서 정보
        public string ResumeName { get; set; } //이력서 파일명
        public string ResumePath { get; set; } //이력서 파일 경로
    }

    public class MentorDropDownModel
    {
        public string LoginId { get; set; } // LOGIN_ID (Primary key). 로그인식별자
        public string Name { get; set; } // Name. 이름
    }

    public class MentorMyInfoViewModel
    {
        //회원정보
        [Required]
        [Display(Name = "아이디")]
        [MaxLength(12, ErrorMessage = "{0}는 최대 {1}자 입니다..")]
        [MinLength(6, ErrorMessage = "{0}는 {1}자 이상이어야 합니다.")]
        public string LoginId { get; set; } // LOGIN_ID (Primary key). 로그인식별자

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        [MaxLength(12, ErrorMessage = "{0}는 최대 {1}자 입니다..")]
        [MinLength(8, ErrorMessage = "{0}는 {1}자 이상이어야 합니다.")]
        public string LoginPw { get; set; } // LOGIN_PW. 로그인비밀번호
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        [MaxLength(12, ErrorMessage = "{0}는 최대 {1}자 입니다..")]
        [MinLength(8, ErrorMessage = "{0}는 {1}자 이상이어야 합니다.")]
        [Compare("LoginPw", ErrorMessage = "비밀번호와 확인 비밀번호가 일치하지 않습니다.")]
        public string ConfirmLoginPw { get; set; } // LOGIN_PW. 로그인비밀번호 확인

        [Required]
        [Display(Name = "담당자명")]
        [MaxLength(40, ErrorMessage = "{0}은 최대 {1}자 입니다.")]
        public string Name { get; set; } // Name. 이름
        public string TelNo { get; set; } // TEL_NO. 전화번호
        public string TelNo1 { get; set; }
        [Range(0, 9999, ErrorMessage = "올바른 전화번호를 입력하세요")]
        public string TelNo2 { get; set; }
        [Range(0, 9999, ErrorMessage = "올바른 전화번호를 입력하세요")]
        public string TelNo3 { get; set; }
        public string MbNo { get; set; } // MB_NO. 휴대폰
        public string MbNo1 { get; set; }
        [Range(0, 9999, ErrorMessage = "올바른 전화번호를 입력하세요")]
        public string MbNo2 { get; set; }
        [Range(0, 9999, ErrorMessage = "올바른 전화번호를 입력하세요")]
        public string MbNo3 { get; set; }
        public string FaxNo { get; set; } //FAX_NO, 팩스번호
        public string FaxNo1 { get; set; }
        [Range(0, 9999, ErrorMessage = "올바른 전화번호를 입력하세요")]
        public string FaxNo2 { get; set; }
        [Range(0, 9999, ErrorMessage = "올바른 전화번호를 입력하세요")]
        public string FaxNo3 { get; set; }
        public string Addr { get; set; } //POST_NO + ADDR_1 + ADDR_2  전체 주소
        public string PostNo { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        [Required]
        [Display(Name = "이메일")]
        [EmailAddress(ErrorMessage = "올바른 이메일주소를 입력하세요")]
        public string Email { get; set; } // EMAIL. 이메일
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string UsrType { get; set; } // USR_TYPE. 회원유형  C: 기업회원(Company)  M:멘토(Mentor)  B: 사업회원(Business)  P:전문가회원(Professonal)  S: SCP (SCP)
        public string UsrTypeDetail { get; set; } // USR_TYPE_DETAIL. A :  관리자(Admi)  O: 담당자(Operator)    T: 세무/회계사(Tax Accountant)  W: 노무  
        public string UsrTypeName { get; set; } // USR_TYPE. 회원유형  C: 기업회원(Company)  M:멘토(Mentor)  B: 사업회원(Business)  P:전문가회원(Professonal)  S: SCP (SCP)
        public string UsrTypeDetailName { get; set; } // USR_TYPE_DETAIL. A :  관리자(Admi)  O: 담당자(Operator)    T: 세무/회계사(Tax Accountant)  W: 노무  
        public string BankNm { get; set; }  //은행명
        public string AccountNo { get; set; } //계좌번호

        //이력서정보
        public string FileSn { get; set; }
        public string ResumeName { get; set; }  //이력서 파일명
        public string ResumePath { get; set; }  //이력서 경로

        //사업관리정보
        public int MngCompSn { get; set; }      // COMP_SN. 기업식별자 (관리기관 식별자)
        public string MngCompName { get; set; } // COMP_NM. 기업명
        public int BizWorkSn { get; set; }      // BIZ_WORK_SN (Primary key). 사업식별자
        public string BizWorkNm { get; set; } // BIZ_WORK_NM 사업명
    }

    public class ModifyMentorParamModel
    {
        public string LoginIdChk { get; set; }
        public string LoginPwChk { get; set; }
        public string DeleteFileSn { get; set; }
    }

    public class SelectedMentorTotalReportParmModel
    {
        public int SubmitDt { get; set; }
        public int BizWorkSn { get; set; }
        public int CompSn { get; set; }
    }

    public class SubmitDtDropDownModel
    {
        public int SubmitDt { get; set; } // 제출일
        public string SubmitYear { get; set; } // 제출일
    }
}



