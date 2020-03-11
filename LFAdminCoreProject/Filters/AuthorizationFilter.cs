using LFAdminCoreProject.Services.Cache; 
using Mango.Framework.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace LFAdminCoreProject.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    { 
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //权限验证
            //如果登录的用户名为admin2929，则拥有所有的权限
            HttpRequest request = context.HttpContext.Request;
            string accessToken = request.Headers["Access_Token"];//获取 post页面中heder中的token
            
            string controller = context.RouteData.Values["Controller"].ToString();
            string action = context.RouteData.Values["Action"].ToString();
            if (!controller.Equals("Login") && !(controller.Equals("Home") && action.Equals("Index")))
            {
                if (accessToken != null) //判断token是否有效
                {
                    var redisCacheService = ServiceContext.GetService<IRedisCacheService>();
                    //1.获取redis中的token 
                    //string AccessToken = redisCacheService.Get(Startup.InstanceName + Utility.AccessToken.AccessTokenName);
                    //1.获取session中的AccessToken
                    Byte[] bytesTemp; 
                    context.HttpContext.Session.TryGetValue(Utility.AccessToken.AccessTokenName,out bytesTemp);
                    string AccessToken= System.Text.Encoding.UTF8.GetString(bytesTemp);
                    //2.如果token存在
                    if (!string.IsNullOrEmpty(AccessToken) && accessToken.Equals(AccessToken))
                    {
                        //则更新token的时间  
                        //redisCacheService.Replace(Utility.AccessToken.AccessTokenName, AccessToken, 60);
                        
                    }
                    else//登录状态失效
                    {
                        //context.HttpContext.Response.Redirect("/Login/Index");
                        //context.HttpContext.Response.StatusCode = 1001;
                    }
                    //throw new NotImplementedException();
                }
                else//登录状态失效
                {
                   // context.HttpContext.Response.Redirect("/Login/Index");
                    //context.HttpContext.Response.StatusCode=1001;
                }
            }
        }
    }
}
