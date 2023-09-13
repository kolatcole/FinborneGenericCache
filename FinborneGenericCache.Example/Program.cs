
using FinborneGenericCache.Core;

//GenericLinkedList<string, string> dict = new GenericLinkedList<string, string>("1","first");

//var node = new GenericNode<string, string>("2", "second");
//var node1 = new GenericNode<string, string>("3", "third");
//dict.AddNode(node);
//dict.AddNode(node1);

//var x = dict;

//dict.RemoveNode(node);
//x = dict;

//var cache = new GenericCache<string, string>(4,"1","one");
var config = new GenericCacheConfig();
config.Limit = 4;
var cache = new GenericCache<string, string>(config);
cache.Add("1", "one");
cache.Add("3", "three");  // head is 3, Tail is 1     3 -> 1
cache.Add("2", "two");      // head is 2, Tail is 1   2 -> 3 -> 1

var one = cache.Get("1");   // head is 1, Tail is 3   1 -> 2 -> 3
var two = cache.Get("2");   // head is 2, Tail is 3   2 -> 1 -> 3
cache.Add("4", "four");     // head is 4, Tail is 3   4 -> 2 -> 1 -> 3
var three = cache.Get("3");  // head is 3, Tail is 1   3 -> 4 -> 2 -> 1
cache.Add("5", "five");      // head is 5, Tail is 2   5 -> 3 -> 4 -> 2 
