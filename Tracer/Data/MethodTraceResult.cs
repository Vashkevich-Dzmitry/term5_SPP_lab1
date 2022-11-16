using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    [Serializable]
    public class MethodTraceResult
    {
        public string? ClassName { get; internal set; }

        public string? MethodName { get; internal set; }

        public long Time { get; internal set; }
    }
}
