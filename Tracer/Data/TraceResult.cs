using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public class TraceResult
    {
        private ConcurrentDictionary<int, ThreadInfo> ThreadsInfo { get; }

        public TraceResult(ConcurrentDictionary<int, ThreadInfo> threadsInfo)
        {
            ThreadsInfo = threadsInfo;
        }

        internal ThreadInfo GetOrAddThreadInfo(int threadId)
        {
            return ThreadsInfo.GetOrAdd(threadId, new ThreadInfo(threadId));
        }

        internal bool HasThreadWithId(int threadId)
        {
            return ThreadsInfo.ContainsKey(threadId);
        }
        public ConcurrentDictionary<int, ThreadInfo> GetThreadsInfo()
        {
            return ThreadsInfo;
        }

        public TraceResult()
        {
            ThreadsInfo = new ConcurrentDictionary<int, ThreadInfo>();
        }
    }
}
