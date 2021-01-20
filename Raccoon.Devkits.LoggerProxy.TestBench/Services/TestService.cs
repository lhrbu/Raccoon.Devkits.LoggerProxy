using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raccoon.Devkits.LoggerProxy.TestBench.Services
{
    
    public interface ITestService
    {
        int Value { get; set; }
        [IgnoreLogProxy]
        void TestMethod();
    }
    public class TestService: ITestService
    {
        public int Value { get; [IgnoreLogProxy]set; }
        public void TestMethod() => Console.WriteLine("Hello World");
    }
}
