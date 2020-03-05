using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/// <summary>
/// 日志
/// </summary>
namespace HrEvaProject.Log4net
{
    public class LoggerHelper:ILoggerHelper
    {
        static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger(Log4NetConfig.RepositoryName, "loginfo");
        static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        static readonly log4net.ILog logmonitor = log4net.LogManager.GetLogger("logmonitor");
        static readonly log4net.ILog logfatal = log4net.LogManager.GetLogger("logfatal");

        public  void Error(string ErrorMsg, Exception ex = null)
        {
            if (ex != null)
            {
                logerror.Error(ErrorMsg, ex);
            }
            else
            {
                logerror.Error(ErrorMsg);
            }
        }

        public  void Info(string Msg)
        {
            loginfo.Info(Msg);
        }

        public  void Monitor(string Msg)
        {
            logmonitor.Info(Msg);
        }


        public  void Fatal(string ErrorMsg, Exception ex = null)
        {
            if (ex != null)
            {
                logfatal.Fatal(ErrorMsg, ex);
            }
            else
            {
                logfatal.Fatal(ErrorMsg);
            }
        }
    }
}