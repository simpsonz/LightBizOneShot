using System;
using System.Collections.Generic;

namespace BizOneShot.Light.Models.ViewModels
{
    public class CsViewModels
    {
    }

    #region FAQ 뷰모델

    //public class SelectAdminFaqListViewModel
    //{
    //    public int TOT_CNT { get; set; }
    //    public int FAQ_SN { get; set; }
    //    public DateTime REG_DT { get; set; }
    //    public string QST_TXT { get; set; }
    //    public string ANS_TXT { get; set; }
    //    public string QCL_TYPE { get; set; }
    //    public string QCL_NM { get; set; }
    //    public int QCL_SN { get; set; }

    //    public int PRE_FAQ_SN { get; set; }
    //    public int NEXT_FAQ_SN { get; set; }
    //    public string PRE_QST_TXT { get; set; }
    //    public string NEXT_QST_TXT { get; set; }
    //}

    public class FaqViewModel
    {
        public int FaqSn { get; set; } // FAQ_SN (Primary key). 자주하는질문순번
        public int? QclSn { get; set; } // QCL_SN. 질문분류코드(순번)
        public string QstTxt { get; set; } // QST_TXT. 질문
        public string AnsTxt { get; set; } // ANS_TXT. 답변
        public string Stat { get; set; } // STAT. 상태  N: 정상(Normal)  D: 사용안함(Deleted)
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시
        public string QclNm { get; set; } // QCL_NM. 질문분류 이름
    }

    public class QclDropDownModel
    {
        public int QclSn { get; set; } // QCL_SN (Primary key). 질문분류코드(순번)
        public string QclNm { get; set; } // QCL_NM. 질문분류명
    }

    #endregion

    #region Notice(공지사항) 뷰모델

    public class NoticeViewModel
    {
        public int NoticeSn { get; set; } // NOTICE_SN (Primary key). 공지사항순번
        public string Subject { get; set; } // SUBJECT. 제목
        public string RmkTxt { get; set; } // RMK_TXT. 내용
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  D: 사용안함(Deleted)
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시
    }

    public class NoticeDetailViewModel
    {
        public int PreNoticeSn { get; set; }
        public string PreSubject { get; set; }
        public int NextNoticeSn { get; set; }
        public string NextSubject { get; set; }
        public int NoticeSn { get; set; } // NOTICE_SN (Primary key). 공지사항순번
        public string Subject { get; set; } // SUBJECT. 제목
        public string RmkTxt { get; set; } // RMK_TXT. 내용
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  D: 사용안함(Deleted)
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시
    }

    #endregion

    #region Manual(매뉴얼) 뷰모델

    public class ManualViewModel
    {
        public int FormSn { get; set; } // FORM_SN (Primary key). 서식식별자(순번)
        public string Subject { get; set; } // SUBJECT. 제목
        public string Contents { get; set; } // CONTENTS. 내용
        public string FormType { get; set; } // FORM_TYPE. 서식구분  M : 메뉴얼  N : 일반서식  S : SCP표준서식  P : 프로그램
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  D: 사용안함(Deleted)
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
        public string UpdId { get; set; } // UPD_ID. 수정자
        public DateTime? UpdDt { get; set; } // UPD_DT. 수정일시
    }

    public class ManualDetailViewModel
    {
        public int PreFormSn { get; set; }
        public string PreSubject { get; set; }
        public int NextFormSn { get; set; }
        public string NextSubject { get; set; }

        public ManualViewModel Manual { get; set; }

        public IList<FileInfoViewModel> ManualFiles { get; set; }
    }

    public class ManualFileViewModel
    {
        public int FileSn { get; set; } // FILE_SN (Primary key). 파일식별자
        public int FormSn { get; set; } // FORM_SN. 서식식별자(순번)
    }

    #endregion

    public class FileInfoViewModel
    {
        public int FileSn { get; set; } // FILE_SN (Primary key). 파일식별자
        public string FileNm { get; set; } // FILE_NM. 파일명
        public string FilePath { get; set; } // FILE_PATH. 파일경로
        public string Status { get; set; } // STATUS. 상태  N: 정상  D: 삭제됨(Deleted)
        public string RegId { get; set; } // REG_ID. 등록자
        public DateTime? RegDt { get; set; } // REG_DT. 등록일시
    }
}