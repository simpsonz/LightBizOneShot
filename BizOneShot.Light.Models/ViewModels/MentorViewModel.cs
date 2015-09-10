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
}
