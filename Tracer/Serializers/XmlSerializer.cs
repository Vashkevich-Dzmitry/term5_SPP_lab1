using System.Xml.Linq;

namespace Tracer
{
    public class XmlSerializer : ITraceResultSerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            var data = traceResult.GetThreadsInfo().Values.ToArray();
            System.Xml.Serialization.XmlSerializer xmlSerializer =
                new System.Xml.Serialization.XmlSerializer(data.GetType());

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, data);
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
