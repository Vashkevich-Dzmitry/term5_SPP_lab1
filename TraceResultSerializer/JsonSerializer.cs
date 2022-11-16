using Tracer;
using System.Text.Json;

namespace TraceResultSerializer
{
    public class JsonSerializer : ITraceResultSerializer<TraceResult>
    {
        public string Serialize(TraceResult traceResult)
        {
            return System.Text.Json.JsonSerializer.Serialize(traceResult,
               typeof(TraceResult),
               new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
