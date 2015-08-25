using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizOneShot.Light.Models.ViewModels
{
    public class CsViewModels
    {

    }

    #region FAQ 뷰모델
    public class SelectAdminFaqListViewModel
    {
        public int TOT_CNT { get; set; }
        public int FAQ_SN { get; set; }
        public DateTime REG_DT { get; set; }
        public string QST_TXT { get; set; }
        public string ANS_TXT { get; set; }
        public string QCL_TYPE { get; set; }
        public string QCL_NM { get; set; }
        public int QCL_SN { get; set; }

        public int PRE_FAQ_SN { get; set; }
        public int NEXT_FAQ_SN { get; set; }
        public string PRE_QST_TXT { get; set; }
        public string NEXT_QST_TXT { get; set; }
    }

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

    #endregion

}