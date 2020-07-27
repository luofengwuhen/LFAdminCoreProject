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
using LFAdminCoreProject.Services.Login;
using Newtonsoft.Json;
using System.Diagnostics;
using LFAdminCoreProject.Services.Infrastructure;
using LFAdminCoreProject.Services.Cache;
using StackExchange.Redis;
using LFAdminCoreProject.Services.AliSms;
using System.Web.Http.Filters;
using LFAdminCoreProject.Filters;

namespace LFAdminCoreProject.Controllers
{
    //[ServiceFilter(typeof(CheckLoginFilter))]
    public class LoginController : Controller
    {
        private readonly IForgetUser _forgetUser;
        private readonly IRegisterUser _registerUser;
        private readonly ILogin _login;
        private readonly ISendSmsService _sendSmsService; 
        //public IRedisCacheService _redis { get; }

        public LoginController( IForgetUser forgetUser, IRegisterUser registerUser, ILogin login, ISendSmsService sendSmsService)//IRedisCacheService redis,
        {
            //_redis = redis;
            _forgetUser = forgetUser;
            _registerUser = registerUser;
            _login = login;
            _sendSmsService = sendSmsService;
        }
     
        public IActionResult Index() //(IsCheck = false)
        { 
            //string Access_token = Utility.AccessToken.GetAccessToken();
            //Redis.Add("Access_token", Access_token);
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
            if (!_registerUser.IsVeryCode(vercode, cellphone, Utility.Constant.ResetName)) //不正确
            { 
                return ReturnMethod.ReturnWarning(1, "验证码已过期或输入有误，请检查");
            }

            //2.密码重置 
            ReturnViewModel model = _forgetUser.ForgetUser(cellphone, password);

            return Json(model); 
        }

        public IActionResult Login(string username,string password)
        {
            //登录是否成功 
            ReturnViewModel model = _login.LoginOn(username, password);
            if (model.Code == 0)//登录成功
            {
                //生成Access_token ，写入redis
                string Access_token = Utility.AccessToken.GetAccessToken(); 
                model.Data = new { access_token=Access_token };

                //_redis.Add(Utility.AccessToken.AccessTokenName, Access_token, 60);//写入redis

                HttpContext.Session.Set(Utility.AccessToken.AccessTokenName,System.Text.Encoding.UTF8.GetBytes(Access_token));
            }
            return Json(model);
        }

        public IActionResult SendSms(string cellphone,string typeString)
        {  
            string TemplateCode = "";
            //1.判断手机号是否为员工号码，否则不与发送  测试实例不需要判断 
            if (typeString.Equals(Utility.Constant.RegisterName))
            {
                //2.判断手机号是否已经注册
                if (_sendSmsService.IsHasRegister(cellphone))
                { 
                    return ReturnMethod.ReturnWarning(1, "该手机号已经注册，如忘记密码，请点击 <a href='/Login/Forget' style='color: red'>忘记密码</a> 进行密码重置");
                }
                TemplateCode = Utility.AliSendSmsStrings.RegisterTemplateCode;
            }
            else if (typeString.Equals(Utility.Constant.ResetName))
            {
                //2.判断手机号是否已经注册
                if (!_sendSmsService.IsHasRegister(cellphone))
                { 
                    return ReturnMethod.ReturnWarning(1, "该手机号未注册，请点击 <a href='/Login/Register' style='color: red'>注册帐号</a> 进行账号注册");
                }
                TemplateCode = Utility.AliSendSmsStrings.ResetTemplateCode;
            }
            else
            { 
                return ReturnMethod.ReturnWarning(1, "系统出错，请刷新页面后重试");
            }
           
            //3.判断是否频繁注册
            if (_sendSmsService.IsConstantlyRegister(cellphone, typeString))
            { 
                return ReturnMethod.ReturnWarning(1, "请查验是否已收到验证码，验证码30分钟内有效，请勿频繁注册");
            }
            //4.发送 验证码短信，并把发送记录写入
            ReturnViewModel model = _sendSmsService.SendSms(cellphone, typeString, TemplateCode);  

            return Json(model);
        }

        public IActionResult RegisterUser(string cellphone, string password, string nickname, string vercode)
        {  
            //1.判断验证码是否正确  
            if (!_registerUser.IsVeryCode(vercode,cellphone, Utility.Constant.RegisterName)) //不正确
            { 
                return ReturnMethod.ReturnWarning(1, "验证码已过期或输入有误，请检查");
            }

            //2.注册加密 
            ReturnViewModel model =_registerUser.RegisterUser(cellphone, password, nickname);

            return Json(model);
        }


    }
}