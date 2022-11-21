using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public class Tracer : ITracer
    {
        private static readonly Stopwatch _stopwatch = new();

        private readonly TraceResult _traceResult; 
        static Tracer()
        {
            _stopwatch.Start();
        }

        public Tracer()
        {
            _traceResult = new TraceResult();
        }

        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }

        public void StartTrace()
        {
            long elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;

            ThreadInfo threadInfo = _traceResult.GetOrAddThreadInfo(Environment.CurrentManagedThreadId);

            var stackTrace = new StackTrace();

            var methodName = stackTrace.GetFrames()[1].GetMethod().Name;
            var className = stackTrace.GetFrames()[1].GetMethod().DeclaringType.Name;
            var path = stackTrace.ToString().Replace("\r\n", "");

            threadInfo.AddMethod(methodName, className,  path, elapsedMilliseconds);
        }

        public void StopTrace()
        {
            if (_traceResult.HasThreadWithId(Environment.CurrentManagedThreadId))
            {
                ThreadInfo threadInfo = _traceResult.GetOrAddThreadInfo(Environment.CurrentManagedThreadId);

                threadInfo.EjectMethod(new StackTrace().ToString().Replace("\r\n", ""), _stopwatch.ElapsedMilliseconds);
            } else throw new Exception("Incorrect methods call sequence");
        }
    }
}