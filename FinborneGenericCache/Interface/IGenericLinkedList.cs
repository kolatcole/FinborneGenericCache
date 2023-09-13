using FinborneGenericCache.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinborneGenericCache.Interface
{
    public interface IGenericLinkedList<K,V>
    {
        public GenericNode<K, V> AddNode(GenericNode<K, V> node);
        public GenericNode<K, V> UpdateNode(GenericNode<K, V> node);
        public GenericNode<K, V> PopTailNode();
        public bool RemoveNode(GenericNode<K, V> node);
    }
}
