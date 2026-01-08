using backend.DataAccess.Database;
using Microsoft.EntityFrameworkCore;

namespace backend.Tests.DataAccess.Database
{
    [TestFixture]
    public class AppDbContextTests
    {
        [Test]
        public void AppDbContext_ShouldBeInstantiable()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Act
            using var context = new AppDbContext(options);

            // Assert
            Assert.That(context, Is.Not.Null);
        }

        [Test]
        public void AppDbContext_ShouldInheritFromDbContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase2")
                .Options;

            // Act
            using var context = new AppDbContext(options);

            // Assert
            Assert.That(context, Is.InstanceOf<DbContext>());
        }

        [Test]
        public void AppDbContext_ShouldBeDisposable()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase3")
                .Options;

            // Act & Assert
            using var context = new AppDbContext(options);
            Assert.That(context, Is.InstanceOf<IDisposable>());
        }

        [Test]
        public void AppDbContext_ShouldCreateModel_WithoutErrors()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase4")
                .Options;

            // Act
            using var context = new AppDbContext(options);
            var model = context.Model;

            // Assert
            Assert.That(model, Is.Not.Null);
        }
    }
}
