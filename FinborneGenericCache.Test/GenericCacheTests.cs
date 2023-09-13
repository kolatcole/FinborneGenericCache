using FinborneGenericCache.Core;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinborneGenericCache.Test
{
    public class GenericCacheTests
    {
        private ILogger<GenericCache<int, int>> MockLogger;
        private GenericCacheConfig TestCacheConfig;

        [SetUp]
        public void Setup()
        {
            MockLogger= new Mock<ILogger<GenericCache<int, int>>>().Object;
            TestCacheConfig = new GenericCacheConfig();
        }

        [Test]
        public void AddAsync_ShouldAddItemToCache()
        {
            // Arrange 

            this.TestCacheConfig.Limit = 4;
            var cache = new GenericCache<int, int>(this.TestCacheConfig, this.MockLogger);

            // Act

            cache.AddAsync(1, 100).Wait();

            var actual = cache.GetAsync(1);
            // Assert

            Assert.That(actual?.Result?.Value, Is.EqualTo(100));

        }

        [Test]
        public void AddAsync_ShouldRemoveOldestItemAndAddNewItemToFullCache()
        {
            // Arrange 

            this.TestCacheConfig.Limit = 3;
            var cache = new GenericCache<int, int>(this.TestCacheConfig, this.MockLogger);

            // Act

            cache.AddAsync(1, 100).Wait();
            cache.AddAsync(2, 200).Wait();
            cache.AddAsync(3, 300).Wait();
            cache.AddAsync(4, 400).Wait();

            var actual = cache.GetAsync(4);
            // Assert

            Assert.IsNull(cache.GetAsync(1).Result);
            Assert.That(actual?.Result?.Value, Is.EqualTo(400));
        }

        [Test]
        public void AddAsync_ShouldNotAddItemWithSameKey()
        {
            // Arrange 

            this.TestCacheConfig.Limit = 3;
            var cache = new GenericCache<int, int>(this.TestCacheConfig, this.MockLogger);

            // Act

            cache.AddAsync(1, 100).Wait();
            cache.AddAsync(2, 200).Wait();
            cache.AddAsync(2, 500).Wait();

            var actual = cache.GetAsync(2);
            // Assert

            Assert.That(actual?.Result?.Value, Is.EqualTo(200));
        }


    }
}