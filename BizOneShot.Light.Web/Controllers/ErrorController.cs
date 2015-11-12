using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizOneShot.Light.Web.ComLib;

namespace BizOneShot.Light.Web.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error500()
        {
            String area = null;

            if (_LogOnUser != null)
            {
                String usrType = _LogOnUser.UsrType;
                if (usrType == Global.Company)
                {
                    area = "Company";
                }
                else if (usrType == Global.Expert)
                {
                    area = "Expert";
                }
                else if (usrType == Global.Mentor)
                {
                    area = "Mentor";
                }
                else if (usrType == Global.BizManager)
                {
                    area = "BizManager";
                }
                else if (usrType == Global.SysManager)
                {
                    area = "SysManager";
                }
                else
                {
                    area = "";
                }

            }
            else
            {
                area = "";
            }

            ViewBag.area = area;
            return View();
        }
    }
}