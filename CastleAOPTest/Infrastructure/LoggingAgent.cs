using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace CastleAOPTest.Infrastructure
{
    public class LoggingAgent
    {
        private readonly Microsoft.Extensions.Logging.ILogger _log;
        private StringBuilder _logStringBuilder;
        public LoggingAgent(ILogger<LoggingAgent> log)
        {
            _log = log;
            _logStringBuilder = new StringBuilder();
        }

        public LoggingAgent SetError(Exception ex)
        {
            if (ex != null)
            {
                _logStringBuilder.Append($"Process Error : {(ex.InnerException ?? ex).Message} ,");
            }
            return this;
        }
        public LoggingAgent SetClass(string className)
        {
            _logStringBuilder.Append($"Class Name : {className} ,");
            return this;
        }
        public LoggingAgent SetMothed(string mothedName)
        {
            _logStringBuilder.Append($"Mothed Name : {mothedName} ,");
            return this;
        }
        public LoggingAgent SetArguments(dynamic arguments)
        {
            _logStringBuilder.Append($"Arguments : {Newtonsoft.Json.JsonConvert.SerializeObject(arguments)} ,");
            return this;
        }
        public LoggingAgent SetReturnValue(dynamic returnValue)
        {
            if (returnValue != null)
            {
                _logStringBuilder.Append($"ReturnValue : {Newtonsoft.Json.JsonConvert.SerializeObject(returnValue)} ,");
            }
            return this;
        }
        public LoggingAgent SetExecutedTime(double totalMilliseconds)
        {
            _logStringBuilder.Append($"Executed in {totalMilliseconds.ToString("0.000")} Milliseconds .");
            return this;
        }
        public void CollectLogs()
        {
            _log.LogInformation(_logStringBuilder.ToString());
        }
    }
}