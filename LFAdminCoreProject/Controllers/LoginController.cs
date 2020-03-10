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
using LFAdminCoreProject.Services.Infrastructure;

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

        public IActionResult ForgetUser(string cellphone,string password,string vercode)
        {
            //1.判断验证码是否正确 
            IRegisterUser _IRU = new RegisterUser();  
            if (!_IRU.IsVeryCode(vercode, cellphone, Utility.Constant.ResetName)) //不正确
            { 
                return ReturnMethod.ReturnWarning(1, "验证码已过期或输入有误，请检查");
            }

            //2.密码重置
            IForgetUser _IFU = new ForgetUser();
            ReturnViewModel model = _IFU.ForgetUser(cellphone, password);

            return Json(model); 
        }

        public IActionResult Login(string username,string password)
        { 
            //登录是否成功
            ILogin _IL = new Login();
            ReturnViewModel model = _IL.LoginOn(username, password);

            //model.Data.Access_token = "";//access_token
            return Json(model);
        }

        public IActionResult SendSms(string cellphone,string typeString)
        { 
            ISendSmsService _ISS = new SendSmsService(); 
            string TemplateCode = "";
            //1.判断手机号是否为员工号码，否则不与发送  测试实例不需要判断 
            if (typeString.Equals(Utility.Constant.RegisterName))
            {
                //2.判断手机号是否已经注册
                if (_ISS.IsHasRegister(cellphone))
                { 
                    return ReturnMethod.ReturnWarning(1, "该手机号已经注册，如忘记密码，请点击 <a href='/Login/Forget'>忘记密码</a> 进行密码重置");
                }
                TemplateCode = Utility.AliSendSmsStrings.RegisterTemplateCode;
            }
            else if (typeString.Equals(Utility.Constant.ResetName))
            {
                //2.判断手机号是否已经注册
                if (!_ISS.IsHasRegister(cellphone))
                { 
                    return ReturnMethod.ReturnWarning(1, "该手机号未注册，请点击 <a href='/Login/Register'>注册帐号</a> 进行密码重置");
                }
                TemplateCode = Utility.AliSendSmsStrings.ResetTemplateCode;
            }
            else
            { 
                return ReturnMethod.ReturnWarning(1, "系统出错，请刷新页面后重试");
            }
           
            //3.判断是否频繁注册
            if (_ISS.IsConstantlyRegister(cellphone, typeString))
            { 
                return ReturnMethod.ReturnWarning(1, "请查验是否已收到验证码，验证码30分钟内有效，请勿频繁注册");
            }
            //4.发送 验证码短信，并把发送记录写入
            ReturnViewModel model = _ISS.SendSms(cellphone, typeString, TemplateCode);  

            return Json(model);
        }

        public IActionResult RegisterUser(string cellphone, string password, string nickname, string vercode)
        {  
            //1.判断验证码是否正确 
            IRegisterUser _IRU = new RegisterUser(); 
            if (!_IRU.IsVeryCode(vercode,cellphone, Utility.Constant.RegisterName)) //不正确
            { 
                return ReturnMethod.ReturnWarning(1, "验证码已过期或输入有误，请检查");
            }

            //2.注册加密 
            ReturnViewModel model = _IRU.RegisterUser(cellphone, password, nickname);

            return Json(model);
        }


    }
}