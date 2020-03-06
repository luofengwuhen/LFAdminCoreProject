
using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Utility
{
    public class AliSendSmsStrings
    { 
        public static string AccessKeyId = "LTAI4FmsWcnVjchvbK2omNCJ";
        public static string AccessSecret = "TBdxk6B71tLeNmTpdKkq5qeyf3SC8w";
        public static string SignName = "精工控股集团有限公司";
        public static string TemplateCode = "SMS_176880333"; //注册验证码模板

    }

    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevel
    {
        Error,
        Debug,
        Warning,
        Info
    }
    /// <summary>
    /// 单例模式初始化
    /// </summary>
    public class Singleton
    {
        private ILog Log;
        private static Singleton instance;
        private Singleton() { }
        public static Singleton getInstance()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
        /// <summary>
        /// 获取日志初始化器
        /// </summary>
        /// <param name="type">类名 方法名</param>
        /// <returns></returns>
        public ILog Init(string type)
        {
            Log = LogManager.GetLogger(Log4netHelper.Repository.Name, type);
            return Log;
        }
    }
    /// <summary>
    /// 日志操作类
    /// </summary>
    public class Log4netHelper
    {
        /// <summary>
        /// log4net 仓储
        /// </summary>
        public static ILoggerRepository Repository { get; set; }
        /// <summary>
        /// 输出Erro日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Error(string message, Exception exception = null)
        {
            StackTrace trace = new StackTrace();
            //获取是哪个类来调用的  
            var className = trace.GetFrame(1).GetMethod().DeclaringType;
            //获取方法名称
            MethodBase method = trace.GetFrame(1).GetMethod();
            var type = "类名：" + className.Namespace + "\r\n\r\t\r\r方法名：" + method.Name;
            WriteLog(LogLevel.Error, message, type, exception);
        }
        /// <summary>
        /// 输出Warning日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Warning(string message)
        {
            StackTrace trace = new StackTrace();
            //获取是哪个类来调用的  
            var className = trace.GetFrame(1).GetMethod().DeclaringType;
            //获取方法名称
            MethodBase method = trace.GetFrame(1).GetMethod();
            var type = "类名：" + className.Namespace + "\r\n\r\t\r\r方法名：" + method.Name;
            //记录日志
            WriteLog(LogLevel.Warning, message, type);
        }
        /// <summary>
        /// 输出Info日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            StackTrace trace = new StackTrace();
            //获取是哪个类来调用的  
            var className = trace.GetFrame(1).GetMethod().DeclaringType;
            //获取方法名称
            MethodBase method = trace.GetFrame(1).GetMethod();
            var type = "类名：" + className.Namespace + "\r\n\r\t\r\r方法名：" + method.Name;
            //记录日志
            WriteLog(LogLevel.Info, message, type);
        }
        /// <summary>
        /// 输出Debug日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Debug(string message, Exception exception = null)
        {
            StackTrace trace = new StackTrace();
            //获取是哪个类来调用的  
            var className = trace.GetFrame(1).GetMethod().DeclaringType;
            //获取方法名称
            MethodBase method = trace.GetFrame(1).GetMethod();
            var type = "类名：" + className.Namespace + "\r\n\r\t\r\r方法名：" + method.Name;
            //记录日志
            WriteLog(LogLevel.Debug, message, type, exception);
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="message">日志信息</param>
        /// <param name="type">类名 方法名</param>
        private static void WriteLog(LogLevel logLevel, string message, string type, Exception exception = null)
        {
            ILog Log = Singleton.getInstance().Init(type);
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Log.Debug(message, exception);
                    break;
                case LogLevel.Error:
                    Log.Error(message, exception);
                    break;
                case LogLevel.Info:
                    Log.Info(message);
                    break;
                case LogLevel.Warning:
                    Log.Warn(message);
                    break;
            }

        }
    }
}

