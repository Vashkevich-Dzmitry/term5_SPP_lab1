using Newtonsoft.Json;

namespace Tracer
{
    public class JsonSerializer : ITraceResultSerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            var arrays = new Dictionary<string, ICollection<ThreadInfo>>
            {
                {"threads", traceResult.GetThreadsInfo().Values}
            };

            return JsonConvert.SerializeObject(arrays, Formatting.Indented);
        }
    }
}
