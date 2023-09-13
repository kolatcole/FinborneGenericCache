using FinborneGenericCache.Interface;
using FinborneGenericCache.Model;

namespace FinborneGenericCache.Core
{
    public class GenericLinkedList<K,V>: IGenericLinkedList<K, V>
    {
        private GenericNode<K, V> Head;
        private GenericNode<K, V> Tail;
        private readonly object lockObject = new object();

        public GenericLinkedList(K key, V value)
        {
            var node = new GenericNode<K,V>(key, value);
        }
        public GenericLinkedList()
        {

        }

        public GenericNode<K, V> AddNode(GenericNode<K, V> node)
        {
            lock (lockObject)
            {
                // Add first node to linkedList
                if (Head == null && Tail == null)
                {
                    Head = node;
                    Tail = node;
                }
                else
                {
                    Head.Previous = node;
                    Head.Previous.Value = node.Value;
                    Head.Previous.Key = node.Key;
                    node.Next = Head;
                    Head = node;
                }
                
                return Head;
            }

        }

        public GenericNode<K, V> UpdateNode(GenericNode<K, V> node)
        {
            lock (lockObject)
            {
                // if it's the last node
                if (node == Tail && node != Head)
                {
                    Tail = node.Previous;
                    node.Previous.Next = null;
                    node.Previous = null;
                }
                // if it's a middle node
                else if(node != Tail && node != Head)
                {
                    node.Previous.Next = node.Next;
                    node.Next.Previous = node.Previous;
                }

                Head.Previous = node;
                Head.Previous.Value = node.Value;
                Head.Previous.Key = node.Key;
                node.Next = Head;
                Head = node;
            }

            return Head;
            
        }

        public GenericNode<K, V> PopOrPeekTailNode()
        {
            var target = Tail;

            // If linkedList has more than one node
            if (Head!=Tail)
            {
                lock (lockObject)
                {
                    Tail = Tail.Previous;
                    Tail.Next = null;
                }
            }
            return target;
        }

    }



}
