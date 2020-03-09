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

        public IActionResult Login(string username,string password)
        {
            ReturnViewModel model = new ReturnViewModel();
            //登录是否成功
            ILogin _IL = new Login();
            model = _IL.LoginOn(username, password);

            return Json(model);
        }

        public IActionResult SendSms(string cellphone)
        { 
            ISendSmsService _ISS = new SendSmsService();
            ReturnViewModel model = new ReturnViewModel();
            //1.判断手机号是否为员工号码，否则不与发送  测试实例不需要判断


            //2.判断手机号是否已经注册
            if (_ISS.IsHasRegister(cellphone))
            {
                model.Code = 1;
                model.Msg = "该手机号已经注册，如忘记密码，请点击<a href='/Login/Forget'>忘记密码</a>进行密码找回";
                return Json(model);
            }
            //3.判断是否频繁注册
            if (_ISS.IsConstantlyRegister(cellphone))
            {
                model.Code = 1;
                model.Msg = "请查验是否已收到验证码，验证码30分钟内有效，请勿频繁注册";
                return Json(model);
            }
            //4.发送 验证码短信，并把发送记录写入
            model = _ISS.SendSms(cellphone);  

            return Json(model);
        }

        public IActionResult RegisterUser(string cellphone, string password, string nickname, string vercode)
        {  
            //1.判断验证码是否正确 
            IRegisterUser _IRU = new RegisterUser();
            ReturnViewModel model = new ReturnViewModel();
            if (!_IRU.IsVeryCode(vercode,cellphone)) //不正确
            {
                model.Code = 1;
                model.Msg = "验证码已过期或输入有误，请检查";
                return Json(model);
            }

            //2.注册加密 
            model = _IRU.RegisterUser(cellphone, password, nickname);

            return Json(model);
        }


    }
}