using Tracer;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    public class Tools
    {
        internal static int GetMethodsCountSums(ThreadInfo threadInfo)
        {
            int sum = threadInfo.MethodsStack.Count;

            foreach (MethodInfo methodInfo in threadInfo.MethodsStack)
            {
                sum += GetMethodsCountSum(methodInfo);
            }

            return sum;
        }

        internal static int GetMethodsCountSum(MethodInfo methodInfo)
        {
            int sum = methodInfo.ChildMethods.Count;

            foreach (MethodInfo submethodInfo in methodInfo.ChildMethods)
            {
                sum += GetMethodsCountSum(submethodInfo);
            }

            return sum;
        }

        internal static long GetThreadTime(ThreadInfo threadInfo)
        {
            return threadInfo.Time;
        }

        internal static long GetMethodsTimeSums(ThreadInfo threadInfo)
        {
            long sum = 0;

            foreach (MethodInfo methodInfo in threadInfo.MethodsStack)
            {
                sum += GetMethodsTimeSum(methodInfo);
            }

            return sum;
        }

        internal static long GetMethodsTimeSum(MethodInfo methodInfo)
        {
            long sum = methodInfo.Time;

            foreach (MethodInfo submethodInfo in methodInfo.ChildMethods)
            {
                sum += GetMethodsTimeSum(submethodInfo);
            }

            return sum;
        }
    }

    [TestClass]
    public class Test1
    {
        private const int ExpectedThreadCount = 1;
        private const int ExpectedMethodCount = 1;

        [TestMethod]
        public void CheckOneThreadOneMethod()
        {
            var tracer = new Tracer.Tracer();
            TraceResult traceResult;

            SomeMethod(tracer);

            traceResult = tracer.GetTraceResult();

            Assert.IsTrue(traceResult.GetThreadsInfo().Count == ExpectedThreadCount && Tools.GetMethodsCountSums(traceResult.GetThreadsInfo().Values.ToArray()[0]) == ExpectedMethodCount);
        }

        private void SomeMethod(ITracer tracer)
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
        }
    }

    [TestClass]
    public class Test2
    {
        private const int ExpectedThreadCount = 2;
        private const int ExpectedMethodCount = 1;

        [TestMethod]
        public void CheckTwoThreadsOneMethod()
        {
            var tracer = new Tracer.Tracer();

            var tempFoo = new Foo(tracer);

            var thread1 = new Thread(tempFoo.MyMethod1);
            thread1.Start();
            var thread2 = new Thread(tempFoo.MyMethod2);
            thread2.Start();

            Thread.Sleep(250);

            var traceResult = tracer.GetTraceResult();

            Assert.IsTrue(traceResult.GetThreadsInfo().Count == ExpectedThreadCount &&
                Tools.GetMethodsCountSums(traceResult.GetThreadsInfo().Values.ToArray()[0]) == ExpectedMethodCount &&
                Tools.GetMethodsCountSums(traceResult.GetThreadsInfo().Values.ToArray()[0]) == ExpectedMethodCount);
        }
               
        private class Foo
        {
            private readonly ITracer _tracer;

            internal Foo(ITracer tracer)
            {
                _tracer = tracer;
            }

            public void MyMethod1()
            {
                _tracer.StartTrace();

                Thread.Sleep(50);

                _tracer.StopTrace();
            }
            public void MyMethod2()
            {
                _tracer.StartTrace();

                Thread.Sleep(75);

                _tracer.StopTrace();
            }
        }
    }

    [TestClass]
    public class Test3
    {
        private const int ExpectedThreadCount = 1;
        private const int ExpectedMethodCount = 2;

        [TestMethod]
        public void CheckOneThreadTwoMethods()
        {
            var tracer = new Tracer.Tracer();

            var tempFoo = new Foo(tracer);

            var thread = new Thread(tempFoo.MyMethod);
            thread.Start();

            Thread.Sleep(250);

            var traceResult = tracer.GetTraceResult();

            int threadCount = traceResult.GetThreadsInfo().Count;
            int methodsCount = Tools.GetMethodsCountSums(traceResult.GetThreadsInfo().Values.ToArray()[0]);

            Assert.IsTrue(threadCount == ExpectedThreadCount && methodsCount == ExpectedMethodCount);
        }

        private class Foo
        {
            private readonly Bar _bar;
            private readonly ITracer _tracer;

            internal Foo(ITracer tracer)
            {
                _tracer = tracer;
                _bar = new Bar(_tracer);
            }

            public void MyMethod()
            {
                _tracer.StartTrace();

                _bar.InnerMethod();

                Thread.Sleep(75);

                _tracer.StopTrace();
            }
        }

        public class Bar
        {
            private readonly ITracer _tracer;

            internal Bar(ITracer tracer)
            {
                _tracer = tracer;
            }

            public void InnerMethod()
            {
                _tracer.StartTrace();

                Thread.Sleep(125);

                _tracer.StopTrace();
            }
        }
    }

    [TestClass]
    public class Test4
    {

        [TestMethod]
        public void CheckThreadAndMethodsTime()
        {
            var tracer = new Tracer.Tracer();

            var tempFoo = new Foo(tracer);

            var thread = new Thread(tempFoo.MyMethod);
            thread.Start();

            Thread.Sleep(250);
            var traceResult = tracer.GetTraceResult();

            Assert.AreEqual(Tools.GetThreadTime(traceResult.GetThreadsInfo().Values.ToArray()[0]), Tools.GetMethodsTimeSums(traceResult.GetThreadsInfo().Values.ToArray()[0]));
        }

        private class Foo
        {
            private readonly Bar _bar;
            private readonly ITracer _tracer;

            internal Foo(ITracer tracer)
            {
                _tracer = tracer;
                _bar = new Bar(_tracer);
            }

            public void MyMethod()
            {
                _tracer.StartTrace();

                _bar.InnerMethod();

                Thread.Sleep(75);

                _tracer.StopTrace();
            }
        }

        public class Bar
        {
            private readonly ITracer _tracer;

            internal Bar(ITracer tracer)
            {
                _tracer = tracer;
            }

            public void InnerMethod()
            {
                _tracer.StartTrace();

                Thread.Sleep(125);

                _tracer.StopTrace();
            }
        }
    }
}
