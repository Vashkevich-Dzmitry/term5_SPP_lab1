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
        private readonly object _locker = new();

        private readonly TraceResult _traceResult;
        private readonly ConcurrentDictionary<int, ThreadInfo> _threadsDictionary;     
        static Tracer()
        {
            _stopwatch.Start();
        }

        public Tracer()
        {
            _traceResult = new TraceResult();
            _threadsDictionary = new ConcurrentDictionary<int, ThreadInfo>();
        }

        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }

        public void StartTrace()
        {
            ThreadInfo currentThread;

            var currentThreadId = Environment.CurrentManagedThreadId;
            lock (_locker)
            {
                if (_threadsDictionary.ContainsKey(currentThreadId))
                {
                    currentThread = _threadsDictionary[currentThreadId];
                }
                else
                {
                    currentThread = new ThreadInfo
                    {
                        Id = currentThreadId
                    };
                    _threadsDictionary.TryAdd(currentThreadId, currentThread);
                    _traceResult.ThreadList.Add(currentThread);
                    currentThread.OldTime = _stopwatch.ElapsedMilliseconds;
                    currentThread.Time = 0;
                }
            }



            StackTrace stackTrace = new();
            var frame = stackTrace.GetFrame(1);
            var method = frame.GetMethod();


            if (currentThread.CurrentNode == null)
            {
                currentThread.CurrentNode = new Node<MethodInfo>(new MethodInfo()
                {
                    ClassName = method.DeclaringType.Name,
                    MethodName = method.Name
                })
                {
                    Parent = null
                };
                currentThread.MethodsInfo.Add(currentThread.CurrentNode);
                currentThread.CurrentNode.Data.Time = _stopwatch.ElapsedMilliseconds;
            }
            else
            {
                var temp = new Node<MethodInfo>(new MethodInfo()
                {
                    ClassName = method.DeclaringType.Name,
                    MethodName = method.Name,
                    Time = _stopwatch.ElapsedMilliseconds
                });
                currentThread.CurrentNode.Children.Add(temp);
                temp.Parent = currentThread.CurrentNode;
                currentThread.CurrentNode = temp;
            }
        }

        public void StopTrace()
        {
            ThreadInfo currentThread;
            var currentThreadId = Environment.CurrentManagedThreadId;
            if (_threadsDictionary.ContainsKey(currentThreadId))
            {
                currentThread = _threadsDictionary[currentThreadId];
            }
            else
                throw new Exception("Incorrect methods call sequence");

            var tempTime = _stopwatch.ElapsedMilliseconds;
            currentThread.CurrentNode.Data.Time = tempTime - currentThread.CurrentNode.Data.Time;
            currentThread.CurrentNode = currentThread.CurrentNode.Parent;

            currentThread.Time = tempTime - currentThread.OldTime + currentThread.Time;
            currentThread.OldTime = tempTime;
        }
    }
}