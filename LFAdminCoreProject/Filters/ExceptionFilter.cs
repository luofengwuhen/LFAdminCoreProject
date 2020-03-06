using LFAdminCoreProject.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// 错误处理Filter
/// </summary>
namespace LFAdminCoreProject.Filters
{ 
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled) //处理未处理的错误
            {
                //写入错误日志，并跳转到错误页 ，记录RequestId 方便查找
                Utility.Log4netHelper.Error(Activity.Current?.Id, context.Exception);
                context.ExceptionHandled = true;
                //跳转到错误页
                throw new NotImplementedException();
             
            }
         }
    }
}
