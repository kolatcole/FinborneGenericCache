using System;
using System.Collections.Generic;
using System.Linq;
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
            Head = node;
            Tail = node;
        }

        public void AddNode(GenericNode<K, V> node)
        {
            lock (lockObject)
            {
                //GenericNode<K, V> current = Head;

                //while (current.Next != null)
                //{
                //    current = current.Next;
                //}

                //current.Next = node;
                //current.Next.Value = node.Value;
                //current.Next.Key = node.Key;
                //node.Previous = current;
                //Tail = node;

                Head.Previous = node;
                Head.Previous.Value = node.Value;
                Head.Previous.Key = node.Key;
                node.Next = Head;
                Head = node;
            }
        }

        //public void RemoveNode()
        //{
        //    lock (lockObject)
        //    {
        //        Tail = Tail.Previous;
        //        Tail.Next = null;
        //    }
        //}

        public void RemoveNode(GenericNode<K, V> node)
        {
            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;
            node.Next = null;
            node.Previous = null;
        }

    }



}
