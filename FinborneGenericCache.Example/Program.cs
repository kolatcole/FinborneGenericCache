// See https://aka.ms/new-console-template for more information

using FinborneGenericCache.Model;

GenericLinkedList<string, string> dict = new GenericLinkedList<string, string>("1","first");

var node = new GenericNode<string, string>("2", "second");
var node1 = new GenericNode<string, string>("3", "third");
dict.AddNode(node);
dict.AddNode(node1);

var x = dict;

dict.RemoveNode(node);
x = dict;