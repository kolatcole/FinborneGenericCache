using FinborneGenericCache.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinborneGenericCache.Interface
{
    public interface IGenericCache<K,V>
    {
        public Task<Tuple<bool, string>> Add(K key, V value);
        public Task<GenericNode<K, V>> Get(K key);
    }

}
