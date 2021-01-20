using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raccoon.Devkits.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Raccoon.Devkits.LoggerProxy
{
    public static class ServiceLoggerProxyExtensions
    {
        public static IServiceCollection AddTransientLogProxy<TIService,TService>(this IServiceCollection services,
            Action<TIService,LogBeforeMethodInvokeEventArgs<TIService>>? beforeInvokeAction=null,
            Action<TIService,LogAfterMethodInvokeEventArgs<TIService>>? afterInvokeAction=null) 
            where TIService :class
            where TService: class,TIService
        {
            services.AddTransientProxy<TIService, TService, ServiceLoggerProxy<TIService>>();
            if (beforeInvokeAction is not null) {
                ServiceLoggerProxy<TIService>.BeforeMethodInvoke += 
                    (sender,e)=>beforeInvokeAction.Invoke((sender as TIService)!,e);
            }
            if(afterInvokeAction is not null)
            {
                ServiceLoggerProxy<TIService>.AfterMethodInvoke +=
                    (sender, e) => afterInvokeAction.Invoke((sender as TIService)!, e);
            }
            return services;
        }
    }
}
