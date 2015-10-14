using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.DareModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Web.ComLib
{
    public static class ReportHelper
    {
        /// <summary>
        /// 사업정보를 이용하여 사업의 시작년 부터 종료년 까지 년도 리스트 생성
        /// </summary>
        /// <param name="scBizWork"></param>
        /// <returns></returns>
        public static SelectList MakeBizYear(ScBizWork scBizWork)
        {
            //사업년도
            var year = new List<SelectListItem>();

            year.Add(new SelectListItem { Value = "", Text = "년도선택", Selected = true });

            if(scBizWork != null)
            {
                for(int i = scBizWork.BizWorkStDt.GetValueOrDefault().Year; i <= scBizWork.BizWorkEdDt.GetValueOrDefault().Year; i++)
                {
                    if (i > DateTime.Now.Year)
                    {
                        break;
                    }
                    year.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                }
            }

            SelectList yearList = new SelectList(year, "Value", "Text");

            return yearList;
        }

        public static SelectList MakeYear(int startYear)
        {
            //사업년도
            var year = new List<SelectListItem>();
            year.Add(new SelectListItem { Value = "", Text = "년도선택", Selected = true });
           
            for (int i = DateTime.Now.Year; i >= startYear; i--)
            {
                year.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() + "년" });
            }

            SelectList yearList = new SelectList(year, "Value", "Text");

            return yearList;
        }


        /// <summary>
        /// 사업정보를 이용하여 특정년도의 유효한 월을 생성
        /// </summary>
        /// <param name="scBizWork"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static SelectList MakeBizMonth(ScBizWork scBizWork, int year = 0)
        {
            //사업년도 범위의 월
            var momth = new List<SelectListItem>();

            momth.Add(new SelectListItem { Value = "", Text = "월선택", Selected = true });

            if(year > DateTime.Now.Year || scBizWork == null)
            {
                return new SelectList(momth, "Value", "Text");
            }

            //사업시작년과 종료년이 같을경우
            if (scBizWork.BizWorkStDt.GetValueOrDefault().Year == scBizWork.BizWorkEdDt.GetValueOrDefault().Year)
            {
                for (int i = scBizWork.BizWorkStDt.GetValueOrDefault().Month; i <= scBizWork.BizWorkEdDt.GetValueOrDefault().Month; i++)
                {
                    if (year == DateTime.Now.Year && i == DateTime.Now.Month)
                    {
                        break;
                    }
                    momth.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() + "월" });
                }
                return new SelectList(momth, "Value", "Text");
            }
            
            //사업시작년이 선택년과 같을경우
            if (year == scBizWork.BizWorkStDt.GetValueOrDefault().Year)
            {
                for (int i = scBizWork.BizWorkStDt.GetValueOrDefault().Month; i <= 12; i++)
                {
                    if (year == DateTime.Now.Year && i == DateTime.Now.Month)
                    {
                        break;
                    }
                    momth.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() + "월" });
                }
                return new SelectList(momth, "Value", "Text");
            }

            //사업 종료년이 선택년과 같을경우
            if (year == scBizWork.BizWorkEdDt.GetValueOrDefault().Year)
            {
                for (int i = 1; i<= scBizWork.BizWorkStDt.GetValueOrDefault().Month; i++)
                {
                    if (year == DateTime.Now.Year && i == DateTime.Now.Month)
                    {
                        break;
                    }
                    momth.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() + "월" });
                }
                return new SelectList(momth, "Value", "Text");
            }

            //선택한 년도가 사업시작년도와 종료년도 사이에 있을경우
            for (int i = 1; i <= 12; i++)
            {
                if (year == DateTime.Now.Year && i == DateTime.Now.Month)
                {
                    break;
                }
                momth.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() + "월" });
            }
            return new SelectList(momth, "Value", "Text");
        }

        public static SelectList MakeMonth(int year = 0)
        {
            //사업년도 범위의 월
            var momth = new List<SelectListItem>();

            momth.Add(new SelectListItem { Value = "", Text = "월선택", Selected = true });

            if(year == 0)
            {
                return new SelectList(momth, "Value", "Text");
            }


            //과거 년도 선택
            if (year < DateTime.Now.Year)
            {
                for (int i = 1; i <= 12; i++)
                {
                    momth.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() + "월" });
                }
                return new SelectList(momth, "Value", "Text");
            }

            //현재 년도 선택
            for (int i = 1; i < DateTime.Now.Month; i++)
            {
                momth.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() + "월" });
            }
            return new SelectList(momth, "Value", "Text");

        }


        //Cash Model 생성
        public static CashViewModel MakeCashViewModel(IList<SHUSER_SboMonthlyCashSelectReturnModel> cashList)
        {
            CashViewModel cashViewModel = new CashViewModel();

            cashViewModel.ForwardAmt = string.Format("{0:n0}", Convert.ToInt64((cashList[1].LAST_AMT / 1000)));   //이월액
            cashViewModel.LastMonthCashBalance = string.Format("{0:n0}", Convert.ToInt64((cashList[1].LAST_AMT / 1000))); //전월잔고
            cashViewModel.CashBalance = string.Format("{0:n0}", Convert.ToInt64((cashList[0].LAST_AMT / 1000))); //현재잔고
            cashViewModel.ReceivedAmt = string.Format("{0:n0}", Convert.ToInt64((cashList[0].INPUT_AMT / 1000))); //입금액
            cashViewModel.ContributionAmt = string.Format("{0:n0}", Convert.ToInt64((cashList[0].OUTPUT_AMT / 1000))); //출급액

            var qm = CalcBeforQuarter(int.Parse(cashList[0].ACC_YEAR), int.Parse(cashList[0].ACC_MONTH));

            Int64 avrBeforQuarter = 0;
            int cnt = 0;
            foreach(var cash in cashList)
            {
                if(int.Parse(cash.ACC_YEAR) == qm.Year && (int.Parse(cash.ACC_MONTH) >= qm.Quarter*3-2 && int.Parse(cash.ACC_MONTH) <= qm.Quarter * 3))
                {
                    avrBeforQuarter = avrBeforQuarter + Convert.ToInt64(cash.LAST_AMT);
                    cnt++;
                }
            }

            cashViewModel.BeforeQuarterlyCashBalance = string.Format("{0:n0}", ((avrBeforQuarter / 3) / 1000)); //전분기
            return cashViewModel;
        }

        public static QuarterModel CalcBeforQuarter(int year, int month)
        {
            QuarterModel qm = new QuarterModel();
            if(month >= 1 && month <= 3)
            {
                qm.Year = year - 1;
                qm.Quarter = 4;
            }
            else if(month >= 4 && month <= 6)
            {
                qm.Year = year;
                qm.Quarter = 1;
            }
            else if (month >= 7 && month <= 9)
            {
                qm.Year = year;
                qm.Quarter = 2;
            }
            else
            {
                qm.Year = year;
                qm.Quarter = 3;
            }


            return qm;
        }

    }
}