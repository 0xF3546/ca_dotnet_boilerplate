using backend.Core.Extensions;

namespace backend.Tests.Core.Extensions
{
    [TestFixture]
    public class PageRequestTests
    {
        [Test]
        public void PageRequest_ShouldImplementIPageable()
        {
            // Arrange
            var pageRequest = new PageRequest { Page = 1, PageSize = 10 };

            // Act & Assert
            Assert.That(pageRequest, Is.InstanceOf<IPageable>());
        }

        [Test]
        public void PageRequest_ShouldSetProperties_WhenInitialized()
        {
            // Arrange & Act
            var pageRequest = new PageRequest
            {
                Page = 2,
                PageSize = 25
            };

            // Assert
            Assert.That(pageRequest.Page, Is.EqualTo(2));
            Assert.That(pageRequest.PageSize, Is.EqualTo(25));
        }

        [Test]
        public void PageRequest_ShouldAllowMinimumValues()
        {
            // Arrange & Act
            var pageRequest = new PageRequest
            {
                Page = 1,
                PageSize = 1
            };

            // Assert
            Assert.That(pageRequest.Page, Is.EqualTo(1));
            Assert.That(pageRequest.PageSize, Is.EqualTo(1));
        }

        [Test]
        public void PageRequest_ShouldAllowLargeValues()
        {
            // Arrange & Act
            var pageRequest = new PageRequest
            {
                Page = 1000,
                PageSize = 100
            };

            // Assert
            Assert.That(pageRequest.Page, Is.EqualTo(1000));
            Assert.That(pageRequest.PageSize, Is.EqualTo(100));
        }
    }
}
