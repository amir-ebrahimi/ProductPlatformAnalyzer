using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPlatformAnalyzer
{
    class Node<T>: IEnumerable<Node<T>>
    {
        private T data;
        private LinkedList<Node<T>> children;

        public T Data
        {
            get { return this.data; }
        }

        public LinkedList<Node<T>> Children
        {
            get { return this.children; }
        }

        public Node(T data)
        {
            this.data = data;
            children = new LinkedList<Node<T>>();
        }

        public void AddChildNode(Node<T> node)
        {
            children.AddFirst(node);
        }

        public void MyTraverse(Node<T> node, List<T> visited)
        {
            visited.Add(node.data);
            foreach (Node<T> kid in node.children)
                MyTraverse(kid, visited);

        }

        public IEnumerator<Node<T>> GetEnumerator()
        {
            return children.GetEnumerator();
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
