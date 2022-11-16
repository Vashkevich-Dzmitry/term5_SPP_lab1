using Tracer;
using System.Xml.Linq;

namespace TraceResultSerializer
{
    public class XmlSerializer : ITraceResultSerializer<TraceResult>
    {
        public string Serialize(TraceResult traceResult)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer =
                new System.Xml.Serialization.XmlSerializer(typeof(TraceResult));

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, traceResult);
                return FormatXml(stringWriter.ToString());
            }

        }
        private static string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                Console.WriteLine("uncoorect xml");
                return xml;
            }
        }
    }
}
