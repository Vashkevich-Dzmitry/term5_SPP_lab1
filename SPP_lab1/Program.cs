using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer;

namespace SPP_lab1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var tracer = new Tracer.Tracer();


            for (int i = 0; i < 4; i++)
            {
                var tempFoo = new Foo(tracer);
                var thread = new Thread(tempFoo.MyMethod);
                thread.Start();
            }

            var traceResult = tracer.GetTraceResult();


            ITraceResultSerializer serializer = new JsonSerializer();
            var json = serializer.Serialize(traceResult);

            serializer = new XmlSerializer();
            var xml = serializer.Serialize(traceResult);


            ISerializationWriter writer = new SerializationConsoleWriter();
            writer.Write(json);
            writer.Write(xml);


            writer = new SerializationFileWriter("traceResult.xml");
            writer.Write(xml);
            ((SerializationFileWriter)writer).Path = "traceResult.json";
            writer.Write(json);
        }
    }

    public class Foo
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

            int x = 0;
            for (int i = 0; i < 10000; i++)
            {
                x++;
            }
            _bar.InnerMethod();

            byte[] arr = new byte[1000];
            Random rand = new();
            rand.NextBytes(arr);
            Array.Sort(arr);

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
            
            int x = 0;
            for (int i = 0; i < 250; i++)
            {
                x = i * i;
            }

            _tracer.StopTrace();
        }
    } 
}