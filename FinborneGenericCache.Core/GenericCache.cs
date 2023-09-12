using FinborneGenericCache.Model;
using System.Collections.Concurrent;
using System.Text;
using FinborneGenericCache.Interface;
using System.Xml.Linq;

namespace FinborneGenericCache.Core
{
    public class GenericCache<K,V> : IGenericCache<K, V>
    {
        private int Limit;
        private readonly ConcurrentDictionary<K, GenericNode<K,V>>? DictionaryStore;
        private readonly GenericLinkedList<K, V> LinkedList;

        public GenericCache(int limit, K key, V value)
        {
            this.Limit = limit;
            this.DictionaryStore = new ConcurrentDictionary<K, GenericNode<K, V>>();
            this.LinkedList = new GenericLinkedList<K, V>(key, value);
            var node = this.LinkedList.AddNode(new GenericNode<K, V>(key, value));
            this.DictionaryStore.AddOrUpdate(key, node,
                                                (key, oldValue) => node);


            //this.DictionaryStore.AddOrUpdate(key, new GenericNode<K, V>(key, value),
            //                                    (key, oldValue) => new GenericNode<K, V>(key, value));
            //var fullNode = this.LinkedList.AddNode(new GenericNode<K, V>(key, value));
            //DictionaryStore.TryUpdate(fullNode.Key, fullNode, new GenericNode<K, V>(key, value));
            //this.LinkedList = new GenericLinkedList<K, V>(key, value);


        }


        public Task<Tuple<bool,string>> Add(K key, V value)
        {

            // Add when limit is not reach

            StringBuilder action=new StringBuilder();

            var node = new GenericNode<K, V>(key, value);

            if (DictionaryStore.Count >= Limit)
            {
                var lruNode = this.LinkedList.PopTailNode();
                if (this.DictionaryStore.TryRemove(lruNode.Key, out var removedNode))
                {
                    action.Append($"Item with Key {lruNode.Key} was removed from cache due being filled up");
                }
            }

            var wasAdded = DictionaryStore.TryAdd(key, node);
            if (wasAdded)
            {
                var fullNode = this.LinkedList.AddNode(node);
                DictionaryStore.TryUpdate(fullNode.Key, fullNode, node);

                // I need a full node object returned here to update the dictionary node with prev and next
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

               // if(this.LinkedList.RemoveNode(retrievedItem))
                this.LinkedList.UpdateNode(retrievedItem);

                return Task.FromResult(retrievedItem);
            }

            // Return null if not found
            return Task.FromResult<GenericNode<K, V>?>(null);
        }
    }
} 