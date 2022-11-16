using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceResultSerializer
{
    public interface ITraceResultSerializer<T> where T : class
    {
        string Serialize(T obj);
    }
}
