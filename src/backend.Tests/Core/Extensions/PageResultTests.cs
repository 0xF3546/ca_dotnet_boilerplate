using backend.Core.Extensions;

namespace backend.Tests.Core.Extensions
{
    [TestFixture]
    public class PageResultTests
    {
        [Test]
        public void PageResult_ShouldSetProperties_WhenInitialized()
        {
            // Arrange
            var items = new List<string> { "Item1", "Item2", "Item3" };
            var count = 50;

            // Act
            var pageResult = new PageResult<string>(items, count);

            // Assert
            Assert.That(pageResult.Items, Is.EqualTo(items));
            Assert.That(pageResult.Count, Is.EqualTo(count));
        }

        [Test]
        public void PageResult_ShouldAllowEmptyItems()
        {
            // Arrange
            var items = new List<int>();
            var count = 0;

            // Act
            var pageResult = new PageResult<int>(items, count);

            // Assert
            Assert.That(pageResult.Items, Is.Empty);
            Assert.That(pageResult.Count, Is.EqualTo(0));
        }

        [Test]
        public void PageResult_ShouldWorkWithDifferentTypes()
        {
            // Arrange
            var items = new List<int> { 1, 2, 3, 4, 5 };
            var count = 100;

            // Act
            var pageResult = new PageResult<int>(items, count);

            // Assert
            Assert.That(pageResult.Items.Count(), Is.EqualTo(5));
            Assert.That(pageResult.Count, Is.EqualTo(100));
        }

        [Test]
        public void PageResult_Count_CanBeModified()
        {
            // Arrange
            var items = new List<string> { "Test" };
            var pageResult = new PageResult<string>(items, 10)
            {
                // Act
                Count = 20
            };

            // Assert
            Assert.That(pageResult.Count, Is.EqualTo(20));
        }
    }
}
