using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizOneShot.Light.Models.ViewModels
{
    class ReportViewModels
    {
    }

    public class YearDropDownModel
    {
        public int Year { get; set; } // year
        public string YearText { get; set; } // display year
    }

    public class MonthDropDownModel
    {
        public int Month { get; set; } // month
        public string MonthText { get; set; } // display month
    }

    public class QuarterDropDownModel
    {
        public int Quarter { get; set; } // quarter
        public string QuarterText { get; set; } // display quarter
    }
}
