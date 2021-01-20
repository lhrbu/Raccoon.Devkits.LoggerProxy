using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Raccoon.Devkits.LoggerProxy
{
    public class LogAfterMethodInvokeEventArgs<TIService>:EventArgs
    {
        public MethodInfo MethodInfo {get;}
        public object?[]? Args {get;}
        public object? Result {get;}
        public ILogger<TIService> Logger {get;}

        public LogAfterMethodInvokeEventArgs(MethodInfo methodInfo,object?[]? args,object? result,
            ILogger<TIService> logger)
        {
            MethodInfo = methodInfo;
            Args = args;
            Logger = logger;
            Result = result;
        }
    }
}