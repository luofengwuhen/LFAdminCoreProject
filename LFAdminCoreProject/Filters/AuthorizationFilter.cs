using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LFAdminCoreProject.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //权限验证
            //如果登录的用户名为admin2929，则拥有所有的权限
            HttpRequest request = context.HttpContext.Request;
            string token = request.Headers["Token"];//获取 post页面中heder中的token
            if (true) //判断token是否有效
            {

            }

            //string userName = request.ReadFormAsync("UserName");

        }
    }
}
