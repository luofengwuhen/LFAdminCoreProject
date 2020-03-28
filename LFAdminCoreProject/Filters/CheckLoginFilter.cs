using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Filters
{

    public class CheckLoginFilter : Attribute, IActionFilter
    {
        private readonly IDistributedCache _cache;

        public bool IsCheck { get; set; }

        public CheckLoginFilter(IDistributedCache cache)
        {
            _cache = cache;
        }

        public void OnActionExecuted(ActionExecutedContext context)//方法执行后执行
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)//方法执行前执行
        {
            try
            {

                //权限验证
                //如果登录的用户名为admin2929，则拥有所有的权限
                HttpRequest request = context.HttpContext.Request;
                string accessToken = request.Headers["Access_Token"];//获取 post页面中heder中的token

                string controller = context.RouteData.Values["Controller"].ToString();
                string action = context.RouteData.Values["Action"].ToString();
                if (!controller.Equals("Login"))
                {
                    #region  session 检查  是否登录超时,默认为30分钟，在startup里设置
                    if (!controller.Equals("Login") && !(controller.Equals("Home") && action.Equals("Index")))
                    {
                        Byte[] bytesTemp;
                        context.HttpContext.Session.TryGetValue(Utility.AccessToken.AccessTokenName, out bytesTemp);
                        if (bytesTemp == null)//如果为空，则跳转到登录页
                        {
                            context.Result = new EmptyResult(); //防止执行后续的代码
                            context.HttpContext.Response.Redirect("/Login/Index");
                        }
                    }

                    #endregion

                    #region token检查 redis 和 sqlserver cache 两者使用方式一致
                    //if (!controller.Equals("Login") && !(controller.Equals("Home") && action.Equals("Index")))
                    //{
                    //    if (accessToken != null) //判断token是否有效
                    //    {
                    //        string[] str = accessToken.Split(" ");
                    //        string token = _cache.GetString(str[0]);
                    //        if (!str[1].Equals(token))
                    //        {
                    //            context.Result = new EmptyResult(); //防止执行后续的代码
                    //            context.HttpContext.Response.Redirect("/Login/Index");
                    //        }
                    //        //2.如果token存在
                    //        if (!string.IsNullOrEmpty(accessToken) && accessToken.Equals(accessToken))
                    //        {
                    //            //则更新token的时间   
                    //            _cache.Refresh(Utility.AccessToken.AccessTokenName);
                    //        }
                    //        else//登录状态失效
                    //        {
                    //            context.Result = new EmptyResult(); //防止执行后续的代码
                    //            context.HttpContext.Response.Redirect("/Login/Index");
                    //        }
                    //        //throw new NotImplementedException();
                    //    }
                    //    else//登录状态失效
                    //    {
                    //        context.Result = new EmptyResult(); //防止执行后续的代码
                    //        context.HttpContext.Response.Redirect("/Login/Index");
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }
    }
}
