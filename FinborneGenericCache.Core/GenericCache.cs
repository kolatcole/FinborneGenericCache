using FinborneGenericCache.Model;
using System.Collections.Concurrent;
using System.Text;
using FinborneGenericCache.Interface;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace FinborneGenericCache.Core
{
    public class GenericCache<K,V> : IGenericCache<K, V>
    {
        private readonly GenericCacheConfig Config;
        private readonly ILogger<GenericCache<K, V>> Logger;
        private readonly ConcurrentDictionary<K, GenericNode<K,V>>? DictionaryStore;
        private readonly GenericLinkedList<K, V> LinkedList;

        public GenericCache(GenericCacheConfig config, ILogger<GenericCache<K,V>> logger)
        {
            if (config.Limit <= 0)
                throw new ArgumentException("Cache limit must be greater than 0.", nameof(this.Config.Limit));
            
            this.Config = config;
            this.Logger = logger;
            this.DictionaryStore = new ConcurrentDictionary<K, GenericNode<K, V>>();
            this.LinkedList = new GenericLinkedList<K, V>();
        }


        public Task<Tuple<bool,string>> Add(K key, V value)
        {

            // Add when limit is not reach

            StringBuilder action=new StringBuilder();

            var node = new GenericNode<K, V>(key, value);

            if (DictionaryStore.Count >= this.Config.Limit)
            {
                var lruNode = this.LinkedList.PopTailNode();
                if (this.DictionaryStore.TryRemove(lruNode.Key, out var removedNode))
                {
                    this.Logger?.LogInformation($"Item with Key {lruNode.Key} was removed from cache due being filled up");
                    action.Append($"Item with Key {lruNode.Key} was removed from cache due being filled up");
                }
            }

            var wasAdded = DictionaryStore.TryAdd(key, node);
            if (wasAdded)
            {
                var fullNode = this.LinkedList.AddNode(node); 
                DictionaryStore.TryUpdate(fullNode.Key, fullNode, node);

                // I need a full node object returned here to update the dictionary node with prev and next
                this.Logger?.LogInformation($"Item with Key {key} was added to cache.");
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