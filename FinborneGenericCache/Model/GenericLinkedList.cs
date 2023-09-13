using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FinborneGenericCache.Model
{
    public class GenericLinkedList<K,V>
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
                if (node == Tail)
                {
                    Tail = node.Previous;
                    node.Previous.Next = null;
                    node.Previous = null;
                }
                else
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

        public GenericNode<K, V> PopTailNode()
        {
            var target = Tail;
            lock (lockObject)
            {
                Tail = Tail.Previous;
                Tail.Next = null;
            }
            return target;
        }

    }



}
