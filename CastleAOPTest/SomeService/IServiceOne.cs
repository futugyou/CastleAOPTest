using CastleAOPTest.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CastleAOPTest.SomeService
{
    public interface IServiceOne
    {
        string WriteLog();
        string WriteLog2();
        Task<string> WriteLog3();
    }
    public class ServiceOne : IServiceOne
    {
        [Logging]
        public string WriteLog()
        {
            return ($"this is service one ！{Guid.NewGuid()}");
        }
        public string WriteLog2()
        {
            return ($"this is service one2 ！{Guid.NewGuid()}");
        }
        [Logging]
        public async Task<string> WriteLog3()
        {
            await Task.Delay(1000);
            return ($"this is service one3 ！{Guid.NewGuid()}");
        }
    }
}
