using System;
using System.Collections.Generic;

namespace BizOneShot.Light.Models.ViewModels
{
    internal class BasicSurveyViewModels
    {
    }

    public class QuesMasterViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string RegistrationNo { get; set; } // REGISTRATION_NO
        public int? BasicYear { get; set; } // BASIC_YEAR
        public string SaveStatus { get; set; } // SAVE_STATUS
        public string Status { get; set; } // STATUS
        public string SubmitType { get; set; } // Submit 방식
        public QuesWriterViewModel QuesWriter { get; set; } // 작성자 정보
    }

    public class QuesWriterViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string CompNm { get; set; } // COMP_NM
        public string Name { get; set; } // Name
        public string DeptNm { get; set; } // DEPT_NM
        public string Position { get; set; } // POSITION
        public string TelNo { get; set; } // TEL_NO
        public string Email { get; set; } // EMAIL
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT
    }

    public class QuestionDropDownModel
    {
        public string SnStatus { get; set; } // QUESTION_SN (Primary key)
        public int? BasicYear { get; set; } // BASIC_YEAR
    }

    public class QuesCompanyInfoViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string Status { get; set; } // STATUS
        public string CompNm { get; set; } // COMP_NM
        public string EngCompNm { get; set; } // ENG_COMP_NM
        public string TelNo { get; set; } // TEL_NO
        public string FaxNo { get; set; } // FAX_NO
        public string Name { get; set; } // NAME
        public string Email { get; set; } // EMAIL
        public string RegistrationNo { get; set; } // REGISTRATION_NO
        public string CoRegistrationNo { get; set; } // CO_REGISTRATION_NO
        public string PublishDt { get; set; } // PUBLISH_DT
        public string HomeUrl { get; set; } // HOME_URL
        public string CompAddr { get; set; } // COMP_ADDR
        public string FactoryAddr { get; set; } // FACTORY_ADDR
        public string LabAddr { get; set; } // LAB_ADDR
        public string FacPossessYn { get; set; } // FAC_POSSESS_YN
        public string RndYn { get; set; } // RND_YN
        public string ProductNm1 { get; set; } // PRODUCT_NM1
        public string ProductNm2 { get; set; } // PRODUCT_NM2
        public string ProductNm3 { get; set; } // PRODUCT_NM3
        public bool MarketPublic { get; set; } // MARKET_PUBLIC
        public bool MarketCivil { get; set; } // MARKET_CIVIL
        public bool MarketConsumer { get; set; } // MARKET_CONSUMER
        public bool MarketForeing { get; set; } // MARKET_FOREING
        public bool MarketEtc { get; set; } // MARKET_ETC
        public string CompType { get; set; } // COMP_TYPE
        public string ResidentType { get; set; } // RESIDENT_TYPE
        public bool CertiVenture { get; set; } // CERTI_VENTURE
        public bool CertiRnd { get; set; } // CERTI_RND
        public bool CertiMainbiz { get; set; } // CERTI_MAINBIZ
        public bool CertiInnobiz { get; set; } // CERTI_INNOBIZ
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT
        public string SubmitType { get; set; } // Submit 방식
    }

    public class QuesCompExtentionViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string Status { get; set; } // STATUS
        public string PresidentNm { get; set; } // PRESIDENT_NM
        public string BirthDate { get; set; } // BIRTH_DATE
        public string AcademicDegree { get; set; } // ACADEMIC_DEGREE
        public string Major { get; set; } // MAJOR
        public string CareerComp1 { get; set; } // CAREER_COMP1
        public string Job1 { get; set; } // JOB1
        public string CareerComp2 { get; set; } // CAREER_COMP2
        public string Job2 { get; set; } // JOB2
        public string CareerComp3 { get; set; } // CAREER_COMP3
        public string Job3 { get; set; } // JOB3
        public string CareerBasicYear { get; set; } // CAREER_BASIC_YEAR
        public string CareerBasicMonth { get; set; } // CAREER_BASIC_MONTH
        public int? TotalYear { get; set; } // TOTAL_YEAR
        public int? TotalMonth { get; set; } // TOTAL_MONTY
        public string HistotyDate10 { get; set; } // HISTOTY_DATE10
        public string HistotyDate9 { get; set; } // HISTOTY_DATE9
        public string HistotyDate8 { get; set; } // HISTOTY_DATE8
        public string HistotyDate7 { get; set; } // HISTOTY_DATE7
        public string HistotyDate6 { get; set; } // HISTOTY_DATE6
        public string HistotyDate5 { get; set; } // HISTOTY_DATE5
        public string HistotyDate4 { get; set; } // HISTOTY_DATE4
        public string HistotyDate3 { get; set; } // HISTOTY_DATE3
        public string HistotyDate2 { get; set; } // HISTOTY_DATE2
        public string HistotyDate1 { get; set; } // HISTOTY_DATE1
        public string HistoryContent10 { get; set; } // HISTORY_CONTENT10
        public string HistoryContent9 { get; set; } // HISTORY_CONTENT9
        public string HistoryContent8 { get; set; } // HISTORY_CONTENT8
        public string HistoryContent7 { get; set; } // HISTORY_CONTENT7
        public string HistoryContent6 { get; set; } // HISTORY_CONTENT6
        public string HistoryContent5 { get; set; } // HISTORY_CONTENT5
        public string HistoryContent4 { get; set; } // HISTORY_CONTENT4
        public string HistoryContent3 { get; set; } // HISTORY_CONTENT3
        public string HistoryContent2 { get; set; } // HISTORY_CONTENT2
        public string HistoryContent1 { get; set; } // HISTORY_CONTENT1
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT
        public string SubmitType { get; set; } // Submit 방식
    }

    public class QuesViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string Status { get; set; } // STATUS
    }

    public class BizCheck02ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> BizPurpose { get; set; }
        public IList<QuesCheckListViewModel> Leadership { get; set; }
    }

    public class BizCheck03ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> LeaderReliability { get; set; }
    }

    public class BizCheck04ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> WorkEnv { get; set; }
        public QuesYearListViewModel TotalEmp { get; set; }
        public QuesYearListViewModel MoveEmp { get; set; }
        public QuesYearListViewModel NewEmp { get; set; }
    }

    public class QuesCheckListViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public int CheckListSn { get; set; } // CHECK_LIST_SN (Primary key)
        public string Title { get; set; } // TITLE
        public string Content1 { get; set; } // CONTENT
        public string Content2 { get; set; } // CONTENT
        public bool AnsVal { get; set; } // ANS_VAL
    }

    public class QuesYearListViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public int CheckListSn { get; set; } // CHECK_LIST_SN (Primary key)
        public int? BasicYear { get; set; } // BASIC_YEAR
        public string D { get; set; } // D
        public string D451 { get; set; } // D-1
        public string D452 { get; set; } // D-2
        public string D453 { get; set; } // D-3
        public string SmallClassCd { get; set; } // SMALL_CLASS_CD
        public string DetailCd { get; set; } // DETAIL_CD
        public string RegId { get; set; } // REG_ID
        public DateTime? RegDt { get; set; } // REG_DT
        public string UpdId { get; set; } // UPD_ID
        public DateTime? UpdDt { get; set; } // UPD_DT
    }


    public class BizCheck05ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> InfoSystem { get; set; }
    }


    public class BizCheck06ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public QuesYearListViewModel TotalEmp { get; set; } //전체 임직원
        public QuesYearListViewModel RndEmp { get; set; } // 연구개발 인력
        public QuesYearListViewModel DoctorEmp { get; set; } //박사급
        public QuesYearListViewModel MasterEmp { get; set; } //석사급
        public QuesYearListViewModel CollegeEmp { get; set; } //학사급
        public QuesYearListViewModel TechEmp { get; set; } //기능사급
        public QuesYearListViewModel HighEmp { get; set; } //고졸이하
    }

    public class BizCheck07ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> BizCapa { get; set; }
        public QuesYearListViewModel BizResult { get; set; }
        public QuesYearListViewModel BizResultCnt { get; set; }
    }

    public class BizCheck08ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> ProducEquip { get; set; }
        public IList<QuesCheckListViewModel> ProcessControl { get; set; }
    }

    public class BizCheck09ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> QualityControl { get; set; }
    }

    public class BizCheck10ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> MarketingPlan { get; set; }
    }

    public class BizCheck11ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> CustomerMng { get; set; }
    }

    public class BizCheck12ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public IList<QuesCheckListViewModel> HRMng { get; set; }
        public IList<QuesCheckListViewModel> HRMaintenance { get; set; }
    }


    public class BizCheck13ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public QuesYearListViewModel RegPatent { get; set; } //등록된 특허
        public QuesYearListViewModel RegUtilityModel { get; set; } // 등록된 실용신안
        public QuesYearListViewModel ApplyPatent { get; set; } //출원중인 특허
        public QuesYearListViewModel ApplyUtilityModel { get; set; } //출원중인 실용신안
        public QuesYearListViewModel Etc { get; set; } //기타
        public QuesYearListViewModel TotalEmp { get; set; } //전체임직원
    }

    public class OrgCheck01ViewModel
    {
        public int QuestionSn { get; set; } // QUESTION_SN (Primary key)
        public string SubmitType { get; set; } // Submit 방식
        public string Status { get; set; } // STATUS
        public OrgCompositionViewModel Management { get; set; } //기획/관리
        public OrgCompositionViewModel Produce { get; set; } // 생산/생산관리
        public OrgCompositionViewModel RND { get; set; } // 연구개발/연구지원
        public OrgCompositionViewModel Salse { get; set; } // 마케팅기획 / 판매영업
        public int? OfficerSumCount { get; set; } // OFFICER_COUNT
        public int? ChiefSumCount { get; set; } // CHIEF_COUNT
        public int? StaffSumCount { get; set; } // STAFF_COUNT
        public int? BeginnerSumCount { get; set; } // BEGINNER_COUNT
        public int? TotalSumCount { get; set; } // BEGINNER_COUNT
    }

    public class OrgCompositionViewModel
    {
        public string Dept1 { get; set; } // DEPT1
        public string Dept2 { get; set; } // DEPT2
        public int OfficerCount { get; set; } // OFFICER_COUNT
        public int ChiefCount { get; set; } // CHIEF_COUNT
        public int StaffCount { get; set; } // STAFF_COUNT
        public int BeginnerCount { get; set; } // BEGINNER_COUNT
        public int PartialSum { get; set; } // 부문별 합계
    }
}