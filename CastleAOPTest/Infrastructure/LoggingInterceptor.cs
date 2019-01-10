using Castle.Core.Internal;
using Castle.DynamicProxy;
using System.Linq;
using System.Reflection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace CastleAOPTest.Infrastructure
{
    public class LoggingInterceptor : IInterceptor
    {
        private readonly LoggingAgent  _loggingAgent;

        public LoggingInterceptor(LoggingAgent  loggingAgent)
        {
            _loggingAgent = loggingAgent;
        }
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            var logAttributes = method.GetAttributes<LoggingAttribute>();
            if (logAttributes == null || logAttributes.FirstOrDefault() == null || !logAttributes.FirstOrDefault().IsValid)
            {
                invocation.Proceed();
                return;
            }

            if (method.IsAsync())
            {
                InterceptAsyncMethod(invocation);
            }
            else
            {
                InterceptSyncMethod(invocation);
            }
        }
        #region private method
        private void InterceptSyncMethod(IInvocation invocation)
        {
            var stopwatch = Stopwatch.StartNew();
            Exception exception = null;
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                CollectProcessLog(invocation, invocation.ReturnValue, exception, stopwatch);
            }
        }

        private void InterceptAsyncMethod(IInvocation invocation)
        {
            var stopwatch = Stopwatch.StartNew();
            invocation.Proceed();
            if ((invocation.MethodInvocationTarget ?? invocation.Method).ReturnType == typeof(Task))
            {
                invocation.ReturnValue = AsyncHelper.AwaitTaskWithPostActionAndFinally(
                    (Task)invocation.ReturnValue,
                    null,
                    ex =>
                    {
                        CollectProcessLog(invocation, null, ex, stopwatch);
                    }
                    );
            }
            else
            {
                invocation.ReturnValue = AsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    null,
                    (returnValue, ex) =>
                    {
                        CollectProcessLog(invocation, returnValue, ex, stopwatch);
                    });
            }
        }

        private void CollectProcessLog(IInvocation invocation, dynamic returnValue, Exception ex, Stopwatch stopwatch)
        {
            stopwatch.Stop();

            _loggingAgent.SetClass(invocation.TargetType.ToString())
                    .SetMothed((invocation.MethodInvocationTarget ?? invocation.Method).Name)
                    .SetArguments(invocation.Arguments)
                    .SetReturnValue(returnValue)
                    .SetError(ex)
                    .SetExecutedTime(stopwatch.Elapsed.TotalMilliseconds)
                    .CollectLogs();
        }

        private async Task InterceptAsync(IInvocation invocation)
        {
            invocation.Proceed();
            object _actualReturnValue = invocation.ReturnValue;
            await (invocation.Method.IsAsync() ? (Task)_actualReturnValue : Task.FromResult(_actualReturnValue));
        }
        #endregion
    }
}