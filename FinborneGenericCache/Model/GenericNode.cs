using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinborneGenericCache.Model
{
    public class GenericNode<K,V>
    {
        public K Key { get; set; }
        public V Value { get; set; } 
        public GenericNode<K,V> Next { get; set; }
        public GenericNode<K,V> Previous { get; set; }

       
        public GenericNode(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }

}
