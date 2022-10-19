using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP_lab1
{
    public class TraceResult
    {
        public List<ThreadTraceResult> ThreadList;

        public TraceResult()
        {
            ThreadList = new List<ThreadTraceResult>();
        }
    }
}
