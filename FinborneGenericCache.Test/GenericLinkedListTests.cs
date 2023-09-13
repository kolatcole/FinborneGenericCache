using FinborneGenericCache.Core;
using FinborneGenericCache.Interface;
using FinborneGenericCache.Model;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinborneGenericCache.Test
{
    public class GenericLinkedListTests
    {

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void AddNode_ShouldAddNodeToEmptyLinkedList()
        {
            // Arrange

            var linkedList = new GenericLinkedList<int, string>();
            var node = new GenericNode<int, string>(1, "100");

            // Act

            var result = linkedList.AddNode(node);

            // Assert

            Assert.IsNull(result.Next);
            Assert.IsNull(linkedList.PopOrPeekTailNode().Previous);
            Assert.That(linkedList.PopOrPeekTailNode().Value, Is.EqualTo("100"));

        }

        [Test]
        public void AddNode_ShouldAddNodeToNonEmptyLinkedList()
        {
            // Arrange

            var linkedList = new GenericLinkedList<int, string>();
            var firstNode = new GenericNode<int, string>(1, "100");
            var secondNode = new GenericNode<int, string>(2, "200");

            // Act

            linkedList.AddNode(firstNode);

            // Assert

            Assert.That(linkedList.AddNode(secondNode).Value, Is.EqualTo("200"));
            Assert.That(linkedList.PopOrPeekTailNode().Value, Is.EqualTo("100"));
        }

        [Test]
        public void UpdateNode_ShouldMakeLastNodeTheFirstInLinkedList()
        {
            // Arrange

            var linkedList = new GenericLinkedList<int, string>();
            var firstNode = new GenericNode<int, string>(1, "100");
            var secondNode = new GenericNode<int, string>(2, "200");
            var thirdNode = new GenericNode<int, string>(3, "300");
            var fourthNode = new GenericNode<int, string>(4, "400");

            // Act

            linkedList.AddNode(firstNode);
            linkedList.AddNode(secondNode);
            linkedList.AddNode(thirdNode);
            linkedList.AddNode(fourthNode);

            var result = linkedList.UpdateNode(firstNode);

            // Assert

            Assert.That(result.Value, Is.EqualTo("100"));
            Assert.That(linkedList.PopOrPeekTailNode().Value, Is.EqualTo("200"));
        }

        [Test]
        public void UpdateNode_ShouldMakeMiddleNodeTheFirstInLinkedList()
        {

            var linkedList = new GenericLinkedList<int, string>();
            var firstNode = new GenericNode<int, string>(1, "100");
            var secondNode = new GenericNode<int, string>(2, "200");
            var thirdNode = new GenericNode<int, string>(3, "300");
            var fourthNode = new GenericNode<int, string>(4, "400");

            // Act

            linkedList.AddNode(firstNode);
            linkedList.AddNode(secondNode);
            linkedList.AddNode(thirdNode);
            linkedList.AddNode(fourthNode);

            var result = linkedList.UpdateNode(thirdNode);

            // Assert

            Assert.That(result.Value, Is.EqualTo("300"));
            Assert.That(result.Next, Is.EqualTo(fourthNode));
            Assert.That(linkedList.PopOrPeekTailNode().Value, Is.EqualTo("100"));

        }
    }
}
