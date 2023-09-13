using FinborneGenericCache.Model;
using System.Collections.Concurrent;
using System.Text;
using FinborneGenericCache.Interface;
using Microsoft.Extensions.Logging;

namespace FinborneGenericCache.Core
{
    public class GenericCache<K,V> : IGenericCache<K, V>
    {
        private readonly GenericCacheConfig Config;
        private readonly ILogger<GenericCache<K, V>> Logger;
        private readonly ConcurrentDictionary<K, GenericNode<K,V>>? DictionaryStore;
        private readonly GenericLinkedList<K, V> LinkedList;
        private readonly object lockObject = new object();

        public GenericCache(GenericCacheConfig config, ILogger<GenericCache<K,V>> logger)
        {
            if (config.Limit <= 0)
                throw new ArgumentException("Cache limit must be greater than 0.", nameof(this.Config.Limit));
            
            this.Config = config;
            this.Logger = logger;
            this.DictionaryStore = new ConcurrentDictionary<K, GenericNode<K, V>>();
            this.LinkedList = new GenericLinkedList<K, V>();
        }


        public Task<Tuple<bool,string>> AddAsync(K key, V value)
        {
            StringBuilder action=new StringBuilder();
            var node = new GenericNode<K, V>(key, value);

            lock (lockObject)
            {
                // Evict the oldest item when cache is full
                if (DictionaryStore.Count >= this.Config.Limit)
                {
                    var lruNode = this.LinkedList.PopOrPeekTailNode();
                    if (this.DictionaryStore.TryRemove(lruNode.Key, out var removedNode))
                    {
                        this.Logger?.LogInformation($"Item with Key {lruNode.Key} was removed from cache beacuse the limit was reached \n");
                        action.AppendLine($"Item with Key {lruNode.Key} was removed from cache beacuse the limit was reached");
                    }
                }

                var wasAdded = DictionaryStore.TryAdd(key, node);
                
                // Should not take an item that has the same key with an existing item
                if (!wasAdded)
                {
                    this.Logger?.LogInformation($"Item with Key {key} already exist in cache.\n");
                    action.AppendLine($"Item with Key {key} already exist in cache.");
                    return Task.FromResult(new Tuple<bool, string>(false, action.ToString()));
                }

                var fullNode = this.LinkedList.AddNode(node);
                DictionaryStore.TryUpdate(fullNode.Key, fullNode, node);

                // Update the node so that it has next and pre
                this.Logger?.LogInformation($"Item with Key {key} was added to cache.\n");
                action.AppendLine($"Item with Key {key} was added to cache.");
                return Task.FromResult(new Tuple<bool, string>(wasAdded, action.ToString()));
            }
            
        }

        public Task<GenericNode<K, V>?> GetAsync(K key)
        {
            // Return item if found in store using the provided key 
            if((bool)this.DictionaryStore.TryGetValue(key, out var retrievedItem))
            {
                // Remove the corresponding node from the linkedlist and add it to the top
                // to represent the last used node
                lock (lockObject)
                {
                    this.LinkedList.UpdateNode(retrievedItem);
                }

                return Task.FromResult(retrievedItem);
            }

            // Return null if not found
            return Task.FromResult<GenericNode<K, V>?>(null);
        }
    }
} 