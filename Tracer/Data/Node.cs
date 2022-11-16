using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public class Node<T> where T : class
    {
        public T Data { get; set; }
        public Node(T data)
        {
            Data = data;
            Children = new List<Node<T>>();
        }

        internal Node<T>? Parent { get; set; }
        public List<Node<T>> Children { get; set; }
    }
}
