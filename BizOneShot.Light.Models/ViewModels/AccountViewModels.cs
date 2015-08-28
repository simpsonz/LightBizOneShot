using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BizOneShot.Light.Models.ViewModels
{
    class AccountViewModels
    {

    }

    public class JoinCompanyViewModel
    {
        //회원정보
        [Required]
        [Display(Name = "아이디")]
        [MaxLength(12, ErrorMessage = "{0}는 최대 {1}자 입니다..")]
        [MinLength(6, ErrorMessage = "{0}는 {1}자 이상이어야 합니다.")]
        public string LoginId { get; set; } // LOGIN_ID (Primary key). 로그인식별자
        public int CompSn { get; set; } // COMP_SN. 기업식별자
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
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  R: 탈퇴 요청(Retired)  D: 탈퇴 완료(Deleted)
        public string UsrType { get; set; } // USR_TYPE. 회원유형  C: 기업회원(Company)  M:멘토(Mentor)  B: 사업회원(Business)  P:전문가회원(Professonal)  S: SCP (SCP)
        public string UsrTypeDetail { get; set; } // USR_TYPE_DETAIL. A :  관리자(Admi)  O: 담당자(Operator)    T: 세무/회계사(Tax Accountant)  W: 노무  L:법무  P:특허  M:마케팅  F:자금  D:기술개발  Z:기타
        public string DbType { get; set; } // DB_TYPE. DB유형
        [Required]
        [Display(Name = "담당자명")]
        [MaxLength(40, ErrorMessage = "{0}은 최대 {1}자 입니다..")]
        public string Name { get; set; } // Name. 이름
        public string FaxNo { get; set; } // FAX_NO. Fax
        public string TelNo1 { get; set; } // TEL_NO. 전화번호
        public string TelNo2 { get; set; } // TEL_NO. 전화번호
        public string TelNo3 { get; set; } // TEL_NO. 전화번호
        public string MbNo1 { get; set; } // MB_NO. 휴대폰
        public string MbNo2 { get; set; } // MB_NO. 휴대폰
        public string MbNo3 { get; set; } // MB_NO. 휴대폰
        [Required]
        [Display(Name = "이메일")]
        public string Email1 { get; set; } // EMAIL. 이메일
        [Required]
        [Display(Name = "이메일")]
        [MaxLength(40, ErrorMessage = "{0}은 최대 {1}자 입니다..")]
        public string Email2 { get; set; } // EMAIL. 이메일
        public string PostNo { get; set; } // POST_NO. 우편번호
        public string Addr1 { get; set; } // ADDR_1. 주소1
        public string Addr2 { get; set; } // ADDR_2. 주소2
        public string AccountNo { get; set; } // ACCOUNT_NO. 계좌번호
        public string BankNm { get; set; } // BANK_NM. 계좌은행
        public string DeptNm { get; set; } // DEPT_NM. 소속부서명
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시

        //기업정보
        public string ComStatus { get; set; } // STATUS. 상태  N: 정상(Normal)  R: 탈퇴 요청(Retired)  D: 탈퇴 완료(Deleted)
        public string ComCompType { get; set; } // COMP_TYPE. 사업자유형  I: 개인사업자(Individual Company)  C: 법인사업자(Corporate Company)    다시정의해야함
        [Required]
        [Display(Name = "사업자번호")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "숫자만 입력할 수 있습니다.")]
        [StringLength(10, ErrorMessage = "{0}는 {1}자 입니다.", MinimumLength = 10)]
        public string ComRegistrationNo { get; set; } // REGISTRATION_NO. 사업자등록번호
        [Required]
        [Display(Name = "회사명")]
        [MaxLength(35, ErrorMessage = "{0}은 최대 {1}자 입니다..")]
        public string CompNm { get; set; } // COMP_NM. 회사명
        public string ComEmail { get; set; } // EMAIL. 대표이메일주소
        public string ComTelNo1 { get; set; } // TEL_NO. 대표전화번호
        public string ComTelNo2 { get; set; } // TEL_NO. 대표전화번호
        public string ComTelNo3 { get; set; } // TEL_NO. 대표전화번호
        public string ComPostNo { get; set; } // POST_NO. 우편번호
        public string ComAddr1 { get; set; } // ADDR_1. 주소1
        public string ComAddr2 { get; set; } // ADDR_2. 주소2
        [Required]
        [Display(Name = "대표자명")]
        [MaxLength(40, ErrorMessage = "{0}은 최대 {1}자 입니다..")]
        public string ComOwnNm { get; set; } // OWN_NM. 대표자명
        public string ComRegId { get; set; } // REG_ID. 등록자
        public DateTime? ComRegDt { get; set; } // REG_DT. 등록일시
        public string ComUpdId { get; set; } // UPD_ID. 수정자
        public DateTime? ComUpdDt { get; set; } // UPD_DT. 수정일시
        [Required]
        [Display(Name = "업태")]
        [MaxLength(40, ErrorMessage = "{0}은 최대 {1}자 입니다..")]
        public string ComBizClass { get; set; } // 업태
        [Required]
        [Display(Name = "업종")]
        [MaxLength(40, ErrorMessage = "{0}은 최대 {1}자 입니다..")]
        public string ComBizType { get; set; } // 업종

        //사업관리정보
        public int MngCompSn { get; set; } // COMP_SN. 기업식별자 (관리기관 식별자)
        public int BizWorkSn { get; set; } // BIZ_WORK_SN (Primary key). 사업식별자
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "아이디")]
        [MaxLength(12, ErrorMessage = "{0}는 최대 {1}자 입니다..")]
        [MinLength(6, ErrorMessage = "{0}는 {1}자 이상이어야 합니다.")]
        public string ID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "암호")]
        [MaxLength(12, ErrorMessage = "{0}는 최대 {1}자 입니다..")]
        [MinLength(8, ErrorMessage = "{0}는 {1}자 이상이어야 합니다.")]
        public string Password { get; set; }

        [Display(Name = "아이디 저장")]
        public bool RememberMe { get; set; }
    }
}
