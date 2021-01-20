using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raccoon.Devkits.DynamicProxy;

namespace Raccoon.Devkits.LoggerProxy
{
    public class ServiceLoggerProxy<TIService>:DynamicProxyBase<TIService>
        where TIService:class
    {
        public static EventHandler<LogBeforeMethodInvokeEventArgs<TIService>>? BeforeMethodInvoke;
        public static EventHandler<LogAfterMethodInvokeEventArgs<TIService>>? AfterMethodInvoke;
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            bool ignoreMethodFlag = HasIgnoreFlag(targetMethod!);
            ILogger<TIService> logger = ServiceProvider.GetRequiredService<ILogger<TIService>>();
            if (!ignoreMethodFlag)
            { BeforeMethodInvoke?.Invoke(Target, new LogBeforeMethodInvokeEventArgs<TIService>(targetMethod!, args, logger));}
            object? result = targetMethod?.Invoke(Target,args);
            if (!ignoreMethodFlag)
            { AfterMethodInvoke?.Invoke(Target, new LogAfterMethodInvokeEventArgs<TIService>(targetMethod!, args, result, logger));}
            return result;
        }

        private bool HasIgnoreFlag(MethodInfo targetMethod)
        {
            if (HasIgnoreFlagInImplementation(targetMethod)) { return true; }
            else
            {
                return HasIgnoreFlagInInterface(targetMethod);
            }
        }

        private bool HasIgnoreFlagInImplementation(MethodInfo targetMethod)
        {
            MethodInfo implementMethod = ImplementationType.GetMethod(targetMethod.Name)!;
            if(IsPropertyAccessor(implementMethod.Name))
            {
                // Check in property accessor method
                if (HasIgnoreFlagInMetadata(implementMethod)) { return true; }
                else
                {
                    // Check in property
                    PropertyInfo propertyInfo = ImplementationType.GetProperty(implementMethod.Name.Substring(4))!;
                    return HasIgnoreFlagInMetadata(propertyInfo);
                }
            }
            else { return HasIgnoreFlagInMetadata(implementMethod); }
        }
        private bool HasIgnoreFlagInInterface(MethodInfo targetMethod)
        {
            if (IsPropertyAccessor(targetMethod.Name))
            {
                // Check in property accessor method
                if (HasIgnoreFlagInMetadata(targetMethod)) { return true; }
                else
                {
                    // Check in property
                    PropertyInfo propertyInfo = targetMethod.DeclaringType!.GetProperty(targetMethod.Name.Substring(4))!;
                    return HasIgnoreFlagInMetadata(propertyInfo);
                }
            }
            else { return HasIgnoreFlagInMetadata(targetMethod); }
        }

        private bool IsPropertyAccessor(string methodName) =>
            methodName.StartsWith("get_") ||
            methodName.StartsWith("set_");

        private bool HasIgnoreFlagInMetadata(MemberInfo memberInfo) =>
            memberInfo.GetCustomAttribute<IgnoreLogProxyAttribute>() is not null;
    }
}