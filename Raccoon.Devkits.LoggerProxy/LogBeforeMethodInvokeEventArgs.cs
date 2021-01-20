using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Raccoon.Devkits.LoggerProxy
{
    public class LogBeforeMethodInvokeEventArgs<TIService>:EventArgs
        where TIService:class
    {
        public MethodInfo MethodInfo {get;}
        public object?[]? Args {get;}
        public ILogger<TIService> Logger {get;}

        public LogBeforeMethodInvokeEventArgs(MethodInfo methodInfo,object?[]? args,
            ILogger<TIService> logger)
        {
            MethodInfo = methodInfo;
            Args = args;
            Logger = logger;
        }
    }
}