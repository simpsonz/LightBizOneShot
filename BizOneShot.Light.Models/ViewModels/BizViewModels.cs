using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizOneShot.Light.Models.ViewModels
{
    class BizViewModels
    {
    }

    /// <summary>
    /// 사업관리자 DropDownList Model
    /// </summary>
    public class BizMngDropDownModel
    {
        public int CompSn { get; set; } // COMP_SN (Primary key). 기업식별자
        public string CompNm { get; set; } // COMP_NM. 회사명
    }

    /// <summary>
    /// 사업 DropDownList Model
    /// </summary>
    public class BizWorkDropDownModel
    {
        public int BizWorkSn { get; set; } // 사업식별자
        public string BizWorkNm { get; set; } // 사업명
    }
}
