using backend.Core.Extensions;

namespace backend.Tests.Core.Extensions
{
    [TestFixture]
    public class QueryableExtensionsTests
    {
        private class TestEntity
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private class TestPageable : IPageable
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
        }

        [Test]
        public void AsPage_ShouldReturnCorrectPage_ForFirstPage()
        {
            // Arrange
            var items = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Item1" },
                new() { Id = Guid.NewGuid(), Name = "Item2" },
                new() { Id = Guid.NewGuid(), Name = "Item3" },
                new() { Id = Guid.NewGuid(), Name = "Item4" },
                new() { Id = Guid.NewGuid(), Name = "Item5" }
            }.AsQueryable().OrderBy(x => x.Name);

            var page = new TestPageable { Page = 1, PageSize = 2 };

            // Act
            var result = items.AsPage(page).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void AsPage_ShouldReturnCorrectPage_ForSecondPage()
        {
            // Arrange
            var items = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Item1" },
                new() { Id = Guid.NewGuid(), Name = "Item2" },
                new() { Id = Guid.NewGuid(), Name = "Item3" },
                new() { Id = Guid.NewGuid(), Name = "Item4" },
                new() { Id = Guid.NewGuid(), Name = "Item5" }
            }.AsQueryable().OrderBy(x => x.Name);

            var page = new TestPageable { Page = 2, PageSize = 2 };

            // Act
            var result = items.AsPage(page).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void AsPage_ShouldReturnPartialPage_ForLastPage()
        {
            // Arrange
            var items = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Item1" },
                new() { Id = Guid.NewGuid(), Name = "Item2" },
                new() { Id = Guid.NewGuid(), Name = "Item3" }
            }.AsQueryable().OrderBy(x => x.Name);

            var page = new TestPageable { Page = 2, PageSize = 2 };

            // Act
            var result = items.AsPage(page).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void PageResponse_ShouldReturnCorrectPageResult()
        {
            // Arrange
            var items = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Item1" },
                new() { Id = Guid.NewGuid(), Name = "Item2" },
                new() { Id = Guid.NewGuid(), Name = "Item3" },
                new() { Id = Guid.NewGuid(), Name = "Item4" },
                new() { Id = Guid.NewGuid(), Name = "Item5" }
            }.AsQueryable().OrderBy(x => x.Name);

            var page = new TestPageable { Page = 1, PageSize = 2 };

            // Act
            var result = items.PageResponse(page, x => x.Name);

            // Assert
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.Items.Count(), Is.EqualTo(2));
        }

        [Test]
        public void PageResponse_ShouldProjectCorrectly()
        {
            // Arrange
            var items = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Item1" },
                new() { Id = Guid.NewGuid(), Name = "Item2" }
            }.AsQueryable().OrderBy(x => x.Name);

            var page = new TestPageable { Page = 1, PageSize = 10 };

            // Act
            var result = items.PageResponse(page, x => x.Name);

            // Assert
            Assert.That(result.Items.Count(), Is.EqualTo(2));
        }
    }
}
