﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.DareModels;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using System.Data.SqlClient;

namespace BizOneShot.Light.Web.ComLib
{
    public static class ReportHelper
    {
        public static SelectList MakeBizWorkList(IList<ScBizWork> scBizWorkList)
        {
            var bizWorkList = new List<SelectListItem>();
            bizWorkList.Add(new SelectListItem { Value = "0", Text = "사업명 선택", Selected = true });

            if(scBizWorkList != null)
            {
                foreach(var item in scBizWorkList)
                { 
                    bizWorkList.Add(new SelectListItem { Value = item.BizWorkSn.ToString(), Text = item.BizWorkNm });
                }
            }

            SelectList list = new SelectList(bizWorkList, "Value", "Text");
            return list;
        }

        public static SelectList MakeCompanyList(IList<ScCompInfo> scCompInfoList)
        {
            var companyList = new List<SelectListItem>();
            companyList.Add(new SelectListItem { Value = "0", Text = "기업명 선택", Selected = true });

            if (scCompInfoList != null)
            {
                foreach (var item in scCompInfoList)
                {
                    companyList.Add(new SelectListItem { Value = item.CompSn.ToString(), Text = item.CompNm });
                }
            }

            SelectList list = new SelectList(companyList, "Value", "Text");
            return list;
        }

        public static SelectList MakeReportStatusList()
        {
            var statusList = new List<SelectListItem>();
            statusList.Add(new SelectListItem { Value = "0", Text = "작성상태 선택", Selected = true });
            statusList.Add(new SelectListItem { Value = "T", Text = "미작성", Selected = true });
            statusList.Add(new SelectListItem { Value = "W", Text = "작성중", Selected = true });
            statusList.Add(new SelectListItem { Value = "C", Text = "작성완료", Selected = true });
            SelectList list = new SelectList(statusList, "Value", "Text");
            return list;
        }
        /// <summary>
        /// 사업정보를 이용하여 사업의 시작년 부터 종료년 까지 년도 리스트 생성
        /// </summary>
        /// <param name="scBizWork"></param>
        /// <returns></returns>
        public static SelectList MakeBizYear(ScBizWork scBizWork)
        {
            //사업년도
            var year = new List<SelectListItem>();

            year.Add(new SelectListItem { Value = "0", Text = "년도선택", Selected = true });

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
            year.Add(new SelectListItem { Value = "0", Text = "년도선택", Selected = true });
           
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

            momth.Add(new SelectListItem { Value = "0", Text = "월선택", Selected = true });

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

            momth.Add(new SelectListItem { Value = "0", Text = "월선택", Selected = true });

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

        public static SelectList MakeBizQuarter(ScBizWork scBizWork, int year = 0)
        {
            //사업년도 범위의 월
            var quarter = new List<SelectListItem>();

            quarter.Add(new SelectListItem { Value = "0", Text = "분기선택", Selected = true });

            if (year > DateTime.Now.Year || scBizWork == null)
            {
                return new SelectList(quarter, "Value", "Text");
            }

            //사업시작년과 종료년이 같을경우
            if (scBizWork.BizWorkStDt.GetValueOrDefault().Year == scBizWork.BizWorkEdDt.GetValueOrDefault().Year)
            {
                for (int i = scBizWork.BizWorkStDt.GetValueOrDefault().Month; i <= scBizWork.BizWorkEdDt.GetValueOrDefault().Month; i++)
                {
                    int tempQuarter = 0;
                    if (year == DateTime.Now.Year && i == DateTime.Now.Month)
                    {
                        break;
                    }

                    int temp = ((i - 1) / 3) + 1;
                    if(tempQuarter < temp)
                    {
                        tempQuarter = temp;
                        quarter.Add(new SelectListItem { Value = temp.ToString(), Text = temp.ToString() + "분기" });
                    }
                }
                return new SelectList(quarter, "Value", "Text");
            }

            //사업시작년이 선택년과 같을경우
            if (year == scBizWork.BizWorkStDt.GetValueOrDefault().Year)
            {
                int tempQuarter = 0;
                for (int i = scBizWork.BizWorkStDt.GetValueOrDefault().Month; i <= 12; i++)
                {
                    if (year == DateTime.Now.Year && i == DateTime.Now.Month)
                    {
                        break;
                    }
                    int temp = ((i - 1) / 3) + 1;
                    if (tempQuarter < temp)
                    {
                        tempQuarter = temp;
                        quarter.Add(new SelectListItem { Value = temp.ToString(), Text = temp.ToString() + "분기" });
                    }
                }
                return new SelectList(quarter, "Value", "Text");
            }

            //사업 종료년이 선택년과 같을경우
            if (year == scBizWork.BizWorkEdDt.GetValueOrDefault().Year)
            {
                int tempQuarter = 0;
                for (int i = 1; i <= scBizWork.BizWorkStDt.GetValueOrDefault().Month; i++)
                {
                    if (year == DateTime.Now.Year && i == DateTime.Now.Month)
                    {
                        break;
                    }
                    int temp = ((i - 1) / 3) + 1;
                    if (tempQuarter < temp)
                    {
                        tempQuarter = temp;
                        quarter.Add(new SelectListItem { Value = temp.ToString(), Text = temp.ToString() + "분기" });
                    }
                }
                return new SelectList(quarter, "Value", "Text");
            }

            //선택한 년도가 사업시작년도와 종료년도 사이에 있을경우
            if(true)
            { 
                int tempQuarter = 0;
                for (int i = 1; i <= 12; i++)
                {
                
                    if (year == DateTime.Now.Year && i == DateTime.Now.Month)
                    {
                        break;
                    }
                    int temp = ((i - 1) / 3) + 1;
                    if (tempQuarter < temp)
                    {
                        tempQuarter = temp;
                        quarter.Add(new SelectListItem { Value = temp.ToString(), Text = temp.ToString() + "분기" });
                    }
                }
                return new SelectList(quarter, "Value", "Text");
            }
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


        //Sales Model 생성
        public static SalesViewModel MakeSalesViewModel(IList<SHUSER_SboMonthlySalesSelectReturnModel> slaesList, SHUSER_SboMonthlyYearSalesSelectReturnModel yearTotal)
        {
            SalesViewModel salesViewModel = new SalesViewModel();

            salesViewModel.CurMonth = string.Format("{0:n0}", Convert.ToInt64((slaesList[0].SALES_AMT / 1000)));   //현월매출
            salesViewModel.LastMonth = string.Format("{0:n0}", Convert.ToInt64((slaesList[1].SALES_AMT / 1000))); //전월매출

            if(yearTotal == null)
            {
                salesViewModel.CurYear = "0";
            }
            else
            { 
                salesViewModel.CurYear = string.Format("{0:n0}", Convert.ToInt64((yearTotal.SALES_AMT / 1000))); // 누적매출
            }
            return salesViewModel;
        }

        //이익분석 Model 생성
        public static TotalCostViewModel MakeCostAnalysisViewModel(SHUSER_SboMonthlyCostAnalysisSelectReturnModel cost)
        {
            TotalCostViewModel totalCostViewModel = new TotalCostViewModel();
            totalCostViewModel.AllOtherAmt = string.Format("{0:n0}", Convert.ToInt64((cost.ALL_OTHER_AMT / 1000)));   //영업외비용
            totalCostViewModel.ManufacturingAmt = string.Format("{0:n0}", Convert.ToInt64((cost.MANUFACTURING_AMT / 1000)));   //제조비
            totalCostViewModel.MaterialAmt = string.Format("{0:n0}", Convert.ToInt64((cost.MATERIALS_AMT / 1000)));   //자재비
            totalCostViewModel.OperatingAmt = string.Format("{0:n0}", Convert.ToInt64((cost.OPERATING_AMT / 1000)));   //판관비
            totalCostViewModel.ProfitAmt = string.Format("{0:n0}", Convert.ToInt64((cost.PROFIT_AMT / 1000)));   //이익
            totalCostViewModel.SalesAmt = string.Format("{0:n0}", Convert.ToInt64((cost.SALES_AMT / 1000)));   //매출액    

            return totalCostViewModel;
        }

        //비용분석 Model 생성
        public static ExpenseCostViewModel MakeExpenseCostViewModel(SHUSER_SboMonthlyExpenseCostSelectReturnModel expenseCost)
        {
            ExpenseCostViewModel expenseCostViewModel = new ExpenseCostViewModel();
            expenseCostViewModel.ManAmt = string.Format("{0:n0}", Convert.ToInt64((expenseCost.MAN_AMT / 1000)));   //인건비
            expenseCostViewModel.SalesAmt = string.Format("{0:n0}", Convert.ToInt64((expenseCost.SALES_AMT / 1000)));   //지급 임차료
            expenseCostViewModel.StaticEtcAmt = string.Format("{0:n0}", Convert.ToInt64((expenseCost.STATIC_ETC_AMT / 1000)));   //이자비용
            expenseCostViewModel.WelfareAmt = string.Format("{0:n0}", Convert.ToInt64((expenseCost.WELFARE_AMT / 1000)));   //복리후생비
            expenseCostViewModel.TaxAmt = string.Format("{0:n0}", Convert.ToInt64((expenseCost.TAX_AMT / 1000)));   //세금공과
            expenseCostViewModel.WasteAmt = string.Format("{0:n0}", Convert.ToInt64((expenseCost.WASTE_AMT / 1000)));   //소모품비
            expenseCostViewModel.FloatEtcAmt = string.Format("{0:n0}", Convert.ToInt64((expenseCost.FLOAT_ETC_AMT / 1000)));   //기타    
            expenseCostViewModel.FixTotalAmt = string.Format("{0:n0}", Convert.ToInt64(((expenseCost.MAN_AMT + expenseCost.SALES_AMT + expenseCost.STATIC_ETC_AMT) / 1000)));   //고정경비 합계
            expenseCostViewModel.MoveTotalAmt = string.Format("{0:n0}", Convert.ToInt64(((expenseCost.WELFARE_AMT + expenseCost.TAX_AMT + expenseCost.WASTE_AMT + expenseCost.FLOAT_ETC_AMT) / 1000)));   //유동경비 합계

            return expenseCostViewModel;
        }

        //주요매출 Model 생성
        public static IList<TaxSalesViewModel> MakeTaxSalseListViewModel(IList<SHUSER_SboMonthlyTaxSalesSelectReturnModel> taxSalesList, IList<SHUSER_SboMonthlySalesSelectReturnModel> slaesList)
        {
            IList<TaxSalesViewModel> taxSalesListViewModel = new List<TaxSalesViewModel>();
            foreach(var taxSales in taxSalesList)
            {
                TaxSalesViewModel taxSalesViewModel = new TaxSalesViewModel();
                taxSalesViewModel.CustName = taxSales.ACPT_TR_NM; //매입자 회사명
                taxSalesViewModel.WriteDate = taxSales.JNLYZ_DT.Substring(0, 4) + "-" + taxSales.JNLYZ_DT.Substring(4, 2) + "-" + taxSales.JNLYZ_DT.Substring(6, 2); ; //작성일자
                taxSalesViewModel.ItemName = taxSales.ITM_NM; //품목명
                taxSalesViewModel.TotalAmt = string.Format("{0:n0}", Convert.ToInt64((taxSales.SUM_AMT / 1000)));   //합계금액

                if(slaesList[0].SALES_AMT != 0)
                { 
                    taxSalesViewModel.Share = string.Format("{0:n0}", Convert.ToInt64((((taxSales.SUM_AMT / slaesList[0].SALES_AMT)*100)/1000)));
                }
                else
                {
                    taxSalesViewModel.Share = "0";
                }

                taxSalesListViewModel.Add(taxSalesViewModel);
            }
            return taxSalesListViewModel;
        }

        //주요지울 Model 생성
        public static IList<BankOutViewModel> MakeBnakOutListViewModel(IList<SHUSER_SboMonthlyBankOutSelectReturnModel> bankOutList)
        {
            IList<BankOutViewModel> bankOutListViewModel = new List<BankOutViewModel>();
            foreach (var bankOut in bankOutList)
            {
                BankOutViewModel bankOutViewModel = new BankOutViewModel();
                bankOutViewModel.BankName = bankOut.BANK_CD; //은행명
                bankOutViewModel.ItemName = bankOut.HISTCD_4; //적요
                bankOutViewModel.OutDate = bankOut.TRANDATE.Substring(0,4)+"-"+bankOut.TRANDATE.Substring(4, 2)+ "-" + bankOut.TRANDATE.Substring(6, 2); //출금일
                bankOutViewModel.TotalAmt = string.Format("{0:n0}", Convert.ToInt64((bankOut.HISTCD_O / 1000)));   //금액
                bankOutViewModel.Share = "10";
                bankOutListViewModel.Add(bankOutViewModel);
            }
            return bankOutListViewModel;
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

        public static object[] MakeProcedureParams(string bpNo, string corpCd, string bizCd, string year, string month)
        {
            SqlParameter compRegNo = new SqlParameter("MEMB_BUSNPERS_NO", bpNo);
            SqlParameter corpCode = new SqlParameter("CORP_CODE", corpCd);
            SqlParameter bizCode = new SqlParameter("BIZ_CD", bizCd);
            SqlParameter setYear = new SqlParameter("SET_YEAR", year);
            SqlParameter setMonth = new SqlParameter("SET_MONTH", month);

            object[] parameters = new object[] { compRegNo, corpCode, bizCode, setYear, setMonth };

            return parameters;
        }

    }
}