using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BizOneShot.Light.Models.ViewModels
{
    internal class BizViewModels
    {
    }

    /// <summary>
    ///     사업관리자 DropDownList Model
    /// </summary>
    public class BizMngDropDownModel
    {
        public int CompSn { get; set; } // COMP_SN (Primary key). 기업식별자
        public string CompNm { get; set; } // COMP_NM. 회사명
    }

    /// <summary>
    ///     사업 DropDownList Model
    /// </summary>
    public class BizWorkDropDownModel
    {
        public int BizWorkSn { get; set; } // 사업식별자
        public string BizWorkNm { get; set; } // 사업명
    }

    public class BizWorkViewModel
    {
        //사업정보
        public int BizWorkSn { get; set; } // BIZ_WORK_SN (Primary key). 사업식별자
        public int CompSn { get; set; } // COMP_SN. 주관기관식별자

        [Required]
        [Display(Name = "사업명")]
        public string BizWorkNm { get; set; } // BIZ_WORK_NM. 사업명

        [Required]
        [Display(Name = "사업개요")]
        public string BizWorkSummary { get; set; } // BIZ_WORK_SUMMARY. 사업개요

        [Required]
        [Display(Name = "사업시작일")]
        public string BizWorkStDt { get; set; } // BIZ_WORK_ST_DT. 사업시작일

        [Required]
        [Display(Name = "사업종료일")]
        public string BizWorkEdDt { get; set; } // BIZ_WORK_ED_DT. 사업종료일

        [Required]
        [Display(Name = "주관기관")]
        public string MngDept { get; set; } // MNG_DEPT 주관기관

        //담당자
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
        public string UsrTypeDetail { get; set; }
        // USR_TYPE_DETAIL. A :  관리자(Admi)  O: 담당자(Operator)    T: 세무/회계사(Tax Accountant)  W: 노무  L:법무  P:특허  M:마케팅  F:자금  D:기술개발  Z:기타

        [Required]
        [Display(Name = "담당자명")]
        [MaxLength(40, ErrorMessage = "{0}은 최대 {1}자 입니다..")]
        public string Name { get; set; } // Name. 이름

        [Required]
        public string TelNo1 { get; set; } // TEL_NO. 전화번호

        [Required]
        [Display(Name = "전화번호")]
        public string TelNo2 { get; set; } // TEL_NO. 전화번호

        [Required]
        [Display(Name = "전화번호")]
        public string TelNo3 { get; set; } // TEL_NO. 전화번호
        public string TelNo { get; set; } // TEL_NO. 전화번호

        [Required]
        public string MbNo1 { get; set; } // MB_NO. 휴대폰

        [Required]
        [Display(Name = "핸드폰번호")]
        public string MbNo2 { get; set; } // MB_NO. 휴대폰

        [Required]
        [Display(Name = "핸드폰번호")]
        public string MbNo3 { get; set; } // MB_NO. 휴대폰
        public string MbNo { get; set; } // MB_NO. 휴대폰

        [Required]
        [Display(Name = "이메일")]
        public string Email1 { get; set; } // EMAIL. 이메일

        [Required]
        [Display(Name = "이메일")]
        public string Email2 { get; set; } // EMAIL. 이메일
        public string Email { get; set; } // EMAIL. 이메일

        [Required]
        public string FaxNo1 { get; set; } // FAX_NO. Fax

        [Required]
        [Display(Name = "팩스번호")]
        public string FaxNo2 { get; set; } // FAX_NO. Fax

        [Required]
        [Display(Name = "팩스번호")]
        public string FaxNo3 { get; set; } // FAX_NO. Fax
        public string FaxNo { get; set; } // FAX_NO. Fax

        [Required]
        [Display(Name = "부서명")]
        public string DeptNm { get; set; } // DEPT_NM. 소속부서명

        public string ComCount { get; set; } // 참여기업수
    }

    public class CompanyMngViewModel
    {
        public int BizWorkSn { get; set; } // BIZ_WORK_SN (Primary key). 사업식별자
        public int MngCompSn { get; set; } // COMP_SN (Primary key). 관리기업식별자
        public int CompSn { get; set; } // COMP_SN (Primary key). 기업식별자
        public string ApproveRequestDate { get; set; } // 승인요청일
        public string CompNm { get; set; } // COMP_NM. 회사명
        public string RegistrationNo { get; set; } // REGISTRATION_NO. 사업자등록번호
        public string OwnNm { get; set; } // OWN_NM. 대표자명
        public string BizWorkNm { get; set; } // BIZ_WORK_NM. 사업명
        public string Status { get; set; } // STATUS. 상태 R : 승인대기 A : 승인
        public string PostNo { get; set; } // POST_NO. 우편번호
        public string Addr1 { get; set; } // ADDR_1. 주소1
        public string Addr2 { get; set; } // ADDR_2. 주소2
        public string Name { get; set; } // Name. 이름
        public string TelNo { get; set; } // TEL_NO. 전화번호
        public string MbNo { get; set; } // MB_NO. 휴대폰
        public string Email { get; set; } // EMAIL. 이메일
        public string MentorLoginId { get; set; } // Name. 멘토이름
        public string MentorName { get; set; } // Name. 이름
        public string MentorTelNo { get; set; } // TEL_NO. 전화번호
        public string MentorMbNo { get; set; } // MB_NO. 휴대폰
        public string MentorEmail { get; set; } // EMAIL. 이메일

        public IList<BizTypeViewModel> BizTypes { get; set; } //업태, 업종
    }
}