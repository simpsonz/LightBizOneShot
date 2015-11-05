using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BizOneShot.Light.Models.ViewModels
{
    public class DataRequstViewModels
    {
        public int ReqDocSn { get; set; } // REQ_DOC_SN (Primary key). 자료요청식별자(순번)
        public string SenderId { get; set; } // SENDER_ID. 발신자식별자
        public string ReceiverId { get; set; } // RECEIVER_ID. 수신자식별자
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  D: 삭제됨(Deleted)
        public string ChkYn { get; set; } // CHK_YN. 수신확인여부  Y: 수신함  N 수신안함
        public DateTime? ReqDt { get; set; } // REQ_DT. 요청일시
        public string ReqSubject { get; set; } // REQ_SUBJECT. 송신제목
        public string ReqContents { get; set; } // REQ_CONTENTS. 송신내용
        public DateTime? ResDt { get; set; } // RES_DT. 답변일시

        [Required]
        [Display(Name = "답변내용")]
        public string ResContents { get; set; } // RES_CONTENTS. 답변내용
        public string SenderComNm { get; set; } // 발신기업 사업자명
        public string SenderName { get; set; } //발신자 명
        public string SenderRegistrationNo { get; set; } // 발신기업 사업자번호
        public string ReceiverComNm { get; set; } // 수신기업 사업자명
        public string ReceiverName { get; set; } // 수신담당자명
        public string ReceiverRegistrationNo { get; set; } // 수신기업 사업자번호
        public IList<FileInfoViewModel> SenderFiles { get; set; }
        public IList<FileInfoViewModel> ReceiverFiles { get; set; }
    }

    public class RegSendViewModels
    {
        public int ReqDocSn { get; set; } // REQ_DOC_SN (Primary key). 자료요청식별자(순번)
        public string SenderId { get; set; } // SENDER_ID. 발신자식별자
        public string ReceiverId { get; set; } // RECEIVER_ID. 수신자식별자
        public string Status { get; set; } // STATUS. 상태  N: 정상(Normal)  D: 삭제됨(Deleted)
        public string ChkYn { get; set; } // CHK_YN. 수신확인여부  Y: 수신함  N 수신안함
        public DateTime? ReqDt { get; set; } // REQ_DT. 요청일시

        [Required]
        [Display(Name = "제목")]
        public string ReqSubject { get; set; } // REQ_SUBJECT. 송신제목

        [Required]
        [Display(Name = "내용")]
        public string ReqContents { get; set; } // REQ_CONTENTS. 송신내용
        public DateTime? ResDt { get; set; } // RES_DT. 답변일시
        public string ResContents { get; set; } // RES_CONTENTS. 답변내용
        public string SenderComNm { get; set; } // 발신기업 사업자명
        public string SenderName { get; set; } //발신자 명
        public string SenderRegistrationNo { get; set; } // 발신기업 사업자번호
        public string ReceiverComNm { get; set; } // 수신기업 사업자명
        public string ReceiverName { get; set; } // 수신담당자명
        public string ReceiverRegistrationNo { get; set; } // 수신기업 사업자번호
        public IList<FileInfoViewModel> SenderFiles { get; set; }
        public IList<FileInfoViewModel> ReceiverFiles { get; set; }
    }

    public class QaRequstViewModels
    {
        public int UsrQaSn { get; set; } // USR_QA_SN (Primary key). 회원문의답변순번
        public string QuestionId { get; set; } // QUESTION_ID. 질문자회원순번
        public string AnswerId { get; set; } // ANSWER_ID. 답변자회원순번
        public DateTime? AskDt { get; set; } // ASK_DT. 질문일시
        public string Subject { get; set; } // SUBJECT. 제목
        public string Question { get; set; } // QUESTION. 질문
        public DateTime? AnsDt { get; set; } // ANS_DT. 답변일시
        public string Answer { get; set; } // ANSWER. 답변
        public string AnsYn { get; set; } // ANS_YN. 답변완료여부  Y:답변완료함  N:답변완료안함(default)
        public string QuestionComNm { get; set; } // 발신기업 사업자명
        public string QuestionRegistrationNo { get; set; } // 발신기업 사업자번호
        public string AnswerComNm { get; set; } // 수신기업 사업자명
        public string AnswerRegistrationNo { get; set; } // 수신기업 사업자번호
    }

    public class QaRequstDetailViewModel
    {
        public int PreUsrQaSn { get; set; }
        public string PreSubject { get; set; }
        public string PreAnsYn { get; set; }

        public int NextUsrQaSn { get; set; }
        public string NextSubject { get; set; }
        public string NextAnsYn { get; set; }

        public int UsrQaSn { get; set; } // USR_QA_SN (Primary key). 회원문의답변순번
        public string QuestionId { get; set; } // QUESTION_ID. 질문자회원순번
        public string AnswerId { get; set; } // ANSWER_ID. 답변자회원순번
        public DateTime? AskDt { get; set; } // ASK_DT. 질문일시
        public string Subject { get; set; } // SUBJECT. 제목
        public string Question { get; set; } // QUESTION. 질문
        public DateTime? AnsDt { get; set; } // ANS_DT. 답변일시
        public string Answer { get; set; } // ANSWER. 답변
        public string AnsYn { get; set; } // ANS_YN. 답변완료여부  Y:답변완료함  N:답변완료안함(default)
        public string QuestionComNm { get; set; } // 발신기업 사업자명
        public string QuestionRegistrationNo { get; set; } // 발신기업 사업자번호
        public string AnswerComNm { get; set; } // 수신기업 사업자명
        public string AnswerRegistrationNo { get; set; } // 수신기업 사업자번호
    }


    public class RegQaRequestViewModels
    {
        public int UsrQaSn { get; set; } // USR_QA_SN (Primary key). 회원문의답변순번
        public string QuestionId { get; set; } // QUESTION_ID. 질문자회원순번
        public string AnswerId { get; set; } // ANSWER_ID. 답변자회원순번
        public DateTime? AskDt { get; set; } // ASK_DT. 질문일시

        [Required]
        [Display(Name = "문의제목")]
        public string Subject { get; set; } // SUBJECT. 제목

        [Required]
        [Display(Name = "문의내용")]
        public string Question { get; set; } // QUESTION. 질문
        public DateTime? AnsDt { get; set; } // ANS_DT. 답변일시
        public string Answer { get; set; } // ANSWER. 답변
        public string AnsYn { get; set; } // ANS_YN. 답변완료여부  Y:답변완료함  N:답변완료안함(default)
        public string QuestionComNm { get; set; } // 발신기업 사업자명
        public string QuestionRegistrationNo { get; set; } // 발신기업 사업자번호
        public string AnswerComNm { get; set; } // 수신기업 사업자명
        public string AnswerRegistrationNo { get; set; } // 수신기업 사업자번호
    }
}