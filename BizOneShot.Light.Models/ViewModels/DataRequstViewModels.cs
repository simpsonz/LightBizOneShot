using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string ResContents { get; set; } // RES_CONTENTS. 답변내용
        public string SenderComNm { get; set; } // 발신기업 사업자명
        public string SenderRegistrationNo { get; set; } // 발신기업 사업자번호
        public string ReceiverComNm { get; set; } // 발신기업 사업자명
        public string ReceiverRegistrationNo { get; set; } // 발신기업 사업자번호
    }
}
