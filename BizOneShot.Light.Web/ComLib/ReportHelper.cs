using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Models.WebModels;

namespace BizOneShot.Light.Web.ComLib
{
    public static class ReportHelper
    {
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
    }
}