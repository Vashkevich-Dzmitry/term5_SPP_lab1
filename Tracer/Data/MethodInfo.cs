using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Tracer
{
    public class MethodInfo
    {
        [JsonProperty, XmlAttribute("name")] public string Name { get; set; }
        [JsonProperty, XmlAttribute("class")] public string ClassName { get; set; }
        [JsonProperty, XmlAttribute("time")] public long Time { get; set; }

        [JsonProperty, XmlElement("methods")] public List<MethodInfo> ChildMethods { get; set; }


        [JsonIgnore] private readonly string _stackTrace;
        [JsonIgnore] private readonly long _ellapsedMilliseconds;

        public MethodInfo(string methodName, string className, string stackTrace, long ellapsedMilliseconds)
        {
            Name = methodName;
            ClassName = className;
            _stackTrace = stackTrace;
            _ellapsedMilliseconds = ellapsedMilliseconds;
        }

        public MethodInfo() {}

        public string GetStackTrace()
        {
            return _stackTrace;
        }

        public void CalculateTime(long ellapsedMilliseconds)
        {
            Time = _ellapsedMilliseconds - ellapsedMilliseconds;
        }
    }
}
