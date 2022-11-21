using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Tracer
{
    [XmlType("thread")]
    public class ThreadInfo
    {

        [DataMember][JsonProperty(PropertyName = "id"), XmlAttribute("id")] public int Id { get; set; }
        [DataMember][JsonProperty(PropertyName = "time"), XmlAttribute("time")] public long Time { get; set; }
        [DataMember][JsonProperty(PropertyName = "methods"), XmlElement("method")] public List<MethodInfo> MethodsStack { get; set; }

        public ThreadInfo() {}

        public ThreadInfo(int id)
        {
            Id = id;
            MethodsStack = new List<MethodInfo>();
        }

        public void AddMethod(string name, string className, string stackTrace, long ellapsedMilliseconds)
        {
            MethodsStack.Add(new MethodInfo(name, className, stackTrace, ellapsedMilliseconds));
        }

        public void EjectMethod(string stackTrace, long ellapsedMilliseconds)
        {
            int index = MethodsStack.FindLastIndex(item => item.GetStackTrace() == stackTrace);

            if (index != MethodsStack.Count - 1)
            {
                int size = MethodsStack.Count - index - 1;
                var childMethods = MethodsStack.GetRange(index + 1, size);

                for (var i = 0; i < size; i++)
                    MethodsStack.RemoveAt(MethodsStack.Count - 1);

                MethodsStack[index].ChildMethods = childMethods;                
            }

            MethodsStack[index].CalculateTime(ellapsedMilliseconds);
            Time += MethodsStack[index].Time;
        }
    }
}