using FinborneGenericCache.Model;
using System.Collections.Concurrent;
using System.Text;
using FinborneGenericCache.Interface;

namespace FinborneGenericCache.Core
{
    public class GenericCache<K,V>: IGenericCache<K, V>
    {
        private int Limit;
        private readonly ConcurrentDictionary<K, GenericNode<K,V>>? DictionaryStore;
        private readonly GenericLinkedList<K, V> LinkedList;

        public GenericCache(int limit, K key, V value)
        {
            this.Limit = limit;
            this.DictionaryStore = new ConcurrentDictionary<K, GenericNode<K, V>>();
            this.DictionaryStore.AddOrUpdate(key,new GenericNode<K, V>(key, value),
                                                (key, oldValue) => new GenericNode<K, V>(key, value));
            this.LinkedList = new GenericLinkedList<K, V>(key, value);
        }


        public Task<Tuple<bool,string>> Add(K key, V value)
        {

            // Add when limit is not reach

            StringBuilder action=new StringBuilder();

            var node = new GenericNode<K, V>(key, value);

            if (DictionaryStore.Count >= Limit)
            {
                if (this.DictionaryStore.TryRemove(key, out var removedNode))
                {
                    this.LinkedList.RemoveNode(removedNode);
                    action.Append($"Item with Key {key} was removed from cache due being filled up");
                }
            }

            var wasAdded = DictionaryStore.TryAdd(key, node);
            if (wasAdded)
            {
                this.LinkedList.AddNode(node);
                action.Append($"Item with Key {key} was added to cache.");
                return Task.FromResult(new Tuple<bool, string>(wasAdded, action.ToString()));
            }

            return Task.FromResult(new Tuple<bool, string>(false, action.ToString()));
        }

        public Task<GenericNode<K, V>?> Get(K key)
        {
            // Return item if found in store using the provided key 
            if((bool)this.DictionaryStore.TryGetValue(key, out var retrievedItem))
            {
                // Remove the corresponding node from the linkedlist and add it to the top
                // to represent the last used node

                this.LinkedList.RemoveNode(retrievedItem);
                this.LinkedList.AddNode(retrievedItem);

                return Task.FromResult(retrievedItem);
            }

            // Return null if not found
            return Task.FromResult<GenericNode<K, V>?>(null);
        }
    }
} 