using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrEvaProject.Log4net
{
   public  interface ILoggerHelper
    {
        void Error(string ErrorMsg, Exception ex = null);
        void Info(string Msg);
        void Monitor(string Msg);
        void Fatal(string ErrorMsg, Exception ex = null);
    }
}
