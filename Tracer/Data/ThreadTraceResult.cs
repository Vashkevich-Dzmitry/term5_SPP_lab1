namespace Tracer
{
    [Serializable]
    public class ThreadTraceResult
    {
        public int Id { get; set; }
        public long Time { get; set; }
        internal long OldTime { get; set; }
        public List<Node<MethodTraceResult>> MethodTraceResult { get; set; }
        
        internal Node<MethodTraceResult>? CurrentNode { get; set; }

        public ThreadTraceResult()
        {
            MethodTraceResult = new List<Node<MethodTraceResult>>();
        }
    }
}