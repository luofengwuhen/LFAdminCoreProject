
using LFAdminCoreProject.Services.Cache;
using Mango.Framework.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System; 

namespace LFAdminCoreProject.Filters
{
    public class MyAuthorizationFilter : IAuthorizationFilter
    {
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
        }
    }
}
