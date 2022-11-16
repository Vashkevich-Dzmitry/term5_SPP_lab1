using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP_lab1
{
    internal class SerializationFileWriter : ISerializationWriter
    {
        public string Path { get; set; }
        public SerializationFileWriter(string path)
        {
            Path = path;
        }
        public void Write(string value)
        {
            using (StreamWriter writer = new StreamWriter(Path))
            {
                writer.Write(value);
            }
        }
    }
}
