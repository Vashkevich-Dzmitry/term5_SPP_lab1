using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP_lab1
{
    internal class SerializationConsoleWriter : ISerializationWriter
    {
        public void Write(string value)
        {
            Console.Write(value);
        }
    }
}
