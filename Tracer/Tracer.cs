using System.Diagnostics;

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
            var path = string.Join("", stackTrace.ToString().Split("\r\n").Skip(1).ToArray());

            threadInfo.AddMethod(methodName, className,  path, elapsedMilliseconds);
        }

        public void StopTrace()
        {
            if (_traceResult.HasThreadWithId(Environment.CurrentManagedThreadId))
            {
                ThreadInfo threadInfo = _traceResult.GetOrAddThreadInfo(Environment.CurrentManagedThreadId);

                threadInfo.EjectMethod(string.Join("", new StackTrace().ToString().Split("\r\n").Skip(1).ToArray()), _stopwatch.ElapsedMilliseconds);
            } else throw new Exception("Incorrect methods call sequence");
        }
    }
}