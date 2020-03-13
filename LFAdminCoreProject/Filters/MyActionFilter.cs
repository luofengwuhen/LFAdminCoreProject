using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LFAdminCoreProject.Filters
{
    public class MyActionFilter : IActionFilter
    {
        public  void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public  void OnActionExecuting(ActionExecutingContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
