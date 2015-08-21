using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizOneShot.Light.Web.ViewModels
{
    public class CsViewModels
    {

    }

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
        public int TOT_CNT { get; set; }
        public int FAQ_SN { get; set; }
        public DateTime REG_DT { get; set; }
        public int REG_USR_SN { get; set; }
        public DateTime UPD_DT { get; set; }
        public int UPD_USR_SN { get; set; }
        public string STAT { get; set; }
        public string QCL_NM { get; set; }
        public string QST_TXT { get; set; }
        public string ANS_TXT { get; set; }
    }
}