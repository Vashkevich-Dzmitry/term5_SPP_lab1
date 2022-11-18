namespace Tracer
{
    [Serializable]
    public class ThreadInfo
    {
        public int Id { get; set; }
        public long Time { get; set; }
        internal long OldTime { get; set; }
        public List<Node<MethodInfo>> MethodsInfo { get; set; }
        
        internal Node<MethodInfo>? CurrentNode { get; set; }

        public ThreadInfo()
        {
            MethodsInfo = new List<Node<MethodInfo>>();
        }
    }
}