// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;

namespace BizOneShot.Light.Models.WebModels
{
    // QUES_CHECK_LIST
    [GeneratedCodeAttribute("EF.Reverse.POCO.Generator", "2.15.1.0")]
    public class QuesCheckList
    {
        public int CheckListSn { get; set; } // CHECK_LIST_SN (Primary key)
        public string LagreClassCd { get; set; } // LAGRE_CLASS_CD
        public string MidiumClassCd { get; set; } // MIDIUM_CLASS_CD
        public string SmallClassCd { get; set; } // SMALL_CLASS_CD
        public string DetailCd { get; set; } // DETAIL_CD
        public string Title { get; set; } // TITLE
        public string Content { get; set; } // CONTENT
        public string CurrentUseYn { get; set; } // CURRENT_USE_YN

        // Reverse navigation
        public virtual ICollection<QuesResult1> QuesResult1 { get; set; } // Many to many mapping
        public virtual ICollection<QuesResult2> QuesResult2 { get; set; } // Many to many mapping
        
        public QuesCheckList()
        {
            QuesResult1 = new List<QuesResult1>();
            QuesResult2 = new List<QuesResult2>();
        }
    }

}
