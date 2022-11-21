using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Tracer
{
    public class MethodInfo
 
    {
        [JsonProperty(PropertyName = "name"), XmlAttribute("name")] public string Name { get; set; }
        [JsonProperty(PropertyName = "time"), XmlAttribute("time")] public long Time { get; set; }
        [JsonProperty(PropertyName = "class"), XmlAttribute("class")] public string ClassName { get; set; }
        [JsonProperty(PropertyName = "methods"), XmlElement("method")] public List<MethodInfo> ChildMethods { get; set; }


        [JsonIgnore] private readonly string _stackTrace;
        [JsonIgnore] private readonly long _ellapsedMilliseconds;

        public MethodInfo(string methodName, string className, string stackTrace, long ellapsedMilliseconds)
        {
            Name = methodName;
            ClassName = className;
            _stackTrace = stackTrace;
            _ellapsedMilliseconds = ellapsedMilliseconds;
            ChildMethods = new List<MethodInfo>();
        }

        public MethodInfo() {}

        public string GetStackTrace()
        {
            return _stackTrace;
        }

        public void CalculateTime(long ellapsedMilliseconds)
        {
            Time = ellapsedMilliseconds - _ellapsedMilliseconds;
        }
    }
}
