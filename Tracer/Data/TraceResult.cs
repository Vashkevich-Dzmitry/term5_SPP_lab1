using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    [Serializable]
    public class TraceResult
    {
        public List<ThreadInfo> ThreadList;

        public TraceResult()
        {
            ThreadList = new List<ThreadInfo>();
        }
    }
}
