using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LFAdminCoreProject.Models;
using Microsoft.AspNetCore.Mvc;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using LFAdminCoreProject.Services.LoginServices;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LFAdminCoreProject.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Forget()
        {
            return View();
        }

        public IActionResult Login()
        {
            ReturnViewModel model = new ReturnViewModel();
            //model.code
            return Json(model);
        }

        public IActionResult SendSms(string cellphone)
        { 
            ISendSmsService _ISS = new SendSmsService();
            ReturnViewModel model = _ISS.SendSms(cellphone); 

            return Json(model);
        }


    }
}